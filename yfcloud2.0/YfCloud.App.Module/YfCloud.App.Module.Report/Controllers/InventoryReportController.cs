using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Report.Models.Inventory;
using YfCloud.App.Module.Report.Models.Warehouse;
using YfCloud.App.Module.Report.Services;
using YfCloud.DBModel;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryReportController : ControllerBase
    {
        private readonly IInventoryReportService _service;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public InventoryReportController(IInventoryReportService service)
        {
            _service = service;
        }
        /// <summary>
        /// 总入库量，出库量，库存分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<InventoryAmountCountModel>> GetInventoryAmountCount()
        {
            //[AllowAnonymous] --去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetInventoryAmountCount(currUser);
        }

        /// <summary>
        /// 成品库存态势
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<InventoryAmountCountDayModel>> GetInventoryAmountCountDay()
        {
            //[AllowAnonymous] --去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetInventoryAmountCountDay(currUser);
        }

        /// <summary>
        /// 库存量分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<InventoryAnalysisModel>>> GetInventoryAnalysis()
        {
            //[AllowAnonymous] --去掉特性不在验证token
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetInventoryAnalysis(currUser);
        }


        /// <summary>
        /// 库存一览表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<InventoryListModel>> GetInventoryCountList(string requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return await _service.GetInventoryCountList(request, currUser);
        }
    }
}