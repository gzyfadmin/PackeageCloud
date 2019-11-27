///////////////////////////////////////////////////////////////////////////////////////
// File: TPMMenusController.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models.TPMMenus;
using YfCloud.App.Module.System.Models.TPMMenus;
using YfCloud.App.Module.System.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_PM_Menus Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMenusController : ControllerBase
    {
        private readonly ITPMMenusService _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMenusController(ITPMMenusService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取T_PM_Menus数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusQueryModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TPMMenusQueryModel>>(requestObject);
            return await _service.GetAsync(request);
        }

        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusTreeModel>>> GetTree(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TPMMenusQueryModel>>(requestObject);
            return await _service.GetTreeAsync(request); 
        }

        /// <summary>
        /// 新增T_PM_Menus数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TPMMenusAddModel, bool>> Post(RequestObject<TPMMenusAddModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_PM_Menus数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TPMMenusEditModel, bool>> Put(RequestObject<TPMMenusEditModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 移动菜单的位置(上移下移)
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<TPMMenusMoveModel, bool>> MovePostion(RequestObject<TPMMenusMoveModel> requestObject)
        {
            return await _service.MovePostion(requestObject);
        }

        /// <summary>
        /// 删除T_PM_Menus数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<TPMenusDeleteAllModel, bool>> Delete(RequestObject<TPMenusDeleteAllModel> requestObject)
        {
            int UserID = TokenManager.GetUserIDbyToken(Request.Headers);
            return await _service.DeleteAsync(requestObject, UserID);
        }
    }
}
