///////////////////////////////////////////////////////////////////////////////////////
// File: TMMFormulaController.cs
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
using YfCloud.App.Module.Manufacturing.Models.TMMFormula;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_Formula Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMFormulaController : ControllerBase
    {
        private readonly ITMMFormulaService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMFormulaController(ITMMFormulaService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_MM_Formula数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<List<TMMFormulaQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, currUser);
        }
        
        /// <summary>
        /// 新增T_MM_Formula数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMFormulaAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);            
            return await _service.PostAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 修改T_MM_Formula数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TMMFormulaEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 删除T_MM_Formula数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
