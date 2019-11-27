///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMPurchaseMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:11:26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseMain;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMPurchaseMain Interface
    /// </summary>
    public interface ITWMPurchaseMainService 
    {
        /// <summary>
        /// 查询TWMPurchaseMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMPurchaseMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser);

        Task<ResponseObject<TWMPurchaseMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);

        /// <summary>
        /// 新增TWMPurchaseMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMPurchaseMainQueryModel>> PostAsync(RequestPost<TWMPurchaseMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TWMPurchaseMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMPurchaseMainQueryModel>> PutAsync(RequestPut<TWMPurchaseMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMPurchaseMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMPurchaseMainAduit> requestObject, CurrentUser currentUser);

    }
}