///////////////////////////////////////////////////////////////////////////////////////
// File: IPTestUserSeedService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/30 14:27:37
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Test.Models.PTestUserSeed;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Test.Service
{
    /// <summary>
    /// PTestUserSeed Interface
    /// </summary>
    public interface IPTestUserSeedService 
    {
        /// <summary>
        /// 新增PTestUserSeed数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<PTestUserSeedAddModel> requestObject);

        /// <summary>
        /// 查询种子用户
        /// </summary>
        /// <returns></returns>
        Task<ResponseObject<List<PTestUserSeedQueryModel>>> GetAsync();
        
    }
}