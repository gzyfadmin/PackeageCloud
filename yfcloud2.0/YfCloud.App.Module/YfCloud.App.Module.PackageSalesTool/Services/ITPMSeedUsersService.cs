///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMSeedUsersService.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14 14:01:19
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.PackageSalesTool.Models.TPMSeedUsers;
using YfCloud.DBModel;

namespace YfCloud.App.Module.PackageSalesTool.Service
{
    /// <summary>
    /// TPMSeedUsers Interface
    /// </summary>
    public interface ITPMSeedUsersService 
    {
        /// <summary>
        /// 查询TPMSeedUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TPMSeedUsersQueryModel>>> GetAsync(RequestGet requestObject);
        
        /// <summary>
        /// 新增TPMSeedUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TPMSeedUsersAddModel> requestObject);
        
        /// <summary>
        /// 修改TPMSeedUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TPMSeedUsersEditModel> requestObject);
        
        /// <summary>
        /// 删除TPMSeedUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
        
    }
}