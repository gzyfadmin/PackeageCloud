///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMProductionOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5 13:41:17
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderMain;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetailSum;
using YfCloud.App.Module.Manufacturing.Models;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMProductionOrderMain Interface
    /// </summary>
    public interface ITMMProductionOrderMainService 
    {

        /// <summary>
        /// 根据生产订单生成BOM
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<(bool Result, string ErrorInfo)> CreateOrderBom(int iMainId, CurrentUser currentUser);

        /// <summary>
        /// 查询TMMProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMProductionOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser);
        
        /// <summary>
        /// 查询TMMProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMProductionOrderDetailQueryModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 新增TMMProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currUser"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMProductionOrderMainQueryModel>> PostAsync(RequestPost<TMMProductionOrderMainAddModel> requestObject,CurrentUser currUser);

        /// <summary>
        /// 修改TMMProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TMMProductionOrderMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 生产单审核
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TMMProductionOrderMainAuditModel> requestPut,CurrentUser currentUser);

        /// <summary>
        /// 删除TMMProductionOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);

        /// <summary>
        /// 生产明细
        /// </summary>
        /// <param name="requestGet"></param>
        /// <param name="isTakeTop"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMProductionOrderMainQuerySumModel>>> GetOrderDeatailSum(RequestGet requestGet, bool isTakeTop, CurrentUser currentUser);


        /// <summary>
        /// 终止生产入库
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser);

        TBMMaterialFileCacheModel GetMaterialFileByPackageColor(CurrentUser currentUser, int PackageID, int? ColorSolutionID);

        /// <summary>
        /// 导出BOM模板
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<AllBomExcel>> ExportBomTemplate(int orderID, CurrentUser currentUser);
    }
}