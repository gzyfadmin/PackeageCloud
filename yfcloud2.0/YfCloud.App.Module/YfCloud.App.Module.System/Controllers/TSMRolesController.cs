///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRolesController.cs
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
using YfCloud.App.Module.System.Models.TSMRoles;
using YfCloud.App.Module.System.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_Roles Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMRolesController : ControllerBase
    {
        private readonly ITSMRolesService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMRolesController(ITSMRolesService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_SM_Roles数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TSMRolesQueryModel,List<TSMRolesQueryTreeModel>>>  Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMRolesQueryModel>>(requestObject);
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.GetAsync(request, userID);
        }
        
        /// <summary>
        /// 新增T_SM_Roles数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSMRolesAddModel,bool>> Post(RequestObject<TSMRolesAddModel> requestObject)
        {
            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, userID);
        }
        
        /// <summary>
        /// 修改T_SM_Roles数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMRolesEditModel,bool>> Put(RequestObject<TSMRolesEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Roles数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Roles数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")]
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<TSMRolesMoveModel, bool>> MovePostion(RequestObject<TSMRolesMoveModel> requestObject)
        {
            return await _service.MovePostion(requestObject);
        }

        /// <summary>
        /// 为员工指派角色 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<TSMRoleUserRelationEditModel, bool>> DispathDept(RequestObject<TSMRoleUserRelationEditModel> requestObject)
        {
            return await _service.DispathDept(requestObject);
        }


        /// <summary>
        /// 获取某角色下的所有员工
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>> GetDispathUserAsync(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMRoleUserRelationEditModel>>(requestObject);

            return await _service.GetDispathUserAsync(request);
        }

        /// <summary>
        /// 删除某个角色下的用户
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        public async Task<ResponseObject<int[], bool>> RemoveRoleForUser(RequestObject<int[]> requestObject)
        {
            return await _service.RemoveRoleForUser(requestObject);
        }
    }
}
