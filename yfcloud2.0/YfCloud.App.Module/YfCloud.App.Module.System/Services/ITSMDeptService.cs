///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMDeptService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 11:11:43
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMDept;
using YfCloud.App.Module.System.Models.TSMUserAccount;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMDept Interface
    /// </summary>
    public interface ITSMDeptService 
    {
        /// <summary>
        /// 查询TSMDept数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>> GetAsync(RequestObject<TSMDeptQueryModel> requestObject,int UserId);

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>> GetOnlyDepAsync(RequestObject<TSMDeptQueryModel> requestObject, int UserId);

        /// <summary>
        /// 新增TSMDept数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId">创建人ID</param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptAddModel, bool>> PostAsync(RequestObject<TSMDeptAddModel> requestObject,int UserId);
        
        /// <summary>
        /// 修改TSMDept数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptEditModel, bool>> PutAsync(RequestObject<TSMDeptEditModel> requestObject);
        
        
        /// <summary>
        /// 删除TSMDept数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMDept数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptMoveModel, bool>> MovePostion(RequestObject<TSMDeptMoveModel> requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 分配员工权限
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<DeptUserRelationEdit, bool>> DispathDept(RequestObject<DeptUserRelationEdit> requestObject);

        /// <summary>
        /// 获取部门下的员工
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>> GetDispathUserAsync(RequestObject<TSMDeptQueryForDispatchModel> requestObject,int userID);


    }
}