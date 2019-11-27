///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionMainController.cs
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
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using System.Data;
using YfCloud.Framework;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_ColorSolutionMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMColorSolutionMainController : ControllerBase
    {
        private readonly ITMMColorSolutionMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMColorSolutionMainController(ITMMColorSolutionMainService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取TMMColorSolutionMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<DataTable>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, user);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMColorSolutionMainQueryModel>>> GetColorSolution(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetColorSolution(request, user);
        }

        /// <summary>
        /// 新增TMMColorSolutionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMColorSolutionMainAddModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }
        
        /// <summary>
        /// 修改TMMColorSolutionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TMMColorSolutionMainEditModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, user);
        }
        
        /// <summary>
        /// 删除TMMColorSolutionMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
