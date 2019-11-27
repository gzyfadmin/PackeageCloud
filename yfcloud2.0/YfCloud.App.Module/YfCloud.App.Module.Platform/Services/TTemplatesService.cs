///////////////////////////////////////////////////////////////////////////////////////
// File: ITTemplatesService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 14:30:47
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.App.Module.Platform.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITTemplatesService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITTemplatesService))]
    public class TTemplatesService : ITTemplatesService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TTemplatesService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TTemplatesService(IDbContext dbContext, ILogger<TTemplatesService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_Templates数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTemplatesModel, List<TTemplatesModel>>> GetAsync(RequestObject<TTemplatesModel> requestObject)
        {
            try
            {
                List<TTemplatesModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TTemplatesModel>();
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TTemplatesModel, List<TTemplatesModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TTemplatesModel, List<TTemplatesModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_Templates数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTemplatesModel,bool>> PostAsync(RequestObject<TTemplatesModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                result = await _db.Instance.Insertable(requestObject.PostData).ExecuteCommandIdentityIntoEntityAsync();
                if (result && requestObject.PostData.isDefault)
                    _db.Instance.Updateable<TTemplatesModel>()
                        .Where(p => p.Id != requestObject.PostData.Id)
                        .SetColumns(p => p.isDefault == false)
                        .ExecuteCommand();
                //返回执行结果
                if (result)
                    return ResponseUtil<TTemplatesModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_Templates数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTemplatesModel, bool>> PutAsync(RequestObject<TTemplatesModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");

                //单记录更新
                result = await _db.Instance.Updateable(requestObject.PostData).ExecuteCommandAsync() > 0;
                if (result && requestObject.PostData.isDefault)
                    _db.Instance.Updateable<TTemplatesModel>()
                        .Where(p => p.Id != requestObject.PostData.Id)
                        .SetColumns(p => p.isDefault == false)
                        .ExecuteCommand();

                //返回执行结果
                if (result)
                    return ResponseUtil<TTemplatesModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Templates数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TTemplatesModel, bool>> DeleteAsync(RequestObject<TTemplatesModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    result = await _db.Instance.Deleteable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    result = await _db.Instance.Deleteable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TTemplatesModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTemplatesModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_Templates数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TTemplatesModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
                //返回执行结果
                if (result)
                    return ResponseUtil<int, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<int, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<int, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Templates数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTemplatesModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
                //返回执行结果
                if (result)
                    return ResponseUtil<int[], bool>.SuccessResult(requestObject, true);
                return ResponseUtil<int[], bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<int[], bool>.FailResult(requestObject, false, ex.Message);
            }
        }
    }
}
