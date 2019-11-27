///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPickApplyMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_PickApplyMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMPickApplyMainController : ControllerBase
    {
        private readonly ITMMPickApplyMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMPickApplyMainController(ITMMPickApplyMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TMMPickApplyMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMPickApplyMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }

        /// <summary>
        /// 获取出库单所有的信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetWholeMainData(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetWholeMainData(requestObject, currUser);
        }

        /// <summary>
        /// 获取生产出库可转单的信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTransWareDate(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.GetTransWareDate(requestObject, currUser);
        }

        /// <summary>
        /// 新增TMMPickApplyMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> Post(RequestPost<TMMPickApplyMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 修改TMMPickApplyMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> Put(RequestPut<TMMPickApplyMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }
        
        /// <summary>
        /// 删除TMMPickApplyMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _service.DeleteAsync(requestObject, currUser);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> Audit(RequestPut<AuditModel> requestObject)
        {
            CurrentUser userInfo = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestObject, userInfo);
        }

        /// <summary>
        /// 获取出库单号生成规则
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            //var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            //return OrderGenerator.Generator(OrderEnum.MR, currUser.CompanyID);
           // OrderGenerator.Generator(OrderEnum.MR, currUser.CompanyID);

            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return  _codeMakers.Where(p => p.ProvideName == OrderEnum.MR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 终止生产出库
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopWare(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopWare(requestObject, currUser);
        }
    }
}
