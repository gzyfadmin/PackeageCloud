///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMSalesMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22 8:49:16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMSalesMain;
using YfCloud.App.Module.Warehouse.Models.TWMSalesDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMSalesMain Interface
    /// </summary>
    public interface ITWMSalesMainService 
    {
        /// <summary>
        /// 查询TWMSalesMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMSalesMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser);

        Task<ResponseObject<TWMSalesMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);

        /// <summary>
        /// 新增TWMSalesMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMSalesMainQueryModel>> PostAsync(RequestPost<TWMSalesMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TWMSalesMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMSalesMainQueryModel>> PutAsync(RequestPut<TWMSalesMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMSalesMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMSalesMainAduit> requestObject, CurrentUser currentUser);


    }
}