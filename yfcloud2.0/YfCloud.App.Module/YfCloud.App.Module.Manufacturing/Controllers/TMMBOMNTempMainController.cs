///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMNTempMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// 无配色模板 Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMBOMNTempMainController : ControllerBase
    {
        private readonly ITMMBOMNTempMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMBOMNTempMainController(ITMMBOMNTempMainService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMBOMNTempMainQueryModel>>>  GetMainList(string requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetMainListAsync(request, user);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMBOMNTempMainQueryModel>> GetWholeMainData(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetWholeMainData(requestObject, user);
        }

        /// <summary>
        /// 新增TMMBOMNTempMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMBOMNTempMainAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject,user);
        }
        
       
    }
}
