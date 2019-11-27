///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderMain;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail;
using YfCloud.App.Module.Purchase.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetailSum;

namespace YfCloud.App.Module.Purchase.Controllers
{
    /// <summary>
    /// T_PSM_PurchaseOrderMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TPSMPurchaseOrderMainController : ControllerBase
    {
        private readonly ITPSMPurchaseOrderMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TPSMPurchaseOrderMainController(ITPSMPurchaseOrderMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.PO, currUser.CompanyID);

            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.PO.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取TPSMPurchaseOrderMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TPSMPurchaseOrderMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request,currUser);
        }
        
        /// <summary>
        /// 获取TPSMPurchaseOrderDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TPSMPurchaseOrderDetailQueryModel>>>  GetDetailList(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetDetailListAsync(requestObject, currUser);
        }

        /// <summary>
        /// 获取采购单转入库单明细数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TPSMPurchaseOrderDetailQuerySumModel>>> GetTransferList(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetTransferListAsync(requestObject, currUser);
        }

        /// <summary>
        /// 新增TPSMPurchaseOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> Post(RequestPost<TPSMPurchaseOrderMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 修改TPSMPurchaseOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TPSMPurchaseOrderMainQueryModel>> Put(RequestPut<TPSMPurchaseOrderMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }

        /// <summary>
        /// 采购单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> PurchaseAudit(RequestPut<TPSMPurchaseOrderAuditModel> requestPut)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PurchaseAuditAsync(requestPut, currUser);
        }

        /// <summary>
        /// 终止采购入库
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopWare(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopWare(requestObject, currUser);
        }

        /// <summary>
        /// 删除TPSMPurchaseOrderMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, currUser);
        }
    }
}
