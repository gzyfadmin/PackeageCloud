using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.App.Module.Warehouse.Services;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// 物料库存量
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseAmountController : ControllerBase
    {
        private readonly IStaticInventory _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="service"></param>
        public WarehouseAmountController(IStaticInventory service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取物料库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SkipLog]
        public ResponseObject<TWMCountModel> GetAmount(RequestPost<TWMStaQuery> requestPost)
        {
            return ResponseUtil<TWMCountModel>.SuccessResult(_service.GeTWMCountModel(requestPost.PostData));
        }
    }
    
}