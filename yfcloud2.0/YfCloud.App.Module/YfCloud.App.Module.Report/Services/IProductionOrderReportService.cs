using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.ProductionOrder;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Services
{
    public interface IProductionOrderReportService
    {
        /// <summary>
        /// 生产车间入库数量
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<ProductionOrderWorkshopCountModel>>> GetProductionOrderWorkshop(CurrentUser currentUser);
        /// <summary>
        /// 生产入库态势(天)
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingDay(CurrentUser currentUser);

        /// <summary>
        /// 生产入库态势(月)
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingMonth(CurrentUser currentUser);

        /// <summary>
        /// 生产入库态势(周)
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingWeek(CurrentUser currentUser);

        /// <summary>
        /// 生产订单数量分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ProductionOrderQuantityAnalysisModel>> GetProductionOrderQuantityAnalysis(CurrentUser currentUser);

        /// <summary>
        /// 生产客户
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<ProductionOrderCustomerModel>>> GetCustomer(CurrentUser currentUser);

        /// <summary>
        /// 生产订单号
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<ProductionOrderNoModel>>> GetProductionNo(CurrentUser currentUser);
        /// <summary>
        /// 生产一览表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ProductionOrderListModel>> GetProdutionCountList(RequestGet request, CurrentUser currentUser);
    }
}
