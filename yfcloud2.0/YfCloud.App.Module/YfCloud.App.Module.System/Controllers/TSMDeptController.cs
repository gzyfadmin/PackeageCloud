///////////////////////////////////////////////////////////////////////////////////////
// File: TSMDeptController.cs
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
using YfCloud.App.Module.System.Models.TSMDept;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.Framework;
using YfCloud.App.Module.System.Service;
using YfCloud.Framework;
using YfCloud.Framework.Log;
using YfCloud.Framework.WebApi;
using YfCloud.Models;
using YfCloud.Utilities;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// T_SM_Dept Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSMDeptController : ControllerBase
    {
        private readonly ITSMDeptService _service;
        private readonly IDbContext _db;//数据库操作实例对象

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSMDeptController(ITSMDeptService service, IDbContext db)
        {
            _service = service;
            _db = db;
        }
        
        /// <summary>
        /// 获取T_SM_Dept数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TSMDeptQueryModel,List<TSMDeptQueryTreeModel>>>  GetTree(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMDeptQueryModel>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);

            return await _service.GetAsync(request, userID);
        }

        /// <summary>
        /// 获取T_SM_Dept数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>> GetOnlyDepAsync(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMDeptQueryModel>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);

            return await _service.GetOnlyDepAsync(request, userID);
        }

        /// <summary>
        /// 新增T_SM_Dept数据表数据，支持批量新增
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSMDeptAddModel,bool>> Post(RequestObject<TSMDeptAddModel> requestObject)
        {
            ResponseObject<TSMDeptAddModel, bool> result;

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);
            result = await _service.PostAsync(requestObject, userID);

            if (result.Data == true)
            {
                string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                //记录的日志
                List<string> deatailLog = new List<string>();
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count() > 0)
                {
                    deatailLog = requestObject.PostDataList.Select(p => $"新增{p.DeptName}").ToList();
                }
                else
                {
                    deatailLog.Add(requestObject.PostData.DeptName);
                }

                OperateLogManager.AddOrDeleteLogString<TSMDeptEditModel>(new List<string>(), OpetateEnum.Add, Request.Headers, _db.Instance, "pathtest", ipAddress);//日志
            }

            return result;
        }
        
        /// <summary>
        /// 修改T_SM_Dept数据表数据，支持批量修改
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TSMDeptEditModel,bool>> Put(RequestObject<TSMDeptEditModel> requestObject)
        {
            ResponseObject<TSMDeptEditModel, bool> result;
            result= await _service.PutAsync(requestObject);

            if (result.Data == true)
            {
                string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                 OperateLogManager.ModifyLog<TSMDeptEditModel>(requestObject, Request.Headers, _db.Instance, "pathtest", ipAddress);//日志
            }

          
            return result;
        }
        
        
        /// <summary>
        /// 删除T_SM_Dept数据表数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public async Task<ResponseObject<int,bool>> Delete(RequestObject<int> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Dept数据表数据，通过主表主键删除主从数据，批量删除
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
        public async Task<ResponseObject<TSMDeptMoveModel, bool>> MovePostion(RequestObject<TSMDeptMoveModel> requestObject)
        {
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.MovePostion(requestObject, curent);
        }

        /// <summary>
        /// 为用户分配部门
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<DeptUserRelationEdit, bool>> DispathDept(RequestObject<DeptUserRelationEdit> requestObject)
        {
            return await _service.DispathDept(requestObject);
        }

        /// <summary>
        /// 获取部门下的员工
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>> GetDispathUserAsync(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestObject<TSMDeptQueryForDispatchModel>>(requestObject);

            int userID = TokenManager.GetUserIDbyToken(Request.Headers);

            return await _service.GetDispathUserAsync(request,userID);
        }
    }
}
