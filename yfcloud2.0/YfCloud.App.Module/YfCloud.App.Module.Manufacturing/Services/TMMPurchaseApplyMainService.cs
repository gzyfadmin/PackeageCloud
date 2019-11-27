///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMPurchaseApplyMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11 8:38:12
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
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// MRP算的转采购的单子，用于生产采购的源数据
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITMMPurchaseApplyMainService))]
    public class TMMPurchaseApplyMainService : ITMMPurchaseApplyMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMPurchaseApplyMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMPurchaseApplyMainService(IDbContext dbContext, ILogger<TMMPurchaseApplyMainService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_MM_PurchaseApplyMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMPurchaseApplyMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMPurchaseApplyMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMPurchaseApplyMainDbModel, TMMProductionOrderMainDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2) => new object[]
                        {
                            JoinType.Left , t.SourceId == t0.ID,
                            JoinType.Left , t.OperatorId == t1.ID,
                            JoinType.Left , t.AuditId == t1.ID
                        }).Where((t,t0,t1,t2)=>t.DeleteFlag==false);
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var otherQuery = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "sourceno").ToList();

                    if (otherQuery.Count() > 0)
                    {

                        var con = SqlSugarUtil.GetConditionalModels(otherQuery);
                        foreach (ConditionalModel item in con)
                        {
                            item.FieldName = "ProductionNo";
                            item.FieldName = $"t0.{item.FieldName}";
                        }
                        query.Where(con);
                    }

                    var thisQuery= requestObject.QueryConditions.Where(p=>p.Column.ToLower()!= "sourceno").ToList();
                    if (thisQuery.Count() > 0)
                    {
                        var conditionals = SqlSugarUtil.GetConditionalModels(thisQuery);
                        foreach (ConditionalModel item in conditionals)
                        {

                            item.FieldName = $"t.{item.FieldName}";
                        }
                        query.Where(conditionals);
                    }
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {

                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMPurchaseApplyMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query
                        .Select((t, t0, t1, t2) => new TMMPurchaseApplyMainQueryModel
                        {
                            ID = t.ID,
                            SourceId = t.SourceId,
                            SourceNo = t0.ProductionNo,
                            PurchaseNo = t.PurchaseNo,
                            ApplyDate = t.ApplyDate,
                            OperatorId = t.OperatorId,
                            OperatorName = t1.AccountName,
                            OperatorTime = t.OperatorTime,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditStatus = t.AuditStatus,
                            AuditTime = t.AuditTime,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TransferFlag = t.TransferFlag,
                        })
                        .Where((t) => !t.DeleteFlag && t.CompanyId == currentUser.CompanyID)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Select((t, t0, t1, t2) => new TMMPurchaseApplyMainQueryModel
                        {
                            ID = t.ID,
                            SourceId = t.SourceId,
                            SourceNo = t0.ProductionNo,
                            PurchaseNo = t.PurchaseNo,
                            ApplyDate = t.ApplyDate,
                            OperatorId = t.OperatorId,
                            OperatorName = t1.AccountName,
                            OperatorTime = t.OperatorTime,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditStatus = t.AuditStatus,
                            AuditTime = t.AuditTime,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TransferFlag = t.TransferFlag,
                        })
                        .Where((t) => !t.DeleteFlag && t.CompanyId == currentUser.CompanyID)
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TMMPurchaseApplyMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMPurchaseApplyMainQueryModel>>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 获取T_MM_PurchaseApplyDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            return await GetDetailList(requestObject);
        }

        private async Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetDetailList(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TMMPurchaseApplyDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TMMPurchaseApplyDetailDbModel,TBMMaterialFileDbModel,TBMDictionaryDbModel,
                    TBMDictionaryDbModel,TBMDictionaryDbModel, TBMDictionaryDbModel>(
                        (t,t0,t1,t2,t3,t4) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.BaseUnitId == t1.ID,
                            JoinType.Left , t0.ProduceUnitId == t2.ID,
                            JoinType.Left , t0.PurchaseUnitId == t3.ID,
                            JoinType.Left,t0.ColorId==t4.ID
                        });

                //执行查询
                queryData = await query
                    .Select((t, t0, t1,t2,t3,t4) => new TMMPurchaseApplyDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        ApplyNum = t.ApplyNum,
                        TransNum = t.TransNum,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t1.DicValue,
                        ProduceUnitId = t0.ProduceUnitId,
                        ProduceUnitName =  SqlFunc.IsNullOrEmpty(t2.ID) ? t1.DicValue : t2.DicValue,
                        ProduceRate = t0.ProduceRate,
                        PurchaseUnitId = t0.PurchaseUnitId,
                        PurchaseUnitName = SqlFunc.IsNullOrEmpty(t3.ID) ? t1.DicValue : t3.DicValue,
                        PurchaseRate = t0.PurchaseRate,
                        ColorName=t4.DicValue

                    })
                    .Where(t => t.MainId == requestObject)
                    .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TMMPurchaseApplyDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMPurchaseApplyDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 查询TMMPurchaseApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetToPurchaseListAsync(int requestObject)
        {
            var result = await GetDetailList(requestObject);
            return ResponseUtil<List<TMMPurchaseApplyDetailQueryModel>>.SuccessResult(result.Data.Where(p => p.TransNum > 0).ToList());
        }

        /// <summary>
        /// 终止采购
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopPurchase(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TMMPurchaseApplyMainDbModel>()
                    .SetColumns(p => new TMMPurchaseApplyMainDbModel
                    {
                        TransferFlag = false
                    })
                    .Where(p => p.ID == ID && p.AuditStatus == 2 && p.CompanyId == currentUser.CompanyID && !p.DeleteFlag && p.TransferFlag == true)
                    .ExecuteCommandAsync() > 0;

                return result
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "终止失败，该数据可能已终止或已删除");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"终止失败{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
