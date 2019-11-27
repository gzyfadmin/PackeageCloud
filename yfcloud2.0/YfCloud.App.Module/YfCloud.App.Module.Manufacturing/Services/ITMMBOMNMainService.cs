///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMNMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4 8:54:33
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMBOMNMain Interface
    /// </summary>
    public interface ITMMBOMNMainService 
    {
        /// <summary>
        /// 新增TMMBOMNMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMNMainAddModel> requestObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMBOMNMainQueryModel>> GetWholeMainData(int requestObject);
    }
}