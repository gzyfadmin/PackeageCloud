using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.LogStatus.Model;
using YfCloud.Models;
using YfCloud.Utilities;

namespace YfCloud.App.Module.LogStatus.Service
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        ResponseObject<LogQueryModel, List<OperateLog>> Get(RequestObject<LogQueryModel> requestObject, int UserId);

        /// <summary>
        /// 获取登陆日志
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        ResponseObject<LoginLogQuery, List<LoginLog>> GetLoginLog(RequestObject<LoginLogQuery> requestObject, int UserId);

    }
}
