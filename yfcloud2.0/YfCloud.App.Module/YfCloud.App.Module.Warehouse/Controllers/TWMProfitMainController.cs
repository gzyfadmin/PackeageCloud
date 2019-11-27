///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMProfitMain;
using YfCloud.App.Module.Warehouse.Models.TWMProfitDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_WM_ProfitMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMProfitMainController : ControllerBase
    {
        private readonly ITWMProfitMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMProfitMainController(ITWMProfitMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 盘盈入库单号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return  _codeMakers.Where(p => p.ProvideName == OrderEnum.IR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取盘盈入库单主单和明细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMProfitMainQueryModel>> GetProfitOrder(int Id)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProfitOrderAsync(Id, currUser);
        }

        /// <summary>
        /// 获取TWMProfitMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMProfitMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request,currUser);
        }
        
        /// <summary>
        /// 获取TWMProfitDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMProfitDetailQueryModel>>>  GetDetailList(int requestObject)
        {
            return await _service.GetDetailListAsync(requestObject);
        }
        
        /// <summary>
        /// 新增TWMProfitMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMProfitMainQueryModel>> Post(RequestPost<TWMProfitMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 修改TWMProfitMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TWMProfitMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 盘盈入库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> ProfitAudit(RequestPut<TWMProfitMainAuditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.ProfitAuditAsync(requestObject, currUser);
        }

        /// <summary>
        /// 删除TWMProfitMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
    }
}
