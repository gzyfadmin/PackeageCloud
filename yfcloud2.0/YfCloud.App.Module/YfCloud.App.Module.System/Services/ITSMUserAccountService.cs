///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMUserAccountService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:49:11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMUserAccount Interface
    /// </summary>
    public interface ITSMUserAccountService 
    {
        /// <summary>
        /// 查询TSMUserAccount数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountQueryModel, List<TSMUserAccountQueryModel>>> GetAsync(RequestObject<TSMUserAccountQueryModel> requestObject);

        /// <summary>
        /// 新增TSMUserAccount数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<string>> PostAsync(RequestPost<TSMUserAccountAddModel> requestObject);

        /// <summary>
        /// 修改TSMUserAccount数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountEditModel, bool>> PutAsync(RequestObject<TSMUserAccountEditModel> requestObject);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountPassWordModel, bool>> ModifyPassWd(RequestObject<TSMUserAccountPassWordModel> requestObject);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserResetPassWordModel, bool>> ResetPassWd(RequestObject<TSMUserResetPassWordModel> requestObject);

        /// <summary>
        /// 删除TSMUserAccount数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountAddModel, bool>> DeleteAsync(RequestObject<TSMUserAccountAddModel> requestObject);
        
        /// <summary>
        /// 删除TSMUserAccount数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMUserAccount数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 验证手机邮箱是否存在
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountInputModel, bool>> CheckIsExistsAsync(RequestObject<TSMUserAccountInputModel> requestObject);


    }
}