using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.App.Module.Manufacturing.Services;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// 配色BOM新
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMBOMMainNewController : ControllerBase
    {
        private readonly ITMMBOMMainNewService _service;

        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="service"></param>
        public TMMBOMMainNewController(ITMMBOMMainNewService service)
        {
            _service = service;
        }





        /// <summary>
        /// 新增TMMBOMMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMBOMMainAddNewModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }

        /// <summary>
        /// 获取BOM清单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMBOMMainQueryNewModel>> GetWholeMainData(int requestObject)
        {
            return await _service.GetWholeMainData(requestObject);
        }
    }
}