using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Inventory;
using YfCloud.App.Module.Report.Models.Warehouse;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IInventoryReportService))]
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IInventoryReportService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        /// <summary>
        /// 默认构造方法
        /// </summary>
        public InventoryReportService(IDbContext dbContext, ILogger<IInventoryReportService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 总入库量，出库量，库存分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<InventoryAmountCountModel>> GetInventoryAmountCount(CurrentUser currentUser)
        {
            try
            {
                #region 期初=统计本月，不考虑期初
                var Beginning = 0;
                #endregion
                #region 其他入库
                var OtherStorage = await _db.Instance.Queryable<TWMOtherWhMainDbModel>()
              .Where(n => n.CompanyId == currentUser.CompanyID
               && n.AuditStatus == 2
               && n.DeleteFlag == false
               && n.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
               && n.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
              .SumAsync(n => n.Number);
                #endregion
                #region 生产入库
                var ProductionWarehousing = await _db.Instance.Queryable<TWMProductionWhMainDbModel>()
           .Where(n => n.CompanyId == currentUser.CompanyID
                 && n.AuditStatus == 2
               && n.DeleteFlag == false
            && n.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
            && n.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
           .SumAsync(n => n.Number);
                #endregion
                #region 采购入库
                var PurchaseWarehousing = await _db.Instance.Queryable<TWMPurchaseMainDbModel>()
        .Where(n => n.CompanyId == currentUser.CompanyID
              && n.AuditStatus == 2
               && n.DeleteFlag == false
        && n.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
        && n.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
        .SumAsync(n => n.Number);
                #endregion
                #region 盘盈入库
                var PanProfitStorage = await _db.Instance.Queryable<TWMProfitMainDbModel>()
     .Where(n => n.CompanyId == currentUser.CompanyID
           && n.AuditStatus == 2
               && n.DeleteFlag == false
     && n.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
     && n.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
     .SumAsync(n => n.Number);
                #endregion
                #region 其他出库
                var OtherOutbound = await _db.Instance.Queryable<TWMOtherWhSendMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false
&& n.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
&& n.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
.SumAsync(n => n.Number);

                #endregion
                #region 盘亏出库

                var DiskLoss = await _db.Instance.Queryable<TWMDeficitMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false
&& n.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
&& n.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
.SumAsync(n => n.Number);
                #endregion
                #region 销售出库

                var SalesOutOfStock = await _db.Instance.Queryable<TWMSalesMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false
&& n.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
&& n.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
.SumAsync(n => n.Number);
                #endregion
                #region 生产出库
                var ProductionOutbound = await _db.Instance.Queryable<TWMProductionMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false
&& n.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
&& n.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
.SumAsync(n => n.Number);

                #endregion
                InventoryAmountCountModel wcmodel = new InventoryAmountCountModel();
                wcmodel.InAmountCount = Beginning + OtherStorage + ProductionWarehousing + PurchaseWarehousing + PanProfitStorage;
                wcmodel.OutAmountCount = OtherOutbound + DiskLoss + SalesOutOfStock + ProductionOutbound;




                #region 库存量
                #region 期初
                var Beginning1 = await _db.Instance.Queryable<TWMPrimeCountDbModel>()
                     .Where(n => n.CompanyId == currentUser.CompanyID)
                     .SumAsync(n => n.PrimeNum);
                #endregion
                #region 其他入库
                var OtherStorage1 = await _db.Instance.Queryable<TWMOtherWhMainDbModel>()
              .Where(n => n.CompanyId == currentUser.CompanyID
               && n.AuditStatus == 2
               && n.DeleteFlag == false)
              .SumAsync(n => n.Number);
                #endregion
                #region 生产入库
                var ProductionWarehousing1 = await _db.Instance.Queryable<TWMProductionWhMainDbModel>()
           .Where(n => n.CompanyId == currentUser.CompanyID
                 && n.AuditStatus == 2
               && n.DeleteFlag == false)
           .SumAsync(n => n.Number);
                #endregion
                #region 采购入库
                var PurchaseWarehousing1 = await _db.Instance.Queryable<TWMPurchaseMainDbModel>()
        .Where(n => n.CompanyId == currentUser.CompanyID
              && n.AuditStatus == 2
               && n.DeleteFlag == false)
        .SumAsync(n => n.Number);
                #endregion
                #region 盘盈入库
                var PanProfitStorage1 = await _db.Instance.Queryable<TWMProfitMainDbModel>()
     .Where(n => n.CompanyId == currentUser.CompanyID
           && n.AuditStatus == 2
               && n.DeleteFlag == false)
     .SumAsync(n => n.Number);
                #endregion
                #region 其他出库
                var OtherOutbound1 = await _db.Instance.Queryable<TWMOtherWhSendMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false)
.SumAsync(n => n.Number);

                #endregion
                #region 盘亏出库

                var DiskLoss1 = await _db.Instance.Queryable<TWMDeficitMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false)
.SumAsync(n => n.Number);
                #endregion
                #region 销售出库

                var SalesOutOfStock1 = await _db.Instance.Queryable<TWMSalesMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false)
.SumAsync(n => n.Number);
                #endregion
                #region 生产出库
                var ProductionOutbound1 = await _db.Instance.Queryable<TWMProductionMainDbModel>()
.Where(n => n.CompanyId == currentUser.CompanyID
&& n.AuditStatus == 2
&& n.DeleteFlag == false)
.SumAsync(n => n.Number);

                #endregion
                decimal _in = Beginning1 + OtherStorage1 + ProductionWarehousing1 + PurchaseWarehousing1 + PanProfitStorage1;
                decimal _out = OtherOutbound1 + DiskLoss1 + SalesOutOfStock1 + ProductionOutbound1;
                wcmodel.Inventory = _in - _out;
                #endregion
                return ResponseUtil<InventoryAmountCountModel>.SuccessResult(wcmodel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<InventoryAmountCountModel>
                    .FailResult(null, $"总入库量，出库量，库存分析发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 成品库存态势 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<InventoryAmountCountDayModel>> GetInventoryAmountCountDay(CurrentUser currentUser)
        {
            try
            {
                #region 其他入库
                var OtherStorage = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
              (t1, t2, t3, t4) => new object[]
              {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
              }
              ).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID
              && t2.AuditStatus == 2
              && t2.DeleteFlag == false
              && t2.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
              && t2.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
              && t3.PackageID != null
              )
        .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
        {
            WarehousingDate = t2.WarehousingDate,
            DicValue = t4.DicValue,
            inAmount = t1.ActualNumber,
            outAmount = 0
        });
                #endregion
                #region 生产入库
                var ProductionWarehousing = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                    }
                    ).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WarehousingDate,
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNum,
                 outAmount = 0
             });
                #endregion
                #region 采购入库
                var PurchaseWarehousing = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                    }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WarehousingDate,
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNum,
                 outAmount = 0
             });
                #endregion
                #region 盘盈入库
                var PanProfitStorage = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WarehousingDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WarehousingDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WarehousingDate,
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNumber,
                 outAmount = 0
             });
                #endregion
                #region 其他出库
                var OtherOutbound = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
        {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
        }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
  .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
  {
      WarehousingDate = t2.WhSendDate,
      DicValue = t4.DicValue,
      inAmount = 0,
      outAmount = t1.ActualNumber
  });
                #endregion
                #region 盘亏出库
                var DiskLoss = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WhSendDate,
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNumber
             });
                #endregion
                #region 销售出库
                var SalesOutOfStock = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WhSendDate,
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNum
             });
                #endregion
                #region 生产出库
                var ProductionOutbound = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01") && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01") && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAmountCountDayModel1
             {
                 WarehousingDate = t2.WhSendDate,
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNum
             });
                #endregion 
                var Amount = _db.Instance.UnionAll(OtherStorage, ProductionWarehousing, PurchaseWarehousing, PanProfitStorage, OtherOutbound, DiskLoss, SalesOutOfStock, ProductionOutbound).GroupBy(p => new { p.WarehousingDate, p.DicValue }).
          Select(p => new InventoryAmountCountDayModel2()
          {
              Amount = SqlFunc.AggregateSum(p.inAmount) - SqlFunc.AggregateSum(p.outAmount),
              WarehousingDate = p.WarehousingDate,
              DicValue = p.DicValue
          }).AS("t103");
                var s113 = Amount.ToListAsync();
                DateTime _thisDateTime = DateTime.Now;
                String y = _thisDateTime.ToString("yyyy");
                String m = _thisDateTime.ToString("MM");
                int day = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                string s1 = "";
                string s2 = "";
                for (int i = 0; i < day; i++)
                {
                    s1 += "," + (i + 1);
                }
                var Package = await _db.Instance.Queryable<TBMPackageDbModel>()
                      .Where(t => t.CompanyId == currentUser.CompanyID && t.DeleteFlag == false).ToListAsync();
                List<InventoryAmountCountDay> SeriesData = new List<InventoryAmountCountDay>();
                for (int i = 0; i < Package.Count; i++)
                {
                    InventoryAmountCountDay iad = new InventoryAmountCountDay();
                    iad.name = Package[i].DicValue;
                    String ss2 = "";
                    for (int j = 0; j < day; j++)
                    {
                        String ss3 = "";
                        for (int kk = 0; kk < s113.Result.Count; kk++)
                        {
                            if (s113.Result[kk].WarehousingDate == Convert.ToDateTime(y + "-" + m + "-" + (j + 1)) && s113.Result[kk].DicValue == Package[i].DicValue)
                            {
                                ss3 += "," + String.Format("{0:N2}", s113.Result[kk].Amount);
                            }
                        }
                        if (ss3 == "")
                        {
                            ss2 += ",0.00";
                        }
                        else
                        {
                            ss2 += ss3;
                        }
                    }
                    iad.SeriesData = ss2.Substring(1).Split(',');
                    SeriesData.Add(iad);
                }
                InventoryAmountCountDayModel _model = new InventoryAmountCountDayModel { xAxisData = s1.Substring(1).Split(','), SeriesData = SeriesData };
                return ResponseUtil<InventoryAmountCountDayModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<InventoryAmountCountDayModel>
                    .FailResult(null, $"成品库存态势发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 库存量分析 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<InventoryAnalysisModel>>> GetInventoryAnalysis(CurrentUser currentUser)
        {
            try
            {
                #region 期初
                var Beginning = _db.Instance.Queryable<TWMPrimeCountDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                     (t1, t2, t3) => new object[]
              {
                         JoinType.Left , t1.MaterialId == t2.ID,
                          JoinType.Left , t2.PackageID == t3.ID
              }
                    )
                           .Where((t1, t2, t3) => t1.CompanyId == currentUser.CompanyID && t3.DicValue != null
                           ).Select((t1, t2, t3) => new InventoryAnalysisModel1
                           {
                               DicValue = t3.DicValue,
                               inAmount = t1.PrimeNum,
                               outAmount = 0
                           });
                #endregion
                #region 其他入库
                var OtherStorage = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
              (t1, t2, t3, t4) => new object[]
              {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
              }
              ).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID
              && t2.AuditStatus == 2
              && t2.DeleteFlag == false
              && t3.PackageID != null
              )
        .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
        {
            DicValue = t4.DicValue,
            inAmount = t1.ActualNumber,
            outAmount = 0
        });
                #endregion
                #region 生产入库
                var ProductionWarehousing = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                    }
                    ).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNum,
                 outAmount = 0
             });
                #endregion
                #region 采购入库
                var PurchaseWarehousing = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                    }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNum,
                 outAmount = 0
             });
                #endregion
                #region 盘盈入库
                var PanProfitStorage = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = t1.ActualNumber,
                 outAmount = 0
             });
                #endregion
                #region 其他出库
                var OtherOutbound = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
        {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
        }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
  .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
  {
      DicValue = t4.DicValue,
      inAmount = 0,
      outAmount = t1.ActualNumber
  });
                #endregion
                #region 盘亏出库
                var DiskLoss = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNumber
             });
                #endregion
                #region 销售出库
                var SalesOutOfStock = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNum
             });
                #endregion
                #region 生产出库
                var ProductionOutbound = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>((t1, t2, t3, t4) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                   }).Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false && t3.PackageID != null)
             .Select((t1, t2, t3, t4) => new InventoryAnalysisModel1
             {
                 DicValue = t4.DicValue,
                 inAmount = 0,
                 outAmount = t1.ActualNum
             });
                #endregion  
                var Amount = await _db.Instance.UnionAll(Beginning, OtherStorage, ProductionWarehousing, PurchaseWarehousing, PanProfitStorage, OtherOutbound, DiskLoss, SalesOutOfStock, ProductionOutbound).GroupBy(p => p.DicValue).
          Select(p => new InventoryAnalysisModel()
          {
              Name = p.DicValue,
              Value = SqlFunc.AggregateSum(p.inAmount) - SqlFunc.AggregateSum(p.outAmount),
          }).ToListAsync();
                return ResponseUtil<List<InventoryAnalysisModel>>.SuccessResult(Amount);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<InventoryAnalysisModel>>
                    .FailResult(null, $"库存量分析发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }


        /// <summary>
        /// 库存一览表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<InventoryListModel>> GetInventoryCountList(RequestGet request, CurrentUser currentUser)
        {
            try
            {
                List<InventoryCountListModel> queryData = null;//查询结果集对象
                #region 期初
                var Beginning = _db.Instance.Queryable<TWMPrimeCountDbModel, TBMMaterialFileDbModel>(
              (t1, t2) => new object[]
              {
                         JoinType.Left , t1.MaterialId == t2.ID
              }
              ).Where((t1, t2) => t2.CompanyId == currentUser.CompanyID && t2.DeleteFlag == false)
              .Select((t1, t2) => new InventoryCountListModel1
              {
                  MaterialId = t1.MaterialId,
                  WarehouseId = Convert.ToInt32(t1.WarehouseId),
                  ToDayinAmount = 0,
                  TotalinAmount = t1.PrimeNum,
                  ToDayoutAmount = 0,
                  TotaloutAmount = 0,
                  TotalinMoneyAmount = 0,
                  TotaloutMoneyAmount = 0
              });
                #endregion
                #region 今日其他入库
                var OtherStorage = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel>(
              (t1, t2, t3) => new object[]
              {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
              }
              ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID
              && t2.AuditStatus == 2
              && t2.DeleteFlag == false
             && t2.WarehousingDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
            && t2.WarehousingDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
              .Select((t1, t2, t3) => new InventoryCountListModel1
              {
                  MaterialId = t1.MaterialId,
                  WarehouseId = t1.WarehouseId,
                  ToDayinAmount = t1.ActualNumber,
                  TotalinAmount = 0,
                  ToDayoutAmount = 0,
                  TotaloutAmount = 0,
                  TotalinMoneyAmount = 0,
                  TotaloutMoneyAmount = 0
              });

                #endregion
                #region 累计其他入库
                var OtherStorage1 = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel>(
              (t1, t2, t3) => new object[]
              {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
              }
              ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID
              && t2.AuditStatus == 2
              && t2.DeleteFlag == false)
              .Select((t1, t2, t3) => new InventoryCountListModel1
              {
                  MaterialId = t1.MaterialId,
                  WarehouseId = t1.WarehouseId,
                  ToDayinAmount = 0,
                  TotalinAmount = t1.ActualNumber,
                  ToDayoutAmount = 0,
                  TotaloutAmount = 0,
                  TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                  TotaloutMoneyAmount = 0
              });
                #endregion
                #region 今日生产入库
                var ProductionWarehousing = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }
                    ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                    && t2.WarehousingDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                    && t2.WarehousingDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = t1.ActualNum,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计生产入库
                var ProductionWarehousing1 = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }
                    ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNum,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 今日采购入库
                var PurchaseWarehousing = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                     && t2.WarehousingDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                    && t2.WarehousingDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = t1.ActualNum,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计采购入库
                var PurchaseWarehousing1 = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNum,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion 
                #region 今日盘盈入库
                var PanProfitStorage = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                && t2.WarehousingDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                    && t2.WarehousingDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = t1.ActualNumber,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计盘盈入库
                var PanProfitStorage1 = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNumber,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion 
                #region 今日其他出库
                var OtherOutbound = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
        {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
        }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
        && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM-dd")}")
       && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}"))
  .Select((t1, t2, t3) => new InventoryCountListModel1
  {
      MaterialId = t1.MaterialId,
      WarehouseId = t1.WarehouseId,
      ToDayinAmount = 0,
      TotalinAmount = 0,
      ToDayoutAmount = t1.ActualNumber,
      TotaloutAmount = 0,
      TotalinMoneyAmount = 0,
      TotaloutMoneyAmount = 0
  });
                #endregion
                #region 累计其他出库
                var OtherOutbound1 = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
        {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
        }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
  .Select((t1, t2, t3) => new InventoryCountListModel1
  {
      MaterialId = t1.MaterialId,
      WarehouseId = t1.WarehouseId,
      ToDayinAmount = 0,
      TotalinAmount = 0,
      ToDayoutAmount = 0,
      TotaloutAmount = t1.ActualNumber,
      TotalinMoneyAmount = 0,
      TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
  });
                #endregion 
                #region 今日盘亏出库
                var DiskLoss = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                 && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM-dd")}")
       && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}"))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = t1.ActualNumber,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计盘亏出库
                var DiskLoss1 = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNumber,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion
                #region 今日销售出库
                var SalesOutOfStock = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                    && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM-dd")}")
       && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}"))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = t1.ActualNum,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计销售出库
                var SalesOutOfStock1 = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNum,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion
                #region 今日生产出库
                var ProductionOutbound = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
                  && t2.WhSendDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM-dd")}")
       && t2.WhSendDate < Convert.ToDateTime($"{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}"))
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = t1.ActualNum,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计生产出库
                var ProductionOutbound1 = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNum,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion
                #region 根据条件查询累计出入库数量
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);
                    List<IConditionalModel> conModels1 = new List<IConditionalModel>();
                    List<IConditionalModel> conModels2 = new List<IConditionalModel>();
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "warehousingdate")
                        {
                            conModels1.Add(new ConditionalModel() { FieldName = item.FieldName, ConditionalType = item.ConditionalType, FieldValue = item.FieldValue });
                            conModels2.Add(new ConditionalModel() { FieldName = "whsenddate", ConditionalType = item.ConditionalType, FieldValue = item.FieldValue });
                            continue;
                        }
                    }
                    #region 累计入库查询条件
                    OtherStorage1.Where(conModels1);
                    ProductionWarehousing1.Where(conModels1);
                    PurchaseWarehousing1.Where(conModels1);
                    PanProfitStorage1.Where(conModels1);
                    #endregion
                    #region 累计出库查询条件
                    OtherOutbound1.Where(conModels2);
                    var s1 = OtherOutbound1.ToListAsync();
                    DiskLoss1.Where(conModels2);
                    var s2 = DiskLoss1.ToListAsync();
                    SalesOutOfStock1.Where(conModels2);
                    var s3 = SalesOutOfStock1.ToListAsync();
                    ProductionOutbound1.Where(conModels2);
                    var s4 = ProductionOutbound1.ToListAsync();
                    #endregion
                }
                #endregion
                var Amount = _db.Instance.UnionAll(Beginning, OtherStorage, OtherStorage1, ProductionWarehousing, ProductionWarehousing1, PurchaseWarehousing, PurchaseWarehousing1, PanProfitStorage, PanProfitStorage1
                    , OtherOutbound, OtherOutbound1, DiskLoss, DiskLoss1, SalesOutOfStock, SalesOutOfStock1, ProductionOutbound, ProductionOutbound1).GroupBy(p => new { p.MaterialId, p.WarehouseId }).
Select(p => new InventoryCountListModel2()
{
    MaterialId = p.MaterialId,
    WarehouseId = p.WarehouseId,
    ToDayinAmount = SqlFunc.AggregateSum(p.ToDayinAmount),
    TotalinAmount = SqlFunc.AggregateSum(p.TotalinAmount),
    ToDayoutAmount = SqlFunc.AggregateSum(p.ToDayoutAmount),
    TotaloutAmount = SqlFunc.AggregateSum(p.TotaloutAmount),
    TotalAmount = SqlFunc.AggregateSum(p.TotalinMoneyAmount) - SqlFunc.AggregateSum(p.TotaloutMoneyAmount)
}).AS("t101");
                #region 库存量
                #region 期初
                var Total_Beginning = _db.Instance.Queryable<TWMPrimeCountDbModel, TBMMaterialFileDbModel>(
              (t1, t2) => new object[]
              {
                         JoinType.Left , t1.MaterialId == t2.ID
              }
              ).Where((t1, t2) => t2.CompanyId == currentUser.CompanyID && t2.DeleteFlag == false)
              .Select((t1, t2) => new InventoryCountListModel1
              {
                  MaterialId = t1.MaterialId,
                  WarehouseId = Convert.ToInt32(t1.WarehouseId),
                  ToDayinAmount = 0,
                  TotalinAmount = t1.PrimeNum,
                  ToDayoutAmount = 0,
                  TotaloutAmount = 0,
                  TotalinMoneyAmount = 0,
                  TotaloutMoneyAmount = 0
              });
                #endregion
                #region 累计其他入库
                var Total_OtherStorage1 = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel>(
              (t1, t2, t3) => new object[]
              {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
              }
              ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID
              && t2.AuditStatus == 2
              && t2.DeleteFlag == false)
              .Select((t1, t2, t3) => new InventoryCountListModel1
              {
                  MaterialId = t1.MaterialId,
                  WarehouseId = t1.WarehouseId,
                  ToDayinAmount = 0,
                  TotalinAmount = t1.ActualNumber,
                  ToDayoutAmount = 0,
                  TotaloutAmount = 0,
                  TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                  TotaloutMoneyAmount = 0
              });
                #endregion
                #region 累计生产入库
                var Total_ProductionWarehousing1 = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }
                    ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNum,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计采购入库
                var Total_PurchaseWarehousing1 = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                    }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNum,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion
                #region 累计盘盈入库
                var Total_PanProfitStorage1 = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = t1.ActualNumber,
                 ToDayoutAmount = 0,
                 TotaloutAmount = 0,
                 TotalinMoneyAmount = Convert.ToDecimal(t1.Amount),
                 TotaloutMoneyAmount = 0
             });
                #endregion 
                #region 累计其他出库
                var Total_OtherOutbound1 = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
        {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
        }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
  .Select((t1, t2, t3) => new InventoryCountListModel1
  {
      MaterialId = t1.MaterialId,
      WarehouseId = t1.WarehouseId,
      ToDayinAmount = 0,
      TotalinAmount = 0,
      ToDayoutAmount = 0,
      TotaloutAmount = t1.ActualNumber,
      TotalinMoneyAmount = 0,
      TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
  });
                #endregion 
                #region 累计盘亏出库
                var Total_DiskLoss1 = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNumber,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion
                #region 累计销售出库
                var Total_SalesOutOfStock1 = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNum,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion
                #region 累计生产出库
                var Total_ProductionOutbound1 = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[]
                   {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.MaterialId == t3.ID
                   }).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
             .Select((t1, t2, t3) => new InventoryCountListModel1
             {
                 MaterialId = t1.MaterialId,
                 WarehouseId = t1.WarehouseId,
                 ToDayinAmount = 0,
                 TotalinAmount = 0,
                 ToDayoutAmount = 0,
                 TotaloutAmount = t1.ActualNum,
                 TotalinMoneyAmount = 0,
                 TotaloutMoneyAmount = Convert.ToDecimal(t1.Amount)
             });
                #endregion 
                #endregion
                var TotalAmount = _db.Instance.UnionAll(Total_Beginning, Total_OtherStorage1, Total_ProductionWarehousing1, Total_PurchaseWarehousing1, Total_PanProfitStorage1
    , Total_OtherOutbound1, Total_DiskLoss1, Total_SalesOutOfStock1, Total_ProductionOutbound1).GroupBy(p => new { p.MaterialId, p.WarehouseId }).
Select(p => new InventoryCountListModel2()
{
    MaterialId = p.MaterialId,
    WarehouseId = p.WarehouseId,
    TotalNum = SqlFunc.AggregateSum(p.TotalinAmount) - SqlFunc.AggregateSum(p.TotaloutAmount),
    TotalAmount = SqlFunc.AggregateSum(p.TotalinMoneyAmount) - SqlFunc.AggregateSum(p.TotaloutMoneyAmount)
}).ToList().ToDictionary(p => new { p.MaterialId, p.WarehouseId }, p => new { MaterialId = p.MaterialId, WarehouseId = p.WarehouseId, TotalNum = p.TotalNum, TotalAmount = p.TotalAmount });
                #region 根据仓库查询
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {

                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);
                    List<IConditionalModel> conModels1 = new List<IConditionalModel>();
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "warehouseid")
                        {
                            conModels1.Add(new ConditionalModel() { FieldName = item.FieldName, ConditionalType = item.ConditionalType, FieldValue = item.FieldValue });
                            continue;
                        }
                    }
                    Amount.Where(conModels1);
                }
                #endregion
                #region 根据条件查询物料
                var query = _db.Instance.Queryable<TBMMaterialFileDbModel>()
                            .Where(t => t.CompanyId == currentUser.CompanyID && t.DeleteFlag == false);
                //查询条件
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);
                    List<IConditionalModel> conModels = new List<IConditionalModel>();
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "materialname"
                            || item.FieldName.ToLower() == "materialcode"
                             || item.FieldName.ToLower() == "materialcode"
                             || item.FieldName.ToLower() == "materialtypeid"
                            )
                        {
                            conModels.Add(new ConditionalModel() { FieldName = item.FieldName, ConditionalType = item.ConditionalType, FieldValue = item.FieldValue });
                            continue;
                        }
                    }
                    query.Where(conModels);
                }
                #endregion
                var ts = _db.Instance.Queryable(query, Amount, JoinType.Inner, (p1, p2) => p1.ID == p2.MaterialId).AS("t102");
                //排序条件
                if (request.OrderByConditions != null && request.OrderByConditions.Count > 0)
                {
                    foreach (var item in request.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMMaterialFileDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        ts.OrderBy($"p1.{item.Column} {item.Condition}");
                    }
                }
                InventoryListModel _list = new InventoryListModel();
                #region 统计
                _list.Total_Number = ts.Count();
                #endregion
                if (request.IsPaging)
                {
                    int skipNum = request.PageSize * (request.PageIndex - 1);
                    queryData = await ts
      .Select((p1, p2) => new InventoryCountListModel
      {
          MaterialID = p1.ID,
          MaterialCode = p1.MaterialCode,
          MaterialName = p1.MaterialName,
          MaterialTypeId = p1.MaterialTypeId,
          ToDayStorageAmount = p2.ToDayinAmount,
          TotalStorageAmount = p2.TotalinAmount,
          ToDayOutStorageAmount = p2.ToDayoutAmount,
          TotalOutStorageAmount = p2.TotaloutAmount,
          TotalAmount = p2.TotalAmount,
          WarehouseId = p2.WarehouseId
      }).Skip(skipNum).Take(request.PageSize).ToListAsync();
                }
                else
                {
                    queryData = await ts
.Select((p1, p2) => new InventoryCountListModel
{
    MaterialID = p1.ID,
    MaterialCode = p1.MaterialCode,
    MaterialName = p1.MaterialName,
    MaterialTypeId = p1.MaterialTypeId,
    ToDayStorageAmount = p2.ToDayinAmount,
    TotalStorageAmount = p2.TotalinAmount,
    ToDayOutStorageAmount = p2.ToDayoutAmount,
    TotalOutStorageAmount = p2.TotaloutAmount,
    TotalAmount = p2.TotalAmount,
    WarehouseId = p2.WarehouseId
}).ToListAsync();
                }
                #region 数据字典
                var tBMDictionary = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.DicValue);
                #endregion
                #region 仓库
                var warehouseDic = _db.Instance.Queryable<TBMWarehouseFileDbModel>().Where(t => SqlFunc.IsNull(t.DeleteFlag, false) != true &&
              t.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => new { Name = p.WarehouseName, Code = p.Code });
                #endregion
                queryData.ForEach(x =>
                { 
                    if (warehouseDic.ContainsKey(x.WarehouseId))
                    {
                        x.WarehouseName = warehouseDic[x.WarehouseId].Name;
                    }
                    if (tBMDictionary.ContainsKey(x.MaterialTypeId))
                    {
                        x.MaterialTypeName = tBMDictionary[x.MaterialTypeId];
                    }
                    if (TotalAmount.Keys.Any(p => p.WarehouseId == x.WarehouseId && p.MaterialId == x.MaterialID))
                    {
                        var xxx = TotalAmount.Keys.Where(p => p.WarehouseId == x.WarehouseId && p.MaterialId == x.MaterialID).FirstOrDefault();
                        x.TotalNum = TotalAmount[xxx].TotalNum;
                        x.TotalAmount = TotalAmount[xxx].TotalAmount;
                    }
                    _list.ToDayinNum += x.ToDayStorageAmount;
                    _list.TotalinNum += x.TotalStorageAmount;
                    _list.ToDayoutNum += x.ToDayOutStorageAmount;
                    _list.TotaloutNum += x.TotalOutStorageAmount;
                    _list.TotalNum += x.TotalNum;
                    _list.TotalAmount += x.TotalAmount;
                });
                _list.List = queryData;
                return ResponseUtil<InventoryListModel>.SuccessResult(_list);
            }
            catch (Exception ex)
            {
                return ResponseUtil<InventoryListModel>
                    .FailResult(null, $"库存一览表发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
    }
}
