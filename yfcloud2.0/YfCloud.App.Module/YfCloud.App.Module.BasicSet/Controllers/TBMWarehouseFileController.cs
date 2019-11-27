///////////////////////////////////////////////////////////////////////////////////////
// File: TBMWarehouseFileController.cs
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
using YfCloud.App.Module.BasicSet.Models.TBMWarehouseFile;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// T_BM_WarehouseFile Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TBMWarehouseFileController : ControllerBase
    {
        private readonly ITBMWarehouseFileService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TBMWarehouseFileController(ITBMWarehouseFileService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取T_BM_WarehouseFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TBMWarehouseFileQueryModel, List<TBMWarehouseFileQueryModel>>> GetAll(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBMWarehouseFileQueryModel>>(requestObject);
            int UserID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetAsync(request, UserID);
        }

        /// <summary>
        /// 获取可用仓库
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TBMWarehouseFileQueryModel,List<TBMWarehouseFileQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TBMWarehouseFileQueryModel>>(requestObject);

            if (request.QueryConditions == null || request.QueryConditions.Count() == 0)
            {
                request.QueryConditions = new List<QueryCondition>();
            }

            if (!request.QueryConditions.Any(p => p.Column.ToLower() == "status"))
            {
                request.QueryConditions.Add(new QueryCondition() { Column = "Status", Condition = ConditionEnum.Equal, Content = "1" });
            }
           

            int UserID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetAsync(request, UserID);
        }

        /// <summary>
        /// 获取仓库编码
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCode")]
        public string GetMaxID()
        {
            int UserID = TokenManager.GetUserIDbyToken(Request.Headers);
            return  _service.GetMaxID(UserID);
        }

        /// <summary>
        /// 新增T_BM_WarehouseFile数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TBMWarehouseFileAddModel,bool>> Post(RequestObject<TBMWarehouseFileAddModel> requestObject)
        {
            int userID= TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, userID);
        }
        
        /// <summary>
        /// 修改T_BM_WarehouseFile数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TBMWarehouseFileEditModel,bool>> Put(RequestObject<TBMWarehouseFileEditModel> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, userID);
        }
        
        /// <summary>
        /// 删除T_BM_WarehouseFile数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<DeleteModel,bool>> Delete(RequestObject<DeleteModel> requestObject)
        {
            var curentUser=TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, curentUser);
        }
    }
}
