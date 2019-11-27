using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models;
using YfCloud.App.Module.Platform.Services;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// 数据字典
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryValuesController : ControllerBase
    {
        private readonly IDictionaryValuesService _service;

        /// <summary>
        /// 默认构造方法
        /// </summary>
        /// <param name="service"></param>
        public DictionaryValuesController(IDictionaryValuesService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取Key对应的value集合
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TDataDicDetailModel>>(requestObject);
            return await _service.GetAsync(request);
        }
    }
}