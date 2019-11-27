///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMInventoryMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13 15:42:54
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMInventoryMain;
using YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMInventoryMain Interface
    /// </summary>
    public interface ITWMInventoryMainService 
    {
        /// <summary>
        /// 获取盘点单主单和明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMInventoryMainQueryModel>> GetInventoryOrder(int id, CurrentUser currentUser);

        /// <summary>
        /// 盘点单审核
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> InventoryAuditAsync(RequestPut<TWMInventoryAuditModel> request, CurrentUser currentUser);

        /// <summary>
        /// 获取盘点单明细列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseObject<List<TWMInventoryMainQueryModel>>> GetInventoryOrderListAsync(RequestGet requestGet,CurrentUser currentUser);

        /// <summary>
        /// 查询TWMInventoryMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMInventoryMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);
        
        /// <summary>
        /// 查询TWMInventoryMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMInventoryDetailQueryModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 新增TWMInventoryMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMInventoryMainQueryModel>> PostAsync(RequestPost<TWMInventoryMainAddModel> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 修改TWMInventoryMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TWMInventoryMainEditModel> requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 删除TWMInventoryMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
        
    }
}