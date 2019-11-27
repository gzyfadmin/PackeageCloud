///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRolePermissionsController.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.TSMRolePermissions;
using YfCloud.App.Module.System.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_RolePermissions Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMRolePermissionsController : ControllerBase
    {
        private readonly ITSMRolePermissionsService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMRolePermissionsController(ITSMRolePermissionsService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_SM_RolePermissions数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMRolePermissionsQueryModel,List<TSMRolePermissionsQueryModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMRolePermissionsQueryModel>>(requestObject);
            return await _service.GetAsync(request);
        }

        /// <summary>
        /// 获取当前用户所在公司的菜单树
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetMenuTree")]
        public async Task<ResponseObject<string,List<MenuTreeModel>>> GetMenuTree()
        {
            var userId = TokenManager.GetUserIDbyToken(HttpContext.Request.Headers);
            return await _service.GetMenuTree(userId);
        }


        /// <summary>
        /// 新增T_SM_RolePermissions数据表数据，只支持PostDataList传值
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSMRolePermissionsAddModel,bool>> Post(RequestObject<TSMRolePermissionsAddModel> requestObject)
        {
            var userId = TokenManager.GetUserIDbyToken(HttpContext.Request.Headers);
            return await _service.PostAsync(requestObject, userId);
        }
        
        /// <summary>
        /// 删除T_SM_RolePermissions数据表数据，PostData传角色Id的值
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            var userId = TokenManager.GetUserIDbyToken(HttpContext.Request.Headers);
            return await _service.DeleteAsync(requestObject,userId);
        }

    }
}
