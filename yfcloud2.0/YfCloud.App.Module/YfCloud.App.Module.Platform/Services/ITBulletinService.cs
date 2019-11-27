///////////////////////////////////////////////////////////////////////////////////////
// File: ITBulletinService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:09:35

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TBulletin Interface
    /// </summary>
    public interface ITBulletinService 
    {
        /// <summary>
        /// 查询TBulletin数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBulletinModel, List<TBulletinModel>>> GetAsync(RequestObject<TBulletinModel> requestObject);

        /// <summary>
        /// 新增TBulletin数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBulletinModel, bool>> PostAsync(RequestObject<TBulletinModel> requestObject);

        /// <summary>
        /// 修改TBulletin数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBulletinModel, bool>> PutAsync(RequestObject<TBulletinModel> requestObject);

        /// <summary>
        /// 删除TBulletin数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TBulletinModel, bool>> DeleteAsync(RequestObject<TBulletinModel> requestObject);
        
        /// <summary>
        /// 删除TBulletin数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TBulletin数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);
    }
}