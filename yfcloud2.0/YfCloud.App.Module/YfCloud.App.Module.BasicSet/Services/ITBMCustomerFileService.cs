///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMCustomerFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22 10:07:20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerFile;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerContact;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMCustomerFile Interface
    /// </summary>
    public interface ITBMCustomerFileService 
    {
        /// <summary>
        /// 查询TBMCustomerFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMCustomerFileQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);


        /// <summary>
        /// 查询TBMCustomerFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMCustomerContactQueryModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 新增TBMCustomerFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TBMCustomerFileAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TBMCustomerFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TBMCustomerFileEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TBMCustomerFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser);

    }
}