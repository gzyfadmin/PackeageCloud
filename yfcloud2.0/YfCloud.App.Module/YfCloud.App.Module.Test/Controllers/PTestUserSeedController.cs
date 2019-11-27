///////////////////////////////////////////////////////////////////////////////////////
// File: PTestUserSeedController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/30
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
using YfCloud.App.Module.Test.Models.PTestUserSeed;
using YfCloud.App.Module.Test.Service;
using YfCloud.Models;

namespace YfCloud.App.Module.Test.Controllers
{
    /// <summary>
    /// P_Test_UserSeed Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PTestUserSeedController : ControllerBase
    {
        private readonly IPTestUserSeedService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PTestUserSeedController(IPTestUserSeedService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 新增P_Test_UserSeed数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> Post(RequestPost<PTestUserSeedAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 查询种子用户信息
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseObject<List<PTestUserSeedQueryModel>>> Get()
        {
            return await _service.GetAsync();
        }

    }
}
