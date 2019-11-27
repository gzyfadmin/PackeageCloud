///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierFileController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierFile;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierContact;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// 供应商
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMSupplierFileController : ControllerBase
    {
        private readonly ITBMSupplierFileService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMSupplierFileController(ITBMSupplierFileService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取TBMSupplierFile主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TBMSupplierFileQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetMainListAsync(request, curent);
        }
        
        
        /// <summary>
        /// 新增TBMSupplierFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TBMSupplierFileAddModel> requestObject)
        {
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, curent);
        }
        
        /// <summary>
        /// 修改TBMSupplierFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TBMSupplierFileEditModel> requestObject)
        {
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, curent);
        }
        
        /// <summary>
        /// 删除TBMSupplierFile数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, curent);
        }
    }
}
