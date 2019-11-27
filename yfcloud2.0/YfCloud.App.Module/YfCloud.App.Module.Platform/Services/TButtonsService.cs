///////////////////////////////////////////////////////////////////////////////////////
// File: ITButtonsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18 11:34:29
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
    /// ITButtonsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITButtonsService))]
    public class TButtonsService : ITButtonsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TButtonsService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TButtonsService(IDbContext dbContext, ILogger<TButtonsService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_Buttons数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TButtonsModel, List<TButtonsModel>>> GetAsync(RequestObject<TButtonsModel> requestObject)
        {
            try
            {
                List<TButtonsModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TButtonsModel, TUsersModel>(
								(t, t0) => new object[] 
								{
									JoinType.Left,  t.CreateId== t0.Id, 
								});
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where($"t.{p}"));
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }
                
                //设置多表查询返回实体类
                query.Select((t, t0) => new TButtonsModel 
				{
					Id = t.Id,
					ButtonCaption = t.ButtonCaption,
					ButtonKey = t.ButtonKey,
					IsDefault = t.IsDefault,
					ButtonDesc = t.ButtonDesc,
					CreateTime = t.CreateTime,
					CreateId = t.CreateId,
					TUsersUserName = t0.UserName,
				});
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TButtonsModel, List<TButtonsModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TButtonsModel, List<TButtonsModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_Buttons数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TButtonsModel,bool>> PostAsync(RequestObject<TButtonsModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 1)
                {
                    result = await _db.Instance.Insertable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    result = await _db.Instance.Insertable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TButtonsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_Buttons数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TButtonsModel, bool>> PutAsync(RequestObject<TButtonsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    result = await _db.Instance.Updateable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    result = await _db.Instance.Updateable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TButtonsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Buttons数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TButtonsModel, bool>> DeleteAsync(RequestObject<TButtonsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TButtonsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TButtonsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_Buttons数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TButtonsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_Buttons数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TButtonsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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
