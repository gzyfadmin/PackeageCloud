///////////////////////////////////////////////////////////////////////////////////////
// File: ITButtonsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18 11:34:29
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
    /// TButtons Interface
    /// </summary>
    public interface ITButtonsService 
    {
        /// <summary>
        /// 查询TButtons数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TButtonsModel, List<TButtonsModel>>> GetAsync(RequestObject<TButtonsModel> requestObject);

        /// <summary>
        /// 新增TButtons数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TButtonsModel, bool>> PostAsync(RequestObject<TButtonsModel> requestObject);

        /// <summary>
        /// 修改TButtons数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TButtonsModel, bool>> PutAsync(RequestObject<TButtonsModel> requestObject);

        /// <summary>
        /// 删除TButtons数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TButtonsModel, bool>> DeleteAsync(RequestObject<TButtonsModel> requestObject);
        
        /// <summary>
        /// 删除TButtons数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TButtons数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
    }
}