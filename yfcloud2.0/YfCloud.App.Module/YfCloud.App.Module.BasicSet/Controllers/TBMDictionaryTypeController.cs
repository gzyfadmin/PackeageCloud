///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryTypeController.cs
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
using YfCloud.App.Module.BasicSet.Models.TBMDictionaryType;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// T_BM_DictionaryType Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMDictionaryTypeController : ControllerBase
    {
        private readonly ITBMDictionaryTypeService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMDictionaryTypeController(ITBMDictionaryTypeService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_BM_DictionaryType数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TBMDictionaryTypeQueryModel,List<TBMDictionaryTypeQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBMDictionaryTypeQueryModel>>(requestObject);
            int userID= TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetAsync(request, userID);
        }
        
        /// <summary>
        /// 新增T_BM_DictionaryType数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TBMDictionaryTypeAddModel,bool>> Post(RequestObject<TBMDictionaryTypeAddModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 修改T_BM_DictionaryType数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TBMDictionaryTypeEditModel,bool>> Put(RequestObject<TBMDictionaryTypeEditModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 删除T_BM_DictionaryType数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<DeleteModel,bool>> Delete(RequestObject<DeleteModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, currentUser);
        }
    }
}
