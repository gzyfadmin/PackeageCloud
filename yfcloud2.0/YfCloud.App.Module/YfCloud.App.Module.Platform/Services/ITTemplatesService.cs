///////////////////////////////////////////////////////////////////////////////////////
// File: ITTemplatesService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 14:30:47
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TTemplates Interface
    /// </summary>
    public interface ITTemplatesService 
    {
        /// <summary>
        /// 查询TTemplates数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTemplatesModel, List<TTemplatesModel>>> GetAsync(RequestObject<TTemplatesModel> requestObject);

        /// <summary>
        /// 新增TTemplates数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTemplatesModel, bool>> PostAsync(RequestObject<TTemplatesModel> requestObject);

        /// <summary>
        /// 修改TTemplates数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTemplatesModel, bool>> PutAsync(RequestObject<TTemplatesModel> requestObject);

        /// <summary>
        /// 删除TTemplates数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTemplatesModel, bool>> DeleteAsync(RequestObject<TTemplatesModel> requestObject);
        
        /// <summary>
        /// 删除TTemplates数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TTemplates数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
    }
}