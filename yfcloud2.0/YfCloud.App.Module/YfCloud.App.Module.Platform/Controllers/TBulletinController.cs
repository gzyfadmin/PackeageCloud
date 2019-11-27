///////////////////////////////////////////////////////////////////////////////////////
// File: TBulletinController.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models;
using YfCloud.App.Module.Platform.Service;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_Bulletin Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBulletinController : ControllerBase
    {
        private readonly ITBulletinService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBulletinController(ITBulletinService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_Bulletin数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TBulletinModel,List<TBulletinModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBulletinModel>>(requestObject);
            return await _service.GetAsync(request);
        }


        /// <summary>
        /// 新增T_Bulletin数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TBulletinModel,bool>> Post(RequestObject<TBulletinModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_Bulletin数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TBulletinModel,bool>> Put(RequestObject<TBulletinModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_Bulletin数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TBulletinModel,bool>> Delete(RequestObject<TBulletinModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_Bulletin数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_Bulletin数据表数据，通过主表主键删除主从数据，批量删除
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
