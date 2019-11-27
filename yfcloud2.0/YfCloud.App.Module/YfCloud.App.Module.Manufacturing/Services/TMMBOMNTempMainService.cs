///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMNTempMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4 8:56:17
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
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMBOMNTempMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITMMBOMNTempMainService))]
    public class TMMBOMNTempMainService : ITMMBOMNTempMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMBOMNTempMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMBOMNTempMainService(IDbContext dbContext, ILogger<TMMBOMNTempMainService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_MM_BOMNTempMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMBOMNTempMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMBOMNTempMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMBOMNTempMainDbModel>();
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMBOMNTempMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t) => new TMMBOMNTempMainQueryModel 
										{
											ID = t.ID,
											PackageId = t.PackageId,
											PagerCode = t.PagerCode,
											Maker = t.Maker,
											FrontImgPath = t.FrontImgPath,
											SideImgPath = t.SideImgPath,
											BackImgPath = t.BackImgPath,
											TempName = t.TempName,
											CompanyId = t.CompanyId,
										})
                                            .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t) => new TMMBOMNTempMainQueryModel 
										{
											ID = t.ID,
											PackageId = t.PackageId,
											PagerCode = t.PagerCode,
											Maker = t.Maker,
											FrontImgPath = t.FrontImgPath,
											SideImgPath = t.SideImgPath,
											BackImgPath = t.BackImgPath,
											TempName = t.TempName,
											CompanyId = t.CompanyId,
										})
                                            .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TMMBOMNTempMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMNTempMainQueryModel>>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 获取T_MM_BOMNTempDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMBOMNTempDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TMMBOMNTempDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TMMBOMNTempDetailDbModel, TBMDictionaryDbModel,TBMMaterialFileDbModel>((t, t1,t2) => new object[] {
                    JoinType.Left,t.PartId==t1.ID,
                    JoinType.Left,t.MaterialId==t2.ID
                });


                //执行查询
                queryData = await query.Select((t,t1, t2) => new TMMBOMNTempDetailQueryModel 
										{
											ID = t.ID,
											MainId = t.MainId,
                                            MaterialId=t.MaterialId,
                                            MaterialName=t2.MaterialName,
                                            PartId = t.PartId,
                                            PartName=t1.DicValue,
											LengthValue = t.LengthValue,
											WidthValue = t.WidthValue,
											NumValue = t.NumValue,
											WideValue = t.WideValue,
											LossValue = t.LossValue,
											SingleValue = t.SingleValue,
											Formula = t.Formula,
										})
                                       .Where(t => t.MainId == requestObject)
                                       .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TMMBOMNTempDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMBOMNTempDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_MM_BOMNTempMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMNTempMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();

                bool isExsit = _db.Instance.Queryable<TMMBOMNTempMainDbModel>().Any(p => p.TempName == requestObject.PostData.TempName && p.CompanyId == currentUser.CompanyID);
                if (isExsit)
                {
                    return ResponseUtil<bool>.FailResult(false, "模板名称已经存在!");
                }

                //插入主表数据
                var mapMainModel = _mapper.Map<TMMBOMNTempMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TMMBOMNTempDetailAddModel>, List<TMMBOMNTempDetailDbModel>>(requestObject.PostData.ChildList);
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

        public async Task<ResponseObject<TMMBOMNTempMainQueryModel>> GetWholeMainData(int requestObject, CurrentUser currentUser)
        {
            TMMBOMNTempMainQueryModel result;
            
            try
            {
                RequestGet requestGet = new RequestGet()
                {
                    IsPaging = false,
                    QueryConditions =
                    new List<QueryCondition>() { new QueryCondition() { Column = "Id", Condition = ConditionEnum.Equal, Content = requestObject.ToString() } }
                };

                var main = await GetMainListAsync(requestGet, currentUser);

                result = main.Data.FirstOrDefault();
                if (result != null)
                {
                    var detailList = await GetDetailListAsync(result.ID);
                    result.ChildList = detailList.Data;
                }

                //返回执行结果
                return ResponseUtil<TMMBOMNTempMainQueryModel>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TMMBOMNTempMainQueryModel>.FailResult(null, ex.Message);
            }
        }
    }
}
