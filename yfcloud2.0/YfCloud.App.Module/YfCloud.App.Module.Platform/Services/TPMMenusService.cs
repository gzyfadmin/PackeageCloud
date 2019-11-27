///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMMenusService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15 10:49:00
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.System.Models.TPMMenus;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TPMMenus;
using YfCloud.App.Module.Platform.Service;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITPMMenusService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITPMMenusService))]
    public class TPMMenusService : ITPMMenusService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TPMMenusService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly ITUsersService _tUsersService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="tUsersService"></param>
        public TPMMenusService(IDbContext dbContext, ILogger<TPMMenusService> logger,IMapper mapper, ITUsersService tUsersService)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            this._tUsersService = tUsersService;
        }
        
        /// <summary>
        /// 获取T_PM_Menus数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusQueryModel>>> GetAsync(RequestObject<TPMMenusQueryModel> requestObject)
        {
            try
            {
                List<TPMMenusQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TPMMenusDbModel>();
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TPMMenusDbModel>(item.Column);
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
						(t) => new TPMMenusQueryModel 
						{
							Id = t.Id,
							ParentID = t.ParentID,
							MenuName = t.MenuName,
							MenuPath = t.MenuPath,
							Seq = t.Seq,
							IsMenu = t.IsMenu,
							MenuAnotherName = t.MenuAnotherName,
							Status = t.Status,
							MenuIcon = t.MenuIcon,
							MenuDesc = t.MenuDesc,
							CreateTime = t.CreateTime,
							CreateId = t.CreateId,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TPMMenusQueryModel 
						{
							Id = t.Id,
							ParentID = t.ParentID,
							MenuName = t.MenuName,
							MenuPath = t.MenuPath,
							Seq = t.Seq,
							IsMenu = t.IsMenu,
							MenuAnotherName = t.MenuAnotherName,
							Status = t.Status,
							MenuIcon = t.MenuIcon,
							MenuDesc = t.MenuDesc,
							CreateTime = t.CreateTime,
							CreateId = t.CreateId,
						})
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<TPMMenusQueryModel, List<TPMMenusQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TPMMenusQueryModel, List<TPMMenusQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMMenusQueryModel, List<TPMMenusTreeModel>>> GetTreeAsync(RequestObject<TPMMenusQueryModel> requestObject)
        {
            var result = new List<TPMMenusTreeModel>();

            try
            {
                int totalNumber = -1;
                var allList = _db.Instance.Queryable<TPMMenusDbModel>().ToList();

                if (requestObject.QueryConditions == null || requestObject.QueryConditions.Count() == 0)
                {
                    requestObject.QueryConditions = new List<QueryCondition>() { new QueryCondition {
                         Column="ParentID",
                         Condition=ConditionEnum.Equal,
                         Content="-1"
                    } };
                }

                var all = await GetAsync(requestObject);


                foreach (var item in all.Data.Select(p=>p.ParentID).Distinct())
                {
                    
                    result.AddRange(GetMenuTree(allList, item));
                }

                return ResponseUtil<TPMMenusQueryModel, List<TPMMenusTreeModel>>.SuccessResult(requestObject, result, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TPMMenusQueryModel, List<TPMMenusTreeModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

       



        private List<TPMMenusTreeModel> GetMenuTree(List<TPMMenusDbModel> aimData, int pid)
        {
            List<TPMMenusTreeModel> tree = new List<TPMMenusTreeModel>();
            var children = aimData.Where(p => p.ParentID == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentID == pid).OrderBy(p => p.Seq))
                {
                    TPMMenusTreeModel node = ExpressionGenericMapper<TPMMenusDbModel, TPMMenusTreeModel>.Trans(item);
                    node.Children = GetMenuTree(aimData, item.Id);
                    tree.Add(node);
                }
            }
            return tree;
        }


            /// <summary>
            /// 新增T_PM_Menus数据
            /// </summary>
            /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
            /// <returns></returns>
        public async Task<ResponseObject<TPMMenusAddModel,bool>> PostAsync(RequestObject<TPMMenusAddModel> requestObject)
        {
            try
            {
                _db.Instance.BeginTran();
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TPMMenusAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = true;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    var addList = _mapper.Map<List<TPMMenusAddModel>, List<TPMMenusDbModel>>(requestObject.PostDataList);

                    List<TPMMenusDbModel> parentNodes = new List<TPMMenusDbModel>();
                    foreach (var item in addList)
                    {
                        parentNodes.AddRange(FindParentNodes(item.ParentID));

                    }

                    foreach (var item in parentNodes.Distinct())
                    {
                        item.IsMenu = false;
                        await _db.Instance.Updateable(item).UpdateColumns(p => new { p.IsMenu }).ExecuteCommandAsync();
                    }

                    result = await _db.Instance.Insertable(addList).ExecuteCommandIdentityIntoEntityAsync();

                    foreach (var item in addList)
                    {
                        if (item.ParentID == -1)
                        {
                            item.LogicPath = item.Id.ToString();
                        }
                        else {
                            var pNode = parentNodes.Where(p => p.Id == item.ParentID).FirstOrDefault();

                            if (pNode != null)
                            {
                                item.LogicPath = pNode.LogicPath + "." + item.Id.ToString();
                            }
                        }
                    }

                    await _db.Instance.Updateable(addList).UpdateColumns(p => new { p.LogicPath }).ExecuteCommandAsync();
                }
                else
                {
                    var addModel = _mapper.Map<TPMMenusDbModel>(requestObject.PostData);
                    List<TPMMenusDbModel> parentNodes= FindParentNodes(addModel.ParentID);

                    foreach (var item in parentNodes)
                    {
                        item.IsMenu = false;
                        await _db.Instance.Updateable(item).UpdateColumns(p =>new { p.IsMenu }).ExecuteCommandAsync();
                    }


                    result = await _db.Instance.Insertable(addModel).ExecuteCommandIdentityIntoEntityAsync();

                    if (addModel.ParentID == -1)
                    {
                        addModel.LogicPath = addModel.Id.ToString();
                    }
                    else
                    {
                        var pNode = parentNodes.Where(p => p.Id == addModel.ParentID).FirstOrDefault();
                        if (pNode != null)
                        {
                            addModel.LogicPath = pNode.LogicPath + "." + addModel.Id.ToString();
                        }
                    }

                    await _db.Instance.Updateable(addModel).UpdateColumns(p => new { p.LogicPath }).ExecuteCommandAsync();
                }

                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TPMMenusAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TPMMenusAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TPMMenusAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        private List<TPMMenusDbModel> FindParentNodes(int id)
        {
            List<TPMMenusDbModel> result = new List<TPMMenusDbModel>();
            var parentNode= _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == id).First();

            while (parentNode != null)
            {
                result.Add(parentNode);
                parentNode= _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == parentNode.ParentID).First();
            }

            return result;
        }
        
        /// <summary>
        /// 修改T_PM_Menus数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMMenusEditModel, bool>> PutAsync(RequestObject<TPMMenusEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TPMMenusEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    var editList = _mapper.Map<List<TPMMenusEditModel>, List<TPMMenusDbModel>>(requestObject.PostDataList);

                    var childList = new List<TPMMenusDbModel>();
                    foreach (var item in editList)
                    {
                        var editModelDB = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == item.Id).First();
                        string oldlogicPath = editModelDB.LogicPath;//原来的路径
                        string newlogicPath = oldlogicPath;

                        if (item.ParentID == -1)
                        {
                            newlogicPath = item.Id.ToString();

                            item.LogicPath = newlogicPath;
                        }
                        else
                        {
                            var pnode = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == item.ParentID).First();
                            if (pnode != null)
                            {
                                newlogicPath= pnode.LogicPath + "." + item.Id.ToString();
                                item.LogicPath = newlogicPath;
                            }
                        }

                        if (!string.Equals(oldlogicPath, newlogicPath))// 如果
                        {
                            int oldLength = oldlogicPath.Length;

                            var cList = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.LogicPath.StartsWith(oldlogicPath) && p.Id != item.Id).ToList();
                            cList.ForEach(x => {
                                x.LogicPath = newlogicPath + x.LogicPath.Substring(oldLength);
                            });
                            childList.AddRange(cList);
                        }                       
                    }

                    if (childList.Count() > 0)
                    {
                        await _db.Instance.Updateable(childList).UpdateColumns(p => new { p.LogicPath }).ExecuteCommandAsync();
                    }

                    result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TPMMenusDbModel>(requestObject.PostData);

                    var editModelDB=  _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == editModel.Id).First();

                    string oldlogicPath1 = editModelDB.LogicPath.ToString();//原来的路径

                    string newlogicPath2 = oldlogicPath1;
                    var childList = new List<TPMMenusDbModel>();

                    if (editModel.ParentID == -1)
                    {
                        newlogicPath2= editModel.Id.ToString();
                        editModel.LogicPath = newlogicPath2;
                    }
                    else
                    {
                        var pnode = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == editModel.ParentID).First();
                        if (pnode != null)
                        {
                            newlogicPath2= pnode.LogicPath + "." + editModel.Id.ToString();
                            editModel.LogicPath = newlogicPath2;
                        }
                    }
                    if (!string.Equals(oldlogicPath1, newlogicPath2))// 如果
                    {
                        int oldLength = oldlogicPath1.Length;

                        var cList = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.LogicPath.StartsWith(oldlogicPath1) && p.Id != editModel.Id).ToList();
                        cList.ForEach(x => {
                            x.LogicPath = newlogicPath2 + x.LogicPath.Substring(oldLength);
                        });

                        childList.AddRange(cList);
                    }

                    if (childList.Count() > 0)
                    {
                        await _db.Instance.Updateable(childList).UpdateColumns(p => new { p.LogicPath }).ExecuteCommandAsync();
                    }

                    result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;
                }


                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TPMMenusEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TPMMenusEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TPMMenusEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMMenusMoveModel, bool>> MovePostion(RequestObject<TPMMenusMoveModel> requestObject)
        {
            try
            {
                //执行结果
                var result = true;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TPMMenusMoveModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    //var editList = _mapper.Map<List<TPMMenusEditModel>, List<TPMMenusDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    //var editModel = _mapper.Map<TPMMenusDbModel>(requestObject.PostData);
                    //result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;
                    var thisMenu= _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id == requestObject.PostData.Id).First();

                    if (thisMenu != null)
                    {
                        if (requestObject.PostData.Type == 0) //上移
                        {
                            var nodes = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.ParentID == thisMenu.ParentID).OrderBy(p => p.Seq).ToList();

                            var tempNodes = nodes.Where(p => p.Id == thisMenu.Id).FirstOrDefault();

                            int index = nodes.IndexOf(tempNodes);

                            if (index != 0)
                            {
                                for (var i = 0; i < nodes.Count; i++)
                                {
                                    if (i + 1 == index)//前一位置 
                                    {
                                        nodes[i].Seq = index;
                                    }
                                    else if (i == index)//当前位置
                                    {
                                        nodes[i].Seq = i - 1;
                                    }
                                    else
                                    {
                                        nodes[i].Seq = i;
                                    }
                                }

                               await _db.Instance.Updateable(nodes).UpdateColumns(p => new { p.Seq }).ExecuteCommandAsync();
                            }

                        }
                        else if (requestObject.PostData.Type == 1)//下移
                        {
                            var nodes = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.ParentID == thisMenu.ParentID).OrderBy(p => p.Seq).ToList();
                            var tempNodes = nodes.Where(p => p.Id == thisMenu.Id).FirstOrDefault();

                            int index = nodes.IndexOf(tempNodes);

                            if (index != nodes.Count)
                            {
                                for (var i = nodes.Count-1; i >-1 ; i--)
                                {
                                    if (i-1  == index)//下一个位置
                                    {
                                        nodes[i].Seq = index;
                                    }
                                    else if (i == index)//当前位置
                                    {
                                        nodes[i].Seq = i + 1;
                                    }
                                    else
                                    {
                                        nodes[i].Seq = i;
                                    }
                                }

                                await _db.Instance.Updateable(nodes).UpdateColumns(p => new { p.Seq }).ExecuteCommandAsync();
                            }
                        }
                    }
                }
                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TPMMenusMoveModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TPMMenusMoveModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TPMMenusMoveModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

       

        /// <summary>
        /// 删除T_PM_Menus数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMenusDeleteAllModel, bool>> DeleteAsync(RequestObject<TPMenusDeleteAllModel> requestObject,int UserID)
        {
            try
            {
                //执行结果
                var result = false;

               

                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    if (requestObject.PostDataList.Select(p => p.PassWord).Count() !=1)
                    {
                        return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, "密码无效");
                    }
                    else
                    {
                        //验证密码
                        bool checkP = _tUsersService.CheckPassword(UserID, requestObject.PostDataList.Select(p=>p.PassWord).FirstOrDefault());

                        if (checkP != true)
                        {
                            return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, "密码无效");
                        }
                    }


                    var ids = requestObject.PostDataList.Select(p => p.Id).ToList();

                    //批量删除
                    var delList = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => ids.Contains(p.Id)).ToList();

                    List<TPMMenusDbModel> toDelList = new List<TPMMenusDbModel>();

                    foreach (var item in delList)
                    {
                        var toDel = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.LogicPath.StartsWith(item.LogicPath)).ToList();

                        toDelList.AddRange(toDel);
                    }

                    result = await _db.Instance.Deleteable(toDelList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //验证密码
                   bool checkP= _tUsersService.CheckPassword(UserID, requestObject.PostData.PassWord);

                    if (checkP != true)
                    {
                        return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, "密码无效");
                    }

                    //单记录删除
                    var delModel = requestObject.PostData;

                    var delModelDB = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.Id==delModel.Id).First();

                    if (delModelDB != null)//删除该节点以及所有子节点
                    {
                        var toDelList = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => p.LogicPath.StartsWith(delModelDB.LogicPath)).ToList();

                        result = await _db.Instance.Deleteable(toDelList).ExecuteCommandAsync() > 0;
                    }

                    
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TPMenusDeleteAllModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TPMenusDeleteAllModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_PM_Menus数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_PM_Menus数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            return DeleteAsync<int[]>(requestObject);
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        private async Task<ResponseObject<T, bool>> DeleteAsync<T>(RequestObject<T> requestObject)
        {
            try
            {
                //删除主表信息
                var ids = requestObject.PostData as int[];
                var result = await _db.Instance.Deleteable<TPMMenusDbModel>().In(ids).ExecuteCommandAsync() > 0;
                
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
        
    }
}
