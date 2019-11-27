///////////////////////////////////////////////////////////////////////////////////////
// File: TTenantPermissionsController.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Platform.Models;
using YfCloud.App.Module.Platform.Service;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Platform.Controllers
{
    /// <summary>
    /// T_TenantPermissions Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TTenantPermissionsController : ControllerBase
    {
        private readonly ITTenantPermissionsService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TTenantPermissionsController(ITTenantPermissionsService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// 获取T_TenantPermissions数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<TTenantPermissionsModel,List<TTenantPermissionsModel>>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TTenantPermissionsModel>>(requestObject);
            return await _service.GetAsync(request);
        }


        /// <summary>
        /// 新增或修改数据T_TenantPermissions数据表数据，RequestObject.PostData不可用
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TTenantPermissionsModel,bool>> Post(RequestObject<TTenantPermissionsModel> requestObject)
        {
            return await _service.PostAsync(requestObject);
        }

        /// <summary>
        /// 修改T_TenantPermissions数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut] 
        public async Task<ResponseObject<TTenantPermissionsModel,bool>> Put(RequestObject<TTenantPermissionsModel> requestObject)
        {
            return await _service.PutAsync(requestObject);
        }

        /// <summary>
        /// 删除T_TenantPermissions数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete] 
        public async Task<ResponseObject<TTenantPermissionsModel,bool>> Delete(RequestObject<TTenantPermissionsModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }
        
        /// <summary>
        /// 删除T_TenantPermissions数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")] 
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_TenantPermissions数据表数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByIds")] 
        public async Task<ResponseObject<int[], bool>> Delete(RequestObject<int[]> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_TenantPermissions数据表数据，通过TenantId删除数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ByTenantId")]
        public async Task<ResponseObject<int, bool>> DeleteByTenantId(RequestObject<int> requestObject)
        {
            return await _service.DeleteByTenantIdAsync(requestObject);
        }
    }
}
