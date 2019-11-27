using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Report.Models.Inventory;
using YfCloud.App.Module.Report.Models.ProductionOrder;
using YfCloud.App.Module.Report.Models.Warehouse;
using YfCloud.App.Module.Report.Services;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrderReportController : ControllerBase
    {
        private readonly IProductionOrderReportService _service;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProductionOrderReportController(IProductionOrderReportService service)
        {
            _service = service;
        }
        /// <summary>
        /// 生产车间入库数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<ProductionOrderWorkshopCountModel>>> GetInventoryAmountCount()
        {
            // [AllowAnonymous]--去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionOrderWorkshop(currUser);
        }

        /// <summary>
        /// 生产入库态势(天) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingDay()
        {
            // [AllowAnonymous]--去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionWarehousingDay(currUser);
        }

        /// <summary>
        /// 生产入库态势(月) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingMonth()
        {
            // [AllowAnonymous]--去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionWarehousingMonth(currUser);
        }
        /// <summary>
        /// 生产入库态势(周) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<ProductionWarehousingModel>> GetProductionWarehousingWeek()
        {
            // [AllowAnonymous]--去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionWarehousingWeek(currUser);
        }

        /// <summary>
        /// 生产订单数量分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<ProductionOrderQuantityAnalysisModel>> GetProductionOrderQuantityAnalysis()
        {
            // [AllowAnonymous]--去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionOrderQuantityAnalysis(currUser);
        }

        /// <summary>
        /// 生产客户 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<ProductionOrderCustomerModel>>> GetCustomer()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetCustomer(currUser);
        }
        /// <summary>
        /// 生产订单号 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<ProductionOrderNoModel>>> GetProductionNo()
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetProductionNo(currUser);
        }

        /// <summary>
        /// 生产一览表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<ProductionOrderListModel>> GetProdutionCountList(string requestObject)
        {
            //[AllowAnonymous] --去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetProdutionCountList(request, currUser);
        }
    }
}