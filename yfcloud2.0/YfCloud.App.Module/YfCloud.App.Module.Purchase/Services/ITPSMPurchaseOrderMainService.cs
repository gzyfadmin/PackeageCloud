///////////////////////////////////////////////////////////////////////////////////////
// File: ITPSMPurchaseOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:35:08
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderMain;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail;
using YfCloud.DBModel;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetailSum;

namespace YfCloud.App.Module.Purchase.Service
{
    /// <summary>
    /// TPSMPurchaseOrderMain Interface
    /// </summary>
    public interface ITPSMPurchaseOrderMainService 
    {
        /// <summary>
        /// 查询TPSMPurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TPSMPurchaseOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currUser);
        
        /// <summary>
        /// 查询TPSMPurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TPSMPurchaseOrderDetailQueryModel>>> GetDetailListAsync(int requestObject,CurrentUser currentUser);

        /// <summary>
        /// 查询采购单转入库单明细数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TPSMPurchaseOrderDetailQuerySumModel>>> GetTransferListAsync(int requestObject, CurrentUser current);

        /// <summary>
        /// 新增TPSMPurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> PostAsync(RequestPost<TPSMPurchaseOrderMainAddModel> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 修改TPSMPurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> PutAsync(RequestPut<TPSMPurchaseOrderMainEditModel> requestObject, CurrentUser currentUser);
        
        /// <summary>
        /// 采购单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PurchaseAuditAsync(RequestPut<TPSMPurchaseOrderAuditModel> requestPut, CurrentUser currentUser);
       
        /// <summary>
        /// 终止采购入库
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser);

        /// <summary>
        /// 删除TPSMPurchaseOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);
        
    }
}