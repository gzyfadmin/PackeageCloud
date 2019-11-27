///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderMain;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetail;
using YfCloud.App.Module.Sales.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailSum;

namespace YfCloud.App.Module.Sales.Controllers
{
    /// <summary>
    /// T_SSM_SalesOrderMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TSSMSalesOrderMainController : ControllerBase
    {
        private readonly ITSSMSalesOrderMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSSMSalesOrderMainController(ITSSMSalesOrderMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }        
        
        /// <summary>
        /// 获取TSSMSalesOrderMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderMainQueryModel>>>  GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }
        
        /// <summary>
        /// 获取TSSMSalesOrderDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderDetailQueryModel>>>  GetDetailList(int requestObject)
        {
            return await _service.GetDetailListAsync(requestObject);
        }

        /// <summary>
        /// 获取可转出库单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetTransferList(int requestObject)
        {
            return await _service.GetTransferList(requestObject);
        }

        /// <summary>
        /// 获取可转生产单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetToProduceList(int requestObject)
        {
            return await _service.GetToProduceList(requestObject);
        }

        /// <summary>
        /// 新增TSSMSalesOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSSMSalesOrderMainQueryModel>> Post(RequestPost<TSSMSalesOrderMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject,currUser);
        }
        
        /// <summary>
        /// 修改TSSMSalesOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TSSMSalesOrderMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }

        /// <summary>
        /// 销售订单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> SalesOrderAudit(RequestPut<TSSMSalesOrderMainAuditModel> requestPut)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestPut, currUser);
        }

        /// <summary>
        /// 终止出库
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopDeposit(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopDeposit(requestObject, currUser);
        }

        /// <summary>
        /// 终止生产
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> StopProduct(int requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.StopProduct(requestObject, currUser);
        }

        /// <summary>
        /// 删除TSSMSalesOrderMain数据表数据，支持批量删除
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

            var excuteEntity = _codeMakers.Where(p => p.ProvideName == OrderEnum.SO.GetDescription()).FirstOrDefault();
            return excuteEntity.MakeNo(currUser.CompanyID);
        }

        /// <summary>
        /// 根据包型和配色方案获取物料
        /// </summary>
        /// <param name="packageColor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ResponseObject<TBMMaterialFileCacheModel> GetMaterialFileByPackageColor(PackageColor packageColor)
        {
            var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            ResponseObject<TBMMaterialFileCacheModel> result = new ResponseObject<TBMMaterialFileCacheModel>();
            try
            {
               
                result.Data=  _service.GetMaterialFileByPackageColor(currUser, packageColor.PackageID, packageColor.ColorSolutionID);
                result.Code = 0;
                return result;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Code = -1;
                return result;
            }

        }
    }
}
