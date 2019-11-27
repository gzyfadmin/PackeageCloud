///////////////////////////////////////////////////////////////////////////////////////
// File: ITDataDicDetailService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 9:06:14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TDataDicDetail Interface
    /// </summary>
    public interface ITDataDicDetailService 
    {
        /// <summary>
        /// 查询TDataDicDetail数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>> GetAsync(RequestObject<TDataDicDetailModel> requestObject);

        /// <summary>
        /// 新增TDataDicDetail数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicDetailModel, bool>> PostAsync(RequestObject<TDataDicDetailModel> requestObject);

        /// <summary>
        /// 修改TDataDicDetail数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicDetailModel, bool>> PutAsync(RequestObject<TDataDicDetailModel> requestObject);

        /// <summary>
        /// 删除TDataDicDetail数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicDetailModel, bool>> DeleteAsync(RequestObject<TDataDicDetailModel> requestObject);
        
        /// <summary>
        /// 删除TDataDicDetail数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TDataDicDetail数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
    }
}