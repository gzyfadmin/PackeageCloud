using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Sales;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 销售订单
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ISaleRepostServcie))]
    public class SaleRepostServcie : ISaleRepostServcie
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<ISaleRepostServcie> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        /// <summary>
        /// 默认构造方法
        /// </summary>
        public SaleRepostServcie(IDbContext dbContext, ILogger<ISaleRepostServcie> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取所有已有底单的销售业务员
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<SalesManModel>>> GetSaleMan(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel, TSMUserAccountDbModel>(
                    (t1, t2) => new object[]
                    {
                        JoinType.Left , t1.SalesmanId == t2.ID,
                    })
                    .Where((t1, t2) => t1.CompanyId == currentUser.CompanyID)
                    .GroupBy((t1, t2) => new { t1.SalesmanId })
                    .Select((t1, t2) => new SalesManModel { Value = t1.SalesmanId, Name = t2.AccountName })
                    .ToListAsync();
                return ResponseUtil<List<SalesManModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<SalesManModel>>
                    .FailResult(null, $"销售业务员发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }
        /// <summary>
        /// 获取所有已有订单的客户信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<SalesCustomerModel>>> GetCustomers(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel>(
                    (t, t0) => new object[]
                    {
                        JoinType.Left , t.CustomerId == t0.ID
                    })
                    .Where((t, t0) => t.CompanyId == currentUser.CompanyID)
                    .GroupBy((t, t0) => new { t0.ID, t0.CustomerName })
                    .Select((t, t0) => new SalesCustomerModel
                    {
                        ID = t0.ID,
                        CustomerName = t0.CustomerName
                    })
                    .ToListAsync();

                return ResponseUtil<List<SalesCustomerModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<SalesCustomerModel>>
                    .FailResult(null, $"获取客户信息发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }
        /// <summary>
        /// 获取销售订单金额统计信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<SalesAmountCountModel>> GetSalesAmountCount(CurrentUser currentUser)
        {
            try
            {
                var today = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel>()
                    .Where(p => p.CompanyId == currentUser.CompanyID && p.OrderDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                    )
                    .SumAsync(p => p.SalesAmount);
                var month = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel>()
                    .Where(p => p.CompanyId == currentUser.CompanyID
                    && p.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                    && p.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
                    .SumAsync(p => p.SalesAmount);
                var model = new SalesAmountCountModel
                {
                    xAxisData = new string[] { "今日", "本月" },
                    SeriesData = new string[] { today.ToString(), month.ToString() }
                };
                return ResponseUtil<SalesAmountCountModel>
                    .SuccessResult(model);
            }
            catch (Exception ex)
            {
                return ResponseUtil<SalesAmountCountModel>
                    .FailResult(null, $"统计销售订单金额发生异常{System.Environment.NewLine} {ex.Message}");
            }
        }
        /// <summary>
        /// 获取当前月份销售订单统计信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<SalesmanAmountCountModel>>> GetSalesmanAmountCount(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TSSMSalesOrderMainDbModel, TSMUserAccountDbModel>(
                    (t, t0) => new object[]
                    {
                        JoinType.Left , t.SalesmanId == t0.ID
                    })
                    .Where((t, t0) => t.CompanyId == currentUser.CompanyID
                     && t.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                     && t.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01"))
                    .GroupBy((t, t0) => t0.AccountName)
                    .Select((t, t0) => new SalesmanAmountCountModel { Value = SqlFunc.AggregateSum(t.SalesAmount), Name = t0.AccountName })
                    .ToListAsync();
                return ResponseUtil<List<SalesmanAmountCountModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<SalesmanAmountCountModel>>
                    .FailResult(null, $"统计当月销售订单信息发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }
        /// <summary>
        /// 款型分布
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<SalesPackageAmountCountModel>>> GetSalePackageCount(CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Queryable<TSSMSalesOrderDetailDbModel, TSSMSalesOrderMainDbModel, TBMMaterialFileDbModel, TBMPackageDbModel>(
                    (t1, t2, t3, t4) => new object[]
                    {
                        JoinType.Left , t1.MainId == t2.ID,
                        JoinType.Left , t1.MaterialId == t3.ID,
                         JoinType.Left , t3.PackageID == t4.ID
                    })
                    .Where((t1, t2, t3, t4) => t2.CompanyId == currentUser.CompanyID
                     && t2.OrderDate >= Convert.ToDateTime($"{DateTime.Now.ToString("yyyy-MM")}-01")
                     && t2.OrderDate < Convert.ToDateTime($"{DateTime.Now.AddMonths(1).ToString("yyyy-MM")}-01")
                     && t3.PackageID != null)
                    .GroupBy((t1, t2, t3, t4) => new { t4.DicValue })
                    .Select((t1, t2, t3, t4) => new SalesPackageAmountCountModel { Value = SqlFunc.AggregateSum(t1.SalesNum), Name = t4.DicValue })
                    .ToListAsync();
                return ResponseUtil<List<SalesPackageAmountCountModel>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<SalesPackageAmountCountModel>>
                    .FailResult(null, $"款型分布发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }
        /// <summary>
        /// 获取销售一览表信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<SalesListModel>> GetSalesList(RequestGet request, CurrentUser currentUser)
        { 
            try
            {
                List<SalesCountListModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数

                var query = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel, TSSMSalesOrderMainDbModel,
                    TBMDictionaryDbModel, TBMCustomerFileDbModel, TSMUserAccountDbModel, TBMPackageDbModel>(
                    (t, t0, t1, t2, t3, t4) => new object[]
                    {
                        JoinType.Left , t.MainId == t0.ID,
                        JoinType.Left , t0.OrderTypeId == t1.ID,
                        JoinType.Left , t0.CustomerId == t2.ID,
                        JoinType.Left , t0.SalesmanId == t3.ID,
                        JoinType.Left , t.PackageId == t4.ID
                    })
                    .Where((t, t0, t1, t2, t3, t4) => t0.CompanyId == currentUser.CompanyID && t4.DicValue != null);
                //.OrderBy((t, t0, t1, t2, t3, t4) => t0.OrderNo);

                //查询条件
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);

                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "orderdate" ||
                            item.FieldName.ToLower() == "customerid" ||
                            item.FieldName.ToLower() == "ordertypeid" ||
                            item.FieldName.ToLower() == "salesmanid")
                        {
                            item.FieldName = $"t0.{item.FieldName}";
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TSSMSalesOrderMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t0.{item.Column} {item.Condition}");
                    }
                }
                else
                {
                    query.OrderBy((t, t0, t1, t2, t3, t4) => t0.OrderNo);
                }

                SalesListModel _list = new SalesListModel();
                _list.Total_Number = query.Count();
                #region 统计数量
                var SalesNum = query.Select((t, t0, t1, t2, t3, t4) => SqlFunc.AggregateSum(t.SalesNum)).ToList();
                _list.Total_Num = SalesNum.Count > 0 ? Convert.ToDecimal(SalesNum.ToList()[0]) : 0;
                #endregion
                #region 统计金额
                var SalesAmount = query.Select((t, t0, t1, t2, t3, t4) => SqlFunc.AggregateSum(t.SalesAmount)).ToList();
                _list.Total_Amount = SalesAmount.Count > 0 ? Convert.ToDecimal(SalesAmount.ToList()[0]) : 0;
                #endregion
                if (request.IsPaging)
                {
                    int skipNum = request.PageSize * (request.PageIndex - 1);
                    queryData = await query
                                   .Select((t, t0, t1, t2, t3, t4) => new SalesCountListModel
                                   {
                                       OrderNo = t0.OrderNo,
                                       OrderTypeId = t0.OrderTypeId,
                                       OrderTypeName = t1.DicValue,
                                       CustomerId = t0.CustomerId,
                                       CustomerName = t2.CustomerName,
                                       SalesManId = t0.SalesmanId,
                                       SalesManName = t3.AccountName,
                                       PackageId = t.PackageId,
                                       PackageName = t4.DicValue,
                                       SalesNum = t.SalesNum,
                                       SalesAmount = t.UnitPrice,
                                       AuditStatus = (byte)t0.AuditStatus,
                                       OrderDate = t0.OrderDate
                                   }).Skip(skipNum).Take(request.PageSize).ToListAsync();
                }
                else
                {
                    queryData = await query
               .Select((t, t0, t1, t2, t3, t4) => new SalesCountListModel
               {
                   OrderNo = t0.OrderNo,
                   OrderTypeId = t0.OrderTypeId,
                   OrderTypeName = t1.DicValue,
                   CustomerId = t0.CustomerId,
                   CustomerName = t2.CustomerName,
                   SalesManId = t0.SalesmanId,
                   SalesManName = t3.AccountName,
                   PackageId = t.PackageId,
                   PackageName = t4.DicValue,
                   SalesNum = t.SalesNum,
                   SalesAmount = t.UnitPrice,
                   AuditStatus = (byte)t0.AuditStatus,
                   OrderDate = t0.OrderDate
               })
               .ToListAsync();
                }
                _list.List = queryData;
                return ResponseUtil<SalesListModel>.SuccessResult(_list);
            }
            catch (Exception ex)
            {
                return ResponseUtil<SalesListModel>
                    .FailResult(null, $"查询销售一览表发生异常{System.Environment.NewLine}{ex.Message}");
            }
        }
    }
}
