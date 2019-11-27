///////////////////////////////////////////////////////////////////////////////////////
// File: TPMDictionaryInitInfoController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/23
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models.TPMDictionaryInitInfo;
using YfCloud.App.Module.Platform.Service;
using YfCloud.Models;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_PM_DictionaryInitInfo Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TPMDictionaryInitInfoController : ControllerBase
    {
        private readonly ITPMDictionaryInitInfoService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TPMDictionaryInitInfoController(ITPMDictionaryInitInfoService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_PM_DictionaryInitInfo数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<List<TPMDictionaryInitInfoQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetAsync(request);
        }
        
        /// <summary>
        /// 新增T_PM_DictionaryInitInfo数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TPMDictionaryInitInfoAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }
        
        /// <summary>
        /// 修改T_PM_DictionaryInitInfo数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TPMDictionaryInitInfoEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_PM_DictionaryInitInfo数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
