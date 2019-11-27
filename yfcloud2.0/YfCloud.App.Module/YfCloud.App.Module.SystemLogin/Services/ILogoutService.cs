using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.SystemLogin.Services
{
    /// <summary>
    /// 系统退出服务接口
    /// </summary>
    public interface ILogoutService
    {
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        Task<ResponseObject<bool>> LogoutAsync(CurrentUser currentUser);
    }
}
