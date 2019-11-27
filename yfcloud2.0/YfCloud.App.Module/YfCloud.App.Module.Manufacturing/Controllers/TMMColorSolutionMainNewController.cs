using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMainNew;
using YfCloud.App.Module.Manufacturing.Services;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// 配色方案新
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMColorSolutionMainNewController : ControllerBase
    {
        private readonly ITMMColorSolutionMainNewService _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMColorSolutionMainNewController(ITMMColorSolutionMainNewService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取TMMColorSolutionMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseObject<DataTable>> Get(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetAsync(request, user);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMColorSolutionMainQueryNewModel>>> GetColorSolution(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetColorSolution(request, user);
        }

        /// <summary>
        /// 新增TMMColorSolutionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> Post(RequestPost<TMMColorSolutionMainAddNewModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, user);
        }

        /// <summary>
        /// 修改TMMColorSolutionMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TMMColorSolutionMainEditNewModel> requestObject)
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, user);
        }

        /// <summary>
        /// 删除TMMColorSolutionMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        ///  包型配色方案树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMPackageColorItem>>> GetTMMPackageColorItem()
        {
            var user = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetTMMPackageColorItem(user);
        }
    }
}