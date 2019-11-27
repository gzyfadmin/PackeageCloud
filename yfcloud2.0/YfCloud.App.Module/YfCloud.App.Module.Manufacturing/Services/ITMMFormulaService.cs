///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMFormulaService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:21:05
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMFormula;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// TMMFormula Interface
    /// </summary>
    public interface ITMMFormulaService 
    {
        /// <summary>
        /// 查询TMMFormula数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMFormulaQueryModel>>> GetAsync(RequestGet requestObject,CurrentUser currentUser);

        /// <summary>
        /// 新增TMMFormula数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMFormulaAddModel> requestObject,CurrentUser currentUser);

        /// <summary>
        /// 修改TMMFormula数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TMMFormulaEditModel> requestObject,CurrentUser currentUser);
        
        /// <summary>
        /// 删除TMMFormula数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);
        
    }
}