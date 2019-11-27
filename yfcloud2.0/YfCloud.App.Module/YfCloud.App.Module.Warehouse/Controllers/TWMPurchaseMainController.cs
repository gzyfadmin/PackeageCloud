///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPurchaseMainController.cs
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
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseMain;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// 采购入库
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMPurchaseMainController : ControllerBase
    {
        private readonly ITWMPurchaseMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMPurchaseMainController(ITWMPurchaseMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TWMPurchaseMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMPurchaseMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetMainListAsync(request, currentUser);
        }

        /// <summary>
        /// 获取入库单的所有信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> GetWholeMainData(int requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetWholeMainData(requestObject, currentUser);
        }

        /// <summary>
        /// 新增TWMPurchaseMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> Post(RequestPost<TWMPurchaseMainAddModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 修改TWMPurchaseMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> Put(RequestPut<TWMPurchaseMainEditModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 删除TWMPurchaseMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, currentUser);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMPurchaseMainAduit> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestObject,currentUser);
        }

        /// <summary>
        /// 获取单号生成规则
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.WO, currUser.CompanyID);

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

               return _codeMakers.Where(p => p.ProvideName == OrderEnum.WO.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
