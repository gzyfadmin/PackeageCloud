///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMPolicyCompanyService.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14 14:00:36
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.PackageSalesTool.Models.TPMPolicyCompany;
using YfCloud.DBModel;

namespace YfCloud.App.Module.PackageSalesTool.Service
{
    /// <summary>
    /// TPMPolicyCompany Interface
    /// </summary>
    public interface ITPMPolicyCompanyService 
    {
        /// <summary>
        /// 查询TPMPolicyCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TPMPolicyCompanyQueryModel>>> GetAsync(RequestGet requestObject);
        
        /// <summary>
        /// 新增TPMPolicyCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TPMPolicyCompanyAddModel> requestObject);
        
        /// <summary>
        /// 修改TPMPolicyCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TPMPolicyCompanyEditModel> requestObject);
        
        /// <summary>
        /// 删除TPMPolicyCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
        
    }
}