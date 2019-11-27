///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMMainController.cs
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
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_BOMMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMBOMMainController : ControllerBase
    {
        private readonly ITMMBOMMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMBOMMainController(ITMMBOMMainService service)
        {
            _service = service;
        }        
        
       
        
        /// <summary>
        /// 新增TMMBOMMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMBOMMainAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }

        /// <summary>
        /// 获取BOM清单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMBOMMainQueryModel>> GetWholeMainData(int requestObject)
        {
            return await _service.GetWholeMainData(requestObject);
        }
    }
}
