///////////////////////////////////////////////////////////////////////////////////////
// File: ITRolesService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 15:02:47
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
    /// TRoles Interface
    /// </summary>
    public interface ITRolesService 
    {
        /// <summary>
        /// 查询TRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolesModel, List<TRolesModel>>> GetAsync(RequestObject<TRolesModel> requestObject);

        /// <summary>
        /// 新增TRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolesModel, bool>> PostAsync(RequestObject<TRolesModel> requestObject);

        /// <summary>
        /// 修改TRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolesModel, bool>> PutAsync(RequestObject<TRolesModel> requestObject);

        /// <summary>
        /// 删除TRoles数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TRolesModel, bool>> DeleteAsync(RequestObject<TRolesModel> requestObject);
        
        /// <summary>
        /// 删除TRoles数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TRoles数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
    }
}