///////////////////////////////////////////////////////////////////////////////////////
// File: ITTenantPermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:08:39
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
    /// ITTenantPermissionsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITTenantPermissionsService))]
    public class TTenantPermissionsService : ITTenantPermissionsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TTenantPermissionsService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TTenantPermissionsService(IDbContext dbContext, ILogger<TTenantPermissionsService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_TenantPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantPermissionsModel, List<TTenantPermissionsModel>>> GetAsync(RequestObject<TTenantPermissionsModel> requestObject)
        {
            try
            {
                List<TTenantPermissionsModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TTenantPermissionsModel, TTenantsModel, TPMMenusDbModel, TUsersModel, TSMCompanyDbModel>(
								(t, t0, t2, t3,t4) => new object[] 
								{
									JoinType.Left,  t.TenantId== t0.ID, 
									JoinType.Left,  t.MenuId== t2.Id, 
									JoinType.Left,  t.CreateId== t3.Id, 
                                    JoinType.Left,t0.ID==t4.CompanyInfoId
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
                query.Select((t, t0, t2, t3,t4) => new TTenantPermissionsModel 
				{
					Id = t.Id,
					TenantId = t.TenantId,
					TTenantsTenantName = t4.CompanyName,
					MenuId = t.MenuId,
					TMenusMenuName = t2.MenuName,
					ButtonIds = t.ButtonIds,
					CreateTime = t.CreateTime,
					CreateId = t.CreateId,
					TUsersUserName = t3.UserName,
                    MenuParentId = t2.ParentID
                });
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TTenantPermissionsModel, List<TTenantPermissionsModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TTenantPermissionsModel, List<TTenantPermissionsModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增或修改T_TenantPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantPermissionsModel, bool>> PostAsync(RequestObject<TTenantPermissionsModel> requestObject)
        {
            var currDb = _db.Instance;
            try
            {
                currDb.BeginTran();
                //如果没有新增数据，返回错误信息
                if (requestObject.PostDataList == null || requestObject.PostDataList.Count < 1)
                    return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "PostDataList不能为null");
                var result = false;
                //获取当前租户所有已有权限，根据提交
                var permissions = currDb.Queryable<TTenantPermissionsModel>()
                    .Where(p => p.TenantId == requestObject.PostDataList[0].TenantId)
                    .ToList();
                //新增或修改权限
                foreach (var item in requestObject.PostDataList)
                {
                    //新增或修改
                    var dbCurrItem = permissions.Where(p => p.TenantId == item.TenantId && p.MenuId == item.MenuId).FirstOrDefault();
                    if (dbCurrItem == null) {
                        result = await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync();
                    }
                    else
                    {
                        result = await currDb.Updateable(item).UpdateColumns(p => p.ButtonIds)
                        .Where(p => p.TenantId == item.TenantId && p.MenuId == item.MenuId)
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
                    result = await currDb.Deleteable<TTenantPermissionsModel>()
                        .Where(p => !ids.Contains(p.Id)
                                    && p.TenantId == requestObject.PostDataList[0].TenantId)
                        .ExecuteCommandAsync() >= 0;
                }
                else
                {
                    currDb.RollbackTran();
                    return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
                }

                //返回执行结果
                if (result)
                {
                    currDb.CommitTran();
                    return ResponseUtil<TTenantPermissionsModel, bool>.SuccessResult(requestObject, true);
                }

                currDb.RollbackTran();
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
            }
            catch (Exception ex)
            {
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_TenantPermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantPermissionsModel, bool>> PutAsync(RequestObject<TTenantPermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TTenantPermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_TenantPermissions数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantPermissionsModel, bool>> DeleteAsync(RequestObject<TTenantPermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TTenantPermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTenantPermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_TenantPermissions数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TTenantPermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_TenantPermissions数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTenantPermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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
        /// 删除TTenantPermissions数据，通过TenantId删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteByTenantIdAsync(RequestObject<int> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTenantPermissionsModel>()
                    .Where(p=>p.TenantId == requestObject.PostData).ExecuteCommandAsync() > 0;

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
