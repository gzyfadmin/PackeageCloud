using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.App.Module.SystemLogin.Services;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;
using YfCloud.DBModel;
using Microsoft.AspNetCore.Authorization;
using YfCloud.Utilities.MongoDb;
using YfCloud.Framework;
using YfCloud.Framework.WebApi;
using YfCloud.Framework;
using YfCloud.App.Module.SystemLogin.Models.TSMLoginLog;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YfCloud.Caches;
using YfCloud.Utilities;

namespace YfCloud.App.Module.SystemLogin.Controllers
{
    /// <summary>
    /// 系统登录
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        private readonly IDbContext _db;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="service"></param>
        public LoginController(ILoginService service, IDbContext db)
        {
            _service = service;
            _db = db;
        }

        /// <summary>
        /// 验证登录是否需要验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回[0=错误,1=无需,2=验证]</returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public Task<ResponseObject<int>> GetVerification(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return _service.GetVerification(request);
        }
        /// <summary>
        /// 系统登录，手机账号或邮箱账号必填一个
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        [AllowAnonymous]
        public async Task<ResponseObject<TSMUserLoginResult>> LoginPost(RequestPost<TSMUserAccountAddModel> requestObject)
        {
            ResponseObject<TSMUserLoginResult> result;

            result = await _service.LoginAsync(requestObject);



            var task = Task.Factory.StartNew(() =>
              {
                  try
                  {
                      bool isSucess = string.IsNullOrEmpty(result.Data.Token) ? false : true;

                      if (isSucess == true)
                      {
                          string token = result.Data.Token.Split('.')[1];
                          var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(token));
                          int userID = Convert.ToInt32(payLoad["UserID"]);
                          string ID = payLoad["ID"].ToString();
                          int CompanyID = Convert.ToInt32(payLoad["CompanyID"]);
                          if (CompanyID == 0) //没有加入公司的员工不保存登陆日志
                          {
                              return;
                          }


                          //写 登陆状态到redis 
                          var redis = CacheFactory.Instance(CacheType.Redis);
                          UserStatus userStatus = new UserStatus() { ID = ID, LastRefreshTime = DateTime.Now };
                          string redisKey = string.Format(CacheKeyString.UserLoginAllKey, userID, CompanyID, ID);
                          redis.AddOrUpdateKey<UserStatus>(redisKey, userStatus, 90);


                          //写登陆日志到 MangoDB
                          SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(userID, _db.Instance);
                          string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                          LoginLog loginLog = new LoginLog();
                          loginLog.LoginID = ID;
                          loginLog.Account = sMUserInfo.AccountName;
                          loginLog.CompanyID = sMUserInfo.CompanyId.Value;
                          loginLog.Description = isSucess ? LoginTypeEum.LoginSuccess : LoginTypeEum.LoginFail;
                          loginLog.IpAddress = ipAddress;
                          loginLog.LoginTime = DateTime.Now;
                          loginLog.RealName = sMUserInfo == null ? "" : sMUserInfo.RealName;
                          loginLog.RoleName = sMUserInfo == null ? "" : sMUserInfo.RoleName;
                          loginLog.Status = isSucess ? LoginStatusEum.Logining : LoginStatusEum.LogOut;

                          MongoDbUtil.AddDoc(loginLog);
                      }
                      else
                      {
                          //LoginLog loginLog = new LoginLog();
                          //loginLog.ID = Guid.NewGuid().ToString();
                          //loginLog.Account = sMUserInfo.AccountName;
                          //loginLog.CompanyID = sMUserInfo.CompanyId.Value;
                          //loginLog.Description = isSucess ? LoginTypeEum.LoginSuccess : LoginTypeEum.LoginFail;
                          //loginLog.IpAddress = ipAddress;
                          //loginLog.LoginTime = DateTime.Now;
                          //loginLog.RealName = sMUserInfo?.RealName;
                          //loginLog.RoleName = sMUserInfo?.RoleName;
                          //loginLog.Status = isSucess ? LoginStatusEum.Logining : LoginStatusEum.LogOut;

                          //MongoDbUtil.AddDoc(loginLog);
                      }



                  }
                  catch (Exception EX)
                  {
                  }
              });

            return result;

        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            return new
            {
                code = 0,
                data = new
                {
                    roles = new string[] { "admin" },
                    introduction = "I am a super administrator",
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    name = "Super Admin"
                }
            };
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseObject<int, UserInfo> GetInfo()
        {
            var userId = TokenManager.GetUserIDbyToken(Request.Headers);

            return _service.GetInfo(userId);
        }
    }
}