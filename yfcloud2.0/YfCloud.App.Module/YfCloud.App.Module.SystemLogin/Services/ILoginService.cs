using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.SystemLogin.Models.TSMLoginLog;

namespace YfCloud.App.Module.SystemLogin.Services
{
    /// <summary>
    /// 系统登录服务接口
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// 验证登录是否需要验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResponseObject<int>> GetVerification(RequestGet request);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserLoginResult>> LoginAsync(RequestPost<TSMUserAccountAddModel> requestObject);

        ResponseObject<int, UserInfo> GetInfo(int iUserId);
    }
}
