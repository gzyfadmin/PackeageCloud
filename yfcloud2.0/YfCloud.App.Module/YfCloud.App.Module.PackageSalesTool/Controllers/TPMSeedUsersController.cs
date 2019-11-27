///////////////////////////////////////////////////////////////////////////////////////
// File: TPMSeedUsersController.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
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
using YfCloud.App.Module.PackageSalesTool.Models.TPMSeedUsers;
using YfCloud.App.Module.PackageSalesTool.Service;
using YfCloud.Models;

namespace YfCloud.App.Module.PackageSalesTool.Controllers
{
    /// <summary>
    /// T_PM_SeedUsers Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TPMSeedUsersController : ControllerBase
    {
        private readonly ITPMSeedUsersService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TPMSeedUsersController(ITPMSeedUsersService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_PM_SeedUsers数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseObject<List<TPMSeedUsersQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetAsync(request);
        }
        
        /// <summary>
        /// 新增T_PM_SeedUsers数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> Post(RequestPost<TPMSeedUsersAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }
        
        /// <summary>
        /// 修改T_PM_SeedUsers数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> Put(RequestPut<TPMSeedUsersEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_PM_SeedUsers数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
