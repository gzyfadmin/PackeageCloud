using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.SystemLogin.Models.TSMLoginLog;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;
using YfCloud.DBModel;

namespace YfCloud.App.Module.SystemLogin.Services
{
    /// <summary>
    /// 系统退出服务接口
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ILogoutService))]
    public class LogoutService : ILogoutService
    {
        private readonly IDbContext _db;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public LogoutService(IDbContext dbContext)
        {
            _db = dbContext;
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> LogoutAsync(CurrentUser currentUser)
        {
            try
            {
                var logDbModel = new TSMLoginLogDbModel
                {
                    AccountId = currentUser.UserID,
                    LogTime = DateTime.Now,
                    LogType = 1
                };
                _db.Instance.Insertable(logDbModel).ExecuteCommand();
                return ResponseUtil<bool>
                    .SuccessResult(true);
            }catch(Exception ex)
            {
                return ResponseUtil<bool>
                    .FailResult(false, $"退出发生异常,信息如下：{Environment.NewLine} {ex.Message}");
            }
        }
    }
}
