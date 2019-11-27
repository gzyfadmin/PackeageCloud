using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Purchase;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 采购报表
    /// </summary>
    public interface IPurchaseReportService
    {
        /// <summary>
        /// 获取订单金额统计信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<PurchaseSupplierAmountCountModel>>> GetPurchaseSupplierAmountCount(CurrentUser currentUser);

        /// <summary>
        /// 采购订单金额态势(天) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountDay(CurrentUser currentUser);
        /// <summary>
        /// 采购订单金额态势(月) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountMonth(CurrentUser currentUser);
        /// <summary>
        /// 采购订单金额态势(周) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountWeek(CurrentUser currentUser);
        /// <summary>
        /// 采购员采购业绩
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<PurchaseBuyerAmountCountModel>>> GetPurchaseBuyerAmountCount(CurrentUser currentUser);
        /// <summary>
        /// 采购业务员
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<PurchaseBuyerModel>>> GetSaleMan(CurrentUser currentUser);
        /// <summary>
        /// 采购供应商
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<PurchaseSupplierModel>>> GetSupplier(CurrentUser currentUser);
        /// <summary>
        /// 采购单号
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<PurchaseOrderNoModel>>> GetPurchaseOrderNo(CurrentUser currentUser);
        /// <summary>
        /// 采购一览表
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<PurchaseListModel>> GetPurchaseCountList(RequestGet request, CurrentUser currentUser);

    }
}
