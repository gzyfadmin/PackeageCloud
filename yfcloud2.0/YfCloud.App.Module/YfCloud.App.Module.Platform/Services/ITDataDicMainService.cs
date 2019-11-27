///////////////////////////////////////////////////////////////////////////////////////
// File: ITDataDicMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18 11:49:34
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TDataDicMain;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TDataDicMain Interface
    /// </summary>
    public interface ITDataDicMainService 
    {
        /// <summary>
        /// 查询TDataDicMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicMainModel, List<TDataDicMainModel>>> GetAsync(RequestObject<TDataDicMainModel> requestObject);

        /// <summary>
        /// 新增TDataDicMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicMainModel, bool>> PostAsync(RequestObject<TDataDicMainModel> requestObject);

        /// <summary>
        /// 修改TDataDicMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicMainModel, bool>> PutAsync(RequestObject<TDataDicMainModel> requestObject);

        /// <summary>
        /// 删除TDataDicMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicMainModel, bool>> DeleteAsync(RequestObject<TDataDicMainModel> requestObject);
        
        /// <summary>
        /// 删除TDataDicMain数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TDataDicMain数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicMainModel, List<DataTreeNode>>> GetTreeAsync(RequestObject<TDataDicMainModel> requestObject);
    }
}