///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMProfitMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13 15:44:44
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMProfitMain;
using YfCloud.App.Module.Warehouse.Models.TWMProfitDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMProfitMain Interface
    /// </summary>
    public interface ITWMProfitMainService 
    {
        /// <summary>
        /// 获取盘盈入库单信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMProfitMainQueryModel>> GetProfitOrderAsync(int Id, CurrentUser currentUser);

        /// <summary>
        /// 查询TWMProfitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMProfitMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);
        
        /// <summary>
        /// 查询TWMProfitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMProfitDetailQueryModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 新增TWMProfitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMProfitMainQueryModel>> PostAsync(RequestPost<TWMProfitMainAddModel> requestObject , CurrentUser currentUser);

        /// <summary>
        /// 修改TWMProfitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TWMProfitMainEditModel> requestObject,CurrentUser currUser);
        
        /// <summary>
        /// 删除TWMProfitMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);

        /// <summary>
        /// 盘盈入库单审核
        /// </summary>
        /// <param name="request"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> ProfitAuditAsync(RequestPut<TWMProfitMainAuditModel> request, CurrentUser current);


    }
}