///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMNMainController.cs
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
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNMain;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// 无配色Bom Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMBOMNMainController : ControllerBase
    {
        private readonly ITMMBOMNMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMBOMNMainController(ITMMBOMNMainService service)
        {
            _service = service;
        }        
        

        /// <summary>
        /// 获取BOM清单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMBOMNMainQueryModel>> GetWholeMainData(int requestObject)
        {
            return await _service.GetWholeMainData(requestObject);
        }
        /// <summary>
        /// 新增TMMBOMNMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMBOMNMainAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }
        
       
    }
}
