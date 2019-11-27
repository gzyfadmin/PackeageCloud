///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMColorItemService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:29:37
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMColorItem;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMColorItem Interface
    /// </summary>
    public interface ITMMColorItemService 
    {
        /// <summary>
        /// 查询TMMColorItem数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMColorItemQueryModel>>> GetAsync(RequestGet requestObject,CurrentUser currentUser);
       
        /// <summary>
        /// 查询拥有配色方案的配色项目
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMColorItemQueryModel>>> GetHasColorSolutionAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TMMColorItem数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<List<TMMColorItemAddModel>> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 删除配色项目，单个节点
        /// </summary>
        /// <param name="requestDelete"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestDelete);

        /// <summary>
        /// 删除配色项目，所有节点
        /// </summary>
        /// <param name="requestDelete"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAllAsync(RequestDelete<DeleteModel> requestDelete, CurrentUser currentUser);


    }
}