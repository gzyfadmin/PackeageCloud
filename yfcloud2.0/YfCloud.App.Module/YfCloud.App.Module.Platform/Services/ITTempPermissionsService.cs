///////////////////////////////////////////////////////////////////////////////////////
// File: ITTempPermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 14:32:16
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
    /// TTempPermissions Interface
    /// </summary>
    public interface ITTempPermissionsService 
    {
        /// <summary>
        /// 查询TTempPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTempPermissionsModel, List<TTempPermissionsModel>>> GetAsync(RequestObject<TTempPermissionsModel> requestObject);

        /// <summary>
        /// 新增TTempPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTempPermissionsModel, bool>> PostAsync(RequestObject<TTempPermissionsModel> requestObject);

        /// <summary>
        /// 修改TTempPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTempPermissionsModel, bool>> PutAsync(RequestObject<TTempPermissionsModel> requestObject);

        /// <summary>
        /// 删除TTempPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTempPermissionsModel, bool>> DeleteAsync(RequestObject<TTempPermissionsModel> requestObject);
        
        /// <summary>
        /// 删除TTempPermissions数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TTempPermissions数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 删除TTempPermissions数据，通过TemplateId删除数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteByTempId(RequestObject<int> requestObject);
    }
}