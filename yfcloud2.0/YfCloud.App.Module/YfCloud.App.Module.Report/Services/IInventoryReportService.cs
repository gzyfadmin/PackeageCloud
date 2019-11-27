using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Inventory;
using YfCloud.App.Module.Report.Models.Warehouse;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Services
{
    public interface IInventoryReportService
    {
        /// <summary>
        /// 总入库量，出库量，库存分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<InventoryAmountCountModel>> GetInventoryAmountCount(CurrentUser currentUser);

        /// <summary>
        /// 成品库存态势
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<InventoryAmountCountDayModel>> GetInventoryAmountCountDay(CurrentUser currentUser);
        /// <summary>
        /// 库存量分析
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<InventoryAnalysisModel>>> GetInventoryAnalysis(CurrentUser currentUser);

        /// <summary>
        /// 库存一览表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<InventoryListModel>> GetInventoryCountList(RequestGet request, CurrentUser currentUser);
    }
}
