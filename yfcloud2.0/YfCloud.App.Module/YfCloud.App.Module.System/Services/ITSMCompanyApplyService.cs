///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMCompanyApplyService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:50:04
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMCompanyApply;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMCompanyApply Interface
    /// </summary>
    public interface ITSMCompanyApplyService 
    {
        /// <summary>
        /// 查询TSMCompanyApply数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyApplyQueryModel, List<TSMCompanyApplyQueryModel>>> GetAsync(RequestObject<TSMCompanyApplyQueryModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TSMCompanyApply数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TSMCompanyApplyAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TSMCompanyApply数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TSMCompanyApplyEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TSMCompanyApply数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyApplyAddModel, bool>> DeleteAsync(RequestObject<TSMCompanyApplyAddModel> requestObject);
        
        /// <summary>
        /// 删除TSMCompanyApply数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMCompanyApply数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
        
    }
}