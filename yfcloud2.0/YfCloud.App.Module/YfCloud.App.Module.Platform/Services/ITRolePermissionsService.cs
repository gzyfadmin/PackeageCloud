///////////////////////////////////////////////////////////////////////////////////////
// File: ITRolePermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 15:03:24
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TRolePermissions;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TRolePermissions Interface
    /// </summary>
    public interface ITRolePermissionsService 
    {
        /// <summary>
        /// 查询TRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolePermissionsModel, List<TRolePermissionsModel>>> GetAsync(RequestObject<TRolePermissionsModel> requestObject);

        /// <summary>
        /// 新增TRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolePermissionsModel, bool>> PostAsync(RequestObject<TRolePermissionsModel> requestObject);

        /// <summary>
        /// 修改TRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolePermissionsModel, bool>> PutAsync(RequestObject<TRolePermissionsModel> requestObject);

        /// <summary>
        /// 删除TRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolePermissionsModel, bool>> DeleteAsync(RequestObject<TRolePermissionsModel> requestObject);
        
        /// <summary>
        /// 删除TRolePermissions数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TRolePermissions数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 删除TRolePermissions数据，通过角色Id删除整个角色权限
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteByRoleIdAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 根据角色ID 获取菜单树
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TRolePermissionsModel, List<MenuViewModel>>> LoadMenuByRoles(RequestObject<TRolePermissionsModel> requestObject);
    }
}