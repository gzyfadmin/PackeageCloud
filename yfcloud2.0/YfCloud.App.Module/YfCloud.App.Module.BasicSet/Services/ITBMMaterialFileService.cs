///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMMaterialFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7 8:49:08
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TBMMaterialFile;
using YfCloud.DBModel;
using YfCloud.App.Module.BasicSet.Models;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// TBMMaterialFile Interface
    /// </summary>
    public interface ITBMMaterialFileService 
    {
        /// <summary>
        /// 查询TBMMaterialFile数据
        /// </summary>
        /// <param name="requestObject">查询条件</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns></returns>
        Task<ResponseObject<TBMMaterialFileQueryModel, List<TBMMaterialFileCacheModel>>> GetAsync(RequestObject<TBMMaterialFileQueryModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 查询TBMMaterialFile数据
        /// </summary>
        /// <param name="requestObject">查询条件</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns></returns>
        Task<ResponseObject<TBMMaterialFileQueryModel, List<TBMMaterialFileQueryModel>>> GetNoMemory(RequestObject<TBMMaterialFileQueryModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 物料
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<List<TBMMaterialFileCacheModel>> GetAllAsync(CurrentUser currentUser, bool isContainDelete=false);

        /// <summary>
        /// 新增TBMMaterialFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBMMaterialFileAddModel, bool>> PostAsync(RequestObject<TBMMaterialFileAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 复制物料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> Copy(TBMMaterialFileCopyModel copyModel, CurrentUser currentUser);

        /// <summary>
        /// 修改TBMMaterialFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBMMaterialFileEditModel, bool>> PutAsync(RequestObject<TBMMaterialFileEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TBMMaterialFile数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 清除物料缓存
        /// </summary>
        /// <param name="currentUser"></param>
       void ClearCache(CurrentUser currentUser);

    }
}