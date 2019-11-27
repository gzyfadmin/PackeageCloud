using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using YfCloud.App.Module.Warehouse.Models.InventoryReport;
using YfCloud.App.Module.Warehouse.Services;
using YfCloud.Framework;
using YfCloud.Framework.Excel;
using YfCloud.Framework.WebApi;
using YfCloud.Models;
using YfCloud.Utilities.NetOperater;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Warehouse.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryReportController : ControllerBase
    {
        private readonly IInventoryReportService _inventoryReportService;
        /// <summary>
        /// 构造
        /// </summary>
        public InventoryReportController(IInventoryReportService inventoryReportService)
        {
            _inventoryReportService = inventoryReportService;
        }

        /// <summary>
        /// 获取库存报表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<List<InventoryResultModel>>> LoadReport(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
            return await _inventoryReportService.LoadReport(request, currentUser);
        }

        /// <summary>
        /// 导出库存
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<string>> ExportStaReport(string requestObject)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
                var result = await _inventoryReportService.LoadReport(request, currentUser);
                ExportTemplateVM<InventoryResultModel> model = new ExportTemplateVM<InventoryResultModel>();
                model.Entitys = result.Data == null ? new List<InventoryResultModel>() : result.Data;

                model.Columns = new List<string>() { "MaterialCode","MaterialName","Spec","ColorName","BatchNo",
                "WarehouseCode","WarehouseName","BaseUnitName","BaseUnitNumber","WarehouseUnitName","WarehouseAmount","WarehouseRate","InDate",
                "ShelfLife","ValidDate","Remark",};

                model.Titles = new List<string>() { "MaterialCode","MaterialName","Spec","ColorName","BatchNo",
                "WarehouseCode","WarehouseName","BaseUnitName","BaseUnitNumber","WarehouseUnitName","WarehouseAmount","WarehouseRate","InDate",
                "ShelfLife","ValidDate","Remark",};

                model.HeaderText = "库存统计";
                model.TableName = "库存统计";
                model.FillRow = 3;


                model.Path = Path.Combine(AppContext.BaseDirectory, "Template", "InventoryReport.xlsx");

                Stream stream = ExcelHelp.ExportExcelByTemplate<InventoryResultModel>(model);

                string fileName = "库存统计.xlsx";

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

        /// <summary>
        /// 导出期初模板
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject<string>> ExportOpeningTemplate()
        {
            try
            {
                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);
                var result = await _inventoryReportService.ExportOpeningTemplate(currentUser);
                ExportOpeningTemplateVM<MaterialFileTemplate> model = new ExportOpeningTemplateVM<MaterialFileTemplate>();
                model.Entitys = result.Data.List == null ? new List<MaterialFileTemplate>() : result.Data.List;
                model.Columns = new List<string>() { "MaterialCode", "MaterialName", "Spec", "WareName", "WareUnitName" };
                model.Titles = new List<string>() { "MaterialCode", "MaterialName", "Spec", "WareName", "WareUnitName" };
                model.HeaderText = "期初模板";
                model.TableName = "期初模板";
                model.FillRow = 1;
                model.DropDownEntitys = result.Data.WarehouseName;
                model.DropDownFillStartCell = 3;
                model.DropDownFillEndCell = 3;
                model.Path = Path.Combine(AppContext.BaseDirectory, "Template", "ExportOpeningTemplate.xlsx");
                Stream stream = ExcelHelp.ExportOpeningTemplate<MaterialFileTemplate>(model);
                string fileName = "期初模板.xlsx";
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
        /// <summary>
        /// 是否已经导入期初
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<bool>> IsImported()
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

            return await _inventoryReportService.IsImported(currentUser);
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject<List<string>>> ImportPrime(RequestPost<ImportPrimeModel> requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

            return await _inventoryReportService.ImportPrime(requestObject, currentUser);

            //IFormFile


        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ResponseObject<List<HistoryInventory>>> History(HistoryInventoryQuery requestObject)
        {
            CurrentUser currentUser = TokenManager.GetCurentUserbyToken(HttpContext.Request.Headers);

            return await _inventoryReportService.History(requestObject, currentUser);
        }
    }
}