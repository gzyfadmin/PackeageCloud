using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Report.Models.Purchase;
using YfCloud.App.Module.Report.Services;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Controllers
{
    /// <summary>
    /// 采购报表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseReportController : ControllerBase
    {
        private readonly IPurchaseReportService _service;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PurchaseReportController(IPurchaseReportService service)
        {
            _service = service;
        }
        /// <summary>
        /// 供应商采购金额分析 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<PurchaseSupplierAmountCountModel>>> GetPurchaseSupplierAmountCount()
        {
            //[AllowAnonymous] --去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseSupplierAmountCount(currUser);
        }

        /// <summary>
        /// 采购订单金额态势(天) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountDay()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseAmountCountDay(currUser);
        }

        /// <summary>
        /// 采购订单金额态势(周) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountWeek()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseAmountCountWeek(currUser);
        }
        /// <summary>
        /// 采购订单金额态势(月) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<PurchaseSupplierAmountCountDayModel>> GetPurchaseAmountCountMonth()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseAmountCountMonth(currUser);
        }
        /// <summary>
        /// 采购员采购业绩 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<PurchaseBuyerAmountCountModel>>> GetPurchaseBuyerAmountCount()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseBuyerAmountCount(currUser);
        }

        /// <summary>
        /// 采购业务员 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<PurchaseBuyerModel>>> GetSaleMan()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSaleMan(currUser);
        }
        /// <summary>
        /// 采购供应商 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<PurchaseSupplierModel>>> GetSupplier()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetSupplier(currUser);
        }

        /// <summary>
        /// 采购单号 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<PurchaseOrderNoModel>>> GetPurchaseOrderNo()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetPurchaseOrderNo(currUser);
        }
        /// <summary>
        /// 采购一览表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<PurchaseListModel>> GetPurchase(string requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetPurchaseCountList(request, currUser);
        }
    }
}
