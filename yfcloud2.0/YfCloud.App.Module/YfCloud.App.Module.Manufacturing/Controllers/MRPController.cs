using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOMSum;
using YfCloud.App.Module.Manufacturing.Services;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Controllers
{

    /// <summary>
    /// MRP
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MRPController : ControllerBase
    {
        private readonly IMRPService _service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mRPService"></param>
        public MRPController(IMRPService mRPService)
        {
            _service = mRPService;
        }

        /// <summary>
        /// 自动计算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<MrpResultModel>> AutoComputeMRP(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AutoComputeMRP(requestObject,1, user);
        }

        /// <summary>
        /// 自动计算无配色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<MrpResultModel>> AutoComputeNOMRP(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AutoComputeMRP(requestObject, 2, user);
        }

        /// <summary>
        /// 转领料
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<NOEntity>> TransPick(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.TransPick(requestObject, user);
        }

        /// <summary>
        /// 转采购
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<NOEntity>> TransPurchase(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.TransPurchase(requestObject, user);
        }

        /// <summary>
        /// 采购完成重新领料
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]

        public async Task<ResponseObject<NOEntity>> RePick(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.RePick(requestObject, user);
        }

        /// <summary>
        /// 获取BOM
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<SumQueryApiModel>> GetBom(int requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetBom(requestObject, user);
        }



    }
}