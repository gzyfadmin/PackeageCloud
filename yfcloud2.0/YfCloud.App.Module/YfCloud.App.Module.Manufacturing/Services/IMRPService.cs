using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOMSum;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Services
{
    /// <summary>
    /// MRP接口
    /// </summary>
    public interface IMRPService
    {
        /// <summary>
        /// 自动计算
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="Type"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<MrpResultModel>> AutoComputeMRP(int orderID, int Type, CurrentUser currentUser);

        /// <summary>
        /// 转领料
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<NOEntity>> TransPick(int orderID, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<NOEntity>> TransPurchase(int orderID, CurrentUser currentUser);

        /// <summary>
        /// BOM清单
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<SumQueryApiModel>> GetBom(int orderID, CurrentUser currentUser);

        Task<ResponseObject<NOEntity>> RePick(int orderID, CurrentUser currentUser);

        /// <summary>
        /// 获取生产单对应的EXCEL数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        AllBomExcel GetProductDataForExcel(int orderID, CurrentUser currentUser);
    }
}
