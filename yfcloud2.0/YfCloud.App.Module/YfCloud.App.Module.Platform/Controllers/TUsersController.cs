///////////////////////////////////////////////////////////////////////////////////////
// File: TUsersController.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models;
using YfCloud.App.Module.Platform.Models.TPMUser;
using YfCloud.App.Module.Platform.Models.TRolePermissions;
using YfCloud.App.Module.Platform.Service;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_Users Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TUsersController : ControllerBase
    {
        private readonly ITUsersService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TUsersController(ITUsersService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_Users数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TUsersModel,List<TUsersModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TUsersModel>>(requestObject);
            return await _service.GetAsync(request);
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<int,UserInfo>> GetInfo()
        {
            var userId = TokenManager.GetUserIDbyToken(Request.Headers);
            if(userId < 0)
            {
                return ResponseUtil<int, UserInfo>.FailResult(new RequestObject<int>(), null, "用户ID信息错误");
            }
            return await _service.GetInfoAsync(userId);
        }


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ResponseObject<TPMUserAccountAddModel, LoginResult>> LoginPost(RequestObject<TPMUserAccountAddModel> requestObject)
        {
            var result= await _service.LoginAsync(requestObject);
            return result;
        }




        /// <summary>
        /// 新增T_Users数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TUsersModel,bool>> Post(RequestObject<TUsersModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_Users数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TUsersModel,bool>> Put(RequestObject<TUsersModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_Users数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TUsersModel,bool>> Delete(RequestObject<TUsersModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject); 
        }
        
        /// <summary>
        /// 删除T_Users数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_Users数据表数据，通过主表主键删除主从数据，批量删除
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
