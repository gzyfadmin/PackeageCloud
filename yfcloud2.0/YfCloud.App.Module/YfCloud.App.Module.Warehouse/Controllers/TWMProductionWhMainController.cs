///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionWhMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// 生产入库
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMProductionWhMainController : ControllerBase
    {
        private readonly ITWMProductionWhMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMProductionWhMainController(ITWMProductionWhMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TWMProductionWhMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMProductionWhMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            CurrentUser currentUser= TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currentUser);
        }
        
        
        /// <summary>
        /// 新增TWMProductionWhMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> Post(RequestPost<TWMProductionWhMainAddModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 修改TWMProductionWhMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> Put(RequestPut<TWMProductionWhMainEditModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currentUser);
        }
        
        /// <summary>
        /// 删除TWMProductionWhMain数据表数据，支持批量删除
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
        /// 获取单号生成规则
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.PDR, currUser.CompanyID);

            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return  _codeMakers.Where(p => p.ProvideName == OrderEnum.PDR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> GetWholeMainData(int requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetWholeMainData(requestObject, currentUser);
        }
    }
}
