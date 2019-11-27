///////////////////////////////////////////////////////////////////////////////////////
// File: ITTenantPermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:08:39
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
    /// TTenantPermissions Interface
    /// </summary>
    public interface ITTenantPermissionsService 
    {
        /// <summary>
        /// 查询TTenantPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantPermissionsModel, List<TTenantPermissionsModel>>> GetAsync(RequestObject<TTenantPermissionsModel> requestObject);

        /// <summary>
        /// 新增TTenantPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantPermissionsModel, bool>> PostAsync(RequestObject<TTenantPermissionsModel> requestObject);

        /// <summary>
        /// 修改TTenantPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantPermissionsModel, bool>> PutAsync(RequestObject<TTenantPermissionsModel> requestObject);

        /// <summary>
        /// 删除TTenantPermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantPermissionsModel, bool>> DeleteAsync(RequestObject<TTenantPermissionsModel> requestObject);
        
        /// <summary>
        /// 删除TTenantPermissions数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TTenantPermissions数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
                        
        /// <summary>
        /// 删除TTenantPermissions数据，通过TenantId删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteByTenantIdAsync(RequestObject<int> requestObject);
    }
}