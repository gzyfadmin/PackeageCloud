using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.ProductionOrder;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;


namespace YfCloud.App.Module.Report.Services
{
    [UseDI(ServiceLifetime.Scoped, typeof(IProductionOrderReportService))]
    public class ProductionOrderReportService : IProductionOrderReportService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IProductionOrderReportService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        /// <summary>
        /// 默认构造方法
        /// </summary>
        public ProductionOrderReportService(IDbContext dbContext, ILogger<IProductionOrderReportService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 生产车间入库数量
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<ProductionOrderWorkshopCountModel>>> GetProductionOrderWorkshop(CurrentUser currentUser)
        {
            try
            {
                List<ProductionOrderWorkshopCountModel> queryData = null;//查询结果集对象
                var query1 = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel, TBMDictionaryDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                          JoinType.Left , t1.MainId == t2.ID,
                          JoinType.Left , t1.WorkshopId == t3.ID
                    }
                    ).Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID
                    )
                    .GroupBy((t1, t2, t3) => new { t2.ID, t1.WorkshopId, t3.DicValue })
                    .Select((t1, t2, t3) => new ProductionOrderWorkshopCountModel1 { ID = t2.ID, WorkshopId = Convert.ToInt32(t1.WorkshopId), WorkshopName = t3.DicValue });

                var s1 = query1.ToListAsync();
                var query2 = _db.Instance.Queryable<TMMWhApplyMainDbModel, TMMWhApplyDetailDbModel>(
                     (t1, t2) => new object[]
                    {
                          JoinType.Left , t1.ID == t2.MainId
                    }
                    ).Where((t1, t2) => t1.CompanyId == currentUser.CompanyID && t1.AuditStatus == 2 && t1.DeleteFlag == false
                    )
                    .GroupBy((t1, t2) => new { t1.SourceId, t1.ID })
                    .Select((t1, t2) => new ProductionOrderWorkshopCountModel2 { ID = t1.ID, SourceId = Convert.ToInt32(t1.SourceId), Number = SqlFunc.AggregateSum(t2.ApplyNum) });

                var s2 = query2.ToListAsync();
                var ss2 = query2.ToSql();
                var query3 = _db.Instance.Queryable<TWMProductionWhMainDbModel>().Where(t1 => t1.CompanyId == currentUser.CompanyID && t1.AuditStatus == 2 && t1.DeleteFlag == false
                && t1.WarehousingDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                )
                    .GroupBy(t => t.SourceId).Select(t => new ProductionOrderWorkshopCountModel3 { SourceId = t.SourceId, Number = SqlFunc.AggregateSum(t.Number) });
                var s3 = query3.ToListAsync();
                var ss3 = query3.ToSql();
                var truequery = _db.Instance.Queryable(query2, query3, JoinType.Inner, (p1, p2) => p1.ID == p2.SourceId)
                    .Select((p1, p2) => new ProductionOrderWorkshopCountModel2 { ID = p1.ID, SourceId = p1.SourceId, Number = p2.Number });
                var s4 = truequery.ToListAsync();
                var ss4 = truequery.ToSql();



                var ts = _db.Instance.Queryable(query1, truequery, JoinType.Inner, (p1, p2) => p1.ID == p2.SourceId);

                var Count = ts
                     .Where((p1, p2) => p1.WorkshopName != null)
                     .Select((p1, p2) => SqlFunc.AggregateSum(p2.Number));
                var Count1 = Count.ToListAsync().Result[0];

                queryData = await ts
                     .GroupBy((p1, p2) => p1.WorkshopName)
                     .Where((p1, p2) => p1.WorkshopName != null)
     .Select((p1, p2) => new ProductionOrderWorkshopCountModel
     {
         Name = p1.WorkshopName,
         Value = SqlFunc.AggregateSum(p2.Number),
         NumberCount = Count1
     }).ToListAsync();
                return ResponseUtil<List<ProductionOrderWorkshopCountModel>>.SuccessResult(queryData);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ProductionOrderWorkshopCountModel>>
                    .FailResult(null, $"生产车间入库数量发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 生产入库态势(天) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingDay(CurrentUser currentUser)
        {
            try
            {
                DateTime _thisDateTime = DateTime.Now;
                String y = _thisDateTime.ToString("yyyy");
                String m = _thisDateTime.ToString("MM");
                int day = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var today = await _db.Instance.Queryable<TWMProductionWhMainDbModel>()
                     .Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false)
                     .GroupBy(n => n.WarehousingDate)
                    .Select(n => new { n.WarehousingDate, Number = SqlFunc.AggregateSum(n.Number) })
                    .ToListAsync();

                string s1 = "";
                string s2 = "";
                for (int i = 0; i < day; i++)
                {
                    s1 += "|" + (i + 1);
                    int Count = today.Where(n => n.WarehousingDate == Convert.ToDateTime(y + "-" + m + "-" + (i + 1))).Count();
                    if (Count > 0)
                    {
                        var model = today.Where(n => n.WarehousingDate == Convert.ToDateTime(y + "-" + m + "-" + (i + 1)));
                        if (model != null)
                        {
                            s2 += "|" + String.Format("{0:N2}", model.Select(n => n.Number).ToList()[0]);
                        }
                    }
                    else
                    {
                        s2 += "|0.00";
                    }
                }
                ProductionWarehousingModel _model = new ProductionWarehousingModel { xAxisData = s1.Substring(1).Split('|'), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<ProductionWarehousingModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<ProductionWarehousingModel>
                    .FailResult(null, $"生产入库态势(天) 发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 生产入库态势(月) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingMonth(CurrentUser currentUser)
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

                    var today = await _db.Instance.Queryable<TWMProductionWhMainDbModel>()
     .Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false
     && t.WarehousingDate >= dt1 && t.WarehousingDate < dt2
     ).Select(t => SqlFunc.AggregateSum(t.Number)).ToListAsync();
                    if (today.Count > 0)
                    {
                        s2 += "|" + String.Format("{0:N2}", today[0]);
                    }
                    else
                    {
                        s2 += "|0.00";
                    }
                }
                ProductionWarehousingModel _model = new ProductionWarehousingModel { xAxisData = s1.Substring(1).Split('|'), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<ProductionWarehousingModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<ProductionWarehousingModel>
                    .FailResult(null, $"生产入库态势(月) 发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 生产入库态势(周) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingWeek(CurrentUser currentUser)
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

                    var today = await _db.Instance.Queryable<TWMProductionWhMainDbModel>()
.Where(t => t.CompanyId == currentUser.CompanyID && t.AuditStatus == 2 && t.DeleteFlag == false
&& t.WarehousingDate >= dt1 && t.WarehousingDate < dt2
).Select(t => SqlFunc.AggregateSum(t.Number)).ToListAsync();
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
                ProductionWarehousingModel _model = new ProductionWarehousingModel { xAxisData = s1.Substring(1).Split(','), SeriesData = s2.Substring(1).Split('|') };
                return ResponseUtil<ProductionWarehousingModel>.SuccessResult(_model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<ProductionWarehousingModel>
                    .FailResult(null, $"生产入库态势(周) 发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 生产订单数量分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ProductionOrderQuantityAnalysisModel>> GetProductionOrderQuantityAnalysis(CurrentUser currentUser)
        {
            try
            {
                var Totalquery = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>(
                     (t1, t2) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID
                    }
                    )
                    .Where((t1, t2) => t2.CompanyId == currentUser.CompanyID
                    && t2.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                    && t2.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
                    )
                    .Select((t1, t2) => SqlFunc.AggregateSum(t1.ProductionNum)).ToList();


                var ToDayquery = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>(
                     (t1, t2) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID
                    }
                    )
                    .Where((t1, t2) => t2.CompanyId == currentUser.CompanyID
                    && t2.OrderDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                && t2.OrderDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"))
                    )
                    .Select((t1, t2) => SqlFunc.AggregateSum(t1.ProductionNum)).ToList();
                ProductionOrderQuantityAnalysisModel poqam = new ProductionOrderQuantityAnalysisModel
                {
                    SeriesData = Convert.ToString(Totalquery[0] + "|" + ToDayquery[0]).Split("|")
                };
                return ResponseUtil<ProductionOrderQuantityAnalysisModel>.SuccessResult(poqam);
            }
            catch (Exception ex)
            {
                return ResponseUtil<ProductionOrderQuantityAnalysisModel>
                    .FailResult(null, $"生产订单数量分析发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 销售客户
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<ProductionOrderCustomerModel>>> GetCustomer(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel, TBMCustomerFileDbModel>(
                    (t1, t2, t3) => new object[]
                    {
                        JoinType.Left , t1.MainId == t2.ID,
                         JoinType.Left , t2.CustomerId == t3.ID,
                    })
                    .Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && !SqlFunc.IsNullOrEmpty(t3.CustomerName))
                    .GroupBy((t1, t2, t3) => new { t3.CustomerName })
                    .Select((t1, t2, t3) => new ProductionOrderCustomerModel { Value = Convert.ToInt32(t2.CustomerId), Name = t3.CustomerName })
                    .ToListAsync();
                return ResponseUtil<List<ProductionOrderCustomerModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ProductionOrderCustomerModel>>
                    .FailResult(null, $"销售客户发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 生产订单号
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<ProductionOrderNoModel>>> GetProductionNo(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TMMProductionOrderMainDbModel>()
             .Where(t => t.CompanyId == currentUser.CompanyID)
             .Select(t => new ProductionOrderNoModel { ProductionNo = t.ProductionNo })
             .ToListAsync();
                return ResponseUtil<List<ProductionOrderNoModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ProductionOrderNoModel>>
                    .FailResult(null, $"生产订单号发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }


        /// <summary>
        /// 生产一览表
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ProductionOrderListModel>> GetProdutionCountList(RequestGet request, CurrentUser currentUser)
        {
            try
            {
                List<ProductionOrderCountListModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel
                    , TBMDictionaryDbModel, TBMDictionaryDbModel, TBMCustomerFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4, t5, t6) => new object[]
                    {
                         JoinType.Left , t1.MainId == t2.ID,
                        JoinType.Left , t2.ProductionType == t3.ID,
                        JoinType.Left , t1.WorkshopId == t4.ID,
                        JoinType.Left , t2.CustomerId == t5.ID,
                        JoinType.Left , t1.PackageId == t6.ID
                    }
                    ).Where((t1, t2, t3, t4, t5, t6) => t2.CompanyId == currentUser.CompanyID)
                    .OrderBy((t1, t2, t3, t4, t5, t6) => t2.ID, OrderByType.Desc);
                //查询条件
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);

                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "orderdate" || item.FieldName.ToLower() == "customerid" || item.FieldName.ToLower() == "productionno" || item.FieldName.ToLower() == "productiontype")
                        {
                            item.FieldName = $"t2.{item.FieldName}";
                            continue;
                        }
                        if (item.FieldName.ToLower() == "packageid" || item.FieldName.ToLower() == "workshopid")
                        {
                            item.FieldName = $"t1.{item.FieldName}";
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMDictionaryDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t2.{item.Column} {item.Condition}");
                    }
                }


                var today = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel
                    , TMMWhApplyDetailDbModel>(
                    (t1, t2, t3) => new object[] {
                        JoinType.Left, t1.MainId == t2.ID,
                        JoinType.Left, t1.ProOrderDetailId == t3.ID
                    }
                    )
.Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false
&& t2.WarehousingDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
&& t2.WarehousingDate < Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
.GroupBy((t1, t2, t3) => t3.ProOrderDetailId)
.Select((t1, t2, t3) => new { ProOrderDetailId = t3.ProOrderDetailId, ToDayNum = SqlFunc.AggregateSum(t1.ActualNum) })
.ToList().ToDictionary(p => p.ProOrderDetailId, p => p.ToDayNum);

                var Total = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel
                    , TMMWhApplyDetailDbModel>(
                    (t1, t2, t3) => new object[] {
                        JoinType.Left, t1.MainId == t2.ID,
                        JoinType.Left, t1.ProOrderDetailId == t3.ID
                    }
                    )
.Where((t1, t2, t3) => t2.CompanyId == currentUser.CompanyID && t2.AuditStatus == 2 && t2.DeleteFlag == false)
.GroupBy((t1, t2, t3) => t3.ProOrderDetailId)
.Select((t1, t2, t3) => new { ProOrderDetailId = t3.ProOrderDetailId, ToDayNum = SqlFunc.AggregateSum(t1.ActualNum) })
.ToList().ToDictionary(p => p.ProOrderDetailId, p => p.ToDayNum);

                ProductionOrderListModel _list = new ProductionOrderListModel();
                _list.Total_Number = query.Count();
                #region 统计生产数量
                var PurchaseNum = query.Select((t1, t2, t3, t4, t5, t6) => SqlFunc.AggregateSum(t1.ProductionNum)).ToList();
                _list.Total_Num = PurchaseNum.Count > 0 ? Convert.ToDecimal(PurchaseNum.ToList()[0]) : 0;
                #endregion 
                if (request.IsPaging)
                {
                    int skipNum = request.PageSize * (request.PageIndex - 1);
                    queryData = await query
                   .Select((t1, t2, t3, t4, t5, t6) => new ProductionOrderCountListModel
                   {
                       ID = t2.ID,
                       ProductionNo = t2.ProductionNo,
                       DetailID = t1.ID,
                       PackageName = t6.DicValue,
                       CustomerName = t5.CustomerName,
                       ProductionTypeId = t2.ProductionType,
                       ProductionNum = t1.ProductionNum,
                       WorkshopName = t4.DicValue,
                       ToDayNum = 0,
                       TotalNum = 0,
                       OrderDate = t2.OrderDate
                   }).Skip(skipNum).Take(request.PageSize).ToListAsync();
                }
                else
                {
                    queryData = await query
                      .Select((t1, t2, t3, t4, t5, t6) => new ProductionOrderCountListModel
                      {
                          ID = t2.ID,
                          ProductionNo = t2.ProductionNo,
                          DetailID = t1.ID,
                          PackageName = t6.DicValue,
                          CustomerName = t5.CustomerName,
                          ProductionTypeId = t2.ProductionType,
                          ProductionNum = t1.ProductionNum,
                          WorkshopName = t4.DicValue,
                          ToDayNum = 0,
                          TotalNum = 0,
                          OrderDate = t2.OrderDate
                      })
                      .ToListAsync();
                }

                queryData.ForEach(x =>
                {
                    if (today.ContainsKey(x.DetailID))
                    {
                        x.ToDayNum = today[x.DetailID];
                        _list.Total_DayNum += x.ToDayNum;
                    }
                    if (Total.ContainsKey(x.DetailID))
                    {
                        x.TotalNum = Total[x.DetailID];
                        _list.Total_TotalNum += x.TotalNum;
                    }
                });
                _list.List = queryData;
                return ResponseUtil<ProductionOrderListModel>.SuccessResult(_list);
            }
            catch (Exception ex)
            {
                return ResponseUtil<ProductionOrderListModel>
                    .FailResult(null, $"生产一览表发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
    }
}
