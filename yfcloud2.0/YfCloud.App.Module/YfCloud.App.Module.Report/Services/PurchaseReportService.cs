using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Purchase;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 采购订单
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IPurchaseReportService))]
    public class PurchaseReportService : IPurchaseReportService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IPurchaseReportService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        /// <summary>
        /// 默认构造方法
        /// </summary>
        public PurchaseReportService(IDbContext dbContext, ILogger<IPurchaseReportService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取供应商采购金额分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<PurchaseSupplierAmountCountModel>>> GetPurchaseSupplierAmountCount(CurrentUser currentUser)
        {
            try
            {
                var today = await _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TPSMPurchaseOrderMainDbModel, TBMSupplierFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.SupplierId == t3.ID
                    }
                    ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID
                   && t2.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                   && t2.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
                    )
                    .GroupBy((t1, t2, t3) => t3.SupplierName)
                    .Where((t1, t2, t3) => !SqlFunc.IsNullOrEmpty(t3.SupplierName))
                    .Select((t1, t2, t3) => new PurchaseSupplierAmountCountModel { Value = SqlFunc.AggregateSum(t1.PurchaseAmount), Name = t3.SupplierName })
                    .ToListAsync();
                return ResponseUtil<List<PurchaseSupplierAmountCountModel>>.SuccessResult(today);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<PurchaseSupplierAmountCountModel>>
                    .FailResult(null, $"统计采购订单金额发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 采购订单金额态势(天) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountDay(CurrentUser currentUser)
        {
            try
            {
                DateTime _thisDateTime = DateTime.Now;
                String y = _thisDateTime.ToString("yyyy");
                String m = _thisDateTime.ToString("MM");
                int day = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var today = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>()
                     .Where(t => t.CompanyId == currentUser.CompanyID
                       && t.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                   && t.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
                     )
                     .GroupBy(n => n.OrderDate)
                    .Select(n => new { n.OrderDate, PurchaseAmount = SqlFunc.AggregateSum(n.PurchaseAmount) })
                    .ToListAsync();

                string s1 = "";
                string s2 = "";
                for (int i = 0; i < day; i++)
                {
                    s1 += "|" + (i + 1);
                    int Count = today.Where(n => n.OrderDate == Convert.ToDateTime(y + "-" + m + "-" + (i + 1))).Count();
                    if (Count > 0)
                    {
                        var model = today.Where(n => n.OrderDate == Convert.ToDateTime(y + "-" + m + "-" + (i + 1)));
                        if (model != null)
                        {
                            s2 += "|" + String.Format("{0:N2}", model.Select(n => n.PurchaseAmount).ToList()[0]);
                        }
                    }
                    else
                    {
                        s2 += "|0.00";
                    }
                }
                PurchaseSupplierAmountCountDayModel _model = new PurchaseSupplierAmountCountDayModel { xAxisData = s1.Substring(1).Split('|'), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>
                    .FailResult(null, $"统计采购订单金额发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 采购订单金额态势(月) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountMonth(CurrentUser currentUser)
        {
            try
            {
                DateTime _thisDateTime = DateTime.Now;
                String y = _thisDateTime.ToString("yyyy");
                String m = _thisDateTime.ToString("MM");
                int day = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                //                var queryData = _db.Instance.Queryable<TWMProductionWhMainDbModel>()
                //.Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false);
                string s1 = "";
                string s2 = "";
                for (int i = 0; i < 12; i++)
                {
                    s1 += "|" + (i + 1);
                    var Month = (y + "-" + (i + 1));
                    DateTime dt1 = Convert.ToDateTime("" + y + "-" + (i + 1) + "-01");
                    DateTime dt2;
                    if ((i + 1) == 12)
                    {
                        dt2 = Convert.ToDateTime("" + (Convert.ToInt32(y) + 1) + "-01-01");
                    }
                    else
                    {
                        dt2 = Convert.ToDateTime("" + y + "-" + (i + 2) + "-01");
                    }
                    //            var query = await queryData.Where(t => t.WarehousingDate >= dt1 && t.WarehousingDate < dt2
                    //).Select(t => SqlFunc.AggregateSum(t.Number)).ToListAsync();
                    //            if (query.Count > 0)
                    //            {
                    //                s2 += "," + String.Format("{0:N2}", query[0]);
                    //            }
                    //            else
                    //            {
                    //                s2 += ", " + queryData.Where(t => t.WarehousingDate >= dt1 && t.WarehousingDate < dt2
                    //).Select(t => SqlFunc.AggregateSum(t.Number)).ToSql();
                    //            }

                    var today = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>()
     .Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false
     && t.OrderDate >= dt1 && t.OrderDate < dt2
     ).Select(t => SqlFunc.AggregateSum(t.PurchaseAmount)).ToListAsync();
                    if (today.Count > 0)
                    {
                        s2 += "|" + String.Format("{0:N2}", today[0]);
                    }
                    else
                    {
                        s2 += "|0.00";
                    }
                }
                PurchaseSupplierAmountCountDayModel _model = new PurchaseSupplierAmountCountDayModel { xAxisData = s1.Substring(1).Split('|'), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>
                    .FailResult(null, $"采购订单金额态势(天) 发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 采购订单金额态势(周) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountWeek(CurrentUser currentUser)
        {
            try
            {
                DateTime _thisDateTime = DateTime.Now;
                int y = Convert.ToInt32(_thisDateTime.ToString("yyyy"));
                int m = Convert.ToInt32(_thisDateTime.ToString("MM"));
                //获取本月的天数 
                var date_count = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                DateTime firstDay = _thisDateTime.AddDays(1 - _thisDateTime.Day);
                int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
                //本月第一周有几天
                int firstWeekEndDay = 7 - (weekday - 1);
                var days = date_count;
                var mod = firstWeekEndDay; //本月第一周有几天
                var count = 1;
                var start = 1;  //起始日期 1 号开始
                var s1 = "";
                var s2 = "";
                var ss1 = "";
                var ss2 = "";
                var end = start + mod - 1;//截止日期
                while (days >= 0)
                {
                    //var end = start + 6;
                    end = end > date_count ? date_count : end;
                    ss1 += "第" + count + "周:" + y + "年" + m + "月" + "（" + start + "-" + end + "）;" + "\n";
                    s1 += ",第" + count + "周";
                    DateTime dt1 = Convert.ToDateTime("" + y + "-" + m + "-" + start);

                    DateTime dt2;
                    if ((end + 1) > date_count)
                    {
                        dt2 = Convert.ToDateTime("" + y + "-" + (m + 1) + "-01");
                    }
                    else
                    {
                        dt2 = Convert.ToDateTime("" + y + "-" + m + "-" + (end + 1));
                    }

                    var today = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>()
.Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false
&& t.OrderDate >= dt1 && t.OrderDate < dt2
).Select(t => SqlFunc.AggregateSum(t.PurchaseAmount)).ToListAsync();
                    if (today.Count > 0)
                    {
                        s2 += "|" + String.Format("{0:N2}", today[0]);
                    }
                    else
                    {
                        s2 += "|0.00";
                    }
                    start = end + 1;
                    end += 7;
                    days -= 7;
                    ss2 += days + ",";
                    count++;
                }
                PurchaseSupplierAmountCountDayModel _model = new PurchaseSupplierAmountCountDayModel { xAxisData = s1.Substring(1).Split(','), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<PurchaseSupplierAmountCountDayModel>
                    .FailResult(null, $"采购订单金额态势(周) 发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 采购员采购业绩
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<PurchaseBuyerAmountCountModel>>> GetPurchaseBuyerAmountCount(CurrentUser currentUser)
        {
            try
            {
                var today = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel, TSMUserAccountDbModel>(
                    (t1, t2) => new object[]
                    {
                         JoinType.Left , t1.BuyerId == t2.ID
                    }
                    ).Where((t1, t2) => t1.CompanyId == currentUser.CompanyID)
                    .GroupBy((t1, t2) => t2.AccountName)
                     .Where((t1, t2) => !SqlFunc.IsNullOrEmpty(t2.AccountName)
                       && t1.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                   && t1.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
                     )
                    .Select((t1, t2) => new PurchaseBuyerAmountCountModel
                    {
                        Value = SqlFunc.AggregateSum(t1.PurchaseAmount),
                        OrderCount = SqlFunc.AggregateCount(t1.ID),
                        UserName = t2.AccountName
                    })
                    .ToListAsync();
                return ResponseUtil<List<PurchaseBuyerAmountCountModel>>.SuccessResult(today);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<PurchaseBuyerAmountCountModel>>
                    .FailResult(null, $"统计采购员采购业绩发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 采购业务员
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<PurchaseBuyerModel>>> GetSaleMan(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel, TSMUserAccountDbModel>(
                    (t1, t2) => new object[]
                    {
                        JoinType.Left , t1.BuyerId == t2.ID,
                    })
                    .Where((t1, t2) => t1.CompanyId == currentUser.CompanyID && !SqlFunc.IsNullOrEmpty(t2.AccountName)
                    )
                    .GroupBy((t1, t2) => new { t1.BuyerId })
                    .Select((t1, t2) => new PurchaseBuyerModel { Value = t1.BuyerId, Name = t2.AccountName })
                    .ToListAsync();
                return ResponseUtil<List<PurchaseBuyerModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<PurchaseBuyerModel>>
                    .FailResult(null, $"采购业务员发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 采购供应商
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<PurchaseSupplierModel>>> GetSupplier(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TPSMPurchaseOrderMainDbModel, TBMSupplierFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                        JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t1.SupplierId == t3.ID,
                    })
                    .Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && !SqlFunc.IsNullOrEmpty(t3.SupplierName))
                    .GroupBy((t1, t2, t3) => new { t1.SupplierId })
                    .Select((t1, t2, t3) => new PurchaseSupplierModel { Value = Convert.ToInt32(t1.SupplierId), Name = t3.SupplierName })
                    .ToListAsync();
                return ResponseUtil<List<PurchaseSupplierModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<PurchaseSupplierModel>>
                    .FailResult(null, $"采购供应商发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 采购单号
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<PurchaseOrderNoModel>>> GetPurchaseOrderNo(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>()
                    .Where(t => t.CompanyId == currentUser.CompanyID)
                    .Select(t => new PurchaseOrderNoModel { OrderNo = t.OrderNo })
                    .ToListAsync();
                return ResponseUtil<List<PurchaseOrderNoModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<PurchaseOrderNoModel>>
                    .FailResult(null, $"采购单号发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 采购一览表
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<PurchaseListModel>> GetPurchaseCountList(RequestGet request, CurrentUser currentUser)
        {
            try
            {
                List<PurchaseCountListModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TPSMPurchaseOrderMainDbModel
                    , TBMDictionaryDbModel, TBMSupplierFileDbModel, TSMUserAccountDbModel, TBMMaterialFileDbModel>(
                    (t1, t2, t3, t4, t5, t6) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                        JoinType.Left , t2.OrderTypeId == t1.ID,
                        JoinType.Left , t1.SupplierId == t4.ID,
                        JoinType.Left , t2.BuyerId == t5.ID,
                          JoinType.Left , t1.MaterialId == t6.ID
                    }
                    ).Where((t1, t2, t3, t4, t5, t6) => t2.CompanyId == currentUser.CompanyID)
                    .OrderBy((t1, t2, t3, t4, t5, t6) => t2.OrderNo);
                //查询条件
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);

                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "orderdate" ||
                            item.FieldName.ToLower() == "supplierid" ||
                            item.FieldName.ToLower() == "ordertypeid" ||
                            item.FieldName.ToLower() == "orderno")
                        {
                            item.FieldName = $"t2.{item.FieldName}";
                            continue;
                        }
                    }
                    query.Where(conditionals);
                }
                //排序条件
                if (request.OrderByConditions != null && request.OrderByConditions.Count > 0)
                {
                    foreach (var item in request.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TPSMPurchaseOrderMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t2.{item.Column} {item.Condition}");
                    }
                }

                PurchaseListModel _list = new PurchaseListModel();
                _list.Total_Number = query.Count();
                #region 统计数量
                var PurchaseNum = query.Select((t1, t2, t3, t4, t5, t6) => SqlFunc.AggregateSum(t1.PurchaseNum)).ToList();
                _list.Total_Num = PurchaseNum.Count > 0 ? Convert.ToDecimal(PurchaseNum.ToList()[0]) : 0;
                #endregion
                #region 统计金额
                var PurchaseAmount = query.Select((t1, t2, t3, t4, t5, t6) => SqlFunc.AggregateSum(t1.PurchaseAmount)).ToList();
                _list.Total_Amount = PurchaseAmount.Count > 0 ? Convert.ToDecimal(PurchaseAmount.ToList()[0]) : 0;
                #endregion
                if (request.IsPaging)
                {
                    int skipNum = request.PageSize * (request.PageIndex - 1);
                    queryData = await query
                      .Select((t1, t2, t3, t4, t5, t6) => new PurchaseCountListModel
                      {
                          OrderNo = t2.OrderNo,
                          OrderTypeId = t2.OrderTypeId,
                          OrderTypeName = t3.DicValue,
                          MaterialCode = t6.MaterialCode,
                          MaterialName = t6.MaterialName,
                          SupplierId = Convert.ToInt32(t1.SupplierId),
                          SupplierName = t4.SupplierName,
                          BuyerId = t2.BuyerId,
                          BuyerName = t5.AccountName,
                          PurchaseNum = t1.PurchaseNum,
                          UnitPrice = t1.UnitPrice,
                          PurchaseAmount = t1.PurchaseAmount,
                          AuditStatus = (byte)t2.AuditStatus,
                          OrderDate = t2.OrderDate
                      }).Skip(skipNum).Take(request.PageSize).ToListAsync();
                }
                else
                {
                    queryData = await query
                       .Select((t1, t2, t3, t4, t5, t6) => new PurchaseCountListModel
                       {
                           OrderNo = t2.OrderNo,
                           OrderTypeId = t2.OrderTypeId,
                           OrderTypeName = t3.DicValue,
                           MaterialCode = t6.MaterialCode,
                           MaterialName = t6.MaterialName,
                           SupplierId = Convert.ToInt32(t1.SupplierId),
                           SupplierName = t4.SupplierName,
                           BuyerId = t2.BuyerId,
                           BuyerName = t5.AccountName,
                           PurchaseNum = t1.PurchaseNum,
                           UnitPrice = t1.UnitPrice,
                           PurchaseAmount = t1.PurchaseAmount,
                           AuditStatus = (byte)t2.AuditStatus,
                           OrderDate = t2.OrderDate
                       })
                       .ToListAsync();
                }
                _list.List = queryData;
                return ResponseUtil<PurchaseListModel>.SuccessResult(_list);
            }
            catch (Exception ex)
            {
                return ResponseUtil<PurchaseListModel>
                    .FailResult(null, $"统计采购列表发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
    }
}