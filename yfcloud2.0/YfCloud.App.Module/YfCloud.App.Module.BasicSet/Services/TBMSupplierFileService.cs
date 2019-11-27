///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMSupplierFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:05:13
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
using YfCloud.App.Module.BasicSet.Models.TBMSupplierFile;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierContact;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// ITBMSupplierFileService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMSupplierFileService))]
    public class TBMSupplierFileService : ITBMSupplierFileService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TBMSupplierFileService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TBMSupplierFileService(IDbContext dbContext, ILogger<TBMSupplierFileService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_BM_SupplierFile主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TBMSupplierFileQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TBMSupplierFileQueryModel> queryData = new List<TBMSupplierFileQueryModel>();//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数

                var query = _db.Instance.Queryable<TBMSupplierFileDbModel>().Where(t => t.CompanyId == currentUser.CompanyID && t.DeleteFlag == false);

                //var zx= SqlFunc.Subqueryable<TBMSupplierContactDbModel>().Where(s => s.SupplierId == t.ID && s.ContactName == "123").Any();

                //数据字典
                List<TBMDictionaryCacheModel> dics = BasicCacheGet.GetDic(currentUser);
                List<ChinaAreaRecord> Areas = BasicCacheGet.GetChinaArea(currentUser);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var customerConditions1 = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "contactname").ToList();
                    if (customerConditions1 != null && customerConditions1.Count() > 0)
                    {
                        var thisC = customerConditions1.FirstOrDefault();
                        query = query.Where(t=>SqlFunc.Subqueryable<TBMSupplierContactDbModel>().Where(s => s.SupplierId == t.ID && s.ContactName.Contains(thisC.Content)).Any());
                    }

                    var customerConditions2 = requestObject.QueryConditions.Where(p => p.Column.ToLower() != "contactname").ToList();
                    if (customerConditions2 != null && customerConditions2.Count() > 0)
                    {
                        var conditionals = SqlSugarUtil.GetConditionalModels(customerConditions2);
                        query.Where(conditionals);
                    }

                }

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMSupplierFileDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                List<TBMSupplierFileDbModel> queryDataTemp;
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryDataTemp = await query
                        .Where(t => !t.DeleteFlag)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryDataTemp = await query
                        .Where(t => !t.DeleteFlag)
                        .ToListAsync();
                }

                queryDataTemp.ForEach((x) =>
                {
                    var itemEnity = ExpressionGenericMapper<TBMSupplierFileDbModel, TBMSupplierFileQueryModel>.Trans(x);
                    itemEnity.ChildList = new List<TBMSupplierContactQueryModel>();
                    foreach (var citem in x.Child.OrderBy(p => p.Priority))
                    {
                        var citemEnity = ExpressionGenericMapper<TBMSupplierContactDbModel, TBMSupplierContactQueryModel>.Trans(citem);
                        itemEnity.ChildList.Add(citemEnity);
                    }

                    var item = dics.Where(p => p.ID == x.SupplierTypeId).FirstOrDefault();
                    if (item != null)
                    {
                        itemEnity.SupplierTypeName = item.DicValue;
                    }

                    if (x.IndustryId != null)
                    {
                        var industry = dics.Where(p => p.ID == x.IndustryId).FirstOrDefault();
                        if (x.IndustryId != null)
                        {
                            itemEnity.IndustryName = industry?.DicValue;
                        }
                    }

                    if (!string.IsNullOrEmpty(x.City))
                    {
                        var cityEntity = Areas.Where(p => p.Code == x.City).FirstOrDefault();
                        itemEnity.CityName = cityEntity?.FullName;
                    }

                    queryData.Add(itemEnity);
                });

                //返回执行结果
                return ResponseUtil<List<TBMSupplierFileQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TBMSupplierFileQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_BM_SupplierContact数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TBMSupplierContactQueryModel>>> GetDetailListAsync(int requestObject, CurrentUser currentUser)
        {
            try
            {
                //查询结果集对象
                List<TBMSupplierContactQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TBMSupplierContactDbModel>();
                
                //执行查询
                queryData = await query.Select((t) => new TBMSupplierContactQueryModel 
										{
											ID = t.ID,
											SupplierId = t.SupplierId,
											ContactName = t.ContactName,
											ContactNumber = t.ContactNumber,
											Priority = t.Priority,
										})
                                       .Where(t => t.SupplierId == requestObject)
                                       .ToListAsync();
                                            
                //返回执行结果
                return ResponseUtil<List<TBMSupplierContactQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TBMSupplierContactQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_BM_SupplierFile数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TBMSupplierFileAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TBMSupplierFileDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;

                if (_db.Instance.Queryable<TBMSupplierFileDbModel>().Any(p => p.SupplierCode == requestObject.PostData.SupplierCode && p.CompanyId == currentUser.CompanyID &&
                 SqlFunc.IsNull(p.DeleteFlag, false) == false)
                )
                {
                    throw new Exception("编码重复");
                }


                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                if (requestObject.PostData.ChildList != null && requestObject.PostData.ChildList.Count() > 0)
                {
                    //更新明细表外键ID值
                    requestObject.PostData.ChildList.ForEach(p => p.SupplierId = mainId);
                    //插入从表数据
                    var mapDetailModelList = _mapper.Map<List<TBMSupplierContactAddModel>, List<TBMSupplierContactDbModel>>(requestObject.PostData.ChildList);
                    var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                }
              
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return  ResponseUtil<bool>.SuccessResult(true);
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
        /// 修改T_BM_SupplierFile数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TBMSupplierFileEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {                
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TBMSupplierFileDbModel>(requestObject.PostData);
                
                 if (_db.Instance.Queryable<TBMSupplierFileDbModel>().Any(p => p.SupplierCode == requestObject.PostData.SupplierCode && p.CompanyId == currentUser.CompanyID &&
                 SqlFunc.IsNull(p.DeleteFlag, false) == false &&p.ID!=requestObject.PostData.ID)
                )
                {
                    throw new Exception("编码重复");
                }

                var mainFlag = await currDb.Updateable(mainModel).IgnoreColumns(p=>new { p.DeleteFlag,p.CompanyId}).ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                if (requestObject.PostData.ChildList != null && requestObject.PostData.ChildList.Count() > 0)
                {
                    var detailModels = _mapper.Map<List<TBMSupplierContactEditModel>,
                   List<TBMSupplierContactDbModel>>(requestObject.PostData.ChildList);
                    foreach (var item in detailModels)
                    {
                        if (!detailFlag) break;
                        item.SupplierId = mainModel.ID;
                        //新增或修改明细数据
                        detailFlag = item.ID <= 0
                            ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                            : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                    }

                    //删除明细数据
                    if (detailFlag)
                    {
                        var detailIds = detailModels.Select(p => p.ID).ToList();
                        detailFlag = currDb.Deleteable<TBMSupplierContactDbModel>()
                            .Where(p => !detailIds.Contains(p.ID) && p.SupplierId == mainModel.ID)
                            .ExecuteCommand() >= 0;
                    }
                }
                else
                {
                    detailFlag = currDb.Deleteable<TBMSupplierContactDbModel>()
                            .Where(p =>  p.SupplierId == mainModel.ID)
                            .ExecuteCommand() >= 0;
                }
               

               

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "修改数据失败!");
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
        /// 删除T_BM_SupplierFile数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData、PostDataList不能都为null");
                //开启事务
                currDb.BeginTran();
                //删除标识
                var mainFlag = true;
                var detailFlag = true;
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();

                    mainFlag = await currDb.Updateable<TBMSupplierFileDbModel>().Where(p => mainIds.Contains(p.ID) && p.CompanyId == currentUser.CompanyID).
                        SetColumns(p => new TBMSupplierFileDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await currDb.Updateable<TBMSupplierFileDbModel>().Where(p => p.ID == requestObject.PostData.ID && p.CompanyId == currentUser.CompanyID).
                         SetColumns(p => new TBMSupplierFileDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
                }
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
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
