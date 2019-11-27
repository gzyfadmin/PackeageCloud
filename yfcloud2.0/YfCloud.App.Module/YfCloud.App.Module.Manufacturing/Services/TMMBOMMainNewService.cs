using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Framework.CacheManager;
using YfCloud.Models;
using YfCloud.Utilities.AutoMapper;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Manufacturing.Services
{
    [UseDI(ServiceLifetime.Scoped, typeof(ITMMBOMMainNewService))]
    public class TMMBOMMainNewService : ITMMBOMMainNewService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMBOMMainNewService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMBOMMainNewService(IDbContext dbContext, ILogger<TMMBOMMainNewService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }



        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMMainAddNewModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //删除以前的数据
                int PackageId = requestObject.PostData.PackageId;

                TMMBOMMainDbModel oldDBMolde = _db.Instance.Queryable<TMMBOMMainDbModel>().Where(p => p.PackageId == PackageId).First();

                if (oldDBMolde != null)
                {
                    _db.Instance.Deleteable<TMMBOMDetailDbModel>().Where(p => p.MainId == PackageId).ExecuteCommand();
                    _db.Instance.Deleteable<TMMBOMMainDbModel>(oldDBMolde).ExecuteCommand();
                }

                //插入主表数据
                var mapMainModel = _mapper.Map<TMMBOMMainDbModel>(requestObject.PostData);
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                var materCache = BasicCacheGet.GetMaterial(currentUser);

                #region 处理配色项目

                List<string> ItemList = requestObject.PostData.ChildList.Select(p => p.ItemName).Distinct().ToList();
                Dictionary<string, int> itemToId = new Dictionary<string, int>();

                foreach (string name in ItemList)
                {
                   var colorItem=  _db.Instance.Queryable<TMMPackageColorItemDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && p.ItemName == name && p.PackageId==PackageId).First();

                    int ItemId = 0;
                    if (colorItem == null)
                    {
                        TMMPackageColorItemDbModel temp = new TMMPackageColorItemDbModel();
                        temp.CompanyId = currentUser.CompanyID;
                        temp.DeleteFlag = false;
                        temp.PackageId = mapMainModel.PackageId;
                        temp.ItemName = name;

                        ItemId = _db.Instance.Insertable(temp).ExecuteReturnIdentity();
                    }
                    else
                    {
                        ItemId = colorItem.ID;
                        if (colorItem.DeleteFlag == true)
                        {
                            colorItem.DeleteFlag = false;
                            _db.Instance.Updateable(colorItem).ExecuteCommand();
                        }
                    }
                    itemToId.Add(name, ItemId);
                }

                var itemIDs = _db.Instance.Queryable<TMMPackageColorItemDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && p.PackageId== PackageId &&
                !ItemList.Contains(p.ItemName)).Select(p=>p.ID).ToList();

                _db.Instance.Updateable<TMMPackageColorItemDbModel>().Where(p=> itemIDs.Contains(p.ID)).
                    SetColumns(it => new TMMPackageColorItemDbModel { DeleteFlag = true }).ExecuteCommand();

                _db.Instance.Deleteable<TMMColorSolutionDetailDbModel>().Where(p => SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().
                Where(p1 =>p1.ID==p.MainId && p1.CompanyId==currentUser.CompanyID && p1.PackageId == PackageId).Any() && itemIDs.Contains(p.ItemId));

                #endregion

                #region 处理部位

                List<string> PartList = requestObject.PostData.ChildList.Where(p=> !string.IsNullOrWhiteSpace(p.PartName)).Select(p => p.PartName).Distinct().ToList();
                Dictionary<string, int>  partId = new Dictionary<string, int>();

                var dictionaryType = _db.Instance.Queryable<TBMDictionaryTypeDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && p.TypeName == "部位档案").First();

                foreach (string name in PartList)
                {
                    var dicItem = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && p.DicValue == name).First();

                    int dicID = 0;
                    if (dicItem == null)
                    {
                        TBMDictionaryDbModel temp = new TBMDictionaryDbModel();
                        temp.CompanyId = currentUser.CompanyID;
                        temp.DeleteFlag = false;
                        temp.TypeId = dictionaryType.ID;
                        temp.DicValue = name;
                        dicID = _db.Instance.Insertable(temp).ExecuteReturnIdentity();
                    }
                    else
                    {
                        dicID = dicItem.ID;

                    }

                    partId.Add(name, dicID);
                }


                #endregion

                List<TMMBOMDetailDbModel> deatail = new List<TMMBOMDetailDbModel>();
                foreach (TMMBOMDetailAddNewModel item in requestObject.PostData.ChildList)
                {
                    TMMBOMDetailDbModel tMMBOMDetailDbModel = ExpressionGenericMapper<TMMBOMDetailAddNewModel, TMMBOMDetailDbModel>.Trans(item);
                    tMMBOMDetailDbModel.MainId = mainId;
                    tMMBOMDetailDbModel.ItemId = itemToId[item.ItemName];

                    if(!string.IsNullOrWhiteSpace(item.PartName))
                    {
                        tMMBOMDetailDbModel.PartId = partId[item.PartName];
                    }

                    if (!materCache.Any(p1 => p1.MaterialName == item.MaterialName))
                    {
                        throw new Exception($"{item.MaterialName}物料不存在");
                    }

                    deatail.Add(tMMBOMDetailDbModel);

                }

                //插入从表数据
               // var mapDetailModelList = _mapper.Map<List<TMMBOMDetailAddNewModel>, List<TMMBOMDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(deatail).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_MM_BOMMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        private async Task<ResponseObject<List<TMMBOMMainQueryNewModel>>> GetMainListAsync(RequestGet requestObject)
        {
            try
            {
                List<TMMBOMMainQueryNewModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMBOMMainDbModel>();
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMBOMMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                var queryDataTemp = query.Select((t) => new TMMBOMMainQueryNewModel
                {
                    ID = t.ID,
                    PackageId = t.PackageId,
                    PagerCode = t.PagerCode,
                    Maker = t.Maker,
                    FrontImgPath = t.FrontImgPath,
                    SideImgPath = t.SideImgPath,
                    BackImgPath = t.BackImgPath,
                });

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await queryDataTemp.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await queryDataTemp.ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TMMBOMMainQueryNewModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMMainQueryNewModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_MM_BOMDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        private async Task<ResponseObject<List<TMMBOMDetailQueryNewModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TMMBOMDetailQueryNewModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TMMBOMDetailDbModel, TBMDictionaryDbModel, TMMFormulaDbModel,TMMPackageColorItemDbModel>((t, t1, t2,t3) => new object[] {
                    JoinType.Left,t.PartId==t1.ID,
                    JoinType.Left,t.Formula==t2.ID.ToString(),
                    JoinType.Inner,t.ItemId==t3.ID
                });
                query.OrderBy($"t3.ItemName asc");
                //执行查询
                queryData = await query.Select((t, t1, t2,t3) => new TMMBOMDetailQueryNewModel
                {
                    ID = t.ID,
                    MainId = t.MainId,
                    ItemId = t.ItemId,
                    ItemName=t3.ItemName,
                    MaterialName = t.MaterialName,
                    PartId = t.PartId,
                    PartName = t1.DicValue,
                    LengthValue = t.LengthValue,
                    WidthValue = t.WidthValue,
                    NumValue = t.NumValue,
                    WideValue = t.WideValue,
                    LossValue = t.LossValue,
                    SingleValue = t.SingleValue,
                    Formula = t.Formula,
                    FormulaName = t2.FormulaName
                })
                                       .Where(t => t.MainId == requestObject)
                                       .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TMMBOMDetailQueryNewModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMDetailQueryNewModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TMMBOMMainQueryNewModel>> GetWholeMainData(int requestObject)
        {
            TMMBOMMainQueryNewModel result;

            try
            {
                RequestGet requestGet = new RequestGet()
                {
                    IsPaging = false,
                    QueryConditions =
                    new List<QueryCondition>() { new QueryCondition() { Column = "PackageId", Condition = ConditionEnum.Equal, Content = requestObject.ToString() } }
                };

                var main = await GetMainListAsync(requestGet);

                result = main.Data.FirstOrDefault();
                if (result != null)
                {
                    var detailList = await GetDetailListAsync(result.ID);
                    result.ChildList = detailList.Data;
                }

                //返回执行结果
                return ResponseUtil<TMMBOMMainQueryNewModel>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TMMBOMMainQueryNewModel>.FailResult(null, ex.Message);
            }
        }
    }
}
