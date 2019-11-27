///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserAccountController.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.App.Module.System.Service;
using YfCloud.Models;
using YfCloud.DBModel;
using Microsoft.AspNetCore.Authorization;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_UserAccount Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMUserAccountController : ControllerBase
    {
        private readonly ITSMUserAccountService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMUserAccountController(ITSMUserAccountService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_SM_UserAccount数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMUserAccountQueryModel,List<TSMUserAccountQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMUserAccountQueryModel>>(requestObject);
            return await _service.GetAsync(request);
        }
        
        /// <summary>
        /// 新增T_SM_UserAccount数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        [AllowAnonymous]
        public async Task<ResponseObject<string>> Post(RequestPost<TSMUserAccountAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

       

        /// <summary>
        /// 修改T_SM_UserAccount数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMUserAccountEditModel,bool>> Put(RequestObject<TSMUserAccountEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 修改T_SM_UserAccount数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountPassWordModel, bool>> ModifyPassWord(RequestObject<TSMUserAccountPassWordModel> requestObject)
        {
            return await _service.ModifyPassWd(requestObject);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ResponseObject<TSMUserResetPassWordModel, bool>> ResetPassword(RequestObject<TSMUserResetPassWordModel> requestObject)
        {
            return await _service.ResetPassWd(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserAccount数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TSMUserAccountAddModel,bool>> Delete(RequestObject<TSMUserAccountAddModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_SM_UserAccount数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserAccount数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")]
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
