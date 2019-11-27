///////////////////////////////////////////////////////////////////////////////////////
// File: ITTempPermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 14:32:16
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
    /// ITTempPermissionsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITTempPermissionsService))]
    public class TTempPermissionsService : ITTempPermissionsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TTempPermissionsService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TTempPermissionsService(IDbContext dbContext, ILogger<TTempPermissionsService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_TempPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTempPermissionsModel, List<TTempPermissionsModel>>> GetAsync(RequestObject<TTempPermissionsModel> requestObject)
        {
            try
            {
                List<TTempPermissionsModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TTempPermissionsModel, TTemplatesModel, TPMMenusDbModel>(
								(t, t0, t2) => new object[] 
								{
									JoinType.Left,  t.TemplateId== t0.Id, 
									JoinType.Left,  t.MenuId== t2.Id, 
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
                query.Select((t, t0, t2) => new TTempPermissionsModel 
				{
					Id = t.Id,
					TemplateId = t.TemplateId,
					TTemplatesTemplateName = t0.TemplateName,
					MenuId = t.MenuId,
					TMenusMenuName = t2.MenuName,
					ButtonIds = t.ButtonIds,
					CreateTime = t.CreateTime,
					CreateId = t.CreateId,
                    MenuParentId = t2.ParentID
				});
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TTempPermissionsModel, List<TTempPermissionsModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TTempPermissionsModel, List<TTempPermissionsModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增或者修改T_TempPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTempPermissionsModel,bool>> PostAsync(RequestObject<TTempPermissionsModel> requestObject)
        {
            var currDb = _db.Instance;
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostDataList == null || requestObject.PostDataList.Count < 1)
                    return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "PostDataList不能为null");
                var result = false;
                //获取当前租户所有已有权限，根据提交
                var permissions = currDb.Queryable<TTempPermissionsModel>()
                    .Where(p => p.TemplateId == requestObject.PostDataList[0].TemplateId)
                    .ToList();
                //新增或修改权限
                foreach (var item in requestObject.PostDataList)
                {
                    //新增或修改
                    var dbCurrItem = permissions.Where(p => p.TemplateId == item.TemplateId && p.MenuId == item.MenuId).FirstOrDefault();
                    if (dbCurrItem == null)
                    {
                        result = await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync();
                    }
                    else
                    {
                        result = await currDb.Updateable(item).UpdateColumns(p => p.ButtonIds)
                        .Where(p => p.TemplateId == item.TemplateId && p.MenuId == item.MenuId)
                        .ExecuteCommandAsync() > 0;
                        item.Id = dbCurrItem.Id;
                    }

                    if (!result) break;
                }

                //删除不存在的权限
                if (result)
                {
                    //删除不存在的权限
                    var ids = requestObject.PostDataList.Select(p => p.Id).ToList();
                    result = await currDb.Deleteable<TTempPermissionsModel>()
                        .Where(p => !ids.Contains(p.Id) && p.TemplateId == requestObject.PostDataList[0].TemplateId)
                        .ExecuteCommandAsync() >= 0;
                }
                else
                {
                    currDb.RollbackTran();
                    return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
                }

                //返回执行结果
                if (result)
                {
                    currDb.CommitTran();
                    return ResponseUtil<TTempPermissionsModel, bool>.SuccessResult(requestObject, true);
                }

                currDb.RollbackTran();
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
            }
            catch(Exception ex)
            {
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_TempPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTempPermissionsModel, bool>> PutAsync(RequestObject<TTempPermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TTempPermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_TempPermissions数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TTempPermissionsModel, bool>> DeleteAsync(RequestObject<TTempPermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TTempPermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTempPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_TempPermissions数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TTempPermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_TempPermissions数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTempPermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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

        /// <summary>
        /// 删除TTempPermissions数据，通过TemplateId删除数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteByTempId(RequestObject<int> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTempPermissionsModel>()
                    .Where(p=>p.TemplateId == requestObject.PostData)
                    .ExecuteCommandAsync() > 0;

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
    }
}
