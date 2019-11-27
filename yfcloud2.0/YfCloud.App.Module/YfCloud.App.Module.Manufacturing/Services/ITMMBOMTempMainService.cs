///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMTempMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/3 15:21:56
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMTempMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMTempDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMBOMTempMain Interface
    /// </summary>
    public interface ITMMBOMTempMainService 
    {
        /// <summary>
        /// 查询TMMBOMTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMBOMTempMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 查询TMMBOMTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMBOMTempDetailQueryModel>>> GetDetailListAsync(int requestObject);
        
        /// <summary>
        /// 新增TMMBOMTempMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMTempMainAddModel> requestObject, CurrentUser currentUser);

        Task<ResponseObject<TMMBOMTempMainQueryModel>> GetWholeMainData(int requestObject, CurrentUser currentUser);
    }
}