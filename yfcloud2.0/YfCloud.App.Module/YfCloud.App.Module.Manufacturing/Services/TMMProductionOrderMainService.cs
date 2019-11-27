///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMProductionOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5 13:41:17
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
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderMain;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetailSum;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Services;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetailNew;
using System.Data;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMProductionOrderMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITMMProductionOrderMainService))]
    public class TMMProductionOrderMainService : ITMMProductionOrderMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMProductionOrderMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly ITMMColorSolutionMainService _colorService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMProductionOrderMainService(IDbContext dbContext, ILogger<TMMProductionOrderMainService> logger, IMapper mapper, ITMMColorSolutionMainService colorService )
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _colorService = colorService;
        }

        /// <summary>
        /// 获取T_MM_ProductionOrderMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMProductionOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMProductionOrderMainQueryModel> queryData = null;//查询结果集对象
                int totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMProductionOrderMainDbModel, TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2, t3) => new object[]
                        {
                            JoinType.Left , t.SourceId == t0.ID,
                            JoinType.Left , t.CustomerId == t1.ID,
                            JoinType.Left , t.AuditId == t2.ID,
                            JoinType.Left , t.OperatorId == t3.ID
                        }
                    );

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "customername")
                        {
                            item.FieldName = $"t1.{item.FieldName}";
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMProductionOrderMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }


                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = query
                        .Select((t, t0, t1, t2, t3) => new TMMProductionOrderMainQueryModel
                        {
                            ID = t.ID,
                            ProductionType = t.ProductionType,
                            ProductionNo = t.ProductionNo,
                            SourceId = t.SourceId,
                            SourceNo = t0.OrderNo,
                            CustomerId = t.CustomerId,
                            CustomerName = t1.CustomerName,
                            OrderDate = t.OrderDate,
                            BeginDate = t.BeginDate,
                            EndDate = t.EndDate,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t3.AccountName,
                            OperatorTime = t.OperatorTime,
                            MRPStatus = t.MRPStatus,
                            MRPTime = t.MRPTime,
                            ProductionStatus = t.ProductionStatus,
                            TransferFlag = t.TransferFlag,
                            IsPurchase = t.IsPurchase,
                            IsPick = t.IsPick,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag
                        }).Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag).Mapper((itemModel, cache) =>
                        {
                            var allItems = cache.Get(orderList =>
                            {
                                var allIds = orderList.Select(it => it.ID).ToList();
                                return new
                                {
                                    a1 = _db.Instance.Queryable<TMMPickApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                    a2 = _db.Instance.Queryable<TMMPurchaseApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                    a3 = _db.Instance.Queryable<TMMWhApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                };//Execute only once
                            });

                            itemModel.PickModel = allItems.a1.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.PickNo }).ToList();//Every time it's executed
                            itemModel.Purchase = allItems.a2.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.PurchaseNo }).ToList();//Eve

                            itemModel.WhApplyMain = allItems.a3.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.WhApplyNo }).ToList();//Eve


                        }).ToPageList(requestObject.PageIndex, requestObject.PageSize, ref totalNumber);
                }
                else
                {
                    queryData = await query
                        .Select((t, t0, t1, t2, t3) => new TMMProductionOrderMainQueryModel
                        {
                            ID = t.ID,
                            ProductionType = t.ProductionType,
                            ProductionNo = t.ProductionNo,
                            SourceId = t.SourceId,
                            SourceNo = t0.OrderNo,
                            CustomerId = t.CustomerId,
                            CustomerName = t1.CustomerName,
                            OrderDate = t.OrderDate,
                            BeginDate = t.BeginDate,
                            EndDate = t.EndDate,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t3.AccountName,
                            OperatorTime = t.OperatorTime,
                            MRPStatus = t.MRPStatus,
                            MRPTime = t.MRPTime,
                            ProductionStatus = t.ProductionStatus,
                            TransferFlag = t.TransferFlag,
                            IsPurchase = t.IsPurchase,
                            IsPick = t.IsPick,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag).Mapper((itemModel, cache) =>
                        {
                            var allItems = cache.Get(orderList =>
                            {
                                var allIds = orderList.Select(it => it.ID).ToList();
                                return new
                                {
                                    a1 = _db.Instance.Queryable<TMMPickApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                    a2 = _db.Instance.Queryable<TMMPurchaseApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                    a3 = _db.Instance.Queryable<TMMWhApplyMainDbModel>().Where(it => it.DeleteFlag == false && allIds.Contains(it.SourceId.Value)).ToList(),
                                };//Execute only once
                            });

                            itemModel.PickModel = allItems.a1.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.PickNo }).ToList();//Every time it's executed
                            itemModel.Purchase = allItems.a2.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.PurchaseNo }).ToList();//Eve

                            itemModel.WhApplyMain = allItems.a3.Where(it => it.SourceId == itemModel.ID).Select(it => new OrderIDenEntity() { ID = it.ID, NO = it.WhApplyNo }).ToList();//Eve


                        })
                        .ToListAsync();
                }

                //设置允许编辑的值
                queryData.ForEach(p => p.AllowEdit = p.OperatorId == currentUser.UserID && p.AuditId != 2);

                //返回执行结果
                return ResponseUtil<List<TMMProductionOrderMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMProductionOrderMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_MM_ProductionOrderDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMProductionOrderDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TMMProductionOrderDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TBMPackageDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TMMColorSolutionMainDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel, TBMDictionaryDbModel>(
                        (t, t0, t1, t2, t3, t4, t5, t6, t7) => new object[]
                        {
                            JoinType.Left , t.PackageId == t0.ID,
                            JoinType.Left , t.MaterialId == t1.ID,
                            JoinType.Left , t1.BaseUnitId == t2.ID,
                            JoinType.Left , t1.ProduceUnitId == t3.ID,
                            JoinType.Left , t.ColorSolutionId == t4.ID,
                            JoinType.Left , t.WorkshopId == t5.ID,
                            JoinType.Left , t.PrincipalId == t6.ID,
                            JoinType.Left,t1.ColorId==t7.ID
                        }
                    );

                //执行查询
                queryData = await query.Select((t, t0, t1, t2, t3, t4, t5, t6, t7) => new TMMProductionOrderDetailQueryModel
                {
                    ID = t.ID,
                    MainId = t.MainId,
                    PackageId = t.PackageId,
                    PackageCode = t0.DicCode,
                    PackageName = t0.DicValue,
                    MaterialId = t.MaterialId,
                    MaterialName = t1.MaterialName,
                    MaterialCode = t1.MaterialCode,
                    Spec = t1.Spec,
                    BaseUnitId = t1.BaseUnitId,
                    BaseUnitName = t2.DicValue,
                    ProduceUnitId = t1.ProduceUnitId,
                    ProduceRate = t1.ProduceRate,
                    ProduceUnitName = SqlFunc.IsNullOrEmpty(t3.ID) ? t2.DicValue : t3.DicValue,
                    ColorSolutionId = t.ColorSolutionId,
                    ColorSolutionName = t4.SolutionCode,
                    WorkshopId = t.WorkshopId,
                    WorkshopName = t5.DicValue,
                    PrincipalId = t.PrincipalId,
                    PrincipalName = t6.AccountName,
                    DeliveryPeriod = t.DeliveryPeriod,
                    GoodsCode = t.GoodsCode,
                    GoodsName = t.GoodsName,
                    ProductionNum = t.ProductionNum,
                    WarehouseRate = t1.WarehouseRate,
                    WarehouseUnitId = t1.WarehouseUnitId,
                    ColorName = t7.DicValue
                })
                                       .Where(t => t.MainId == requestObject)
                                       .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TMMProductionOrderDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMProductionOrderDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        #region 选择生产订单

        public async Task<ResponseObject<List<TMMProductionOrderMainQuerySumModel>>> GetSelectMain(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMProductionOrderMainQuerySumModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMProductionOrderMainDbModel, TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel, TMMWhApplyMainDbModel>(
                        (t, t0, t1, t2, t3, t4) => new object[]
                        {
                            JoinType.Left , t.SourceId == t0.ID,
                            JoinType.Left , t.CustomerId == t1.ID,
                            JoinType.Left , t.AuditId == t2.ID,
                            JoinType.Left , t.OperatorId == t3.ID,
                            JoinType.Inner,t.ID==t4.SourceId
                        }
                    );

                string[] productQuery = { "productionno", "orderdate" };

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "customername")
                        {
                            item.FieldName = $"t1.{item.FieldName}";
                            continue;
                        }
                        else if (productQuery.Contains(item.FieldName.ToLower()))
                        {
                            item.FieldName = $"t.{item.FieldName}";
                            continue;
                        }

                        if (item.FieldName.ToLower() == "transourceid")
                        {
                            item.FieldName = $"t4.ID";
                            continue;

                        }

                        item.FieldName = $"t4.{item.FieldName}";
                    }
                    query.Where(conditionals);
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        if (item.Condition.ToLower() != "asc" && item.Condition.ToLower() != "desc") continue;

                        if (item.Column.ToLower() == "sourcecode")
                        {
                            item.Column = "OrderNo";
                            var exp = SqlSugarUtil.GetOrderByLambda<TSSMSalesOrderMainDbModel>(item.Column);
                            if (exp == null) continue;

                            item.Column = $"t0.{item.Column}";
                        }
                        else if (item.Column.ToLower() == "transourcecode")
                        {
                            item.Column = "WhApplyNo";

                            var exp = SqlSugarUtil.GetOrderByLambda<TMMWhApplyMainDbModel>(item.Column);
                            if (exp == null) continue;

                            item.Column = $"t4.{item.Column}";
                        }
                        else
                        {
                            var exp = SqlSugarUtil.GetOrderByLambda<TMMProductionOrderMainDbModel>(item.Column);
                            if (exp == null) continue;
                        }

                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query
                        .Select((t, t0, t1, t2, t3, t4) => new TMMProductionOrderMainQuerySumModel
                        {
                            ID = t.ID,
                            ProductionType = t.ProductionType,
                            ProductionNo = t.ProductionNo,
                            SourceId = t.SourceId,
                            SourceNo = t0.OrderNo,
                            CustomerId = t.CustomerId,
                            CustomerName = t1.CustomerName,
                            OrderDate = t.OrderDate,
                            BeginDate = t.BeginDate,
                            EndDate = t.EndDate,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t3.AccountName,
                            OperatorTime = t.OperatorTime,
                            MRPStatus = t.MRPStatus,
                            MRPTime = t.MRPTime,
                            ProductionStatus = t.ProductionStatus,
                            TransferFlag = t.TransferFlag,
                            IsPurchase = t.IsPurchase,
                            IsPick = t.IsPick,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TranSourceId = t4.ID,
                            TranSourceCode = t4.WhApplyNo,
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Select((t, t0, t1, t2, t3, t4) => new TMMProductionOrderMainQuerySumModel
                        {
                            ID = t.ID,
                            ProductionType = t.ProductionType,
                            ProductionNo = t.ProductionNo,
                            SourceId = t.SourceId,
                            SourceNo = t0.OrderNo,
                            CustomerId = t.CustomerId,
                            CustomerName = t1.CustomerName,
                            OrderDate = t.OrderDate,
                            BeginDate = t.BeginDate,
                            EndDate = t.EndDate,
                            AuditStatus = t.AuditStatus,
                            AuditId = t.AuditId,
                            AuditName = t2.AccountName,
                            AuditTime = t.AuditTime,
                            OperatorId = t.OperatorId,
                            OperatorName = t3.AccountName,
                            OperatorTime = t.OperatorTime,
                            MRPStatus = t.MRPStatus,
                            MRPTime = t.MRPTime,
                            ProductionStatus = t.ProductionStatus,
                            TransferFlag = t.TransferFlag,
                            IsPurchase = t.IsPurchase,
                            IsPick = t.IsPick,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                            TranSourceId = t4.ID,
                            TranSourceCode = t4.WhApplyNo,
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToListAsync();
                }

                //设置允许编辑的值
                queryData.ForEach(p => p.AllowEdit = p.OperatorId == currentUser.UserID && p.AuditId != 2);

                //返回执行结果
                return ResponseUtil<List<TMMProductionOrderMainQuerySumModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMProductionOrderMainQuerySumModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 生产订单明细
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject<List<TMMProductionOrderMainQuerySumModel>>> GetOrderDeatailSum(RequestGet requestGet, bool isTakeTop, CurrentUser currentUser)
        {
            try
            {
                var mainList = GetSelectMain(requestGet, currentUser).Result;
                var mainIds = mainList.Data.Select(p => p.ID);
                if (mainIds == null || mainIds.Count() < 1)
                    return ResponseUtil<List<TMMProductionOrderMainQuerySumModel>>.SuccessResult(mainList.Data);
                foreach (var item in mainList.Data)
                {
                    var childQuery = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TBMPackageDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TMMColorSolutionMainDbModel, TMMWhApplyMainDbModel, TMMWhApplyDetailDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2, t3, t3_1, t4, t7, t8, t9, t10, t11) => new object[]
                        {
                            JoinType.Left , t.PackageId == t0.ID,
                            JoinType.Left , t.MaterialId == t1.ID,
                            JoinType.Left , t1.BaseUnitId == t2.ID,
                            JoinType.Left , t1.ProduceUnitId == t3.ID,
                            JoinType.Left,t1.WarehouseUnitId==t3_1.ID,
                            JoinType.Left , t.ColorSolutionId == t4.ID,
                            JoinType.Inner,t7.SourceId==t.MainId,
                            JoinType.Inner,t7.ID==t8.MainId &&t.ID==t8.ProOrderDetailId,
                            JoinType.Left,t1.ColorId == t9.ID,
                            JoinType.Left,t.WorkshopId==t10.ID,
                            JoinType.Left,t.PrincipalId==t11.ID
                        }
                    ).Where((t, t0, t1, t2, t3, t3_1, t4, t7, t8, t9, t10, t11) => t.MainId == item.ID && t8.TransNum > 0)
                    .Select((t, t0, t1, t2, t3, t3_1, t4, t7, t8, t9, t10, t11) => new TMMProductionOrderDetailQuerySum
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        PackageId = t.PackageId,
                        PackageCode = t0.DicCode,
                        PackageName = t0.DicValue,
                        MaterialId = t.MaterialId,
                        MaterialName = t1.MaterialName,
                        MaterialCode = t1.MaterialCode,
                        Spec = t1.Spec,
                        BaseUnitId = t1.BaseUnitId,
                        BaseUnitName = t2.DicValue,
                        WarehouseUnitId = t1.WarehouseUnitId,
                        WarehouseRate = t1.WarehouseRate,
                        WarehouseName = SqlFunc.IsNullOrEmpty(t3_1.ID) ? t2.DicValue : t3_1.DicValue,
                        ProduceUnitId = t1.ProduceUnitId,
                        ProduceRate = t1.ProduceRate,
                        ProduceUnitName = SqlFunc.IsNullOrEmpty(t3.ID) ? t2.DicValue : t3.DicValue,
                        ColorSolutionId = t.ColorSolutionId,
                        ColorSolutionName = t4.SolutionCode,
                        GoodsCode = t.GoodsCode,
                        GoodsName = t.GoodsName,
                        ProductionNum = t.ProductionNum,
                        TransNum = t8.TransNum,
                        ColorName = t9.DicValue,
                        WorkshopId = t.WorkshopId,
                        WorkshopName = t10.DicValue,
                        PrincipalId = t.PrincipalId,
                        PrincipalName = t11.AccountName,
                        ProOrderDetailId = t8.ID

                    })
                   ;

                    if (isTakeTop)
                    {
                        item.ChildList = await childQuery.Take(2).ToListAsync();
                    }
                    else
                    {
                        item.ChildList = await childQuery.ToListAsync();
                        item.ChildList = item.ChildList.Where(p => p.TransNum > 0).ToList();
                    }



                }

                return ResponseUtil<List<TMMProductionOrderMainQuerySumModel>>.SuccessResult(mainList.Data, mainList.TotalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMProductionOrderMainQuerySumModel>>.FailResult(null, $"查询数据发生异常{Environment.NewLine}{ex.Message}");
            }

        }


        #endregion



        /// <summary>
        /// 新增T_MM_ProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TMMProductionOrderMainQueryModel>> PostAsync(RequestPost<TMMProductionOrderMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TMMProductionOrderMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TMMProductionOrderMainDbModel>(requestObject.PostData);
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.OperatorTime = DateTime.Now;
                mapMainModel.CompanyId = currentUser.CompanyID;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TMMProductionOrderDetailAddModel>, List<TMMProductionOrderDetailDbModel>>(requestObject.PostData.ChildList);

                var cache = BasicCacheGet.GetMaterial(currentUser);

                mapDetailModelList.ForEach((x) =>
                {
                    var me = GetMaterialFileByPackageColor(cache, currentUser, x.PackageId, x.ColorSolutionId);
                    x.MaterialId = me.ID;
                });

                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;

                //如果有关联到销售订单则更新销售单的转单状态
                if (mapMainModel.SourceId != null && mapMainModel.SourceId > 0)
                {
                    //检查所有明细是否转完，如果全部转完则不可再转单
                    await _db.Instance.Updateable<TSSMSalesOrderMainDbModel>()
                        .SetColumns(p => p.TransProdStatus == false)
                        .Where(p => p.ID == mapMainModel.SourceId)
                        .ExecuteCommandAsync();
                }

                //提交事务
                currDb.CommitTran();
                TMMProductionOrderMainQueryModel returnObject = null;
                if (result)
                    returnObject = await GetMainQueryModel(mainId);
                //返回执行结果
                return result
                    ? ResponseUtil<TMMProductionOrderMainQueryModel>.SuccessResult(returnObject)
                    : ResponseUtil<TMMProductionOrderMainQueryModel>.FailResult(null, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TMMProductionOrderMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        private async Task<TMMProductionOrderMainQueryModel> GetMainQueryModel(int iMainId)
        {
            var mainModel = await _db.Instance.Queryable<TMMProductionOrderMainDbModel, TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2, t3) => new object[]
                        {
                            JoinType.Left , t.SourceId == t0.ID,
                            JoinType.Left , t.CustomerId == t1.ID,
                            JoinType.Left , t.AuditId == t2.ID,
                            JoinType.Left , t.OperatorId == t3.ID
                        })
                    .Select((t, t0, t1, t2, t3) => new TMMProductionOrderMainQueryModel
                    {
                        ID = t.ID,
                        ProductionType = t.ProductionType,
                        ProductionNo = t.ProductionNo,
                        SourceId = t.SourceId,
                        SourceNo = t0.OrderNo,
                        CustomerId = t.CustomerId,
                        CustomerName = t1.CustomerName,
                        OrderDate = t.OrderDate,
                        BeginDate = t.BeginDate,
                        EndDate = t.EndDate,
                        AuditStatus = t.AuditStatus,
                        AuditId = t.AuditId,
                        AuditName = t2.AccountName,
                        AuditTime = t.AuditTime,
                        OperatorId = t.OperatorId,
                        OperatorName = t3.AccountName,
                        OperatorTime = t.OperatorTime,
                        MRPStatus = t.MRPStatus,
                        MRPTime = t.MRPTime,
                        ProductionStatus = t.ProductionStatus,
                        TransferFlag = t.TransferFlag,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                    })
                    .Where(t => t.ID == iMainId)
                    .FirstAsync();
            var detialList = await _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TBMPackageDbModel, TBMMaterialFileDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TMMColorSolutionMainDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2, t3, t4, t5, t6) => new object[]
                        {
                            JoinType.Left , t.PackageId == t0.ID,
                            JoinType.Left , t.MaterialId == t1.ID,
                            JoinType.Left , t1.BaseUnitId == t2.ID,
                            JoinType.Left , t1.ProduceUnitId == t3.ID,
                            JoinType.Left , t.ColorSolutionId == t4.ID,
                            JoinType.Left , t.WorkshopId == t5.ID,
                            JoinType.Left , t.PrincipalId == t6.ID
                        })
                    .Select((t, t0, t1, t2, t3, t4, t5, t6) => new TMMProductionOrderDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        PackageId = t.PackageId,
                        PackageCode = t0.DicCode,
                        PackageName = t0.DicValue,
                        MaterialId = t.MaterialId,
                        MaterialName = t1.MaterialName,
                        MaterialCode = t1.MaterialCode,
                        Spec = t1.Spec,
                        BaseUnitId = t1.BaseUnitId,
                        BaseUnitName = t2.DicValue,
                        ProduceUnitId = t1.ProduceUnitId,
                        ProduceRate = t1.ProduceRate,
                        ProduceUnitName = SqlFunc.IsNullOrEmpty(t3.ID) ? t2.DicValue : t3.DicValue,
                        ColorSolutionId = t.ColorSolutionId,
                        ColorSolutionName = t4.SolutionCode,
                        WorkshopId = t.WorkshopId,
                        WorkshopName = t5.DicValue,
                        PrincipalId = t.PrincipalId,
                        PrincipalName = t6.AccountName,
                        DeliveryPeriod = t.DeliveryPeriod,
                        GoodsCode = t.GoodsCode,
                        GoodsName = t.GoodsName,
                        ProductionNum = t.ProductionNum,
                    })
                    .Where(t => t.MainId == iMainId)
                    .ToListAsync();
            mainModel.ChildList = detialList;
            return mainModel;
        }

        /// <summary>
        /// 修改T_MM_ProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TMMProductionOrderMainEditModel> requestObject, CurrentUser currentUser)
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
                var mainModel = _mapper.Map<TMMProductionOrderMainDbModel>(requestObject.PostData);
                var mainFlag = await currDb.Updateable(mainModel)
                    .UpdateColumns(p => new { p.SourceId, p.ProductionType, p.CustomerId, p.OrderDate, p.BeginDate, p.EndDate })
                    .ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TMMProductionOrderDetailEditModel>,
                    List<TMMProductionOrderDetailDbModel>>(requestObject.PostData.ChildList);

                var cache = BasicCacheGet.GetMaterial(currentUser);
                detailModels.ForEach((x) =>
                {
                    var me = GetMaterialFileByPackageColor(cache, currentUser, x.PackageId, x.ColorSolutionId);
                    x.MaterialId = me.ID;
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
                    detailFlag = currDb.Deleteable<TMMProductionOrderDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
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
        /// 删除T_MM_ProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //删除标识
                var mainFlag = true;
                //单条删除
                mainFlag = await _db.Instance.Updateable<TMMProductionOrderMainDbModel>()
                    .SetColumns(p => p.DeleteFlag == true)
                    .Where(p => p.ID == requestObject.PostData.ID)
                    .ExecuteCommandAsync() > 0;

                //如果该生产单关联了销售单则需要回滚转单状态
                var sourceId = currDb.Queryable<TMMProductionOrderMainDbModel>().Where(p => p.ID == requestObject.PostData.ID).First().SourceId;
                if (sourceId != null)
                {
                    await currDb.Updateable<TSSMSalesOrderMainDbModel>().SetColumns(p => p.TransProdStatus == true).Where(p => p.ID == sourceId).ExecuteCommandAsync();
                }
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
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
        /// 生产单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TMMProductionOrderMainAuditModel> requestPut, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestPut.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();

                var flag = await currDb.Updateable<TMMProductionOrderMainDbModel>()
                    .SetColumns(p => new TMMProductionOrderMainDbModel
                    {
                        AuditId = currentUser.UserID,
                        AuditStatus = requestPut.PostData.AuditStatus,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestPut.PostData.ID && (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2) && !p.DeleteFlag)
                    .ExecuteCommandAsync() > 0;

                int MaindID = requestPut.PostData.ID;
                if (requestPut.PostData.AuditStatus == 2)
                {
                    //合并统计表
                    var proOrdersList = _db.Instance.Queryable<TMMProductionOrderDetailDbModel>().Where(p => p.MainId == requestPut.PostData.ID).ToList();

                    var groupSum = proOrdersList.GroupBy(p => p.MaterialId).ToList();

                    List<TMMProductionOrderDetailDbSumModel> sumList = new List<TMMProductionOrderDetailDbSumModel>();
                    foreach (var item in groupSum)
                    {
                        var key = item.Key;
                        var itemDeatails = item.ToList();
                        var firstItem = itemDeatails.FirstOrDefault();

                        TMMProductionOrderDetailDbSumModel sumDbEntity = new TMMProductionOrderDetailDbSumModel();
                        sumDbEntity.MainId = MaindID;
                        sumDbEntity.PackageId = firstItem.PackageId;
                        sumDbEntity.MaterialId = key;
                        sumDbEntity.ColorSolutionId = firstItem.ColorSolutionId;
                        sumDbEntity.GoodsCode = firstItem.GoodsCode;
                        sumDbEntity.GoodsName = firstItem.GoodsName;
                        sumDbEntity.ProductionNum = itemDeatails.Sum(p => p.ProductionNum);

                        sumList.Add(sumDbEntity);
                    }

                    _db.Instance.Insertable(sumList).ExecuteCommand();

                    #region 生成 生产入库申请单

                    TMMWhApplyMainDbModel applyMain = new TMMWhApplyMainDbModel();
                    applyMain.DeleteFlag = false;
                    applyMain.SourceId = MaindID;
                    applyMain.WhApplyNo = AnyCodeGenerator.CreateNo(currentUser.CompanyID, "MMM");
                    applyMain.ApplyDate = DateTime.Now;
                    applyMain.OperatorId = currentUser.UserID;
                    applyMain.OperatorTime = DateTime.Now;
                    applyMain.AuditStatus = 2;
                    applyMain.CompanyId = currentUser.CompanyID;
                    applyMain.TransferFlag = true;
                    int applyMainid = _db.Instance.Insertable(applyMain).ExecuteReturnIdentity();

                    var applyDeatail = proOrdersList.Select(p => new TMMWhApplyDetailDbModel()
                    {
                        MaterialId = p.MaterialId,
                        ApplyNum = p.ProductionNum,
                        TransNum = p.ProductionNum,
                        MainId = applyMainid,
                        ProOrderDetailId = p.ID,
                        WorkshopId = p.WorkshopId
                    }).ToList();

                    _db.Instance.Insertable(applyDeatail).ExecuteCommand();

                    #endregion
                }


                if (!flag)
                {
                    currDb.RollbackTran();
                    return ResponseUtil<bool>.FailResult(false, "审核数据失败，该数据可能已审核或已删除!");
                }

                if (flag && requestPut.PostData.AuditStatus == 2)
                {
                    ////审核通过后需要生成真实的BOM清单
                    //var createResult = await CreateOrderBom(requestPut.PostData.ID, currentUser);
                    ////如果生成失败，回滚审核信息
                    //if (!createResult.Result)
                    //{
                    //    //回滚
                    //    currDb.RollbackTran();
                    //    return ResponseUtil<bool>.FailResult(false, createResult.ErrorInfo);
                    //}

                    //int mainID = requestPut.PostData.ID;



                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        //可转的入库申请单

        /// <summary>
        /// 生成当前订单的真实BOM信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<(bool Result, string ErrorInfo)> CreateOrderBom(int iMainId, CurrentUser currentUser)
        {
            /*
             * 1. 获取订单信息、BOM信息、物料基础信息
             * 2. 检查所有物料档案信息是否存在，如果不存在返回错误信息
             * 3. 生成订单真实BOM信息 
             */

            //获取生产单详情
            var orderList = _db.Instance.Queryable<TMMProductionOrderDetailDbSumModel>()
                .Where(p => p.MainId == iMainId).ToList();

            //获取Bom信息
            var bomList = await GetOrderBomList(orderList.Select(p => (p.PackageId, p.ColorSolutionId)).ToList(), currentUser);

            #region 检查是否配色方案中 是否所有的配色项目都配置了颜色

            var colorSolutionList = orderList.Where(p => p.ColorSolutionId != null).Select(p => p.ColorSolutionId).ToList();

            var SolutionCodeList = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TMMColorSolutionDetailDbModel>((t, t1) => new object[] { JoinType.Inner, t1.MainId == t.ID }).
                 Where((t, t1) => colorSolutionList.Contains(t.ID) && t1.ColorId == 0).Select((t, t1) => t.SolutionCode).ToList();

            if (SolutionCodeList.Count() > 0)
            {
                string msg = "配色方案编码为:" + string.Join(",", SolutionCodeList) + "的配色方案，存在没有设置颜色的配色项目";
                return (false, msg);
            }
            #endregion


            if (!bomList.Result)
            {
                return (false, bomList.ErrorInfo);
            }
            //获取配色方案详情
            var colorList = await GetColorList(orderList.Select(p => p.ColorSolutionId).ToArray());
            //获取所有物料信息
            var materialList = BasicCacheGet.GetMaterial(currentUser);
            //生成BOM信息
            var addBomList = await GetAddBomList(orderList, bomList.BomList, colorList, materialList, currentUser);
            if (!addBomList.Result)
            {
                return (false, addBomList.ErrorInfo);
            }
            //插入BOM信息
            var result = await _db.Instance.Insertable(addBomList.BomList).ExecuteCommandAsync() > 0;

            #region BOM合计


            var AllmaterialTotal = addBomList.BomList.GroupBy(p => p.MaterialId);


            Dictionary<int, string> colorSolution = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TBMPackageDbModel>((t1, t2) =>
             new object[] { JoinType.Inner, t1.PackageId == t2.ID }).
            Where((t1, t2) => t2.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.SolutionCode);

            List<int> colorIDS = colorSolution.Keys.ToList();

            List<TMMProductionOrderBOMSumDbModel> sumList = new List<TMMProductionOrderBOMSumDbModel>();


            foreach (var s in AllmaterialTotal)
            {
                int MaterialId = s.Key;


                string colorSolutionCode = string.Join(",", s.Select(p => p.ColorSolutionId).Distinct().Where(p => p != null && colorIDS.Contains(p.Value))
                    .Select(p => colorSolution[p.Value])).Trim(',');

                TMMProductionOrderBOMSumDbModel sumItem = new TMMProductionOrderBOMSumDbModel();
                sumItem.MaterialId = MaterialId;
                sumItem.ProOrderId = iMainId;
                sumItem.ColorSolutionCode = colorSolutionCode;
                sumItem.SingleValue = string.Join(",", s.Select(p => p.SingleValue).Distinct()).Trim(',');
                sumItem.TotalValue = s.Sum(p => p.TotalValue);
                sumItem.PurchaseTransNum = 0;
                sumItem.PickTransNum = 0;

                sumList.Add(sumItem);
            }

            _db.Instance.Insertable(sumList).ExecuteCommand();

            #endregion

            return result ? (true, "") : (false, "算料失败，无法插入BOM信息");
        }

        /// <summary>
        /// 生产真实BOM数据
        /// </summary>
        /// <param name="orderList">订单详情</param>
        /// <param name="BomList">BOM清单</param>
        /// <param name="colorList">配色ID,对应的配色项目与颜色</param>
        /// <param name="materialList"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        private async Task<(bool Result, string ErrorInfo, List<TMMProductionOrderBOMDbModel> BomList)> GetAddBomList(List<TMMProductionOrderDetailDbSumModel> orderList
            , List<(int PackageId, int? ColorSolutionId, List<OrderBomModel> bomList)> BomList, Dictionary<int, Dictionary<int, int>> colorList
            , List<TBMMaterialFileCacheModel> materialList, CurrentUser currentUser)
        {
            var errorInfo = new StringBuilder();
            var resultBomList = new List<TMMProductionOrderBOMDbModel>();
            var colorData = await _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>(
                (t, t0) => new object[] { JoinType.Left, t.TypeId == t0.ID })
                .Where((t, t0) => t.CompanyId == currentUser.CompanyID && t0.TypeName == "颜色档案")
                .ToListAsync();

            var dic = BasicCacheGet.GetDic(currentUser);
            List<int> colorIds = colorList.Keys.ToList();
            var colorSolution = _db.Instance.Queryable<TMMColorSolutionMainDbModel>().Where(t => colorIds.Contains(t.ID)).ToList();

            List<string> error = new List<string>();

            foreach (var item in orderList)
            {
                if (item.ColorSolutionId != null)
                {
                    //有配色
                    //获取当前包型的BOM
                    var currBom = BomList.Where(p => p.PackageId == item.PackageId && p.ColorSolutionId == item.ColorSolutionId).FirstOrDefault().bomList;
                    foreach (var bomItem in currBom)
                    {
                        //当前颜色ID
                        var colorMain = colorList[(int)item.ColorSolutionId];
                        if (!colorMain.ContainsKey((int)bomItem.ItemId))
                        {
                            var colorTemp = colorSolution.Where(p => p.ID == item.ColorSolutionId).FirstOrDefault();
                            var colorItemTemp = dic.Where(p => p.ID == bomItem.ItemId).FirstOrDefault();
                            error.Add($"配色方案编码：{colorTemp.SolutionCode},不存在'{colorItemTemp.DicValue}'的配色项目；");
                            continue;
                        }

                        var currColorId = colorMain[(int)bomItem.ItemId];
                        var currColorName = colorData.Where(p => p.ID == currColorId).First().DicValue;
                        //检查物料信息                        
                        if (materialList.Count(p => p.MaterialName == bomItem.MaterialName && p.ColorId == currColorId) != 1)
                        {
                            error.Add($"物料名称为：{bomItem.MaterialName},颜色为：{currColorName}的物料不存在或不具有唯一性。");
                            continue;
                        }
                        var currMaterial = materialList.Where(p => p.MaterialName == bomItem.MaterialName && p.ColorId == currColorId).First();
                        var currBomModel = new TMMProductionOrderBOMDbModel
                        {
                            ID = 0,
                            ProOrderId = item.MainId,
                            ColorSolutionId = item.ColorSolutionId,
                            ItemId = bomItem.ItemId,
                            PackageId = item.PackageId,
                            MaterialId = currMaterial.ID,
                            PartId = bomItem.PartId,
                            LengthValue = bomItem.LengthValue,
                            WidthValue = bomItem.WidthValue,
                            NumValue = bomItem.NumValue,
                            WideValue = bomItem.WideValue,
                            LossValue = bomItem.LossValue,
                            SingleValue = bomItem.SingleValue,
                            Formula = bomItem.Formula,
                            TotalValue = bomItem.SingleValue * item.ProductionNum,
                            ProductionNum = item.ProductionNum
                        };
                        resultBomList.Add(currBomModel);
                    }
                }
                else
                {
                    //无配色
                    var currBom = BomList.Where(p => p.PackageId == item.PackageId && p.ColorSolutionId == null).FirstOrDefault().bomList;
                    foreach (var bomItem in currBom)
                    {
                        var currBomModel = new TMMProductionOrderBOMDbModel
                        {
                            ID = 0,
                            ProOrderId = item.MainId,
                            ItemId = bomItem.ItemId,
                            PackageId = item.PackageId,
                            MaterialId = bomItem.BomMaterialId.Value,
                            PartId = bomItem.PartId,
                            LengthValue = bomItem.LengthValue,
                            WidthValue = bomItem.WidthValue,
                            NumValue = bomItem.NumValue,
                            WideValue = bomItem.WideValue,
                            LossValue = bomItem.LossValue,
                            SingleValue = bomItem.SingleValue,
                            Formula = bomItem.Formula,
                            TotalValue = bomItem.SingleValue * item.ProductionNum,
                            ProductionNum = item.ProductionNum
                        };
                        resultBomList.Add(currBomModel);
                    }
                }
            }

            foreach (var s in error.Distinct())
            {
                errorInfo.AppendLine(s);
            }

            return (errorInfo.Length <= 0, errorInfo.ToString(), resultBomList);
        }


        /// <summary>
        /// 获取配色方案详情
        /// </summary>
        /// <param name="csidList">配色方案主表IDS</param>
        /// <returns></returns>
        private async Task<Dictionary<int, Dictionary<int, int>>> GetColorList(int?[] csidList)
        {
            //获取配色方案信息，生成配色方案数据
            var ids = csidList.Where(p => p != null).ToList();
            var colorList = await _db.Instance.Queryable<TMMColorSolutionDetailDbModel>()
                .Where(p => ids.Contains(p.MainId))
                .ToListAsync();
            var result = new Dictionary<int, Dictionary<int, int>>();
            foreach (int item in csidList.Where(p => p != null))
            {
                var colors = new Dictionary<int, int>();
                colorList.Where(p => p.MainId == item).ToList().ForEach(p => colors.Add(p.ItemId, p.ColorId));
                result.Add(item, colors);
            }
            return result;
        }

        /// <summary>
        /// 根据包型ID和配色方案查询BOM信息
        /// </summary>
        /// <param name="queryList"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        private async Task<(bool Result, string ErrorInfo, List<(int PackageId, int? ColorSolutionId, List<OrderBomModel>)> BomList)> GetOrderBomList(
            List<(int PackageId, int? ColorSolutionId)> queryList, CurrentUser currentUser)
        {
            //获取BOM信息，并同时检查包型、配色方案的BOM是否存在
            var errorInfo = new StringBuilder();
            List<(int PackageId, int? ColorSolutionId, List<OrderBomModel> BomList)> result =
                new List<(int PackageId, int? ColorSolutionId, List<OrderBomModel> BomList)>();

            var packList = await _db.Instance.Queryable<TBMPackageDbModel>()
                .Where(p => p.CompanyId == currentUser.CompanyID)
                .ToListAsync();
            foreach (var item in queryList)
            {
                if (item.ColorSolutionId != null)
                {
                    //有配色
                    var data = await _db.Instance.Queryable<TMMBOMDetailDbModel>()
                    .Where(p1 => p1.MainId == SqlFunc.Subqueryable<TMMBOMMainDbModel>().Where(p2 => p2.PackageId == item.PackageId).Select(p3 => p3.ID))
                    .Select(p4 => new OrderBomModel
                    {
                        ID = p4.ID,
                        MainId = p4.MainId,
                        ItemId = p4.ItemId,
                        MaterialName = p4.MaterialName,
                        PartId = p4.PartId,
                        LengthValue = p4.LengthValue,
                        WidthValue = p4.WidthValue,
                        NumValue = p4.NumValue,
                        WideValue = p4.WideValue,
                        LossValue = p4.LossValue,
                        SingleValue = p4.SingleValue,
                        Formula = p4.Formula
                    })
                    .ToListAsync();

                    if (data == null || data.Count() < 1)
                    {

                        errorInfo.Append($"包型名称为【{packList.Where(p => p.ID == item.PackageId).First().DicValue}】不存在有配色方案的BOM，请先添加BOM{Environment.NewLine}");
                    }
                    else
                    {
                        result.Add((item.PackageId, item.ColorSolutionId, data));
                    }
                }
                else
                {
                    //无配色
                    var data = await _db.Instance.Queryable<TMMBOMNDetailDbModel>()
                    .Where(p1 => p1.MainId == SqlFunc.Subqueryable<TMMBOMNMainDbModel>().Where(p2 => p2.PackageId == item.PackageId).Select(p3 => p3.ID))
                    .Select(p4 => new OrderBomModel
                    {
                        ID = p4.ID,
                        MainId = p4.MainId,
                        BomMaterialId = p4.MaterialId,
                        PartId = p4.PartId,
                        LengthValue = p4.LengthValue,
                        WidthValue = p4.WidthValue,
                        NumValue = p4.NumValue,
                        WideValue = p4.WideValue,
                        LossValue = p4.LossValue,
                        SingleValue = p4.SingleValue,
                        Formula = p4.Formula
                    })
                    .ToListAsync();

                    if (data == null || data.Count() < 1)
                    {
                        errorInfo.Append($"包型名称为【{packList.Where(p => p.ID == item.PackageId).First().DicValue}】不存在无配色方案的BOM，请先添加BOM{Environment.NewLine}");
                    }
                    else
                    {
                        result.Add((item.PackageId, item.ColorSolutionId, data));
                    }
                }
            }
            return (errorInfo.Length <= 0, errorInfo.ToString(), result);
        }

        /// <summary>
        /// 终止生产入库
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TMMWhApplyMainDbModel>()
                    .SetColumns(p => new TMMWhApplyMainDbModel
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



        /// <summary>
        /// 导出BOM模板
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<AllBomExcel>> ExportBomTemplate(int orderID, CurrentUser currentUser)
        {
            //currentUser = new CurrentUser();
            //currentUser.CompanyID = 1;
            AllBomExcel allBomExcel = new AllBomExcel();

            #region 生产单
            var query = _db.Instance.Queryable<TMMProductionOrderBOMDbModel,
                TMMColorSolutionMainDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel, TBMPackageDbModel, TBMDictionaryDbModel,
                TBMDictionaryDbModel>((t, t1, t2, t3, t4, t5, t6) => new object[] {
                JoinType.Left,t.ColorSolutionId==t1.ID,
                JoinType.Inner,t.MaterialId==t2.ID,
                JoinType.Inner, t2.ColorId==t3.ID,
                JoinType.Inner,t4.ID==t.PackageId,
                JoinType.Left,t5.ID==t.PartId,
                JoinType.Left,t6.ID==t.ItemId
            }).
            Where((t, t1, t2, t3, t4, t5, t6) => t.ProOrderId == orderID).Select((t, t1, t2, t3, t4, t5, t6) => new TMMProductionOrderBOMQueryModel()
            {
                ID = t.ID,
                ProOrderId = t.ProOrderId,
                MaterialId = t.MaterialId,
                ColorSolutionCode = SqlFunc.IsNull(t1.SolutionCode, "无配色"),
                MaterialName = t2.MaterialName,
                SingleValue = t.SingleValue,
                PartId = t.PartId,
                PartName = t5.DicValue,
                ProductionNum = t.ProductionNum,
                ColorId = t2.ColorId,
                PackageId = t.PackageId,
                ColorName = SqlFunc.IsNull(t3.DicValue, "无色"),
                PackageName = t4.DicValue,
                ItemId = t.ItemId,
                ItemName = t6.DicValue
            }).ToList();

            allBomExcel.ProductionOrderBOMQueryModelList = query;

            #endregion

            //bom
            var PackageColor = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel, TBMPackageDbModel>((t, t1, t2) => new object[] {
               JoinType.Inner,t.MainId==t1.ID,
               JoinType.Inner,t.PackageId==t2.ID
           }).Where((t, t1, t2) => t1.ID == orderID).Select((t, t1, t2) => new { PackageId = t.PackageId, ColorSolutionId = t.ColorSolutionId, ProductionNum = t.ProductionNum, PackageName = t2.DicValue }).ToList();

            var PackageList = PackageColor.Where(p => p.ColorSolutionId != null).Select(p => p.PackageId).ToList().Distinct();//有配色方案的包型

            var NoPackageList = PackageColor.Where(p => p.ColorSolutionId == null).Select(p => p.PackageId).ToList().Distinct();//无配色方案的包型

            var ColorSolutionIdList = PackageColor.Where(p => p.ColorSolutionId != null).Select(p => new { p.PackageId, p.ColorSolutionId }).ToList().Distinct();

            //有配色的Bom
            var PackBom = _db.Instance.Queryable<TMMBOMDetailDbModel, TBMDictionaryDbModel, TMMFormulaDbModel, TMMPackageColorItemDbModel, TMMBOMMainDbModel>((t, t1, t2, t3, t4) => new object[] {
                    JoinType.Left,t.PartId==t1.ID,
                    JoinType.Left,t.Formula==t2.ID.ToString(),
                    JoinType.Inner,t.ItemId==t3.ID,
                    JoinType.Inner,t.MainId==t4.ID
                }).Where((t, t1, t2, t3, t4) => PackageList.Contains(t4.PackageId));
            PackBom.OrderBy($"t3.ItemName asc");

            var queryData = PackBom.Select((t, t1, t2, t3, t4) => new TMMBOMDetailQueryExcelModel
            {
                ID = t.ID,
                MainId = t.MainId,
                ItemId = t.ItemId,
                ItemName = t3.ItemName,
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
                FormulaName = t2.FormulaName,
                PackageId = t4.PackageId
            }).ToList();

            allBomExcel.TMMBOMDetailQueryExcelList = queryData;

            List<PackageColorExcelModel> PackageColorExcelModelList = new List<PackageColorExcelModel>();
            foreach (var packageItem in PackageList)
            {

                var colorIdList = ColorSolutionIdList.Where(p => p.PackageId == packageItem).Select(p => p.PackageId).ToList();

                var detailList = _db.Instance.Queryable<TMMColorSolutionDetailDbModel, TMMPackageColorItemDbModel, TBMDictionaryDbModel, TMMColorSolutionMainDbModel>(
                          (t, t0, t1, t2) => new object[]
                          {
                            JoinType.Left , t.ItemId == t0.ID,
                            JoinType.Left , t.ColorId == t1.ID,
                            JoinType.Inner,t.MainId==t2.ID
                          }
                      ).Where((t, t0, t1, t2) => colorIdList.Contains(t2.ID)).Select((t, t0, t1, t2) => new TMMColorSolutionDetailQueryExcelModel
                      {
                          ID = t.ID,
                          MainId = t.MainId,
                          ItemId = t.ItemId,
                          ItemName = t0.ItemName,
                          ColorId = t.ColorId,
                          ColorName = t1.DicValue,
                          SolutionCode = t2.SolutionCode
                      }).ToList();

                PackageColorExcelModel packageColorExcelModel = new PackageColorExcelModel();
                packageColorExcelModel.PackageId = packageItem;

                var dt = new DataTable();
                dt.Columns.Add($"数量");

                foreach (var item in detailList.Select(p => new { p.MainId, p.SolutionCode }).Distinct())
                {
                    var num = PackageColor.Where(p => p.ColorSolutionId == item.MainId).Sum(p => p.ProductionNum);
                    //添加动态列
                    foreach (var item2 in detailList.Where(p => p.MainId == item.MainId))
                    {
                        if (!dt.Columns.Contains($"{item2.ItemName}"))
                            dt.Columns.Add($"{item2.ItemName}");

                        var newRow = dt.NewRow();
                        newRow[$"{item2.ItemName}"] = item2.ColorId;
                        newRow["数量"] = num;
                        dt.Rows.Add(newRow);
                    }
                }

                packageColorExcelModel.dt = dt;

                PackageColorExcelModelList.Add(packageColorExcelModel);

            }

            allBomExcel.PackageColorExcelModelList = PackageColorExcelModelList;
            return ResponseUtil<AllBomExcel>.SuccessResult(allBomExcel);
        }

    }
}
