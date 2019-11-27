///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorItemController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Manufacturing.Models.TMMColorItem;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_ColorItem Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMColorItemController : ControllerBase
    {
        private readonly ITMMColorItemService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMColorItemController(ITMMColorItemService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取配色项目数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<List<TMMColorItemQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, currUser);
        }


        /// <summary>
        /// 获取拥有配色方案的配色项目
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMColorItemQueryModel>>> GetHasColorSolution(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetHasColorSolutionAsync(request, currUser);
        }

        /// <summary>
        /// 新增配色项目数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<List<TMMColorItemAddModel>> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }

        /// <summary>
        /// 删除配色项目数据,删除单个子节点
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除配色项目数据，删除所有子节点
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> DeleteAll(RequestDelete<DeleteModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAllAsync(requestObject, currUser);
        }
    }
}
