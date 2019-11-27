///////////////////////////////////////////////////////////////////////////////////////
// File: TBMMaterialFileController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.BasicSet.Models;
using YfCloud.App.Module.Warehouse.Models.TBMMaterialFile;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_BM_MaterialFile Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMMaterialFileController : ControllerBase
    {
        private readonly ITBMMaterialFileService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMMaterialFileController(ITBMMaterialFileService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_BM_MaterialFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TBMMaterialFileQueryModel,List<TBMMaterialFileCacheModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBMMaterialFileQueryModel>>(requestObject);
            var user= TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, user);
        }

        /// <summary>
        /// 获取T_BM_MaterialFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TBMMaterialFileQueryModel, List<TBMMaterialFileQueryModel>>> GetNoMemory(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBMMaterialFileQueryModel>>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetNoMemory(request, user);
        }

        /// <summary>
        /// 新增T_BM_MaterialFile数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TBMMaterialFileAddModel,bool>> Post(RequestObject<TBMMaterialFileAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }


        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> Copy(TBMMaterialFileCopyModel requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.Copy(requestObject, user);
        }

        /// <summary>
        /// 修改T_BM_MaterialFile数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TBMMaterialFileEditModel,bool>> Put(RequestObject<TBMMaterialFileEditModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, user);
        }
        
        /// <summary>
        /// 删除T_BM_MaterialFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<DeleteModel,bool>> Delete(RequestObject<DeleteModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, user);
        }
    }
}
