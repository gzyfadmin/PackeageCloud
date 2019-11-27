///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMRolePermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 13:38:01
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMRolePermissions;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMRolePermissions Interface
    /// </summary>
    public interface ITSMRolePermissionsService 
    {
        /// <summary>
        /// 查询TSMRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolePermissionsQueryModel, List<TSMRolePermissionsQueryModel>>> GetAsync(RequestObject<TSMRolePermissionsQueryModel> requestObject);
        
        /// <summary>
        /// 新增TSMRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="iUserId">用户ID</param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolePermissionsAddModel, bool>> PostAsync(RequestObject<TSMRolePermissionsAddModel> requestObject,int iUserId);
        
        /// <summary>
        /// 修改TSMRolePermissions数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolePermissionsEditModel, bool>> PutAsync(RequestObject<TSMRolePermissionsEditModel> requestObject);
                
        /// <summary>
        /// 删除TSMRolePermissions数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="iUserId">用户ID</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject,int iUserId);

        /// <summary>
        /// 根据用户ID获取当前所在公司的菜单树
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        Task<ResponseObject<string, List<MenuTreeModel>>> GetMenuTree(int iUserId);
        
    }
}