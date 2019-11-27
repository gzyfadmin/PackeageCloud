///////////////////////////////////////////////////////////////////////////////////////
// File: TDataDicDetailController.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models;
using YfCloud.App.Module.Platform.Service;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_DataDicDetail Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TDataDicDetailController : ControllerBase
    {
        private readonly ITDataDicDetailService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TDataDicDetailController(ITDataDicDetailService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_DataDicDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TDataDicDetailModel,List<TDataDicDetailModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TDataDicDetailModel>>(requestObject);
            return await _service.GetAsync(request);
        }


        /// <summary>
        /// 新增T_DataDicDetail数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TDataDicDetailModel,bool>> Post(RequestObject<TDataDicDetailModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_DataDicDetail数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TDataDicDetailModel,bool>> Put(RequestObject<TDataDicDetailModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_DataDicDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TDataDicDetailModel,bool>> Delete(RequestObject<TDataDicDetailModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_DataDicDetail数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_DataDicDetail数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")]
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
