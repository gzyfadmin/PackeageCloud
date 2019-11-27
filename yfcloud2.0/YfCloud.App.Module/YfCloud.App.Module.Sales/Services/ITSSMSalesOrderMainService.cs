///////////////////////////////////////////////////////////////////////////////////////
// File: ITSSMSalesOrderMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22 8:54:34
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderMain;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetail;
using YfCloud.DBModel;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailSum;

namespace YfCloud.App.Module.Sales.Service
{
    /// <summary>
    /// TSSMSalesOrderMain Interface
    /// </summary>
    public interface ITSSMSalesOrderMainService 
    {
        /// <summary>
        /// 查询TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 查询TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderDetailQueryModel>>> GetDetailListAsync(int requestObject);

        /// <summary>
        /// 查询可转出库单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetTransferList(int requestObject);
        
        /// <summary>
        /// 查询可转生产单明细列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TSSMSalesOrderDetailSumQueryModel>>> GetToProduceList(int requestObject);

        /// <summary>
        /// 新增TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<TSSMSalesOrderMainQueryModel>> PostAsync(RequestPost<TSSMSalesOrderMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TSSMSalesOrderMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 审核销售订单
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> AuditAsync(RequestPut<TSSMSalesOrderMainAuditModel> requestPut, CurrentUser currentUser);

        /// <summary>
        /// 终止出库
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopDeposit(int  ID, CurrentUser currentUser);

        /// <summary>
        /// 终止生产
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopProduct(int ID, CurrentUser currentUser);


        /// <summary>
        /// 删除TSSMSalesOrderMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);

        TBMMaterialFileCacheModel GetMaterialFileByPackageColor(CurrentUser currentUser, int PackageID, int? ColorSolutionID);


    }
}