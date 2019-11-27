///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMDictionaryTypeService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 17:52:10
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMDictionaryType;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMDictionaryType Interface
    /// </summary>
    public interface ITBMDictionaryTypeService 
    {
        /// <summary>
        /// 查询TBMDictionaryType数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TBMDictionaryTypeQueryModel, List<TBMDictionaryTypeQueryModel>>> GetAsync(RequestObject<TBMDictionaryTypeQueryModel> requestObject, int UserID);

        /// <summary>
        /// 新增TBMDictionaryType数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TBMDictionaryTypeAddModel, bool>> PostAsync(RequestObject<TBMDictionaryTypeAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TBMDictionaryType数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TBMDictionaryTypeEditModel, bool>> PutAsync(RequestObject<TBMDictionaryTypeEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TBMDictionaryType数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser currentUser);

    }
}