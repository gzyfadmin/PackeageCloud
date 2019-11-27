///////////////////////////////////////////////////////////////////////////////////////
// File: ITRolePermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 15:03:24
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
using YfCloud.App.Module.Platform.Models.TRolePermissions;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITRolePermissionsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITRolePermissionsService))]
    public class TRolePermissionsService : ITRolePermissionsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TRolePermissionsService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TRolePermissionsService(IDbContext dbContext, ILogger<TRolePermissionsService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TRolePermissionsModel, List<TRolePermissionsModel>>> GetAsync(RequestObject<TRolePermissionsModel> requestObject)
        {
            try
            {
                List<TRolePermissionsModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TRolePermissionsModel, TRolesModel, TPMMenusDbModel, TUsersModel>(
								(t, t0, t2, t3) => new object[] 
								{
									JoinType.Left,  t.RoleId== t0.Id, 
									JoinType.Left,  t.MenuId== t2.Id, 
									JoinType.Left,  t.CreateId== t3.Id, 
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
                query.Select((t, t0, t2, t3) => new TRolePermissionsModel 
				{
					Id = t.Id,
					RoleId = t.RoleId,
					TRolesRoleName = t0.RoleName,
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
                return ResponseUtil<TRolePermissionsModel, List<TRolePermissionsModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TRolePermissionsModel, List<TRolePermissionsModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 根据角色ID 获取菜单树
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TRolePermissionsModel, List<MenuViewModel>>> LoadMenuByRoles(RequestObject<TRolePermissionsModel> requestObject)
        {
            try
            {
                List<MenuViewModel> queryData = null;//查询结果集对象


              List< TButtonsModel> buttonsModels=  _db.Instance.Queryable<TButtonsModel>().ToList();

                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TRolePermissionsModel, TRolesModel, TPMMenusDbModel>(
                                (t, t0, t2) => new object[]
                                {
                                    JoinType.Inner,  t.RoleId== t0.Id,
                                    JoinType.Inner,  t.MenuId== t2.Id
                                });
                if (requestObject.QueryConditions == null || requestObject.QueryConditions.Count() == 0)
                {
                    ResponseUtil<TRolePermissionsModel, List<MenuViewModel>>.FailResult(requestObject, null, "查询条件必须含有roleid");
                }
                else
                {
                    if (!requestObject.QueryConditions.Any(p => p.Column.ToLower() == "roleid"))
                    {
                        ResponseUtil<TRolePermissionsModel, List<MenuViewModel>>.FailResult(requestObject, null, "查询条件必须含有roleid");
                    }
                    else
                    {
                        var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                        expressionList.ForEach(p => query.Where($"t.{p}"));
                    }
                }

                //获取菜单
                var memu = query.Select((t, t0, t2) => new { buttons = t.ButtonIds, memu = t2 }).ToList();

                var menuDictoray = memu.ToDictionary(p => p.memu.Id, p => p.buttons);
                

                List<int> allMenuIDS = new List<int>();

                foreach (var item in memu)
                {
                    var ids = item.memu.LogicPath.Split('.').Select(p => Convert.ToInt32(p)).ToList();
                    allMenuIDS.AddRange(ids);
                }

                allMenuIDS = allMenuIDS.Distinct().ToList();

                var allDisplayNodes = await _db.Instance.Queryable<TPMMenusDbModel>().Where(p => allMenuIDS.Contains(p.Id)&&p.Status==true).ToListAsync(); //所有需要展示的菜单

                var data = GetMenuTree(allDisplayNodes, menuDictoray, buttonsModels ,- 1);

                return ResponseUtil<TRolePermissionsModel, List<MenuViewModel>>.SuccessResult(requestObject, data, totalNumber);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TRolePermissionsModel, List<MenuViewModel>>.FailResult(requestObject, null, ex.Message);
            }

        }

        private List<MenuViewModel> GetMenuTree(List<TPMMenusDbModel> aimData,Dictionary<int,string> menuButtons, List<TButtonsModel> buttonsModels,int pid)
        {
            List<MenuViewModel> tree = new List<MenuViewModel>();
            var children = aimData.Where(p => p.ParentID == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentID == pid).OrderBy(p => p.Seq))
                {
                    string code = RandCodeCreate.CreateCodeByID(item.Id);

                    MenuViewModel node = new MenuViewModel();
                    node.path = "/" +code;
                    node.component = item.IsMenu ? item.MenuPath : "Layout";
                    node.meta = new MenuNodeViewModel() { icon=item.MenuIcon, title=item.MenuName};
                    node.alwaysShow = item.IsMenu ? false : true;
                    node.name = item.Id.ToString();

                    if (menuButtons.Keys.Any(p => p == item.Id))
                    {
                        if (!string.IsNullOrEmpty(menuButtons[item.Id]))
                        {
                            var ids = menuButtons[item.Id].Split(",").Select(p=>Convert.ToInt32(p)).ToList();

                            node.buttons = buttonsModels.Where(p => ids.Contains(p.Id)).ToList();

                        }
                    }

                    node.children = GetMenuTree(aimData, menuButtons, buttonsModels, item.Id);

                    tree.Add(node);
                }
            }
            return tree;
        }

        /// <summary>
        /// 新增或修改T_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TRolePermissionsModel,bool>> PostAsync(RequestObject<TRolePermissionsModel> requestObject)
        {
            var currDb = _db.Instance;
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostDataList == null || requestObject.PostDataList.Count < 1)
                    return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "PostDataList不能为null");
                var result = false;
                //获取当前租户所有已有权限，根据提交
                var permissions = currDb.Queryable<TRolePermissionsModel>()
                    .Where(p => p.RoleId == requestObject.PostDataList[0].RoleId)
                    .ToList();
                //新增或修改权限
                foreach (var item in requestObject.PostDataList)
                {
                    //新增或修改
                    var dbCurrItem = permissions.Where(p => p.RoleId == item.RoleId && p.MenuId == item.MenuId).FirstOrDefault();
                    if (dbCurrItem == null)
                    {
                        result = await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync();
                    }
                    else
                    {
                        result = await currDb.Updateable(item).UpdateColumns(p => p.ButtonIds)
                        .Where(p => p.RoleId == item.RoleId && p.MenuId == item.MenuId)
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
                    result = await currDb.Deleteable<TRolePermissionsModel>()
                        .Where(p => !ids.Contains(p.Id) && p.RoleId == requestObject.PostDataList[0].RoleId)
                        .ExecuteCommandAsync() >= 0;
                }
                else
                {
                    currDb.RollbackTran();
                    return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
                }

                //返回执行结果
                if (result)
                {
                    currDb.CommitTran();
                    return ResponseUtil<TRolePermissionsModel, bool>.SuccessResult(requestObject, true);
                }

                currDb.RollbackTran();
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
            }
            catch(Exception ex)
            {
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TRolePermissionsModel, bool>> PutAsync(RequestObject<TRolePermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TRolePermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_RolePermissions数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TRolePermissionsModel, bool>> DeleteAsync(RequestObject<TRolePermissionsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TRolePermissionsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TRolePermissionsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_RolePermissions数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TRolePermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_RolePermissions数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TRolePermissionsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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
        /// 删除角色权限，通过RoleId删除所有权限
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteByRoleIdAsync(RequestObject<int> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TRolePermissionsModel>()
                    .Where(p=>p.RoleId == requestObject.PostData)
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
