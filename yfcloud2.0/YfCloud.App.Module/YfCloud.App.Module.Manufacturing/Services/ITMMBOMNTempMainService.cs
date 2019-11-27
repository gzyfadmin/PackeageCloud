///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMNTempMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4 8:56:17
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMBOMNTempMain Interface
    /// </summary>
    public interface ITMMBOMNTempMainService 
    {
        /// <summary>
        /// 查询TMMBOMNTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMBOMNTempMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 查询TMMBOMNTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMBOMNTempDetailQueryModel>>> GetDetailListAsync(int requestObject);
        
        /// <summary>
        /// 新增TMMBOMNTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMNTempMainAddModel> requestObject, CurrentUser currentUser);

        Task<ResponseObject<TMMBOMNTempMainQueryModel>> GetWholeMainData(int requestObject, CurrentUser currentUser);

    }
}