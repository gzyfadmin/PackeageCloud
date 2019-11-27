///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMDictionaryInitInfoService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/23 15:16:14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models.TPMDictionaryInitInfo;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TPMDictionaryInitInfo Interface
    /// </summary>
    public interface ITPMDictionaryInitInfoService 
    {
        /// <summary>
        /// 查询TPMDictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TPMDictionaryInitInfoQueryModel>>> GetAsync(RequestGet requestObject);
        
        /// <summary>
        /// 新增TPMDictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TPMDictionaryInitInfoAddModel> requestObject);
        
        /// <summary>
        /// 修改TPMDictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TPMDictionaryInitInfoEditModel> requestObject);
        
        /// <summary>
        /// 删除TPMDictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
        
    }
}