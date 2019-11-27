using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Report.Models.Sales;
using YfCloud.App.Module.Report.Services;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Controllers
{
    /// <summary>
    /// 销售报表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {
        private readonly ISaleRepostServcie _service;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SalesReportController(ISaleRepostServcie service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取销售订单金额统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<SalesAmountCountModel>> GetSalesAmountCount()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSalesAmountCount(currUser);
        }

        /// <summary>
        /// 获取当前月份销售订单统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<SalesmanAmountCountModel>>> GetSalesmanAmountCount()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSalesmanAmountCount(currUser);
        }
        /// <summary>
        /// 获取所有销售业务员信息（已产生订单）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<SalesManModel>>> GetSaleMan()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSaleMan(currUser);
        }

        /// <summary>
        /// 获取所有客户信息（已产生订单）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<SalesCustomerModel>>> GetCustomers()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetCustomers(currUser);
        }
        /// <summary>
        /// 款型分布
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<SalesPackageAmountCountModel>>> GetSalePackageCount()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSalePackageCount(currUser);
        }

        /// <summary>
        /// 获取销售一览表信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<SalesListModel>> GetSalesList(string requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetSalesList(request, currUser);
        }

    }
}