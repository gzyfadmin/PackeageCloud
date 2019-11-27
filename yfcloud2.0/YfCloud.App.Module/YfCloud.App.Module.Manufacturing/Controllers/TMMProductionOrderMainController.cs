///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderMainController.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderMain;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.Models;
using Newtonsoft.Json;
using YfCloud.Framework;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetailSum;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using System.IO;
using YfCloud.Framework.Excel;
using YfCloud.Framework.WebApi;
using YfCloud.Utilities.NetOperater;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Manufacturing.Controllers
{
    /// <summary>
    /// T_MM_ProductionOrderMain Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TMMProductionOrderMainController : ControllerBase
    {
        private readonly ITMMProductionOrderMainService _service;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TMMProductionOrderMainController(ITMMProductionOrderMainService service, IEnumerable<ICodeMaker> codeMakers)
        {
            _service = service;
            _codeMakers = codeMakers;
        }

        /// <summary>
        /// 获取TMMProductionOrderMain主表数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMProductionOrderMainQueryModel>>> GetMainList(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.GetMainListAsync(request, currUser);
        }

        /// <summary>
        /// 获取TMMProductionOrderDetail数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMProductionOrderDetailQueryModel>>> GetDetailList(int requestObject)
        {
            return await _service.GetDetailListAsync(requestObject);
        }

        /// <summary>
        /// 新增TMMProductionOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TMMProductionOrderMainQueryModel>> Post(RequestPost<TMMProductionOrderMainAddModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PostAsync(requestObject, currUser);
        }

        /// <summary>
        /// 修改TMMProductionOrderMain数据表数据
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseObject<bool>> Put(RequestPut<TMMProductionOrderMainEditModel> requestObject)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.PutAsync(requestObject, currUser);
        }

        /// <summary>
        /// 生产单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> ProduceAudit(RequestPut<TMMProductionOrderMainAuditModel> requestPut)
        {
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.AuditAsync(requestPut, currUser);
        }

        /// <summary>
        /// 删除TMMProductionOrderMain数据表数据，不支持批量
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpDelete]
        public async Task<ResponseObject<bool>> Delete(RequestDelete<DeleteModel> requestObject)
        {
            return await _service.DeleteAsync(requestObject);
        }

        /// <summary>
        /// 获取生产单号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetOrderNo()
        {
            ResponseObject<string> result = new ResponseObject<string>();

            try
            {
                var currUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

                return _codeMakers.Where(p => p.ProvideName == OrderEnum.MO.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID);

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 选择生产入库订单
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMProductionOrderMainQuerySumModel>>> GetOrder(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetOrderDeatailSum(request, true, currUser);
        }

        /// <summary>
        /// 选择生产入库订单
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<TMMProductionOrderMainQuerySumModel>>> GetOrderResult(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            var currUser = TokenManager.GetCurentUserbyToken(Request.Headers);

            return await _service.GetOrderDeatailSum(request, false, currUser);
        }

        /// <summary>
        /// 终止生产入库
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

                result.Data = _service.GetMaterialFileByPackageColor(currUser, packageColor.PackageID, packageColor.ColorSolutionID);
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
        /// <summary>
        /// 导出BOM
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<string>> ExportBomTemplate()
        {
            try
            {
                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
                var result = await _service.ExportBomTemplate(359, currentUser);
                //return result;
                ExportTemplateVM<TMMBOMDetailQueryExcelModel> model = new ExportTemplateVM<TMMBOMDetailQueryExcelModel>();
                model.Entitys = result.Data.TMMBOMDetailQueryExcelList == null ? new List<TMMBOMDetailQueryExcelModel>() : result.Data.TMMBOMDetailQueryExcelList;
                model.Columns = new List<string>() { "ItemName", "MaterialName", "PartName", "LengthValue", "WidthValue", "NumValue", "WideValue", "LossValue", "SingleValue" };
                model.Titles = new List<string>() { "ItemName", "MaterialName", "PartName", "LengthValue", "WidthValue", "NumValue", "WideValue", "LossValue", "SingleValue" };
                model.HeaderText = "BOM模板";
                model.TableName = "BOM模板";
                model.FillRow = 6;
                model.Path = Path.Combine(AppContext.BaseDirectory, "Template", "ExportBomTemplate.xlsx");
                Stream stream = ExcelHelp.ExportExcelByTemplate<TMMBOMDetailQueryExcelModel>(model);
                string fileName = "生产单模板-有配色方案.xlsx";
                Dictionary<string, string> headsSend = new Dictionary<string, string>();
                headsSend.Add(TokenConfig.Instace.TokenKey, Request.Headers[TokenConfig.Instace.TokenKey]);
                var filePath = RestfulApiClient.UploadFile(RestfulApiClient.UplaodUrl, "api/files/upload", headsSend, stream, fileName);
                return ResponseUtil<string>.SuccessResult(filePath.ToString());
            }
            catch (Exception ex)
            {
                return ResponseUtil<string>.FailResult(null, ex.ToString());
            }

        }
    }
}
