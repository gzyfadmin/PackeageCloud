using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models.Sales;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 销售报表
    /// </summary>
    public interface ISaleRepostServcie
    {
        /// <summary>
        /// 获取订单金额统计信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<SalesAmountCountModel>> GetSalesAmountCount(CurrentUser currentUser);

        /// <summary>
        /// 获取业务员订单金额统计信息，当前月份
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<SalesmanAmountCountModel>>> GetSalesmanAmountCount(CurrentUser currentUser);

        /// <summary>
        /// 获取所有已有底单的销售业务员
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<SalesManModel>>> GetSaleMan(CurrentUser currentUser);

        /// <summary>
        /// 获取所有已有订单的客户信息
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<SalesCustomerModel>>> GetCustomers(CurrentUser currentUser);

        /// <summary>
        /// 款型分布
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<SalesPackageAmountCountModel>>> GetSalePackageCount(CurrentUser currentUser);
        /// <summary>
        /// 获取销售一览表信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<SalesListModel>> GetSalesList(RequestGet request, CurrentUser currentUser);
    }
}
