///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMColorSolutionMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:29:58
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail;
using YfCloud.DBModel;
using System.Data;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMColorSolutionMain Interface
    /// </summary>
    public interface ITMMColorSolutionMainService 
    {
        /// <summary>
        /// 查询TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<DataTable>> GetAsync(RequestGet requestObject,CurrentUser currentUser);

        /// <summary>
        /// 新增TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMColorSolutionMainAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TMMColorSolutionMainEditModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="PackageID"></param>
        /// <param name="ColorSolutionID"></param>
        /// <returns></returns>
        TBMMaterialFileDbModel GetMaterialFileByPackageColor(int PackageID, int? ColorSolutionID);

        Task<ResponseObject<List<TMMColorSolutionMainQueryModel>>> GetColorSolution(RequestGet requestObject, CurrentUser currentUser);

    }
}