using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.LogStatus.Model;
using YfCloud.App.Module.LogStatus.Service;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.MongoDb;

namespace YfCloud.App.Module.LogStatus.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _service;
        
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logService"></param>
        public LogController(ILogService logService)
        {
            _service = logService;
        }

        /// <summary>
        /// 查询操作日志 条件放在postData中
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseObject<LogQueryModel, List<OperateLog>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<LogQueryModel>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);

            return _service.Get(request, userID);
        }

        /// <summary>
        /// 新增操作日志
        /// </summary>
        /// <param name="operateLog"></param>
        [HttpPost]
        [HttpOptions]
        public async Task<bool> PostAsync(List<OperateLog> operateLog)
        {
            bool result = true;
            result=await Task.Factory.StartNew(() => {

                bool tempResult = true;
                try
                {
                    MongoDbUtil.AddDoc(operateLog);
                }
                catch {
                    tempResult = false;
                }
                return tempResult;
            });

            return result;
        }

        /// <summary>
        /// 新增操作日志
        /// </summary>
        /// <param name="operateLog"></param>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public async Task<bool> PostLoginAsync(List<LoginLog> operateLog)
        {
            bool result = true;
            result = await Task.Factory.StartNew(() => {

                bool tempResult = true;
                try
                {
                    MongoDbUtil.AddDoc(operateLog);
                }
                catch
                {
                    tempResult = false;
                }
                return tempResult;
            });

            return result;
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseObject<LoginLogQuery, List<LoginLog>> GetLoginLog(string requestObject, int UserId)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<LoginLogQuery>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);

            return _service.GetLoginLog(request, userID);
        }
    }
}