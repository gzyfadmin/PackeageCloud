///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMRolesService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 13:30:21
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
using YfCloud.App.Module.System.Models.TSMRoles;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.DBModel.System;
using YfCloud.DBModel;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMRolesService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITSMRolesService))]
    public class TSMRolesService : ITSMRolesService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMRolesService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMRolesService(IDbContext dbContext, ILogger<TSMRolesService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_SM_Roles数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolesQueryModel, List<TSMRolesQueryTreeModel>>> GetAsync(RequestObject<TSMRolesQueryModel> requestObject,int UserId)
        {
            try
            {
                List<TSMRolesQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                               (t, t1) => new object[]
                               {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                               }).Where((t, t1) => t.ID == UserId).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();


                var query = _db.Instance.Queryable<TSMRolesDbModel>().Where(p=>p.CompanyId==user.CompanyId);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMRolesDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                queryData = await query.Select(
                        (t) => new TSMRolesQueryModel
                        {
                            Id = t.Id,
                            RoleName = t.RoleName,
                            ParentId = t.ParentId,
                            PathLogic = t.PathLogic,
                            SeqNumber = t.SeqNumber,
                            CompanyId = t.CompanyId,
                            RoleDesc = t.RoleDesc,
                            CreateId = t.CreateId,
                            CreateTime = t.CreateTime,
                        })
                        .ToListAsync();

                var result = GetMenuTree(queryData, -1);

                //返回执行结果
                return ResponseUtil<TSMRolesQueryModel, List<TSMRolesQueryTreeModel>>.SuccessResult(requestObject, result, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMRolesQueryModel, List<TSMRolesQueryTreeModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        private List<TSMRolesQueryTreeModel> GetMenuTree(List<TSMRolesQueryModel> aimData, int pid)
        {
            List<TSMRolesQueryTreeModel> tree = new List<TSMRolesQueryTreeModel>();
            var children = aimData.Where(p => p.ParentId == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentId == pid).OrderBy(p => p.SeqNumber))
                {
                    TSMRolesQueryTreeModel node = ExpressionGenericMapper<TSMRolesQueryModel, TSMRolesQueryTreeModel>.Trans(item);
                    node.Children = GetMenuTree(aimData, item.Id);
                    tree.Add(node);
                }
            }

            return tree;
        }

        /// <summary>
        /// 新增T_SM_Roles数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="UserId">操作用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolesAddModel,bool>> PostAsync(RequestObject<TSMRolesAddModel> requestObject,int UserId)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null )
                    return ResponseUtil<TSMRolesAddModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //var addList = _mapper.Map<List<TSMRolesAddModel>, List<TSMRolesDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                              (t, t1) => new object[]
                              {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                              }).Where((t, t1) => t.ID == UserId).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();

                    var addModel = _mapper.Map<TSMRolesDbModel>(requestObject.PostData);
                    addModel.CreateId = UserId;
                    addModel.CreateTime = DateTime.Now;
                    addModel.CompanyId = user.CompanyId.Value;

                    await _db.Instance.Insertable(addModel).ExecuteCommandIdentityIntoEntityAsync();

                    //处理
                    if (addModel.ParentId == -1)
                    {
                        addModel.PathLogic = addModel.Id.ToString();
                    }
                    else
                    {
                        var pNode = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.Id == addModel.ParentId).First();

                        if (pNode != null)
                        {
                            addModel.PathLogic = pNode.PathLogic + "." + addModel.Id.ToString();
                        }
                    }

                     _db.Instance.Updateable(addModel).UpdateColumns(p => new { p.PathLogic}).ExecuteCommand();
                }
                //返回执行结果
                return ResponseUtil<TSMRolesAddModel, bool>.SuccessResult(requestObject, true);
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMRolesAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 修改T_SM_Roles数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolesEditModel, bool>> PutAsync(RequestObject<TSMRolesEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMRolesEditModel, bool>.FailResult(requestObject, false, "PostData不能都为null");

                //单记录更新
                //var editModel = _mapper.Map<TSMRolesDbModel>(requestObject.PostData);
                //result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;

                //单记录更新
                var editModel = _mapper.Map<TSMRolesDbModel>(requestObject.PostData);

                var editModelDB = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.Id == editModel.Id).First();

                string oldlogicPath1 = editModelDB.PathLogic.ToString();//原来的路径

                string newlogicPath2 = oldlogicPath1;
                var childList = new List<TSMRolesDbModel>();

                if (editModel.ParentId == -1)
                {
                    newlogicPath2 = editModel.Id.ToString();
                    editModel.PathLogic = newlogicPath2;
                }
                else
                {
                    var pnode = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.Id == editModel.ParentId).First();
                    if (pnode != null)
                    {
                        newlogicPath2 = pnode.PathLogic + "." + editModel.Id.ToString();
                        editModel.PathLogic = newlogicPath2;
                    }
                }
                if (!string.Equals(oldlogicPath1, newlogicPath2))// 如果路径变化了
                {
                    int oldLength = oldlogicPath1.Length;

                    string temPath = string.Concat(oldlogicPath1, ".");

                    var cList = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.PathLogic.StartsWith(temPath)).ToList();
                    cList.ForEach(x =>
                    {
                        x.PathLogic = newlogicPath2 + x.PathLogic.Substring(oldLength);
                    });

                    childList.AddRange(cList);
                }

                if (childList.Count() > 0)
                {
                    await _db.Instance.Updateable(childList).UpdateColumns(p => new { p.PathLogic }).ExecuteCommandAsync();
                }

                result = await _db.Instance.Updateable(editModel).IgnoreColumns(p => new
                {
                    p.CreateId,
                    p.CreateTime
                }).ExecuteCommandAsync() > 0;

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMRolesEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMRolesEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMRolesEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        
        /// <summary>
        /// 删除T_SM_Roles数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Roles数据，通过主表主键删除数据，批量删除
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
                var ids = new List<int>();
                if (requestObject.PostData.GetType() == typeof(int))
                {
                    int idTemp = requestObject.PostData.ObjToInt();
                    ids.Add(idTemp);
                }
                else
                {
                    var idTemp = requestObject.PostData as int[];
                    ids.AddRange(idTemp);
                }

                List<int> allDeleteIds = new List<int>();

                foreach (int id in ids)
                {
                    var delModel = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.Id == id).First();

                    var alldelModel = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.PathLogic.StartsWith(delModel.PathLogic)).Select(p => p.Id).ToList();

                    allDeleteIds.AddRange(alldelModel);
                }

                var result = await _db.Instance.Deleteable<TSMRolesDbModel>().In(allDeleteIds.Distinct()).ExecuteCommandAsync() > 0;

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
        /// 上移下移
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRolesMoveModel, bool>> MovePostion(RequestObject<TSMRolesMoveModel> requestObject)
        {
            try
            {
                //执行结果
                var result = true;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMRolesMoveModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
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
                    var thisMenu = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.Id == requestObject.PostData.Id).First();

                    if (thisMenu != null)
                    {
                        if (requestObject.PostData.Type == 0) //上移
                        {
                            var nodes = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.ParentId == thisMenu.ParentId).OrderBy(p => p.SeqNumber).ToList();

                            var tempNodes = nodes.Where(p => p.Id == thisMenu.Id).FirstOrDefault();

                            int index = nodes.IndexOf(tempNodes);

                            if (index != 0)
                            {
                                for (var i = 0; i < nodes.Count; i++)
                                {
                                    if (i + 1 == index)//前一位置 
                                    {
                                        nodes[i].SeqNumber = index;
                                    }
                                    else if (i == index)//当前位置
                                    {
                                        nodes[i].SeqNumber = i - 1;
                                    }
                                    else
                                    {
                                        nodes[i].SeqNumber = i;
                                    }
                                }

                                await _db.Instance.Updateable(nodes).UpdateColumns(p => new { p.SeqNumber }).ExecuteCommandAsync();
                            }

                        }
                        else if (requestObject.PostData.Type == 1)//下移
                        {
                            var nodes = _db.Instance.Queryable<TSMRolesDbModel>().Where(p => p.ParentId == thisMenu.ParentId).OrderBy(p => p.SeqNumber).ToList();
                            var tempNodes = nodes.Where(p => p.Id == thisMenu.Id).FirstOrDefault();

                            int index = nodes.IndexOf(tempNodes);

                            if (index != nodes.Count)
                            {
                                for (var i = nodes.Count - 1; i > -1; i--)
                                {
                                    if (i - 1 == index)//下一个位置
                                    {
                                        nodes[i].SeqNumber = index;
                                    }
                                    else if (i == index)//当前位置
                                    {
                                        nodes[i].SeqNumber = i + 1;
                                    }
                                    else
                                    {
                                        nodes[i].SeqNumber = i;
                                    }
                                }

                                await _db.Instance.Updateable(nodes).UpdateColumns(p => new { p.SeqNumber }).ExecuteCommandAsync();
                            }
                        }
                    }
                }
                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMRolesMoveModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMRolesMoveModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMRolesMoveModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 为员工指派角色 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRoleUserRelationEditModel, bool>> DispathDept(RequestObject<TSMRoleUserRelationEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = true;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null)
                    return ResponseUtil<TSMRoleUserRelationEditModel, bool>.FailResult(requestObject, false, "PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //判断当前数据是否属于当前公司

                     var editList = _mapper.Map<List<TSMRoleUserRelationEditModel>, List<TSMRoleUserRelationDbModel>>(requestObject.PostDataList);

                     var UserAccountIdlist = editList.Select(p => p.UserId).Distinct().ToList();//当前选择的用户

                     var RoleIdList = editList.Select(p => p.RoleId).FirstOrDefault();//当前角色

                    //数据库中当前角色拥有的用户
                     var existsID = _db.Instance.Queryable<TSMRoleUserRelationDbModel>().Where(p => p.RoleId == RoleIdList).Select(p=>p.UserId).ToList();


                    //再删除选择的员工，在其他组织

                    await _db.Instance.Deleteable<TSMRoleUserRelationDbModel>(p => UserAccountIdlist.Contains(p.UserId) && p.RoleId != RoleIdList).ExecuteCommandAsync();

                    var insertObj = editList.Where(p => !existsID.Contains(p.UserId)).ToList();
                    if (insertObj.Count() > 0)
                    {
                        //新增
                        await _db.Instance.Insertable(insertObj).ExecuteCommandAsync();
                    }



                    //////先删除 本次没有选择的员工
                    ////await _db.Instance.Deleteable<TSMRoleUserRelationDbModel>(p => !ids.Contains(p.Id)).ExecuteCommandAsync();



                    //await _db.Instance.Deleteable<TSMRoleUserRelationDbModel>(p => idsExists.Contains(p.Id)).ExecuteCommandAsync();

                    //var updateObj = editList.Where(p => p.Id > 0).ToList();
                    //if(updateObj.Count()>0)
                    //{
                    //    //修改
                    //    await _db.Instance.Updateable(updateObj).ExecuteCommandAsync();
                    //}

                    //var insertObj = editList.Where(p => p.Id == 0).ToList();
                    //if(insertObj.Count()>0)
                    //{
                    //    //新增
                    //    await _db.Instance.Insertable(insertObj).ExecuteCommandAsync();
                    //}
                }
                else
                {

                }
                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMRoleUserRelationEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMRoleUserRelationEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMRoleUserRelationEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 获取某角色下的所有员工
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>> GetDispathUserAsync(RequestObject<TSMRoleUserRelationEditModel> requestObject)
        {
            try
            {
                List<TSMUserRoleModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var condition = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "roleid").FirstOrDefault();
                int roleID = 0;

                if (condition == null)
                {
                    return ResponseUtil<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>.FailResult(requestObject, null, "deptid不能为空");
                }
                if (!int.TryParse(condition.Content, out roleID))
                {
                    return ResponseUtil<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>.FailResult(requestObject, null, "roleID必须为正整数");
                }

                var query = _db.Instance.Queryable<TSMRolesDbModel, TSMRoleUserRelationDbModel, TSMUserAccountDbModel, TSMUserInfoDbModel>(
                                (t, t1,t2,t3) => new object[]
                                {
                                    JoinType.Inner,  t.Id== t1.RoleId,
                                    JoinType.Inner,t1.UserId==t2.ID,
                                    JoinType.Left,t2.UserInfoId==t3.ID
                                }).Where((t, t1,t2,t3) => t.Id == roleID)
                                .Select((t, t1,t2,t3) => new {
                                    t1=t1,t2=t2,t3=t3
                                }
                              );
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMDeptDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }


                if (requestObject.IsPaging)
                {
                   var  queryData1 = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                    queryData = queryData1.Select(p => new TSMUserRoleModel()
                    {
                        RoleId = p.t1.RoleId,
                        ID = p.t1.Id,
                        UserId = p.t1.UserId,
                        UserAccount = new Models.TSMUserAccount.TSMUserAccountQueryAllModel()
                        {
                            ID = p.t2.ID,
                            TelAccount = p.t2.TelAccount,
                            EmailAccount = p.t2.EmailAccount,
                            AccountName = p.t2.AccountName,
                            UserInfoId = p.t2.UserInfoId,
                            CompanyId = p.t2.CompanyId,
                            Status = p.t2.Status,
                            ExpDate = p.t2.ExpDate,
                            CreateTime = p.t2.CreateTime,

                            CId = p.t3.ID,
                            JobNumber = p.t3.JobNumber,
                            RealName = p.t3.RealName,
                            FixedNumber = p.t3.FixedNumber,
                            Address = p.t3.Address,
                            HeadPicPath = p.t3.HeadPicPath,
                            Remarks = p.t3.Remarks
                        }
                    }).ToList();
                }
                else
                {
                    var queryData1 = await query.ToListAsync();
                    queryData = queryData1.Select(p => new TSMUserRoleModel()
                    {
                        RoleId = p.t1.RoleId,
                        ID = p.t1.Id,
                        UserId = p.t1.UserId,
                        UserAccount = new Models.TSMUserAccount.TSMUserAccountQueryAllModel()
                        {
                            ID = p.t2.ID,
                            TelAccount = p.t2.TelAccount,
                            EmailAccount = p.t2.EmailAccount,
                            AccountName = p.t2.AccountName,
                            UserInfoId = p.t2.UserInfoId,
                            CompanyId = p.t2.CompanyId,
                            Status = p.t2.Status,
                            ExpDate = p.t2.ExpDate,
                            CreateTime = p.t2.CreateTime,

                            CId = p.t3.ID,
                            JobNumber = p.t3.JobNumber,
                            RealName = p.t3.RealName,
                            FixedNumber = p.t3.FixedNumber,
                            Address = p.t3.Address,
                            HeadPicPath = p.t3.HeadPicPath,
                            Remarks = p.t3.Remarks
                        }
                    }).ToList();
                }


                //返回执行结果
                return ResponseUtil<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMRoleUserRelationEditModel, List<TSMUserRoleModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 删除某个角色下的用户
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> RemoveRoleForUser(RequestObject<int[]> requestObject)
        {
            try
            {
                bool result = false;
                result = await _db.Instance.Deleteable<TSMRoleUserRelationDbModel>(p => requestObject.PostData.Contains(p.Id)).ExecuteCommandAsync() > 0;

                return ResponseUtil<int[], bool>.SuccessResult(requestObject, result);
            }
            catch(Exception ex){
                return ResponseUtil<int[], bool>.FailResult(requestObject, false,"删除失败");
            }
        }

    }
}
