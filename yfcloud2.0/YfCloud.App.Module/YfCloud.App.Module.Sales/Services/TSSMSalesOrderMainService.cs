///////////////////////////////////////////////////////////////////////////////////////
// File: ITSSMSalesOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22 8:54:34
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
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderMain;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailSum;

namespace YfCloud.App.Module.Sales.Service
{
    /// <summary>
    /// ITSSMSalesOrderMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITSSMSalesOrderMainService))]
    public class TSSMSalesOrderMainService : ITSSMSalesOrderMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TSSMSalesOrderMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSSMSalesOrderMainService(IDbContext dbContext, ILogger<TSSMSalesOrderMainService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_SSM_SalesOrderMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TSSMSalesOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TSSMSalesOrderMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel, TSMUserAccountDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>
                    (
                        (t, t0, t1, t2, t3, t4, t5) => new object[]
                        {
                             JoinType.Left , t.CustomerId == t0.ID,
                             JoinType.Left , t.SalesmanId == t1.ID,
                             JoinType.Left , t.OrderTypeId == t2.ID,
                             JoinType.Left , t.SettlementTypeId == t3.ID,
                             JoinType.Left , t.AuditId == t4.ID,
                             JoinType.Left , t.OperatorId == t5.ID
                        }
                    ).Where((t, t0, t1, t2, t3, t4, t5) => t.DeleteFlag == false && SqlFunc.IsNull(t.IsMaterial, false) == false);
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    //只能查询主表数据，且主表别名必须是t
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "customername")
                        {
                            item.FieldName = $"t0.{item.FieldName}";
                            continue;
                        }
                        item.FieldName = $"t.{item.FieldName}";
                    }
                    query.Where(conditionals);
                }

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSSMSalesOrderMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query
                        .Select((t, t0, t1, t2, t3, t4, t5) => new TSSMSalesOrderMainQueryModel
                        {
                            ID = t.ID,
                            CustomerId = t.CustomerId,
                            CustomerName = t0.CustomerName,
                            SalesmanId = t.SalesmanId,
                            SalesmanName = t1.AccountName,
                            OrderNo = t.OrderNo,
                            OrderTypeId = t.OrderTypeId,
                            OrderTypeName = t2.DicValue,
                            SettlementTypeId = t.SettlementTypeId,
                            SettementTypeName = t3.DicValue,
                            Currency = t.Currency,
                            ReceiptAddress = t.ReceiptAddress,
                            OrderDate = t.OrderDate,
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
                            SalesAmount = t.SalesAmount,
                            SalesNum = t.SalesNum,
                            TransProdStatus = t.TransProdStatus
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Select((t, t0, t1, t2, t3, t4, t5) => new TSSMSalesOrderMainQueryModel
                        {
                            ID = t.ID,
                            CustomerId = t.CustomerId,
                            CustomerName = t0.CustomerName,
                            SalesmanId = t.SalesmanId,
                            SalesmanName = t1.AccountName,
                            OrderNo = t.OrderNo,
                            OrderTypeId = t.OrderTypeId,
                            OrderTypeName = t2.DicValue,
                            SettlementTypeId = t.SettlementTypeId,
                            SettementTypeName = t3.DicValue,
                            Currency = t.Currency,
                            ReceiptAddress = t.ReceiptAddress,
                            OrderDate = t.OrderDate,
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
                            SalesAmount = t.SalesAmount,
                            SalesNum = t.SalesNum,
                            TransProdStatus = t.TransProdStatus
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToListAsync();
                }

                //是否可编辑
                queryData.ForEach(p => p.AllowEdit = p.AuditStatus != 2 && p.OperatorId == currentUser.UserID);

                //返回执行结果
                return ResponseUtil<List<TSSMSalesOrderMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TSSMSalesOrderMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_SSM_SalesOrderDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TSSMSalesOrderDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            return await QueryDetailListAsync(requestObject);
        }

        private async Task<ResponseObject<List<TSSMSalesOrderDetailQueryModel>>> QueryDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TSSMSalesOrderDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>();

                //执行查询
                queryData = await _db.Instance.Queryable<TSSMSalesOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel,
                    TBMPackageDbModel, TMMColorSolutionMainDbModel, TBMDictionaryDbModel>
                    (
                        (t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.SalesUnitId == t4.ID,
                            JoinType.Left , t0.WarehouseUnitId == t5.ID,
                            JoinType.Left , t.PackageId == t6.ID,
                            JoinType.Left , t.ColorSolutionId == t7.ID,
                            JoinType.Left , t0.ProduceUnitId == t8.ID
                        }
                    )
                    .Where(t => t.MainId == requestObject)
                    .Select((t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new TSSMSalesOrderDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        SalesUnitId = t0.SalesUnitId,
                        SalesUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        SalesRate = t0.SalesRate,
                        GoodsCode = t.GoodsCode,
                        GoodsName = t.GoodsName,
                        UnitPrice = t.UnitPrice,
                        SalesNum = t.SalesNum,
                        SalesAmount = t.SalesAmount,
                        DeliveryPeriod = t.DeliveryPeriod,
                        TransferNum = t.TransferNum,
                        Remark = t.Remark,
                        WarehouseRate = t0.WarehouseRate,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t5.ID) ? t3.DicValue : t5.DicValue,
                        PackageId = t.PackageId,
                        PackageCode = t6.DicCode,
                        PackageName = t6.DicValue,
                        ColorSolutionId = t.ColorSolutionId,
                        ColorSolutionName = t7.SolutionCode,
                        TransProdNum = t.TransProdNum,
                        ProduceUnitId = t0.ProduceUnitId,
                        ProduceUnitName = SqlFunc.IsNullOrEmpty(t8.ID) ? t3.DicValue : t8.DicValue,
                        ProduceRate = t0.ProduceRate
                    })
                    .ToListAsync();

                //计算已发和待出库数量
                var dataList = await _db.Instance.Queryable<TWMSalesMainDbModel, TWMSalesDetailDbModel>(
                        (t, t0) => new object[]
                        {
                            JoinType.Left , t.ID == t0.MainId
                        })
                    .Where((t, t0) => t.SourceId == requestObject && SqlFunc.IsNull(t.DeleteFlag, false) == false)
                    .Select((t, t0) => new { t.ID, t.AuditStatus, t0.MaterialId, t0.ActualNum })
                    .ToListAsync();

                queryData.ForEach(p =>
                {
                    p.AlreadyNum = dataList.Where(p1 => p1.AuditStatus == 2 && p1.MaterialId == p.MaterialId).Sum(p2 => p2.ActualNum);
                    p.WaitNum = dataList.Where(p1 => (p1.AuditStatus == null || p1.AuditStatus != 2) && p1.MaterialId == p.MaterialId).Sum(p2 => p2.ActualNum);
                });

                //返回执行结果
                return ResponseUtil<List<TSSMSalesOrderDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TSSMSalesOrderDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }



        #region 获取可转单信息

        /// <summary>
        /// 可转单的信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        private async Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> QueryTranDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TSSMSalesOrderDetailSumQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                //var query = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>();

                //TSSMSalesOrderDetailSumDbModel
                //执行查询
                queryData = await _db.Instance.Queryable<TSSMSalesOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel,
                    TBMPackageDbModel, TMMColorSolutionMainDbModel, TBMDictionaryDbModel>
                    (
                        (t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.SalesUnitId == t4.ID,
                            JoinType.Left , t0.WarehouseUnitId == t5.ID,
                            JoinType.Left , t.PackageId == t6.ID,
                            JoinType.Left , t.ColorSolutionId == t7.ID,
                            JoinType.Left , t0.ProduceUnitId == t8.ID
                        }
                    )
                    .Where(t => t.MainId == requestObject)
                    .Select((t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new TSSMSalesOrderDetailSumQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        SalesUnitId = t0.SalesUnitId,
                        SalesUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        SalesRate = t0.SalesRate,
                        GoodsCode = t.GoodsCode,
                        GoodsName = t.GoodsName,
                        SalesNum = t.SalesNum,
                        UnitPrice=t.UnitPrice,
                        SalesAmount = t.SalesAmount,
                        TransferNum = t.TransferNum,
                        Remark = t.Remark,
                        WarehouseRate = t0.WarehouseRate,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t5.ID) ? t3.DicValue : t5.DicValue,
                        PackageId = t.PackageId,
                        PackageCode = t6.DicCode,
                        PackageName = t6.DicValue,
                        ColorSolutionId = t.ColorSolutionId,
                        ColorSolutionName = t7.SolutionCode,
                        TransProdNum = t.TransProdNum,
                        ProduceUnitId = t0.ProduceUnitId,
                        ProduceUnitName = SqlFunc.IsNullOrEmpty(t8.ID) ? t3.DicValue : t8.DicValue,
                        ProduceRate = t0.ProduceRate
                    })
                    .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TSSMSalesOrderDetailSumQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TSSMSalesOrderDetailSumQueryModel>>.FailResult(null, ex.Message);
            }
        }


        /// <summary>
        /// 查询可转出库单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetTransferList(int requestObject)
        {
            var detailList = await QueryTranDetailListAsync(requestObject);
            var result = detailList.Data.Where(p => p.TransferNum > 0).ToList();
            return ResponseUtil<List<TSSMSalesOrderDetailSumQueryModel>>.SuccessResult(result);
        }

        /// <summary>
        /// 查询可转生产单单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetToProduceList(int requestObject)
        {
            var detailList = await QueryTranDetailListAsync(requestObject);
            var result = detailList.Data.Where(p => p.TransProdNum > 0).ToList();
            return ResponseUtil<List<TSSMSalesOrderDetailSumQueryModel>>.SuccessResult(result);
        }

        #endregion

        /// <summary>
        /// 新增T_SSM_SalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TSSMSalesOrderMainQueryModel>> PostAsync(RequestPost<TSSMSalesOrderMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSSMSalesOrderMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TSSMSalesOrderMainDbModel>(requestObject.PostData);
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.SalesNum = requestObject.PostData.ChildList.Sum(p => p.SalesNum);
                mapMainModel.SalesAmount = requestObject.PostData.ChildList.Sum(p => p.SalesAmount);
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TSSMSalesOrderDetailAddModel>, List<TSSMSalesOrderDetailDbModel>>(requestObject.PostData.ChildList);

                var cache = BasicCacheGet.GetMaterial(currentUser);
                mapDetailModelList.ForEach(x =>
                {
                    TBMMaterialFileCacheModel tBMMaterialFileCacheModel = GetMaterialFileByPackageColor(cache, currentUser, x.PackageId, x.ColorSolutionId);
                    x.MaterialId = tBMMaterialFileCacheModel.ID;
                });

                var groupEntity = from p in mapDetailModelList
                                  group p by new { p.GoodsCode, p.GoodsName, p.MaterialId } into g
                                  select new{g.Key,MaxPrice = g.Max(p => p.UnitPrice),MinPrice = g.Min(p => p.UnitPrice)};

                var differentEntity = groupEntity.Where(p => p.MaxPrice != p.MinPrice).ToList();


                if (differentEntity.Count() > 0)
                {
                    string error = string.Join(",", differentEntity.Select(p => p.Key.GoodsCode));
                    throw new Exception($"{error}商品的单价不一致，请保证相同商品价格一致性");
                }

                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                TSSMSalesOrderMainQueryModel returnObject = null;
                if (result)
                    returnObject = await GetMainQueryModel(mainId);
                //返回执行结果
                return result
                    ? ResponseUtil<TSSMSalesOrderMainQueryModel>.SuccessResult(returnObject)
                    : ResponseUtil<TSSMSalesOrderMainQueryModel>.FailResult(null, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSSMSalesOrderMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取主单和明细单查询模型
        /// </summary>
        /// <param name="iMainId"></param>
        /// <returns></returns>
        private async Task<TSSMSalesOrderMainQueryModel> GetMainQueryModel(int iMainId)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel, TSMUserAccountDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>
                    (
                        (t, t0, t1, t2, t3, t4, t5) => new object[]
                        {
                             JoinType.Left , t.CustomerId == t0.ID,
                             JoinType.Left , t.SalesmanId == t1.ID,
                             JoinType.Left , t.OrderTypeId == t2.ID,
                             JoinType.Left , t.SettlementTypeId == t3.ID,
                             JoinType.Left , t.AuditId == t4.ID,
                             JoinType.Left , t.OperatorId == t5.ID
                        }
                    )
                    .Where(t => t.ID == iMainId)
                    .Select((t, t0, t1, t2, t3, t4, t5) => new TSSMSalesOrderMainQueryModel
                    {
                        ID = t.ID,
                        CustomerId = t.CustomerId,
                        CustomerName = t0.CustomerName,
                        SalesmanId = t.SalesmanId,
                        SalesmanName = t1.AccountName,
                        OrderNo = t.OrderNo,
                        OrderTypeId = t.OrderTypeId,
                        OrderTypeName = t2.DicValue,
                        SettlementTypeId = t.SettlementTypeId,
                        SettementTypeName = t3.DicValue,
                        Currency = t.Currency,
                        ReceiptAddress = t.ReceiptAddress,
                        OrderDate = t.OrderDate,
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
                        SalesAmount = t.SalesAmount,
                        SalesNum = t.SalesNum
                    })
                    .FirstAsync();

                if (mainModel == null) return mainModel;

                var detailModels = await _db.Instance.Queryable<TSSMSalesOrderDetailDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>
                    (
                        (t, t0, t1, t2, t3, t4) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.SalesUnitId == t4.ID
                        }
                    )
                    .Where(t => t.MainId == iMainId)
                    .Select((t, t0, t1, t2, t3, t4) => new TSSMSalesOrderDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        SalesUnitId = t0.SalesUnitId,
                        SalesUnitName = t4.DicValue,
                        SalesRate = t0.SalesRate,
                        GoodsCode = t.GoodsCode,
                        GoodsName = t.GoodsName,
                        UnitPrice = t.UnitPrice,
                        SalesNum = t.SalesNum,
                        SalesAmount = t.SalesAmount,
                        DeliveryPeriod = t.DeliveryPeriod,
                        TransferNum = t.TransferNum,
                        Remark = t.Remark
                    })
                    .ToListAsync();

                mainModel.ChildList = detailModels;
                mainModel.AllowEdit = true;
                return mainModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 修改T_SSM_SalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TSSMSalesOrderMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<bool>.FailResult(false, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TSSMSalesOrderMainDbModel>(requestObject.PostData);
                mainModel.SalesNum = requestObject.PostData.ChildList.Sum(p => p.SalesNum);
                mainModel.SalesAmount = requestObject.PostData.ChildList.Sum(p => p.SalesAmount);
                var mainFlag = await currDb.Updateable(mainModel)
                    .UpdateColumns(p => new
                    {
                        p.CustomerId,
                        p.SalesmanId,
                        p.OrderTypeId,
                        p.SettlementTypeId,
                        p.ReceiptAddress,
                        p.OrderDate,
                        p.ContactName,
                        p.ContactNumber,
                        p.SalesAmount,
                        p.SalesNum
                    })
                    .Where(p => (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2) && p.ID == mainModel.ID)
                    .ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TSSMSalesOrderDetailEditModel>,
                    List<TSSMSalesOrderDetailDbModel>>(requestObject.PostData.ChildList);

                var cache = BasicCacheGet.GetMaterial(currentUser);
                detailModels.ForEach(x =>
                {
                    TBMMaterialFileCacheModel tBMMaterialFileCacheModel = GetMaterialFileByPackageColor(cache, currentUser, x.PackageId, x.ColorSolutionId);
                    x.MaterialId = tBMMaterialFileCacheModel.ID;
                });

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
                    detailFlag = currDb.Deleteable<TSSMSalesOrderDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                var detailModelLists = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Where(p => p.MainId == requestObject.PostData.ID).ToList();

                var groupEntity = from p in detailModelLists
                                  group p by new { p.GoodsCode, p.GoodsName, p.MaterialId } into g
                                  select new { g.Key, MaxPrice = g.Max(p => p.UnitPrice), MinPrice = g.Min(p => p.UnitPrice) };

                var differentEntity = groupEntity.Where(p => p.MaxPrice != p.MinPrice).ToList();


                if (differentEntity.Count() > 0)
                {
                    string error = string.Join(",", differentEntity.Select(p => p.Key.GoodsCode));
                    throw new Exception($"{error}商品的单价不一致，请保证相同商品价格一致性");
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
        /// 审核销售订单
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TSSMSalesOrderMainAuditModel> requestPut, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                    .SetColumns(p => new TSSMSalesOrderMainDbModel
                    {
                        AuditStatus = requestPut.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestPut.PostData.ID && (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2) && !p.DeleteFlag)
                    .ExecuteCommandAsync() > 0;

                int MaindID = requestPut.PostData.ID;

                //审核通过后更新可转数量为销售数量
                if (result && requestPut.PostData.AuditStatus == 2)
                {
                    await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                        .SetColumns(p => new TSSMSalesOrderMainDbModel { TransferStatus = true, TransProdStatus = true })
                        .Where(p => p.ID == MaindID)
                        .ExecuteCommandAsync();
                    await _db.Instance.Updateable<TSSMSalesOrderDetailDbModel>()
                         .SetColumns(p => new TSSMSalesOrderDetailDbModel { TransferNum = p.SalesNum, TransProdNum = p.SalesNum })
                         .Where(p => p.MainId == MaindID)
                         .ExecuteCommandAsync();

                    //合并统计表
                    var TSSMSalesOrdersList = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Where(p => p.MainId == requestPut.PostData.ID).ToList();

                    var groupSum = TSSMSalesOrdersList.GroupBy(p => p.MaterialId).ToList();

                    List<TSSMSalesOrderDetailSumDbModel> sumList = new List<TSSMSalesOrderDetailSumDbModel>();
                    foreach (var item in groupSum)
                    {
                        var key = item.Key;
                        var itemDeatails = item.ToList();
                        var firstItem = itemDeatails.FirstOrDefault();

                        TSSMSalesOrderDetailSumDbModel sumDbEntity = new TSSMSalesOrderDetailSumDbModel();
                        sumDbEntity.MainId = MaindID;
                        sumDbEntity.ColorSolutionId = firstItem.ColorSolutionId;
                        sumDbEntity.MaterialId = key;
                        sumDbEntity.PackageId = firstItem.PackageId;
                        sumDbEntity.GoodsCode = firstItem.GoodsCode;
                        sumDbEntity.GoodsName = firstItem.GoodsName;
                        sumDbEntity.UnitPrice = firstItem.UnitPrice;
                        sumDbEntity.SalesNum = itemDeatails.Sum(p => p.SalesNum);
                        sumDbEntity.SalesAmount = itemDeatails.Sum(p => p.SalesAmount);
                        sumDbEntity.TransferNum = itemDeatails.Sum(p => p.TransferNum);
                        sumDbEntity.TransProdNum = itemDeatails.Sum(p => p.TransProdNum);

                        sumList.Add(sumDbEntity);
                    }

                    _db.Instance.Insertable(sumList).ExecuteCommand();
                }

                return result
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "审核数据失败，改数据可能已审核或已删除");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 终止出库
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopDeposit(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                    .SetColumns(p => new TSSMSalesOrderMainDbModel
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
        /// 终止生产
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopProduct(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                    .SetColumns(p => new TSSMSalesOrderMainDbModel
                    {
                        TransProdStatus = false
                    })
                    .Where(p => p.ID == ID && p.AuditStatus == 2 && p.CompanyId == currentUser.CompanyID && !p.DeleteFlag && p.TransProdStatus == true)
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
        /// 删除T_SSM_SalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject)
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
                    mainFlag = await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
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

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="PackageID"></param>
        /// <param name="ColorSolutionID"></param>
        /// <returns></returns>
        public TBMMaterialFileCacheModel GetMaterialFileByPackageColor(CurrentUser currentUser, int PackageID, int? ColorSolutionID)
        {
            var cache = BasicCacheGet.GetMaterial(currentUser);

            return GetMaterialFileByPackageColor(cache, currentUser, PackageID, ColorSolutionID);
        }

        private TBMMaterialFileCacheModel GetMaterialFileByPackageColor(List<TBMMaterialFileCacheModel> cache, CurrentUser currentUser, int PackageID, int? ColorSolutionID)
        {
            if (ColorSolutionID != null)
            {
                var resutl = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TBMMaterialFileDbModel>((t, t1) => new object[] {
                JoinType.Inner,t.ID==t1.ColorSolutionID && t.PackageId==t1.PackageID
            }).Where((t, t1) => t.PackageId == PackageID && t.ID == ColorSolutionID).Select((t, t1) => t1).First();

                return cache.Where(p => p.ID == resutl.ID).FirstOrDefault();
            }
            else
            {
                var resutl = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(t => t.PackageID == PackageID && SqlFunc.IsNullOrEmpty(t.ColorSolutionID)).Select(t => t);

                var entity = resutl.First();

                return cache.Where(p => p.ID == entity.ID).FirstOrDefault();
            }
        }
    }
}
