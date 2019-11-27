///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMPackageService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5 11:12:11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMPackage;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMPackage Interface
    /// </summary>
    public interface ITBMPackageService 
    {
        /// <summary>
        /// 查询TBMPackage数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMPackageQueryModel>>> GetAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TBMPackage数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TBMPackageAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TBMPackage数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TBMPackageEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TBMPackage数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

    }
}