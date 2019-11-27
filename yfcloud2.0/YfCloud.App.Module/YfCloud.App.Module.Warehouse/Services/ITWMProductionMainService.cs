///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMProductionMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18 13:52:03
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TWMProductionMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionDetail;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TWMProductionMain Interface
    /// </summary>
    public interface ITWMProductionMainService 
    {
        /// <summary>
        /// 查询TWMProductionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TWMProductionMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        Task<ResponseObject<TWMProductionMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);

        
        /// <summary>
        /// 新增TWMProductionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMProductionMainQueryModel>> PostAsync(RequestPost<TWMProductionMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TWMProductionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TWMProductionMainQueryModel>> PutAsync(RequestPut<TWMProductionMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TWMProductionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMProductionAduit> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 获取领料单上的可转物料
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTopWholeMainData(int iMainId, CurrentUser currentUser);

    }
}