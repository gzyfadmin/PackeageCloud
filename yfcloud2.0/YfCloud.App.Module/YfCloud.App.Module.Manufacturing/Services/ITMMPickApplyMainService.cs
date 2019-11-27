///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMPickApplyMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11 15:33:20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMPickApplyMain Interface
    /// </summary>
    public interface ITMMPickApplyMainService 
    {
        /// <summary>
        /// 查询TMMPickApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMPickApplyMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser);
        

        /// <summary>
        /// 新增TMMPickApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TMMPickApplyMainQueryModel>> PostAsync(RequestPost<TMMPickApplyMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TMMPickApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TMMPickApplyMainQueryModel>> PutAsync(RequestPut<TMMPickApplyMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TMMPickApplyMain数据
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
        Task<ResponseObject<bool>> AuditAsync(RequestPut<AuditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMPickApplyMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser);

        Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser);

        Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTransWareDate(int iMainId, CurrentUser currentUser);

    }
}