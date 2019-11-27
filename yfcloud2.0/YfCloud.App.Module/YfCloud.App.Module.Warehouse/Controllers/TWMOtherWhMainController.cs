///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_WM_OtherWhMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMOtherWhMainController : ControllerBase
    {
        private readonly ITWMOtherWhMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMOtherWhMainController(ITWMOtherWhMainService service,IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }
        
        /// <summary>
        /// 获取入库单号生成规则
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.OWR, currUser.CompanyID);
            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.OWR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取入库单主表和详情信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMOtherWhMainQueryModel>> GetWarehouseOrder(int Id)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetWarehouseOrderAsync(Id, currUser);
        }

        /// <summary>
        /// 获取TWMOtherWhMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMOtherWhMainQueryModel,List<TWMOtherWhMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TWMOtherWhMainQueryModel>>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetMainListAsync(request,currUser);
        }
        
        /// <summary>
        /// 获取TWMOtherWhDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<int,List<TWMOtherWhDetailQueryModel>>>  GetDetailList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<int>>(requestObject);
            return await _service.GetDetailListAsync(request);
        }
        
        /// <summary>
        /// 新增TWMOtherWhMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>> Post(RequestObject<TWMOtherWhMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PostAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 修改TWMOtherWhMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMOtherWhMainEditModel,bool>> Put(RequestObject<TWMOtherWhMainEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }
        
        /// <summary>
        /// 其他入库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> OtherWhAudit(RequestPut<OtherWarehousingAuditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.OtherWhAuditAsync(requestObject,currUser);
        }

        /// <summary>
        /// 其他入库单撤销审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> OtherWhCancelAudit(RequestPut<OtherWarehousingAuditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.OtherWhCancelAuditAsync(requestObject, currUser);
        }

        /// <summary>
        /// 删除TWMOtherWhMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<DeleteModel,bool>> Delete(RequestObject<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
