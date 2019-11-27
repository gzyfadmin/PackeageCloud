///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMOtherWhSendMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12 11:06:30
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendMain;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendDetail;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMOtherWhSendMain Interface
    /// </summary>
    public interface ITWMOtherWhSendMainService 
    {
        /// <summary>
        /// 查询TWMOtherWhSendMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMOtherWhSendMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TWMOtherWhSendMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhSendMainQueryModel>> PostAsync(RequestPost<TWMOtherWhSendMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhSendMainQueryModel>> PutAsync(RequestPut<TWMOtherWhSendMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMOtherWhSendMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 出库审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> OtherWhAuditAsync(RequestPut<TWMOtherWhSendAuditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 获取出库单所有的信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhSendMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);
    }
}