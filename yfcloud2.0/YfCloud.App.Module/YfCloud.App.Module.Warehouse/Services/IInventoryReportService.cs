using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Warehouse.Models.InventoryReport;
using YfCloud.Models;

namespace YfCloud.App.Module.Warehouse.Services
{
    /// <summary>
    /// 库存统计报表服务
    /// </summary>
    public interface IInventoryReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<InventoryResultModel>>> LoadReport(RequestGet requestObject, CurrentUser currentUser);
        /// <summary>
        /// 导出期初模板
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<ExportOpeningTemplateModel>> ExportOpeningTemplate(CurrentUser currentUser);

        /// <summary>
        /// 是否已经导入期初
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> IsImported(CurrentUser currentUser);


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<string>>> ImportPrime(RequestPost<ImportPrimeModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<HistoryInventory>>> History(HistoryInventoryQuery requestObject, CurrentUser currentUser);
    }
}
