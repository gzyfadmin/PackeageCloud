///////////////////////////////////////////////////////////////////////////////////////
// File: IPTestCompanyService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 15:57:26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Test.Models.PTestCompany;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Test.Service
{
    /// <summary>
    /// PTestCompany Interface
    /// </summary>
    public interface IPTestCompanyService 
    {
        /// <summary>
        /// 新增PTestCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<PTestCompanyAddModel> requestObject);
        
    }
}