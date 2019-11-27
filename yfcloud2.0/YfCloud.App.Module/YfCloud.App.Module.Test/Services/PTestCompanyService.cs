///////////////////////////////////////////////////////////////////////////////////////
// File: IPTestCompanyService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 15:57:26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.App.Module.Test.Models.PTestCompany;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace YfCloud.App.Module.Test.Service
{
    /// <summary>
    /// IPTestCompanyService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(IPTestCompanyService))]
    public class PTestCompanyService : IPTestCompanyService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<PTestCompanyService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PTestCompanyService(IDbContext dbContext, ILogger<PTestCompanyService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 新增P_Test_Company数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<PTestCompanyAddModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    var addList = _mapper.Map<List<PTestCompanyAddModel>, List<PTestCompanyDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<PTestCompanyDbModel>(requestObject.PostData);
                    result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        
    }
}
