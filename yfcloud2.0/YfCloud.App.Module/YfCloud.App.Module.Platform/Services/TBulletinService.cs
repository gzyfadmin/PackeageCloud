///////////////////////////////////////////////////////////////////////////////////////
// File: ITBulletinService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:09:35

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
    /// ITBulletinService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBulletinService))]
    public class TBulletinService : ITBulletinService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBulletinService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TBulletinService(IDbContext dbContext, ILogger<TBulletinService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_Bulletin数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBulletinModel, List<TBulletinModel>>> GetAsync(RequestObject<TBulletinModel> requestObject)
        {
            try
            {
                List<TBulletinModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TBulletinModel, TUsersModel>(
								(t, t0) => new object[] 
								{
									JoinType.Left,  t.Publisher== t0.Id, 
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
                query.Select((t, t0) => new TBulletinModel 
				{
					Id = t.Id,
					BulletinTitle = t.BulletinTitle,
					BulletinContent = t.BulletinContent,
					PublishDate = t.PublishDate,
					Publisher = t.Publisher,
					TUsersUserName = t0.UserName,
					Receivers = t.Receivers,
					RePublish = t.RePublish,
					CreateTime = t.CreateTime,
					CreateId = t.CreateId,
				});
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TBulletinModel, List<TBulletinModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TBulletinModel, List<TBulletinModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_Bulletin数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBulletinModel,bool>> PostAsync(RequestObject<TBulletinModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TBulletinModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_Bulletin数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBulletinModel, bool>> PutAsync(RequestObject<TBulletinModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    result = await _db.Instance.Updateable(requestObject.PostDataList)
                        .UpdateColumns(p => new { p.BulletinTitle, p.BulletinContent, p.Publisher, p.PublishDate, p.RePublish, p.Receivers })
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    result = await _db.Instance.Updateable(requestObject.PostData)
                        .UpdateColumns(p => new { p.BulletinTitle, p.BulletinContent, p.Publisher, p.PublishDate, p.RePublish, p.Receivers })
                        .ExecuteCommandAsync() > 0;
                    
                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TBulletinModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Bulletin数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBulletinModel, bool>> DeleteAsync(RequestObject<TBulletinModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TBulletinModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBulletinModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_Bulletin数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TBulletinModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_Bulletin数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TBulletinModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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
