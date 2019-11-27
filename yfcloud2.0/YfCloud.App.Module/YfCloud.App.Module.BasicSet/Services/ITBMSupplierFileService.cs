///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMSupplierFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:05:13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierFile;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierContact;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMSupplierFile Interface
    /// </summary>
    public interface ITBMSupplierFileService 
    {
        /// <summary>
        /// 查询TBMSupplierFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMSupplierFileQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 查询TBMSupplierFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMSupplierContactQueryModel>>> GetDetailListAsync(int requestObject, CurrentUser currentUser);


        /// <summary>
        /// 新增TBMSupplierFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TBMSupplierFileAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TBMSupplierFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TBMSupplierFileEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TBMSupplierFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

    }
}