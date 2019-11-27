///////////////////////////////////////////////////////////////////////////////////////
// File: TWMSalesMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMSalesMain;
using YfCloud.App.Module.Warehouse.Models.TWMSalesDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitMain;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    ///销售出库
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMSalesMainController : ControllerBase
    {
        private readonly ITWMSalesMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMSalesMainController(ITWMSalesMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TWMSalesMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMSalesMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }


        /// <summary>
        /// 获取出库单所有的信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMSalesMainQueryModel>> GetWholeMainData(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetWholeMainData(requestObject, currUser);
        }


        /// <summary>
        /// 新增TWMSalesMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMSalesMainQueryModel>> Post(RequestPost<TWMSalesMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 修改TWMSalesMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMSalesMainQueryModel>> Put(RequestPut<TWMSalesMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 删除TWMSalesMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.DeleteAsync(requestObject, currUser);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMSalesMainAduit> requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestObject, userInfo);
        }

        /// <summary>
        /// 获取出库单号生成规则
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.SOWR, currUser.CompanyID);


            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.SOWR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
