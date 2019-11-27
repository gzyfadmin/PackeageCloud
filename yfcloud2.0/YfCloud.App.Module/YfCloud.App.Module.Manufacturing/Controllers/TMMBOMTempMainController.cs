///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMTempMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/3
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMTempMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMTempDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_BOMTempMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMBOMTempMainController : ControllerBase
    {
        private readonly ITMMBOMTempMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMBOMTempMainController(ITMMBOMTempMainService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取TMMBOMTempMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMBOMTempMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user=TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, user);
        }

        /// <summary>
        /// 获取TMMBOMTempDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMBOMTempMainQueryModel>> GetWholeMainData(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetWholeMainData(requestObject, user);
        }

        /// <summary>
        /// 新增TMMBOMTempMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMBOMTempMainAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }
    }
}
