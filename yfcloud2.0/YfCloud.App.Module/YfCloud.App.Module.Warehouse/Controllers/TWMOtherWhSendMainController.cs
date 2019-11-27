///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhSendMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendMain;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Warehouse.Services;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_WM_OtherWhSendMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMOtherWhSendMainController : ControllerBase
    {
        private readonly ITWMOtherWhSendMainService _service;
        private readonly IStaticInventory _staticInventory;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMOtherWhSendMainController(ITWMOtherWhSendMainService service, IStaticInventory staticInventory, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _staticInventory = staticInventory;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TWMOtherWhSendMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMOtherWhSendMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);

            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetMainListAsync(request,userInfo);
        }

        /// <summary>
        /// 获取出库单所有的信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMOtherWhSendMainQueryModel>> GetWholeMainData(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetWholeMainData(requestObject, currUser);
        }

        /// <summary>
        /// 新增TWMOtherWhSendMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMOtherWhSendMainQueryModel>> PostAsync(RequestPost<TWMOtherWhSendMainAddModel> requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.PostAsync(requestObject, userInfo);
        }


        /// <summary>
        /// 修改TWMOtherWhSendMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TWMOtherWhSendMainQueryModel>> Put(RequestPut<TWMOtherWhSendMainEditModel> requestObject)
        {
           

            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.PutAsync(requestObject, userInfo);
        }
        
        /// <summary>
        /// 删除TWMOtherWhSendMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, userInfo);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> OtherWhAuditAsync(RequestPut<TWMOtherWhSendAuditModel> requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.OtherWhAuditAsync(requestObject, userInfo);
        }

        /// <summary>
        /// 计算可用数量
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public  ResponseObject<Decimal> Calculate(CalculateQueryModel requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);
            TWMStaQuery tWMStaQuery = new Models.TWMStaQuery() {
                EditID = requestObject.ID,
                MaterialId = requestObject.MaterialId,
                WarehouseId = requestObject.houseID,
                OperateType = OperateEnum.Other
            };
            var result = _staticInventory.GeTWMCountModel(tWMStaQuery);

            return ResponseUtil<Decimal>.SuccessResult(result.AvaiableNum);
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
            //return OrderGenerator.Generator(OrderEnum.OWS, currUser.CompanyID);
            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.OWS.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
