///////////////////////////////////////////////////////////////////////////////////////
// File: TBMPackageController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.BasicSet.Models.TBMPackage;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// T_BM_Package Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMPackageController : ControllerBase
    {
        private readonly ITBMPackageService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMPackageController(ITBMPackageService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_BM_Package数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<List<TBMPackageQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, user);
        }
        
        /// <summary>
        /// 新增T_BM_Package数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TBMPackageAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }
        
        /// <summary>
        /// 修改T_BM_Package数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TBMPackageEditModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, user);
        }
        
        /// <summary>
        /// 删除T_BM_Package数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, user);
        }
    }
}
