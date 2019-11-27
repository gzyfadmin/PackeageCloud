///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:31:06
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMBOMMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITMMBOMMainService))]
    public class TMMBOMMainService : ITMMBOMMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMBOMMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMBOMMainService(IDbContext dbContext, ILogger<TMMBOMMainService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_MM_BOMMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        private async Task<ResponseObject<List<TMMBOMMainQueryModel>>> GetMainListAsync(RequestGet requestObject)
        {
            try
            {
                List<TMMBOMMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMBOMMainDbModel>();
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
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

               var queryDataTemp = query.Select((t) => new TMMBOMMainQueryModel
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
                return ResponseUtil<List<TMMBOMMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMMainQueryModel>>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 获取T_MM_BOMDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        private async Task<ResponseObject<List<TMMBOMDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TMMBOMDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TMMBOMDetailDbModel, TBMDictionaryDbModel,TMMFormulaDbModel>((t, t1,t2) => new object[] {
                    JoinType.Left,t.PartId==t1.ID,
                    JoinType.Left,t.Formula==t2.ID.ToString()
                });

                //执行查询
                queryData = await query.Select((t, t1, t2) => new TMMBOMDetailQueryModel
                {
                    ID = t.ID,
                    MainId = t.MainId,
                    ItemId = t.ItemId,
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
                return ResponseUtil<List<TMMBOMDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TMMBOMMainQueryModel>> GetWholeMainData(int requestObject)
        {
            TMMBOMMainQueryModel result;

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

                    //_db.Instance.Queryable<TMMColorSolutionDetailDbModel, TBMDictionaryDbModel>((t, t1) => new object[] {
                    //    JoinType.Left, t.ItemId==t1.ID
                    //}).Select((t,t1)=>new ItemModel() { ItemId=t.ItemId, ItemName=t1.DicValue });
                }

                //返回执行结果
                return ResponseUtil<TMMBOMMainQueryModel>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TMMBOMMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_MM_BOMMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMMainAddModel> requestObject,CurrentUser currentUser)
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

                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => {

                    if (!materCache.Any(p1 => p1.MaterialName == p.MaterialName))
                    {
                        throw new Exception($"{p.MaterialName}物料不存在");
                    }

                    p.MainId = mainId;

                    });
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TMMBOMDetailAddModel>, List<TMMBOMDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
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
    }
}
