///////////////////////////////////////////////////////////////////////////////////////
// File: ITUsersService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 9:37:17
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TPMUser;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// TUsers Interface
    /// </summary>
    public interface ITUsersService 
    {
        /// <summary>
        /// 查询TUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TUsersModel, List<TUsersModel>>> GetAsync(RequestObject<TUsersModel> requestObject);

        /// <summary>
        /// 新增TUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TUsersModel, bool>> PostAsync(RequestObject<TUsersModel> requestObject);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TPMUserAccountAddModel, LoginResult>> LoginAsync(RequestObject<TPMUserAccountAddModel> requestObject);

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        bool CheckPassword(int UserID, string Password);

        /// <summary>
        /// 修改TUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TUsersModel, bool>> PutAsync(RequestObject<TUsersModel> requestObject);

        /// <summary>
        /// 删除TUsers数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TUsersModel, bool>> DeleteAsync(RequestObject<TUsersModel> requestObject);
        
        /// <summary>
        /// 删除TUsers数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TUsers数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 根据用户ID获取账户信息
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        Task<ResponseObject<int, UserInfo>> GetInfoAsync(int iUserId);
    }
}