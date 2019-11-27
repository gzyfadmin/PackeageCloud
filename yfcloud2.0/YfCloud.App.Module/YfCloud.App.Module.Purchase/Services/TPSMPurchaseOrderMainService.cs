///////////////////////////////////////////////////////////////////////////////////////
// File: ITPSMPurchaseOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:35:08
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
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderMain;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.UnitChange;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetailSum;

namespace YfCloud.App.Module.Purchase.Service
{
    /// <summary>
    /// ITPSMPurchaseOrderMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITPSMPurchaseOrderMainService))]
    public class TPSMPurchaseOrderMainService : ITPSMPurchaseOrderMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TPSMPurchaseOrderMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TPSMPurchaseOrderMainService(IDbContext dbContext, ILogger<TPSMPurchaseOrderMainService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_PSM_PurchaseOrderMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TPSMPurchaseOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TPSMPurchaseOrderMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel,
                    TSMUserAccountDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel,
                    TSMUserAccountDbModel, TMMPurchaseApplyMainDbModel>(
                        (t, t1, t2, t3, t4, t5, t6) => new object[]
                        {
                            JoinType.Left , t.BuyerId == t1.ID,
                            JoinType.Left , t.OrderTypeId == t2.ID,
                            JoinType.Left , t.SettlementTypeId == t3.ID,
                            JoinType.Left , t.AuditId == t4.ID,
                            JoinType.Left , t.OperatorId == t5.ID,
                            JoinType.Left , t.SourceId == t6.ID
                        });
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var lessSourceID = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "sourceid" && p.Condition == ConditionEnum.LessThan).FirstOrDefault();
                    if (lessSourceID != null)
                    {
                        requestObject.QueryConditions.Remove(lessSourceID);
                        query = query.Where((t, t1, t2, t3, t4, t5, t6) => SqlFunc.IsNull(t6.SourceId, 0) == 0);
                    }

                    if (requestObject.QueryConditions.Count > 0)
                    {
                        var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                        foreach (ConditionalModel item in conditionals)
                        {
                            if (item.FieldName.ToLower() == "suppliername")
                            {
                                item.FieldName = $"t0.{item.FieldName}";
                                continue;
                            }
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TPSMPurchaseOrderMainDbModel>(item.Column);
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
                        .Select((t, t1, t2, t3, t4, t5, t6) => new TPSMPurchaseOrderMainQueryModel
                        {
                            ID = t.ID,
                            BuyerId = t.BuyerId,
                            BuyerName = t1.AccountName,
                            OrderNo = t.OrderNo,
                            OrderDate = t.OrderDate,
                            OrderTypeId = t.OrderTypeId,
                            OrderTypeName = t2.DicValue,
                            SettlementTypeId = t.SettlementTypeId,
                            SettlementTypeName = t3.DicValue,
                            Currency = t.Currency,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t4.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t5.AccountName,
                            ContactName = t.ContactName,
                            ContactNumber = t.ContactNumber,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TransferStatus = t.TransferStatus,
                            PurchaseNum = t.PurchaseNum,
                            PurchaseAmount = t.PurchaseAmount,
                            AllowEdit = t.AuditStatus != 2 && t.OperatorId == currentUser.UserID,
                            SourceId = t.SourceId,
                            SourceNo = t6.PurchaseNo
                        })
                        .Where((t) => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Select((t, t1, t2, t3, t4, t5, t6) => new TPSMPurchaseOrderMainQueryModel
                        {
                            ID = t.ID,
                            BuyerId = t.BuyerId,
                            BuyerName = t1.AccountName,
                            OrderNo = t.OrderNo,
                            OrderDate = t.OrderDate,
                            OrderTypeId = t.OrderTypeId,
                            OrderTypeName = t2.DicValue,
                            SettlementTypeId = t.SettlementTypeId,
                            SettlementTypeName = t3.DicValue,
                            Currency = t.Currency,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t4.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t5.AccountName,
                            ContactName = t.ContactName,
                            ContactNumber = t.ContactNumber,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TransferStatus = t.TransferStatus,
                            PurchaseNum = t.PurchaseNum,
                            PurchaseAmount = t.PurchaseAmount,
                            SourceNo = t6.PurchaseNo,
                            SourceId = t.SourceId
                        })
                        .Where((t) => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TPSMPurchaseOrderMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TPSMPurchaseOrderMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_PSM_PurchaseOrderDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TPSMPurchaseOrderDetailQueryModel>>> GetDetailListAsync(int requestObject, CurrentUser currentUser)
        {
            return await GetDetialList(requestObject, currentUser);
        }

        private async Task<ResponseObject<List<TPSMPurchaseOrderDetailQueryModel>>> GetDetialList(int requestObject, CurrentUser currentUser)
        {
            try
            {
                //查询结果集对象
                List<TPSMPurchaseOrderDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;

                var query = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel,
                    TBMDictionaryDbModel, TPSMPurchaseOrderMainDbModel, TBMSupplierFileDbModel>(
                        (t, t0, t1, t2, t3, t4, t5, t6, t7) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.PurchaseUnitId == t4.ID,
                            JoinType.Left , t0.WarehouseUnitId == t5.ID,
                            JoinType.Inner,t.MainId==t6.ID,
                            JoinType.Left,t.SupplierId==t7.ID
                        });

                //执行查询
                queryData = await query
                    .Select((t, t0, t1, t2, t3, t4, t5, t6, t7) => new TPSMPurchaseOrderDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        SupplierId = t.SupplierId,
                        SupplierName = t7.SupplierName,
                        MaterialId = t.MaterialId,
                        MaterialName = t0.MaterialName,
                        MaterialCode = t0.MaterialCode,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        PurchaseUnitId = t0.PurchaseUnitId,
                        PurchaseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        PurchaseRate = t0.PurchaseRate,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t5.ID) ? t3.DicValue : t5.DicValue,
                        WarehouseRate = t0.WarehouseRate,
                        UnitPrice = t.UnitPrice,
                        PurchaseAmount = t.PurchaseAmount,
                        PurchaseNum = t.PurchaseNum,
                        DeliveryPeriod = t.DeliveryPeriod,
                        TransferNum = t.TransferNum,
                        Remark = t.Remark,
                        ProduceRate = t0.ProduceRate,
                        ProduceUnitId = t0.ProduceUnitId,
                        AuditStatus = t6.AuditStatus
                    })
                    .Where(t => t.MainId == requestObject)
                    .ToListAsync();


                var mainEntity = _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == requestObject).First();

                List<TBMMaterialFileCacheModel> mList = BasicCacheGet.GetMaterial(currentUser);

                //计算生产采购申请单转生产采购单数量
                var prodList = await _db.Instance.Queryable<TMMPurchaseApplyDetailDbModel, TMMPurchaseApplyMainDbModel>
                    ((t, t0) => new object[] { JoinType.Left, t.MainId == t0.ID })
                    .Where((t, t0) => t0.ID == SqlFunc.Subqueryable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == requestObject).Select(p => (int)p.SourceId))
                    .ToListAsync();

                queryData.ForEach(p =>
                {
                    var me = mList.Where(x => x.ID == p.MaterialId).FirstOrDefault();
                    if (mainEntity.SourceId > 0)
                    {
                        p.ProdTransNum = prodList.Where(p1 => p1.MaterialId == p.MaterialId).First().TransNum;
                    }

                    p.TransferWareNum = UnitChange.TranserUnit(me, UnitType.Purchase, UnitType.Warehouse, p.TransferNum);
                });

                //返回执行结果
                return ResponseUtil<List<TPSMPurchaseOrderDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TPSMPurchaseOrderDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> GetWholeMainData(int requestObject, CurrentUser currentUser)
        {
            try
            {

                RequestGet requestGet = new RequestGet()
                {
                    IsPaging = false,
                    QueryConditions = new List<QueryCondition>() {
                new QueryCondition(){ Column="ID", Condition=ConditionEnum.Equal, Content=requestObject.ToString() }}
                };

                var mainModel = await GetMainListAsync(requestGet, currentUser);

                TPSMPurchaseOrderMainQueryModel tPSMPurchaseOrderMainQueryModel = mainModel.Data.FirstOrDefault();

                var deatailModel = await GetDetialList(requestObject, currentUser);
                tPSMPurchaseOrderMainQueryModel.ChildList = deatailModel.Data;

                return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.SuccessResult(tPSMPurchaseOrderMainQueryModel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 查询采购单转入库单明细数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TPSMPurchaseOrderDetailQuerySumModel>>> GetTransferListAsync(int requestObject, CurrentUser current)
        {
            try
            {
                //查询结果集对象
                List<TPSMPurchaseOrderDetailQuerySumModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;

                //TPSMPurchaseOrderDetailSumDbModel
                var query = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel,
                    TBMDictionaryDbModel, TPSMPurchaseOrderMainDbModel>(
                        (t, t0, t1, t2, t3, t4, t5, t6) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.PurchaseUnitId == t4.ID,
                            JoinType.Left , t0.WarehouseUnitId == t5.ID,
                            JoinType.Inner,t.MainId==t6.ID,
                        });

                //执行查询
                queryData = await query
                    .Select((t, t0, t1, t2, t3, t4, t5, t6) => new TPSMPurchaseOrderDetailQuerySumModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialName = t0.MaterialName,
                        MaterialCode = t0.MaterialCode,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        PurchaseUnitId = t0.PurchaseUnitId,
                        PurchaseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        PurchaseRate = t0.PurchaseRate,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t5.ID) ? t3.DicValue : t5.DicValue,
                        WarehouseRate = t0.WarehouseRate,
                        UnitPrice = t.UnitPrice,
                        PurchaseAmount = t.PurchaseAmount,
                        PurchaseNum = t.PurchaseNum,
                        TransferNum = t.TransferNum
                    })
                    .Where(t => t.MainId == requestObject && t.TransferNum > 0)
                    .ToListAsync();
                return ResponseUtil<List<TPSMPurchaseOrderDetailQuerySumModel>>.SuccessResult(queryData);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<TPSMPurchaseOrderDetailQuerySumModel>>.FailResult(null,ex.Message);
            }
        }

        /// <summary>
        /// 新增T_PSM_PurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> PostAsync(RequestPost<TPSMPurchaseOrderMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TPSMPurchaseOrderMainDbModel>(requestObject.PostData);
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.PurchaseNum = requestObject.PostData.ChildList.Sum(p => p.PurchaseNum);
                mapMainModel.PurchaseAmount = requestObject.PostData.ChildList.Sum(p => p.PurchaseAmount);
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TPSMPurchaseOrderDetailAddModel>,
                    List<TPSMPurchaseOrderDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //如果是生产采购申请单转采购单，需要更新转单及数量信息
                if (mapMainModel.SourceId > 0)
                {
                    var transList = await GetTransDetailsAsync((int)mapMainModel.SourceId, mapDetailModelList, currentUser);
                    foreach (var item in transList)
                    {
                        await _db.Instance.Updateable<TMMPurchaseApplyDetailDbModel>()
                            .SetColumns(p => p.TransNum == p.TransNum - item.TransNum)
                            .Where(p => p.MainId == item.MainId && p.MaterialId == item.MaterialId)
                            .ExecuteCommandAsync();
                    }
                    FalseTMMPurchaseApplyMain(mapMainModel.SourceId.Value);
                }
                else
                {
                    var detailModelLists = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Where(p => p.MainId == requestObject.PostData.ID).ToList();

                    var groupEntity = from p in detailModelLists
                                      group p by new { p.MaterialId } into g
                                      select new { g.Key, MaxPrice = g.Max(p => p.UnitPrice), MinPrice = g.Min(p => p.UnitPrice) };

                    var differentEntity = groupEntity.Where(p => p.MaxPrice != p.MinPrice).ToList();

                    if (differentEntity.Count() > 0)
                    {
                        var meList = BasicCacheGet.GetMaterial(currentUser);

                        string error = string.Join(",", differentEntity.Select(p => meList.Where(p1 => p1.ID == p.Key.MaterialId).FirstOrDefault()?.MaterialName));
                        throw new Exception($"{error}商品的单价不一致，请保证相同商品价格一致性");
                    }
                }
                //提交事务
                currDb.CommitTran();
                TPSMPurchaseOrderMainQueryModel returnObject = null;
                if (result)
                {
                    var tempreturnObject = await GetWholeMainData(mainId, currentUser);
                    returnObject = tempreturnObject.Data;
                }
                //返回执行结果
                return result ? ResponseUtil<TPSMPurchaseOrderMainQueryModel>.SuccessResult(returnObject)
                    : ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        private void FalseTMMPurchaseApplyMain(int sourceID)
        {
            bool isunCompleted = _db.Instance.Queryable<TMMPurchaseApplyDetailDbModel>().Any(p => p.MainId == sourceID && p.TransNum > 0);

            if (isunCompleted)
            {
                _db.Instance.Updateable<TMMPurchaseApplyMainDbModel>()
                  .SetColumns(p => new TMMPurchaseApplyMainDbModel
                  {
                      TransferFlag = true
                  })
                  .Where(p => p.ID == sourceID).ExecuteCommand();
            }
            else
            {
                _db.Instance.Updateable<TMMPurchaseApplyMainDbModel>()
                 .SetColumns(p => new TMMPurchaseApplyMainDbModel
                 {
                     TransferFlag = false
                 })
                 .Where(p => p.ID == sourceID).ExecuteCommand();
            }
        }



        private async Task<List<TMMPurchaseApplyDetailDbModel>> GetTransDetailsAsync(int iMainId, List<TPSMPurchaseOrderDetailDbModel> details, CurrentUser currentUser)
        {
            var materialList = await _db.Instance.Queryable<TBMMaterialFileDbModel>()
                .Where(p => p.CompanyId == currentUser.CompanyID).ToListAsync();
            var result = new List<TMMPurchaseApplyDetailDbModel>();
            foreach (var item in details)
            {
                var materialModel = materialList.Where(p => p.ID == item.MaterialId).First();
                var insertModel = details.Where(p => p.MaterialId == item.MaterialId).First();
                var purchaseNum = GetPurchaseNum(materialModel, insertModel.PurchaseNum);
                result.Add(new TMMPurchaseApplyDetailDbModel
                {
                    MainId = iMainId,
                    MaterialId = item.MaterialId,
                    TransNum = purchaseNum
                });
            }
            return result;
        }


        private decimal GetPurchaseNum(TBMMaterialFileDbModel model, decimal num)
        {
            decimal baseNum = num;
            decimal purchaseNum = num;
            //采购单位数量 转生产单位数量
            if (model.PurchaseRate != null && model.PurchaseUnitId != null)
            {
                baseNum = num * (decimal)model.PurchaseRate;
            }

            if (model.ProduceRate != num && model.ProduceUnitId != null)
            {
                purchaseNum = baseNum / (decimal)model.ProduceRate;
            }
            return purchaseNum;
        }

        /// <summary>
        /// 查询主单及明细单信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <returns></returns>
        private async Task<TPSMPurchaseOrderMainQueryModel> GetMainQueryModel(int iMainId)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel,
                    TSMUserAccountDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel,
                    TSMUserAccountDbModel, TMMProductionOrderMainDbModel>(
                        (t, t1, t2, t3, t4, t5, t6) => new object[]
                        {
                            JoinType.Left , t.BuyerId == t1.ID,
                            JoinType.Left , t.OrderTypeId == t2.ID,
                            JoinType.Left , t.SettlementTypeId == t3.ID,
                            JoinType.Left , t.AuditId == t4.ID,
                            JoinType.Left , t.OperatorId == t5.ID,
                            JoinType.Left , t.SourceId == t6.ID
                        })
                    .Select((t, t1, t2, t3, t4, t5, t6) => new TPSMPurchaseOrderMainQueryModel
                    {
                        ID = t.ID,
                        BuyerId = t.BuyerId,
                        BuyerName = t1.AccountName,
                        OrderNo = t.OrderNo,
                        OrderDate = t.OrderDate,
                        OrderTypeId = t.OrderTypeId,
                        OrderTypeName = t2.DicValue,
                        SettlementTypeId = t.SettlementTypeId,
                        SettlementTypeName = t3.DicValue,
                        Currency = t.Currency,
                        AuditStatus = t.AuditStatus,
                        AuditId = t.AuditId,
                        AuditName = t4.AccountName,
                        AuditTime = t.AuditTime,
                        OperatorId = t.OperatorId,
                        OperatorName = t5.AccountName,
                        ContactName = t.ContactName,
                        ContactNumber = t.ContactNumber,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        TransferStatus = t.TransferStatus,
                        PurchaseNum = t.PurchaseNum,
                        PurchaseAmount = t.PurchaseAmount,
                        SourceId = t.SourceId,
                        SourceNo = t6.ProductionNo
                    })
                    .Where((t) => t.ID == iMainId)
                    .FirstAsync();

                if (mainModel == null) return null;

                var detailModels = await _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                        (t, t0, t1, t2, t3, t4, t5) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.PurchaseUnitId == t4.ID,
                            JoinType.Left , t0.WarehouseUnitId == t5.ID
                        })
                    .Select((t, t0, t1, t2, t3, t4, t5) => new TPSMPurchaseOrderDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialName = t0.MaterialName,
                        MaterialCode = t0.MaterialCode,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        PurchaseUnitId = t0.PurchaseUnitId,
                        PurchaseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        PurchaseRate = t0.PurchaseRate,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t5.ID) ? t3.DicValue : t5.DicValue,
                        WarehouseRate = t0.WarehouseRate,
                        UnitPrice = t.UnitPrice,
                        PurchaseAmount = t.PurchaseAmount,
                        PurchaseNum = t.PurchaseNum,
                        DeliveryPeriod = t.DeliveryPeriod,
                        TransferNum = t.TransferNum,
                        Remark = t.Remark
                    })
                    .Where((t) => t.MainId == iMainId)
                    .ToListAsync();

                mainModel.ChildList = detailModels;
                return mainModel;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 修改T_PSM_PurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> PutAsync(RequestPut<TPSMPurchaseOrderMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TPSMPurchaseOrderMainDbModel>(requestObject.PostData);
                mainModel.PurchaseNum = requestObject.PostData.ChildList.Sum(p => p.PurchaseNum);
                mainModel.PurchaseAmount = requestObject.PostData.ChildList.Sum(p => p.PurchaseAmount);
                var mainFlag = await currDb.Updateable(mainModel)
                    .UpdateColumns(p => new
                    {
                        p.PurchaseNum,
                        p.PurchaseAmount,
                        p.OrderDate,
                        p.OrderTypeId,
                        p.SettlementTypeId,
                        p.ContactName,
                        p.ContactNumber,
                        p.BuyerId,
                        p.Currency
                    })
                    .Where(p => (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2) && p.ID == mainModel.ID)
                    .ExecuteCommandAsync() > 0;
                //查询编辑前明细数据
                var beforEditDetailData = await _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>()
                    .Where(p => p.MainId == mainModel.ID)
                    .ToListAsync();

                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TPSMPurchaseOrderDetailEditModel>,
                    List<TPSMPurchaseOrderDetailDbModel>>(requestObject.PostData.ChildList);
                //未审核可转数据都是0
                detailModels.ForEach(p => p.TransferNum = 0);
                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;
                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();
                    detailFlag = currDb.Deleteable<TPSMPurchaseOrderDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                //如果有源单号
                if (mainModel.SourceId > 0)
                {
                    var transList = await GetTransDetailsAsync((int)mainModel.SourceId, detailModels, beforEditDetailData, currentUser);
                    foreach (var item in transList)
                    {
                        await _db.Instance.Updateable<TMMPurchaseApplyDetailDbModel>()
                            .SetColumns(p => p.TransNum == p.TransNum - item.TransNum)
                            .Where(p => p.MainId == item.MainId && p.MaterialId == item.MaterialId)
                            .ExecuteCommandAsync();
                    }

                    FalseTMMPurchaseApplyMain(mainModel.SourceId.Value);
                }
                else
                {
                    var detailModelLists = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Where(p => p.MainId == requestObject.PostData.ID).ToList();

                    var groupEntity = from p in detailModelLists
                                      group p by new { p.MaterialId } into g
                                      select new { g.Key, MaxPrice = g.Max(p => p.UnitPrice), MinPrice = g.Min(p => p.UnitPrice) };

                    var differentEntity = groupEntity.Where(p => p.MaxPrice != p.MinPrice).ToList();

                    if (differentEntity.Count() > 0)
                    {
                        var meList = BasicCacheGet.GetMaterial(currentUser);

                        string error = string.Join(",", differentEntity.Select(p => meList.Where(p1 => p1.ID == p.Key.MaterialId).FirstOrDefault()?.MaterialName));
                        throw new Exception($"{error}商品的单价不一致，请保证相同商品价格一致性");
                    }
                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                TPSMPurchaseOrderMainQueryModel returnObject = null;
                var tempreturnObject = await GetWholeMainData(mainModel.ID, currentUser);
                returnObject = tempreturnObject.Data;

                return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.SuccessResult(returnObject);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TPSMPurchaseOrderMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取编辑后数据
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="editList"></param>
        /// <param name="beforEditList"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        private async Task<List<TMMPurchaseApplyDetailDbModel>> GetTransDetailsAsync(int iMainId, List<TPSMPurchaseOrderDetailDbModel> editList,
            List<TPSMPurchaseOrderDetailDbModel> beforEditList, CurrentUser currentUser)
        {
            var materialList = await _db.Instance.Queryable<TBMMaterialFileDbModel>()
               .Where(p => p.CompanyId == currentUser.CompanyID).ToListAsync();
            var result = new List<TMMPurchaseApplyDetailDbModel>();
            foreach (var item in editList)
            {
                var old = beforEditList.Where(p => p.ID == item.ID).First();
                var materialModel = materialList.Where(p => p.ID == item.MaterialId).First();
                result.Add(new TMMPurchaseApplyDetailDbModel
                {
                    MainId = iMainId,
                    MaterialId = item.MaterialId,
                    TransNum = GetProduceNum(materialModel, (item.PurchaseNum - old.PurchaseNum))
                });
            }
            return result;
        }

        private decimal GetProduceNum(TBMMaterialFileDbModel model, decimal num)
        {
            decimal baseNum = num;
            decimal purchaseNum = num;
            //采购单位数量 转生产单位数量
            if (model.PurchaseRate != null && model.PurchaseUnitId != null)
            {
                baseNum = num * (decimal)model.PurchaseRate;
            }

            if (model.ProduceRate != num && model.ProduceUnitId != null)
            {
                purchaseNum = baseNum / (decimal)model.ProduceRate;
            }
            return purchaseNum;
        }

        /// <summary>
        /// 采购单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PurchaseAuditAsync(RequestPut<TPSMPurchaseOrderAuditModel> requestPut, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>()
                    .SetColumns(p => new TPSMPurchaseOrderMainDbModel
                    {
                        AuditId = currentUser.UserID,
                        AuditStatus = requestPut.PostData.AuditStatus,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestPut.PostData.ID && (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2) && !p.DeleteFlag)
                    .ExecuteCommandAsync() > 0;

                int MaindID = requestPut.PostData.ID;
                //审核通过后更新可转数量为销售数量
                if (result && requestPut.PostData.AuditStatus == 2)
                {
                    await _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>()
                        .SetColumns(p => p.TransferStatus == true)
                        .Where(p => p.ID == requestPut.PostData.ID)
                        .ExecuteCommandAsync();
                    await _db.Instance.Updateable<TPSMPurchaseOrderDetailDbModel>()
                         .SetColumns(p => p.TransferNum == p.PurchaseNum)
                         .Where(p => p.MainId == requestPut.PostData.ID)
                         .ExecuteCommandAsync();


                    //合并统计表
                    var TSSMSalesOrdersList = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Where(p => p.MainId == requestPut.PostData.ID).ToList();

                    var groupSum = TSSMSalesOrdersList.GroupBy(p => p.MaterialId).ToList();

                    List<TPSMPurchaseOrderDetailSumDbModel> sumList = new List<TPSMPurchaseOrderDetailSumDbModel>();
                    foreach (var item in groupSum)
                    {
                        var key = item.Key;
                        var itemDeatails = item.ToList();
                        var firstItem = itemDeatails.FirstOrDefault();

                        TPSMPurchaseOrderDetailSumDbModel sumDbEntity = new TPSMPurchaseOrderDetailSumDbModel();
                        sumDbEntity.MainId = MaindID;
                        sumDbEntity.MaterialId = key;
                        sumDbEntity.UnitPrice = firstItem.UnitPrice;
                        sumDbEntity.PurchaseNum = itemDeatails.Sum(p => p.PurchaseNum);
                        sumDbEntity.PurchaseAmount = itemDeatails.Sum(p => p.PurchaseAmount);
                        sumDbEntity.TransferNum= itemDeatails.Sum(p => p.TransferNum);
                        sumList.Add(sumDbEntity);
                    }

                    _db.Instance.Insertable(sumList).ExecuteCommand();
                }
                return result
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "该数据已审核或已删除");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 终止采购入库
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>()
                    .SetColumns(p => new TPSMPurchaseOrderMainDbModel
                    {
                        TransferStatus = false
                    })
                    .Where(p => p.ID == ID && p.AuditStatus == 2 && p.CompanyId == currentUser.CompanyID && !p.DeleteFlag && p.TransferStatus == true)
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

        /// <summary>
        /// 删除T_PSM_PurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <param name="currentUser"></param>
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
                    mainFlag = await _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                    DeleteTransNum(mainIds.ToArray(), currentUser);
                }
                else
                {
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
                    DeleteTransNum(new int[] { requestObject.PostData.ID }, currentUser);
                }


                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        private async void DeleteTransNum(int[] ids, CurrentUser currentUser)
        {
            //生产申请单可转数据回滚
            var sourceIdList = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>()
                .Where(p => ids.Contains(p.ID) && p.SourceId != null).ToListAsync();
            if (sourceIdList == null || sourceIdList.Count < 1)
                return;
            var materialList = await _db.Instance.Queryable<TBMMaterialFileDbModel>()
               .Where(p => p.CompanyId == currentUser.CompanyID).ToListAsync();
            var result = new List<TMMPurchaseApplyDetailDbModel>();
            foreach (var item in sourceIdList)
            {
                var detailList = await _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>()
                .Where(p => p.MainId == item.ID)
                .ToListAsync();
                foreach (var item2 in detailList)
                {
                    var materialModel = materialList.Where(p => p.ID == item2.MaterialId).First();
                    result.Add(new TMMPurchaseApplyDetailDbModel
                    {
                        MainId = (int)item.SourceId,
                        MaterialId = item2.MaterialId,
                        TransNum = GetProduceNum(materialModel, item2.PurchaseNum)
                    });
                }
            }
            foreach (var item in result)
            {
                await _db.Instance.Updateable<TMMPurchaseApplyDetailDbModel>()
                                .SetColumns(p => p.TransNum == p.TransNum + item.TransNum)
                                .Where(p => p.MainId == item.MainId && p.MaterialId == item.MaterialId)
                                .ExecuteCommandAsync();
            }

            foreach (var item in sourceIdList)
            {
                FalseTMMPurchaseApplyMain(item.SourceId.Value);
            }

        }
    }
}
