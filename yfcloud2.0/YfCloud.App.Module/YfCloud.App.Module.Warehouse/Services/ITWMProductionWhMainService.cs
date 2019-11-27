///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMProductionWhMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16 13:42:00
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMProductionWhMain Interface
    /// </summary>
    public interface ITWMProductionWhMainService 
    {
        /// <summary>
        /// 查询TWMProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMProductionWhMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TWMProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMProductionWhMainQueryModel>> PostAsync(RequestPost<TWMProductionWhMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TWMProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMProductionWhMainQueryModel>> PutAsync(RequestPut<TWMProductionWhMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMProductionAduit> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TWMProductionWhMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);



    }
}