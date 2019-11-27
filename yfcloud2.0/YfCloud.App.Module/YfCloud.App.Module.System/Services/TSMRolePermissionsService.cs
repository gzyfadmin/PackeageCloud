///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMRolePermissionsService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 13:38:01
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
using YfCloud.App.Module.System.Models.TSMRolePermissions;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.DBModel.System;
using YfCloud.DBModel;
using YfCloud.Utilities.AutoMapper;
using System.Threading;
using YfCloud.Framework.WebApi;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMRolePermissionsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITSMRolePermissionsService))]
    public class TSMRolePermissionsService : ITSMRolePermissionsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMRolePermissionsService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMRolePermissionsService(IDbContext dbContext, ILogger<TSMRolePermissionsService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_SM_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolePermissionsQueryModel, List<TSMRolePermissionsQueryModel>>> GetAsync(RequestObject<TSMRolePermissionsQueryModel> requestObject)
        {
            try
            {
                List<TSMRolePermissionsQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMRolePermissionsDbModel>();
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMRolePermissionsDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select(
						(t) => new TSMRolePermissionsQueryModel 
						{
							Id = t.Id,
							CompanyId = t.CompanyId,
							RoleId = t.RoleId,
							MenuId = t.MenuId,
							ButtonIds = t.ButtonIds,
							CreateId = t.CreateId,
							CreateTime = t.CreateTime,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TSMRolePermissionsQueryModel 
						{
							Id = t.Id,
							CompanyId = t.CompanyId,
							RoleId = t.RoleId,
							MenuId = t.MenuId,
							ButtonIds = t.ButtonIds,
							CreateId = t.CreateId,
							CreateTime = t.CreateTime,
						})
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<TSMRolePermissionsQueryModel, List<TSMRolePermissionsQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMRolePermissionsQueryModel, List<TSMRolePermissionsQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }
        
        /// <summary>
        /// 新增T_SM_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="iUserId">用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolePermissionsAddModel,bool>> PostAsync(RequestObject<TSMRolePermissionsAddModel> requestObject,int iUserId)
        {
            var currDb = _db.Instance;
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostDataList.Count < 1)
                    return ResponseUtil<TSMRolePermissionsAddModel, bool>.FailResult(requestObject, false, "PostDataList不能为null");
                var userModel = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == iUserId).First();
                if(userModel == null)
                    return ResponseUtil<TSMRolePermissionsAddModel, bool>.FailResult(requestObject, false, "未找到该用户");
                var result = false;
                //获取当前角色所有已有权限，根据提交
                var permissions = currDb.Queryable<TSMRolePermissionsDbModel>()
                    .Where(p => p.RoleId == requestObject.PostDataList[0].RoleId)
                    .ToList();
                //新增或修改权限
                foreach (var item in requestObject.PostDataList)
                {
                    //新增或修改
                    var dbCurrItem = permissions.Where(p => p.RoleId == item.RoleId && p.MenuId == item.MenuId).FirstOrDefault();
                    if (dbCurrItem == null)
                    {
                        var dbModel = _mapper.Map<TSMRolePermissionsDbModel>(item);
                        dbModel.CompanyId = (int)userModel.CompanyId;
                        result = await currDb.Insertable(dbModel).ExecuteCommandIdentityIntoEntityAsync();
                        item.Id = dbModel.Id;
                    }
                    else
                    {
                        result = await currDb.Updateable(_mapper.Map<TSMRolePermissionsDbModel>(item)).UpdateColumns(p => p.ButtonIds)
                        .Where(p => p.RoleId == item.RoleId && p.MenuId == item.MenuId && p.CompanyId == userModel.CompanyId)
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
                    result = await currDb.Deleteable<TSMRolePermissionsDbModel>()
                        .Where(p => !ids.Contains(p.Id) && p.RoleId == requestObject.PostDataList[0].RoleId && p.CompanyId == userModel.CompanyId)
                        .ExecuteCommandAsync() >= 0;
                }
                else
                {
                    currDb.RollbackTran();
                    return ResponseUtil<TSMRolePermissionsAddModel, bool>.FailResult(requestObject, false, "保存权限信息失败!");
                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMRolePermissionsAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMRolePermissionsAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMRolePermissionsAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 修改T_SM_RolePermissions数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolePermissionsEditModel, bool>> PutAsync(RequestObject<TSMRolePermissionsEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMRolePermissionsEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    var editList = _mapper.Map<List<TSMRolePermissionsEditModel>, List<TSMRolePermissionsDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TSMRolePermissionsDbModel>(requestObject.PostData);
                    result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;
                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMRolePermissionsEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMRolePermissionsEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMRolePermissionsEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_RolePermissions数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject,int iUserId)
        {
            return DeleteAsync<int>(requestObject,iUserId);
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        private async Task<ResponseObject<T, bool>> DeleteAsync<T>(RequestObject<T> requestObject,int iUserId)
        {
            try
            {
                //删除主表信息
                var companyId = SMCurentUserManager.GetCurentUserID(iUserId, _db.Instance)?.CompanyId;
                var result = await _db.Instance.Deleteable<TSMRolePermissionsDbModel>()
                    .Where(p=>p.CompanyId == companyId && p.RoleId == Convert.ToInt32(requestObject.PostData)).ExecuteCommandAsync() > 0;
                //返回执行结果
                if (result)
                    return ResponseUtil<T, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<T, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<T, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 根据用户ID获取当前所在公司的菜单树
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public async Task<ResponseObject<string, List<MenuTreeModel>>> GetMenuTree(int iUserId)
        {
            try
            {
                //获取用户信息，后期需要改成从缓存中获取
                var userModel = await _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == iUserId).FirstAsync();
                if(userModel == null)
                    return ResponseUtil<string, List<MenuTreeModel>>
                    .FailResult(new RequestObject<string>(), null, $"账户信息不存在");
                //获取菜单信息
                var allMenus = await _db.Instance.Queryable<TPMMenusDbModel>().ToListAsync();

                //获取当前公司权限
                var permissions =  _db.Instance.Queryable<TTenantPermissionsModel, TPMMenusDbModel>((p, p1) => new object[]
                                 {
                                    JoinType.Inner, p.MenuId==p1.Id
                                 }).Where((p, p1) => p.TenantId == userModel.CompanyId);


                //获取菜单
                var memu = permissions.Select((p, p1) => new { buttons = p.ButtonIds, memu = p1 }).ToList();

                var menuDictoray = memu.ToDictionary(p => p.memu.Id, p => p.buttons);
                List<int> allMenuIDS = new List<int>();

                foreach (var item in memu)
                {
                    var ids = item.memu.LogicPath.Split('.').Select(p => Convert.ToInt32(p)).ToList();
                    allMenuIDS.AddRange(ids);
                }

                allMenuIDS = allMenuIDS.Distinct().ToList();

                var allButtons = await _db.Instance.Queryable<TButtonsModel>().ToListAsync();

                var allDisplayNodes = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => allMenuIDS.Contains(p.Id) && p.Status == true).ToList(); //所有需要展示的菜单

                var data = GetMenuTree(allDisplayNodes, menuDictoray, allButtons, -1);

                return ResponseUtil<string, List<MenuTreeModel>>
                    .SuccessResult(new RequestObject<string>(), data);

            }
            catch (Exception ex)
            {
                return ResponseUtil<string, List<MenuTreeModel>>
                    .FailResult(new RequestObject<string>(), null, $"查询发生异常{Environment.NewLine}{ex.Message}");
            }
        }


        private List<MenuTreeModel> GetMenuTree(List<TPMMenusDbModel> aimData, Dictionary<int, string> menuButtons, List<TButtonsModel> buttonsModels, int pid)
        {
            List<MenuTreeModel> tree = new List<MenuTreeModel>();
            var children = aimData.Where(p => p.ParentID == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentID == pid).OrderBy(p => p.Seq))
                {
                   
                    MenuTreeModel node = new MenuTreeModel();
                    node.Id = item.Id;
                    node.IsMenu = item.IsMenu;
                    node.MenuAnotherName = item.MenuAnotherName;
                    node.MenuDesc = item.MenuDesc;
                    node.MenuIcon = item.MenuIcon;
                    node.MenuName = item.MenuName;
                    node.MenuPath = item.MenuPath;
                    node.ParentID = item.ParentID;
                    node.Seq = item.Seq;
                    node.Status = item.Status;

                    if (menuButtons.Keys.Any(p => p == item.Id))
                    {
                        if (!string.IsNullOrEmpty(menuButtons[item.Id]))
                        {
                            var ids = menuButtons[item.Id].Split(",").Select(p => Convert.ToInt32(p)).ToList();

                            node.Buttons = buttonsModels.Where(p => ids.Contains(p.Id)).ToList();

                        }
                    }

                    node.Children = GetMenuTree(aimData, menuButtons, buttonsModels, item.Id);

                    tree.Add(node);
                }
            }
            return tree;
        }

        private ResponseObject<string, List<MenuTreeModel>> GetTrees(List<TPMMenusDbModel> menus,List<TTenantPermissionsModel> permissions,
            List<TButtonsModel> buttons)
        {
            var result = new List<MenuTreeModel>();
            foreach (var item in permissions)
            {
                var currMenu = GetMenuTreeModel(menus.Where(p => p.Id == item.MenuId).FirstOrDefault());

                currMenu.Buttons = GetButtons(buttons,item);

                if (currMenu == null) continue;
                //如果父节点是-1则直接添加，作为根节点
                if(currMenu.ParentID == -1)
                {
                    result.Add(currMenu);
                    continue;
                }
                //判断当前节点的父节点是否已经存在
                if (MenuExits(result, currMenu)) continue;
                //获取菜单树
                var tree = GetTree(menus, currMenu,item);
                //检查tree根节点是否已经存在
                if(!CheckRootNode(result, tree))
                    result.Add(tree);
            }
            return ResponseUtil<string,List<MenuTreeModel>>.SuccessResult(new RequestObject<string>(),result,-1);
        }

        /// <summary>
        /// 获取按钮权限信息
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public List<TButtonsModel> GetButtons(List<TButtonsModel> buttons,TTenantPermissionsModel permission)
        {
            var buttonIdList = permission.ButtonIds.Split(",").ToList();
            var result = new List<TButtonsModel>();
            buttonIdList.ForEach(p => result.Add(buttons.Where(p1 => p1.Id == int.Parse(p)).FirstOrDefault()));
            return result;
        }

        /// <summary>
        /// 向上获取树
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="menuTree"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        private MenuTreeModel GetTree(List<TPMMenusDbModel> menus,MenuTreeModel menuTree,TTenantPermissionsModel permission)
        {
            var parent = menus.Where(p => p.Id == menuTree.ParentID).FirstOrDefault();
            if (parent == null) return null;
            var parentMenu = GetMenuTreeModel(parent);
            parentMenu.Children.Add(menuTree);
            if (parentMenu.ParentID != -1)
                return GetTree(menus, parentMenu,permission);
            return parentMenu;
        }

        /// <summary>
        /// 获取菜单对象
        /// </summary>
        /// <param name="menusDbModel"></param>
        /// <returns></returns>
        private MenuTreeModel GetMenuTreeModel(TPMMenusDbModel menusDbModel)
        {
            var result = ExpressionGenericMapper<TPMMenusDbModel, MenuTreeModel>.Trans(menusDbModel);
            result.Children = new List<MenuTreeModel>();
            return result;
        }

        /// <summary>
        /// 判断节点是否已经存在，如果存在则把当前菜单添加到响应节点，如果不存在返回false
        /// </summary>
        /// <param name="menuTrees"></param>
        /// <param name="currMenu"></param>
        /// <returns></returns>
        private bool MenuExits(List<MenuTreeModel> menuTrees,MenuTreeModel currMenu)
        {
            foreach(var item in menuTrees)
            {
                if(item.Id == currMenu.ParentID)
                {
                    item.Children.Add(currMenu);
                    return true;
                }
                if(item.Children != null && item.Children.Count > 0)
                {
                    return MenuExits(item.Children, currMenu);
                }
            }
            return false;
        }

        private bool CheckRootNode(List<MenuTreeModel> menuTrees, MenuTreeModel currMenu)
        {
            foreach (var item in menuTrees)
            {
                if (item.Id == currMenu.Id && item.ParentID == currMenu.ParentID)
                {
                    item.Children.AddRange(currMenu.Children);
                    return true;
                }
                if (item.Children != null && item.Children.Count > 0)
                {
                    return CheckRootNode(menuTrees, currMenu);
                }
            }
            return false;
        }


    }
}
