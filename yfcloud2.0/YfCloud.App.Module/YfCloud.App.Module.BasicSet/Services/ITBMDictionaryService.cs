///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMDictionaryService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 17:56:40
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMDictionary;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// TBMDictionary Interface
    /// </summary>
    public interface ITBMDictionaryService 
    {

        /// <summary>
        /// 获取公司下所有的数据字典
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        Task<List<TBMDictionaryCacheModel>> GetAllDictionary(int CompanyID);

        /// <summary>
        /// 查询TBMDictionary数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TBMDictionaryQueryModel>>> GetAsync(RequestGet requestObject, CurrentUser user);

        /// <summary>
        /// 新增TBMDictionary数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBMDictionaryAddModel, bool>> PostAsync(RequestObject<TBMDictionaryAddModel> requestObject, CurrentUser user);

        /// <summary>
        /// 修改TBMDictionary数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBMDictionaryEditModel, bool>> PutAsync(RequestObject<TBMDictionaryEditModel> requestObject, CurrentUser user);

        /// <summary>
        /// 删除TBMDictionary数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser user);

        string GerRandom();


    }
}