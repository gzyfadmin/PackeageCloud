///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMProductionMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_WM_ProductionMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMProductionMainController : ControllerBase
    {
        private readonly ITWMProductionMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMProductionMainController(ITWMProductionMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TWMProductionMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMProductionMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);

            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currentUser);
        }

        /// <summary>
        /// 获取TWMProductionMain所有信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMProductionMainQueryModel>> GetWholeMainData(int requestObject)
        {

            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetWholeMainData(requestObject, currentUser);
        }

        /// <summary>
        /// 新增TWMProductionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMProductionMainQueryModel>> Post(RequestPost<TWMProductionMainAddModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 修改TWMProductionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMProductionMainQueryModel>> Put(RequestPut<TWMProductionMainEditModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 删除TWMProductionMain数据表数据，支持批量删除
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
        public async Task<ResponseObject<bool>> Audit(RequestPut<TWMProductionAduit> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestObject, currentUser);
        }

        /// <summary>
        /// 获取领料单的可转数量
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTopWholeMainData(int requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetTopWholeMainData(requestObject, currentUser);
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
            //return OrderGenerator.Generator(OrderEnum.PDC, currUser.CompanyID);

            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.PDC.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);


            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
