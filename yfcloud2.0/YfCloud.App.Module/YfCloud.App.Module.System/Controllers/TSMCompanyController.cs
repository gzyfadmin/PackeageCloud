///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompanyController.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.TSMCompany;
using YfCloud.App.Module.System.Service;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.Framework;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_Company Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMCompanyController : ControllerBase
    {
        private readonly ITSMCompanyService _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMCompanyController(ITSMCompanyService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取T_SM_Company数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMCompanyQueryModel, List<TSMCompanyQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMCompanyQueryModel>>(requestObject);
            return await _service.GetAsync(request);
        }

        ///// <summary>
        ///// 获取当前用户所在的企业信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[HttpOptions]
        //[Route("[action]")]
        //public async Task<ResponseObject<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>> FetchCompanyInfo(TSMCompanyForCurentUserIDModel model)
        //{
        //    return await _service.FetchCompanyInfo(model);
        //}


        /// <summary>
        /// 新增T_SM_Company数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<string>> Post(RequestPost<TSMCompanyAddModel> requestObject)
        {
            var userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.PostAsync(requestObject, userInfo);
        }

        /// <summary>
        /// 判断公司是否存在，true存在，false不存在
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> CheckIsExists(RequestPost<TSMCompanyCheckModel> requestObject)
        {
            var userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.CheckIsExists(requestObject, userInfo);
        }


        /// <summary>
        /// 修改T_SM_Company数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMCompanyEditModel, bool>> Put(RequestObject<TSMCompanyEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Company数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TSMCompanyAddModel, bool>> Delete(RequestObject<TSMCompanyAddModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Company数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int, bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Company数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")]
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 获取当前用户企业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<int, TSMCompanyQueryAllModel>> PersonalGet()
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PersonalGet(userID);
        }

        /// <summary>
        /// 企业设置
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TSMCompanyAllEditModel, bool>> ModifyCurentCompany(RequestObject<TSMCompanyAllEditModel> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.ModifyCurentCompany(requestObject, userID);
        }

    }
}
