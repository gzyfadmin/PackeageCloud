///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMOtherWhMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7 14:10:15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMOtherWhMain Interface
    /// </summary>
    public interface ITWMOtherWhMainService 
    {
        /// <summary>
        /// 获取入库单主表和详情信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhMainQueryModel>> GetWarehouseOrderAsync(int iMainId, CurrentUser currentUser);

        /// <summary>
        /// 查询TWMOtherWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currUser">当前操作用户信息</param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhMainQueryModel, List<TWMOtherWhMainQueryModel>>> GetMainListAsync(RequestObject<TWMOtherWhMainQueryModel> requestObject,
            CurrentUser currUser);
        
        /// <summary>
        /// 查询TWMOtherWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, List<TWMOtherWhDetailQueryModel>>> GetDetailListAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 新增TWMOtherWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>> PostAsync(RequestObject<TWMOtherWhMainAddModel> requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 修改TWMOtherWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMOtherWhMainEditModel, bool>> PutAsync(RequestObject<TWMOtherWhMainEditModel> requestObject);

        /// <summary>
        /// 其他入库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> OtherWhAuditAsync(RequestPut<OtherWarehousingAuditModel> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 其他入库单撤销审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> OtherWhCancelAuditAsync(RequestPut<OtherWarehousingAuditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMOtherWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject);
        
    }
}