using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.App.Module.System.Models.TSMUserInfo;
using YfCloud.App.Module.System.Service;
using YfCloud.App.Module.System.Services;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// 用户维护
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMUserController : ControllerBase
    {
        private readonly IUserService _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMUserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取T_SM_UserInfo数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMUserAccountQueryAllModel>>(requestObject);
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetAsync(request, userID);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> GetCurentAsync(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMUserAccountQueryAllModel>>(requestObject);
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetCurentAsync(request, userID);
        }

        /// <summary>
        /// 新增T_SM_UserInfo数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        [AllowAnonymousAttribute]
        public async Task<ResponseObject<TSMUserAccountAddAllModel, bool>> Post(RequestObject<TSMUserAccountAddAllModel> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, userID);
        }

        /// <summary>
        /// 修改T_SM_UserInfo数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMUserAccountEditAllModel, bool>> Put(RequestObject<TSMUserAccountEditAllModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }


        /// <summary>
        /// 删除T_SM_UserInfo数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int, bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserInfo数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")]
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 个人设置 修改密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountPassWord, bool>> PersonalSetMobile(RequestObject<TSMUserAccountPassWord> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.ModifyPassWordAsync(requestObject, userID);
        }

        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountEditAllModel, bool>> PersonalSet(RequestObject<TSMUserAccountEditAllModel> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PersonalSetAsync(requestObject, userID);
        }

        /// <summary>
        /// 个人设置 --修改邮箱
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountEmail, bool>> ModifyEmail(RequestObject<TSMUserAccountEmail> requestObject, int UserID)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.ModifyEmailAsync(requestObject, userID);
        }

        /// <summary>
        /// 个人设置--修改手机[-1=错误,0=失效,1=有效,2=验证码不正确 3 手机号码被占用]
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMUserAccountMobile, int>> ModifyMobile(RequestObject<TSMUserAccountMobile> requestObject, int UserID)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.ModifyMobileAsync(requestObject, userID);
        }

        /// <summary>
        /// 获取当前用户公司员工
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TUserDeatail, List<TUserDeatail>>> GetUserInCurentCompany(string requestObject)
        {

            RequestObject<TUserDeatail> request = JsonConvert.DeserializeObject<RequestObject<TUserDeatail>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetUserInCurentCompany(request, userID);
        }

    }
}