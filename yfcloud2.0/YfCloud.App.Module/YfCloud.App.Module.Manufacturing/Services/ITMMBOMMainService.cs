///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMBOMMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:31:06
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMBOMMain Interface
    /// </summary>
    public interface ITMMBOMMainService 
    {
        /// <summary>
        /// 新增TMMBOMMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMMainAddModel> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMBOMMainQueryModel>> GetWholeMainData(int requestObject);




    }
}