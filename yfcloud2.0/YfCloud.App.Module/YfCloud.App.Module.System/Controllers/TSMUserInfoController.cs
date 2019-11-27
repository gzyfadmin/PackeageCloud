///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserInfoController.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.TSMUserInfo;
using YfCloud.App.Module.System.Service;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_UserInfo Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMUserInfoController : ControllerBase
    {
        private readonly ITSMUserInfoService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMUserInfoController(ITSMUserInfoService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_SM_UserInfo数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMUserInfoQueryModel,List<TSMUserInfoQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMUserInfoQueryModel>>(requestObject);
            return await _service.GetAsync(request);
        }
        
        /// <summary>
        /// 新增T_SM_UserInfo数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSMUserInfoAddModel,bool>> Post(RequestObject<TSMUserInfoAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }
        
        /// <summary>
        /// 修改T_SM_UserInfo数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMUserInfoEditModel,bool>> Put(RequestObject<TSMUserInfoEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_SM_UserInfo数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TSMUserInfoAddModel,bool>> Delete(RequestObject<TSMUserInfoAddModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_SM_UserInfo数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserInfo数据表数据，通过主表主键删除主从数据，批量删除
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
