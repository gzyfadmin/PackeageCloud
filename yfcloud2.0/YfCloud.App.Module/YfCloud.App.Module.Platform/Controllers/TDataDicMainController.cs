///////////////////////////////////////////////////////////////////////////////////////
// File: TDataDicMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18
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
using YfCloud.App.Module.Platform.Models.TDataDicMain;
using YfCloud.App.Module.Platform.Service;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_DataDicMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TDataDicMainController : ControllerBase
    {
        private readonly ITDataDicMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TDataDicMainController(ITDataDicMainService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_DataDicMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TDataDicMainModel,List<TDataDicMainModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TDataDicMainModel>>(requestObject);
            return await _service.GetAsync(request);
        }

        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TDataDicMainModel, List<DataTreeNode>>> GetDataTree(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TDataDicMainModel>>(requestObject);
            return await _service.GetTreeAsync(request);
        }
        


        /// <summary>
        /// 新增T_DataDicMain数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TDataDicMainModel,bool>> Post(RequestObject<TDataDicMainModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_DataDicMain数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TDataDicMainModel,bool>> Put(RequestObject<TDataDicMainModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_DataDicMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]        
        public async Task<ResponseObject<TDataDicMainModel,bool>> Delete(RequestObject<TDataDicMainModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_DataDicMain数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_DataDicMain数据表数据，通过主表主键删除主从数据，批量删除
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
