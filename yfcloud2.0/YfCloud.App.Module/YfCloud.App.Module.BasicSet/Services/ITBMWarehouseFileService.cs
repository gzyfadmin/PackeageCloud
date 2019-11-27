///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMWarehouseFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 13:37:21
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMWarehouseFile;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMWarehouseFile Interface
    /// </summary>
    public interface ITBMWarehouseFileService 
    {
        /// <summary>
        /// 查询TBMWarehouseFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TBMWarehouseFileQueryModel, List<TBMWarehouseFileQueryModel>>> GetAsync(RequestObject<TBMWarehouseFileQueryModel> requestObject, int UserID);

        /// <summary>
        /// 新增TBMWarehouseFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TBMWarehouseFileAddModel, bool>> PostAsync(RequestObject<TBMWarehouseFileAddModel> requestObject,int UserID);

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        string GetMaxID(int UserID);

        /// <summary>
        /// 修改TBMWarehouseFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBMWarehouseFileEditModel, bool>> PutAsync(RequestObject<TBMWarehouseFileEditModel> requestObject, int userID);

        /// <summary>
        /// 删除TBMWarehouseFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject,CurrentUser user);
        
    }
}