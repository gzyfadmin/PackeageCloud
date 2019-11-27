///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMDeficitMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13 15:45:36
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitMain;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMDeficitMain Interface
    /// </summary>
    public interface ITWMDeficitMainService 
    {
        /// <summary>
        /// 查询TWMDeficitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMDeficitMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TWMDeficitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMDeficitMainQueryModel>> PostAsync(RequestPost<TWMDeficitMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TWMDeficitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMDeficitMainQueryModel>> PutAsync(RequestPut<TWMDeficitMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMDeficitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 获取出库单的所有信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMDeficitMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMDeficitMainAuditModel> requestObject, CurrentUser currentUser);

    }
}