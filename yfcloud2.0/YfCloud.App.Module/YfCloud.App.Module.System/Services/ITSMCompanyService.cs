///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMCompanyService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:47:57
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TSMCompany;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// TSMCompany Interface
    /// </summary>
    public interface ITSMCompanyService 
    {
        /// <summary>
        /// 查询TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyQueryModel, List<TSMCompanyQueryModel>>> GetAsync(RequestObject<TSMCompanyQueryModel> requestObject);

        /// <summary>
        /// 获取当前用户的企业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>> FetchCompanyInfo(TSMCompanyForCurentUserIDModel model);

        /// <summary>
        /// 新增TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<string>> PostAsync(RequestPost<TSMCompanyAddModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 判断公司是否存在
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> CheckIsExists(RequestPost<TSMCompanyCheckModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyEditModel, bool>> PutAsync(RequestObject<TSMCompanyEditModel> requestObject);
        
        /// <summary>
        /// 删除TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMCompanyAddModel, bool>> DeleteAsync(RequestObject<TSMCompanyAddModel> requestObject);
        
        /// <summary>
        /// 删除TSMCompany数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMCompany数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<int, TSMCompanyQueryAllModel>> PersonalGet(int UserID);

        Task<ResponseObject<TSMCompanyAllEditModel, bool>> ModifyCurentCompany(RequestObject<TSMCompanyAllEditModel> requestObject, int UserID);




    }
}