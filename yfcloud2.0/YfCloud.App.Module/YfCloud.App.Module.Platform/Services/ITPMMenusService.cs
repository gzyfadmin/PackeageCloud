///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMMenusService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15 10:49:00
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TPMMenus;
using YfCloud.App.Module.Platform.Models.TPMMenus;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TPMMenus Interface
    /// </summary>
    public interface ITPMMenusService 
    {
        /// <summary>
        /// 查询TPMMenus数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusQueryModel>>> GetAsync(RequestObject<TPMMenusQueryModel> requestObject);

        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusTreeModel>>> GetTreeAsync(RequestObject<TPMMenusQueryModel> requestObject);

        /// <summary>
        /// 新增TPMMenus数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TPMMenusAddModel, bool>> PostAsync(RequestObject<TPMMenusAddModel> requestObject);
        
        /// <summary>
        /// 修改TPMMenus数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TPMMenusEditModel, bool>> PutAsync(RequestObject<TPMMenusEditModel> requestObject);

        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TPMMenusMoveModel, bool>> MovePostion(RequestObject<TPMMenusMoveModel> requestObject);

        /// <summary>
        /// 删除TPMMenus数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        Task<ResponseObject<TPMenusDeleteAllModel, bool>> DeleteAsync(RequestObject<TPMenusDeleteAllModel> requestObject,int UserId);
        
        /// <summary>
        /// 删除TPMMenus数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TPMMenus数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
        
    }
}