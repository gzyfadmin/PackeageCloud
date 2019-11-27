///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMUserInfoService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11 10:40:19
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMUserInfo;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMUserInfo Interface
    /// </summary>
    public interface ITSMUserInfoService 
    {
        /// <summary>
        /// 查询TSMUserInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserInfoQueryModel, List<TSMUserInfoQueryModel>>> GetAsync(RequestObject<TSMUserInfoQueryModel> requestObject);
        
        /// <summary>
        /// 新增TSMUserInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserInfoAddModel, bool>> PostAsync(RequestObject<TSMUserInfoAddModel> requestObject);
        
        /// <summary>
        /// 修改TSMUserInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserInfoEditModel, bool>> PutAsync(RequestObject<TSMUserInfoEditModel> requestObject);
        
        /// <summary>
        /// 删除TSMUserInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserInfoAddModel, bool>> DeleteAsync(RequestObject<TSMUserInfoAddModel> requestObject);
        
        /// <summary>
        /// 删除TSMUserInfo数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMUserInfo数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
        
    }
}