using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailNP;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderMainNP;
using YfCloud.Models;

namespace YfCloud.App.Module.Sales.Services
{
    /// <summary>
    /// 物料销售接口
    /// </summary>
    public interface ITSSMSalesOrderMainNPService
    {
        /// <summary>
        /// 查询TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderMainQueryNPModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 查询TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderDetailQueryNPModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 查询可转出库单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderDetailQueryNPModel>>> GetTransferList(int requestObject);

        /// <summary>
        /// 新增TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<TSSMSalesOrderMainQueryNPModel>> PostAsync(RequestPost<TSSMSalesOrderMainAddNPModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TSSMSalesOrderMainEditNPModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 审核销售订单
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TSSMSalesOrderMainAuditNPModel> requestPut, CurrentUser currentUser);

        /// <summary>
        /// 终止出库
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopDeposit(int ID, CurrentUser currentUser);

        /// <summary>
        /// 删除TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
    }
}
