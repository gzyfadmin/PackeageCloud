///////////////////////////////////////////////////////////////////////////////////////
// File: ITTenantsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:14:15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TTenants;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TTenants Interface
    /// </summary>
    public interface ITTenantsService 
    {
        /// <summary>
        /// 查询TTenants数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantsModel, List<TTenantsQueryModel>>> GetAsync(RequestObject<TTenantsModel> requestObject);

        /// <summary>
        /// 新增TTenants数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantsModel, bool>> PostAsync(RequestObject<TTenantsModel> requestObject);

        /// <summary>
        /// 修改TTenants数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantsPutModel, bool>> PutAsync(RequestObject<TTenantsPutModel> requestObject);

        /// <summary>
        /// 删除TTenants数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TTenantsModel, bool>> DeleteAsync(RequestObject<TTenantsModel> requestObject);
        
        /// <summary>
        /// 删除TTenants数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TTenants数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 修改企业信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TTenantsEditModel, bool>> ModifyAsync(RequestObject<TTenantsEditModel> requestObject);
    }
}