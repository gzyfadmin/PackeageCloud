///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.BasicSet.Models.TBMDictionary;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// T_BM_Dictionary Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMDictionaryController : ControllerBase
    {
        private readonly ITBMDictionaryService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMDictionaryController(ITBMDictionaryService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_BM_Dictionary数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<List<TBMDictionaryQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var  user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, user);
        }
        
        /// <summary>
        /// 新增T_BM_Dictionary数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TBMDictionaryAddModel,bool>> Post(RequestObject<TBMDictionaryAddModel> requestObject)
        {
            var userID = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, userID);
        }
        
        /// <summary>
        /// 修改T_BM_Dictionary数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TBMDictionaryEditModel,bool>> Put(RequestObject<TBMDictionaryEditModel> requestObject)
        {
            var userID = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, userID);
        }
        
        /// <summary>
        /// 删除T_BM_Dictionary数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<DeleteModel,bool>> Delete(RequestObject<DeleteModel> requestObject)
        {
            var userID = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, userID);
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GerRandom()
        {
            return _service.GerRandom();
        }

    }
}
