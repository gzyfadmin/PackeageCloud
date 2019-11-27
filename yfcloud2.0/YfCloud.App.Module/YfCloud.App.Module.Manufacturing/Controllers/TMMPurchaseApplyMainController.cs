///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPurchaseApplyMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_PurchaseApplyMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMPurchaseApplyMainController : ControllerBase
    {
        private readonly ITMMPurchaseApplyMainService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMPurchaseApplyMainController(ITMMPurchaseApplyMainService service)
        {
            _service = service;
        }        
        
        /// <summary>
        /// 获取生产采购申请单转采购申请的主单信息列表
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMPurchaseApplyMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }
        
        /// <summary>
        /// 获取TMMPurchaseApplyDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>>  GetDetailList(int requestObject)
        {
            return await _service.GetDetailListAsync(requestObject);
        }

        /// <summary>
        /// 获取生产采购申请单转采购单的明细列表
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetToPurchaseList(int requestObject)
        {
            return await _service.GetToPurchaseListAsync(requestObject);
        }

        /// <summary>
        /// 终止生产采购
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopPurchase(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopPurchase(requestObject, currUser);
        }
    }
}
