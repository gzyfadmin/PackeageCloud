///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryMainController.cs
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
using YfCloud.App.Module.Warehouse.Models.TWMInventoryMain;
using YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// T_WM_InventoryMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TWMInventoryMainController : ControllerBase
    {
        private readonly ITWMInventoryMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TWMInventoryMainController(ITWMInventoryMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        

        /// <summary>
        /// 获取盘点单主单和明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TWMInventoryMainQueryModel>> GetInventoryOrder(int id)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetInventoryOrder(id, currUser);
        }

        /// <summary>
        /// 获取盘点单明细列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMInventoryMainQueryModel>>> GetInventoryOrderList(string requestObject)
        {
            var requestGet = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetInventoryOrderListAsync(requestGet,currUser);
        }
        
        /// <summary>
        /// 获取TWMInventoryMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMInventoryMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request,currUser);
        }
        
        /// <summary>
        /// 获取TWMInventoryDetail数据表数据
        /// </summary>
        /// <param name="id">主单ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TWMInventoryDetailQueryModel>>>  GetDetailList(int id)
        {
            return await _service.GetDetailListAsync(id);
        }
        
        /// <summary>
        /// 新增TWMInventoryMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TWMInventoryMainQueryModel>> Post(RequestPost<TWMInventoryMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 修改TWMInventoryMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TWMInventoryMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject,currUser);
        }

        /// <summary>
        /// 盘点单审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> InventoryAudit(RequestPut<TWMInventoryAuditModel> request)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.InventoryAuditAsync(request, currUser);
        }

        
        /// <summary>
        /// 删除TWMInventoryMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 生成单号
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

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.IC.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

               
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
