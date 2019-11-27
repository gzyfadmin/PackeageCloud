///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMPurchaseApplyMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11 8:38:12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMPurchaseApplyMain Interface
    /// </summary>
    public interface ITMMPurchaseApplyMainService 
    {
        /// <summary>
        /// 查询TMMPurchaseApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMPurchaseApplyMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 查询TMMPurchaseApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetDetailListAsync(int requestObject);


        /// <summary>
        /// 查询TMMPurchaseApplyMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMPurchaseApplyDetailQueryModel>>> GetToPurchaseListAsync(int requestObject);

        /// <summary>
        /// 终止采购
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> StopPurchase(int ID, CurrentUser currentUser);
    }
}