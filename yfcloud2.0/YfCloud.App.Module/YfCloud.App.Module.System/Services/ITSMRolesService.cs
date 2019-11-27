///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMRolesService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 13:30:21
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMRoles;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMRoles Interface
    /// </summary>
    public interface ITSMRolesService 
    {
        /// <summary>
        /// 查询TSMRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolesQueryModel, List<TSMRolesQueryTreeModel>>> GetAsync(RequestObject<TSMRolesQueryModel> requestObject, int UserId);

        /// <summary>
        /// 新增TSMRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolesAddModel, bool>> PostAsync(RequestObject<TSMRolesAddModel> requestObject, int UserId);

        /// <summary>
        /// 修改TSMRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolesEditModel, bool>> PutAsync(RequestObject<TSMRolesEditModel> requestObject);
        
        
        /// <summary>
        /// 删除TSMRoles数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMRoles数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="requestObject">移动入参</param>
        /// <returns></returns>
        Task<ResponseObject<TSMRolesMoveModel, bool>> MovePostion(RequestObject<TSMRolesMoveModel> requestObject);

        /// <summary>
        /// 查询某个角色下的所有员工
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMRoleUserRelationEditModel, bool>> DispathDept(RequestObject<TSMRoleUserRelationEditModel> requestObject);

        /// <summary>
        /// 为员工指派角色
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>> GetDispathUserAsync(RequestObject<TSMRoleUserRelationEditModel> requestObject);

        /// <summary>
        /// 删除某个角色下的用户
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> RemoveRoleForUser(RequestObject<int[]> requestObject);


    }
}