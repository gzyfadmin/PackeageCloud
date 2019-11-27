using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YfCloud.App.Module.Report.Models;
using YfCloud.App.Module.Report.Services;
using YfCloud.App.Module.Warehouse.Models.TPMDoc;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.Models.CacheModels;
using YfCloud.Utilities.MongoDbModel;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StaOnlineTimeController : ControllerBase
    {
        private readonly IStaOnlineTime _staOnlineTime;

        private readonly INoticeService _noticeService;

        private readonly AppConfig _appConfig;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="option"></param>
        /// <param name="staOnlineTime"></param>
        /// <param name="noticeService"></param>
        public StaOnlineTimeController(IOptions<AppConfig> option, IStaOnlineTime staOnlineTime, INoticeService noticeService)
        {
            _staOnlineTime = staOnlineTime;
            _appConfig = option.Value;
            _noticeService = noticeService;
        }


        /// <summary>
        /// 获取TWMPurchaseMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<StaOnlineResult>> GetStaOnline()
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _staOnlineTime.GetStaOnline(currentUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async  Task<ResponseObject<TPMDocQueryModel>> GetFile()
        {
            return  await _staOnlineTime.GetDoc();
        }

        /// <summary>
        /// 获取待处理的消息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
       public  ResponseObject<List<ToDoMgModel>> GetToDoModel(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return _noticeService.GetToDoModel(request, currentUser);
        }

        /// <summary>
        /// 读消息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public ResponseObject<bool> Read(string requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return _noticeService.Read(requestObject, currentUser);

        }
    }
}
