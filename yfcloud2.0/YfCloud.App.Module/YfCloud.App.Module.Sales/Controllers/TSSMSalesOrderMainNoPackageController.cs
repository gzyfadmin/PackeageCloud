using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailNP;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderMainNP;
using YfCloud.App.Module.Sales.Services;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.Models;

namespace YfCloud.App.Module.Sales.Controllers
{
    /// <summary>
    /// 销售物料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSSMSalesOrderMainNoPackageController : ControllerBase
    {
        private readonly ITSSMSalesOrderMainNPService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSSMSalesOrderMainNoPackageController(ITSSMSalesOrderMainNPService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }

        /// <summary>
        /// 获取TSSMSalesOrderMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderMainQueryNPModel>>> GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }

        /// <summary>
        /// 获取TSSMSalesOrderDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderDetailQueryNPModel>>> GetDetailList(int requestObject)
        {
            return await _service.GetDetailListAsync(requestObject);
        }

        /// <summary>
        /// 获取可转出库单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderDetailQueryNPModel>>> GetTransferList(int requestObject)
        {
            return await _service.GetTransferList(requestObject);
        }



        /// <summary>
        /// 新增TSSMSalesOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSSMSalesOrderMainQueryNPModel>> Post(RequestPost<TSSMSalesOrderMainAddNPModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }

        /// <summary>
        /// 修改TSSMSalesOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TSSMSalesOrderMainEditNPModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }

        /// <summary>
        /// 销售订单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> SalesOrderAudit(RequestPut<TSSMSalesOrderMainAuditNPModel> requestPut)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestPut, currUser);
        }

        /// <summary>
        /// 终止出库
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopDeposit(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopDeposit(requestObject, currUser);
        }

        /// <summary>
        /// 删除TSSMSalesOrderMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
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
            //return OrderGenerator.Generator(OrderEnum.SO, currUser.CompanyID);

            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

            return _codeMakers.Where(p => p.ProvideName == OrderEnum.SO.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);
        }

    }
}