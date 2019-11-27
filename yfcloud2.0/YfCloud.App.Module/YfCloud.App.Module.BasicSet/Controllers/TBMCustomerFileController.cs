///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerFileController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerFile;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerContact;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using Microsoft.AspNetCore.Authorization;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// T_BM_CustomerFile Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TBMCustomerFileController : ControllerBase
    {
        private readonly ITBMCustomerFileService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMCustomerFileController(ITBMCustomerFileService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取TBMCustomerFile主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TBMCustomerFileQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }

        /// <summary>
        /// 新增TBMCustomerFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TBMCustomerFileAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 修改TBMCustomerFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TBMCustomerFileEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 删除TBMCustomerFile数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.DeleteAsync(requestObject, currUser);
        }
    }
}
