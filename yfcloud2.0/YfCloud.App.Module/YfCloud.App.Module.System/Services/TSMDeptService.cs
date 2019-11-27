///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMDeptService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22 11:11:43
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
using YfCloud.App.Module.System.Models.TSMDept;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.DBModel;
using YfCloud.DBModel.System;
using YfCloud.Utilities.AutoMapper;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.Framework.WebApi;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMDeptService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITSMDeptService))]
    public class TSMDeptService : ITSMDeptService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMDeptService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMDeptService(IDbContext dbContext, ILogger<TSMDeptService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_SM_Dept数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>> GetAsync(RequestObject<TSMDeptQueryModel> requestObject, int UserId)
        {
            try
            {
                List<TSMDeptQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数

                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                                (t, t1) => new object[]
                                {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                                }).Where((t, t1) => t.ID == UserId).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();

                var query = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.CompanyId == user.CompanyId);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }
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

                queryData = await query.Select(
                        (t) => new TSMDeptQueryModel
                        {
                            Id = t.Id,
                            VitualId = t.Id,
                            ActualId = t.Id,
                            DeptName = t.DeptName,
                            ParentId = t.ParentId,
                            SeqNumber = t.SeqNumber,
                            PathLogic = t.PathLogic,
                            DeptDesc = t.DeptDesc,
                            CompanyId = t.CompanyId,
                            CreateId = t.CreateId,
                            CreateTime = t.CreateTime
                        })
                        .ToListAsync();

                TSMDeptQueryModel tSMDeptQueryModel = new TSMDeptQueryModel();
                tSMDeptQueryModel.ParentId = -2;
                tSMDeptQueryModel.Id = user.CompanyId.Value;
                tSMDeptQueryModel.DeptName = user.Name;
                tSMDeptQueryModel.VitualId = -1;
                tSMDeptQueryModel.ActualId = tSMDeptQueryModel.ActualId;
                queryData.Add(tSMDeptQueryModel);

                TSMDeptQueryModel tSVitual = new TSMDeptQueryModel();
                tSVitual.ParentId = -1;
                tSVitual.Id = -100;
                tSVitual.DeptName = "未分配部门";
                tSVitual.VitualId = -100;
                tSVitual.ActualId = -100;
                tSVitual.SeqNumber = 999999;
                queryData.Add(tSVitual);


                var result = GetMenuTree(queryData, -2);

                //返回执行结果
                return ResponseUtil<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>.SuccessResult(requestObject, result, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_SM_Dept数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>> GetOnlyDepAsync(RequestObject<TSMDeptQueryModel> requestObject, int UserId)
        {
            try
            {
                List<TSMDeptQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数

                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                                (t, t1) => new object[]
                                {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                                }).Where((t, t1) => t.ID == UserId).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();

                var query = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.CompanyId == user.CompanyId);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }
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

                queryData = await query.Select(
                        (t) => new TSMDeptQueryModel
                        {
                            Id = t.Id,
                            VitualId = t.Id,
                            ActualId = t.Id,
                            DeptName = t.DeptName,
                            ParentId = t.ParentId,
                            SeqNumber = t.SeqNumber,
                            PathLogic = t.PathLogic,
                            DeptDesc = t.DeptDesc,
                            CompanyId = t.CompanyId,
                            CreateId = t.CreateId,
                            CreateTime = t.CreateTime
                        })
                        .ToListAsync();



                var result = GetMenuTree(queryData, -1);

                //返回执行结果
                return ResponseUtil<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>.SuccessResult(requestObject, result, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMDeptQueryModel, List<TSMDeptQueryTreeModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        private List<TSMDeptQueryTreeModel> GetMenuTree(List<TSMDeptQueryModel> aimData, int pid)
        {
            List<TSMDeptQueryTreeModel> tree = new List<TSMDeptQueryTreeModel>();
            var children = aimData.Where(p => p.ParentId == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentId == pid).OrderBy(p => p.SeqNumber))
                {
                    TSMDeptQueryTreeModel node = ExpressionGenericMapper<TSMDeptQueryModel, TSMDeptQueryTreeModel>.Trans(item);
                    node.Children = GetMenuTree(aimData, item.VitualId);
                    tree.Add(node);
                }
            }
            return tree;
        }

        /// <summary>
        /// 新增T_SM_Dept数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="UserId">创建人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptAddModel, bool>> PostAsync(RequestObject<TSMDeptAddModel> requestObject, int UserId)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMDeptAddModel, bool>.FailResult(requestObject, false, "PostData不能都为null");

                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //var addList = _mapper.Map<List<TSMDeptAddModel>, List<TSMDeptDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                                (t, t1) => new object[]
                                {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                                }).Where((t, t1) => t.ID == UserId).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();


                    var addModel = _mapper.Map<TSMDeptDbModel>(requestObject.PostData);
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
                        var pNode = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id == addModel.ParentId).First();

                        if (pNode != null)
                        {
                            addModel.PathLogic = pNode.PathLogic + "." + addModel.Id.ToString();
                        }
                    }

                    _db.Instance.Updateable(addModel).UpdateColumns(p => new { p.PathLogic }).ExecuteCommand();
                }

                return ResponseUtil<TSMDeptAddModel, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMDeptAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_SM_Dept数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptEditModel, bool>> PutAsync(RequestObject<TSMDeptEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMDeptEditModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    ////批量更新
                    //var editList = _mapper.Map<List<TSMDeptEditModel>, List<TSMDeptDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {

                    //单记录更新
                    var editModel = _mapper.Map<TSMDeptDbModel>(requestObject.PostData);

                    var editModelDB = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id == editModel.Id).First();

                    string oldlogicPath1 = editModelDB.PathLogic.ToString();//原来的路径

                    string newlogicPath2 = oldlogicPath1;
                    var childList = new List<TSMDeptDbModel>();

                    if (editModel.ParentId == -1)
                    {
                        newlogicPath2 = editModel.Id.ToString();
                        editModel.PathLogic = newlogicPath2;
                    }
                    else
                    {
                        var pnode = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id == editModel.ParentId).First();
                        if (pnode != null)
                        {
                            newlogicPath2 = pnode.PathLogic + "." + editModel.Id.ToString();
                            editModel.PathLogic = newlogicPath2;
                        }
                    }
                    if (!string.Equals(oldlogicPath1, newlogicPath2))// 如果路径变化了
                    {
                        int oldLength = oldlogicPath1.Length;

                        string tempPath = string.Concat(oldlogicPath1.Length, ".");

                        var cList = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.PathLogic.StartsWith(tempPath));

                        var cListTemp = cList.ToList();

                        cListTemp.ToList().ForEach(x =>
                        {
                            x.PathLogic = newlogicPath2 + x.PathLogic.Substring(oldLength);
                        });

                        childList.AddRange(cListTemp);
                    }

                    if (childList.Count() > 0)
                    {
                        await _db.Instance.Updateable(childList).UpdateColumns(p => new { p.PathLogic }).ExecuteCommandAsync();
                    }

                    result = await _db.Instance.Updateable(editModel).UpdateColumns(p => new
                    {
                        p.DeptName,
                        p.ParentId,
                        p.SeqNumber,
                        p.PathLogic,
                        p.DeptDesc
                    }).ExecuteCommandAsync() > 0;
                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMDeptEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMDeptEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMDeptEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 删除T_SM_Dept数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {

            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Dept数据，通过主表主键删除数据，批量删除
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
                _db.Instance.BeginTran();

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
                    var delModel = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id == id).First();

                    var alldelModel = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.PathLogic.StartsWith(delModel.PathLogic)).Select(p => p.Id).ToList();

                    allDeleteIds.AddRange(alldelModel);
                }

                List<int> depitIDS = allDeleteIds.Distinct().ToList();

                var result = await _db.Instance.Deleteable<TSMDeptDbModel>().In(depitIDS).ExecuteCommandAsync() > 0;
                await _db.Instance.Deleteable<TSMDeptUserRelationDbModel>().Where(p => depitIDS.Contains(p.DeptId)).ExecuteCommandAsync();


                _db.Instance.CommitTran();

                //返回执行结果
                if (result)
                    return ResponseUtil<T, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<T, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<T, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptMoveModel, bool>> MovePostion(RequestObject<TSMDeptMoveModel> requestObject,CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = true;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMDeptMoveModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
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
                    var thisMenu = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id == requestObject.PostData.Id).First();

                    if (thisMenu != null)
                    {
                        if (requestObject.PostData.Type == 0) //上移
                        {
                            var nodes = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.ParentId == thisMenu.ParentId &&p.CompanyId==currentUser.CompanyID).OrderBy(p => p.SeqNumber).ToList();

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
                            var nodes = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.ParentId == thisMenu.ParentId && p.CompanyId==currentUser.CompanyID).OrderBy(p => p.SeqNumber).ToList();
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
                    return ResponseUtil<TSMDeptMoveModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMDeptMoveModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMDeptMoveModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 分配员工权限
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<DeptUserRelationEdit, bool>> DispathDept(RequestObject<DeptUserRelationEdit> requestObject)
        {
            try
            {
                //执行结果
                var result = true;

                _db.Instance.BeginTran();
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null)
                    return ResponseUtil<DeptUserRelationEdit, bool>.FailResult(requestObject, false, "PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    var editList = _mapper.Map<List<DeptUserRelationEdit>, List<TSMDeptUserRelationDbModel>>(requestObject.PostDataList);

                    var ids = editList.Where(p => p.Id > 0).Select(p => p.Id).ToList();

                    var UserAccountIdlist = editList.Select(p => p.UserAccountId).Distinct().ToList();

                    var deptidList = editList.Select(p => p.DeptId).FirstOrDefault();

                    //先删除 本次没有选择的员工
                    await _db.Instance.Deleteable<TSMDeptUserRelationDbModel>(p => !ids.Contains(p.Id)).ExecuteCommandAsync();

                    //再删除选择的员工，在其他组织
                    var idsExists = editList.Where(p => UserAccountIdlist.Contains(p.UserAccountId) && p.DeptId != deptidList).Select(p=>p.Id);

                    await _db.Instance.Deleteable<TSMDeptUserRelationDbModel>(p => !idsExists.Contains(p.Id)).ExecuteCommandAsync();

                    var updateObj = editList.Where(p => p.Id > 0).ToList();
                    if (updateObj.Count() > 0)
                    {
                        //修改
                        await _db.Instance.Updateable(updateObj).ExecuteCommandAsync();
                    }

                    var insertObj = editList.Where(p => p.Id == 0).ToList();
                    if (insertObj.Count() > 0)
                    {
                        //新增
                        await _db.Instance.Insertable(insertObj).ExecuteCommandAsync();
                    }

                }
                else
                {

                }
                _db.Instance.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<DeptUserRelationEdit, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<DeptUserRelationEdit, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<DeptUserRelationEdit, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 获取获取部门下的员工
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>> GetDispathUserAsync(RequestObject<TSMDeptQueryForDispatchModel> requestObject,int UserID)
        {
            try
            {
                List<TSMUserAccountQueryAllModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var condition = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "deptid").FirstOrDefault();
                if (condition == null)
                {
                    return ResponseUtil<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>.FailResult(requestObject, null, "deptid不能为空");
                }
               int companyID= SMCurentUserManager.GetCurentUserID(UserID, _db.Instance).CompanyId.Value;

                var mainDept = _db.Instance.Queryable<TSMDeptDbModel>().Where(p => p.Id.ToString() == condition.Content).First();

                var query = _db.Instance.Queryable<TSMUserAccountDbModel, TSMUserInfoDbModel, TSMDeptUserRelationDbModel,TSMDeptDbModel>(
                                (t1,t2,t3,t4) => new object[]
                                {
                                    JoinType.Inner,t1.UserInfoId==t2.ID,
                                    JoinType.Left,  t1.ID== t3.UserAccountId,
                                    JoinType.Left,t3.DeptId==t4.Id
                                }).Where((t1, t2, t3, t4) => t1.CompanyId== companyID);

                if (mainDept != null)
                {
                    query = query.Where((t1, t2, t3, t4) => t4.PathLogic.StartsWith(mainDept.PathLogic));
                }
                
                if (condition.Content == "-100")
                {
                    query = query.Where((t1, t2, t3, t4) => t3.Id==null);
                }

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
                    queryData = await query.Select((t1, t2, t3, t4) => new TSMUserAccountQueryAllModel
                    {
                        ID = t1.ID,
                        TelAccount = t1.TelAccount,
                        EmailAccount = t1.EmailAccount,
                        AccountName = t1.AccountName,
                        UserInfoId = t1.UserInfoId,
                        CompanyId = t1.CompanyId,
                        Status = t1.Status,
                        ExpDate = t1.ExpDate,
                        CreateTime = t1.CreateTime,

                        CId = t2.ID,
                        JobNumber = t2.JobNumber,
                        RealName = t2.RealName,
                        FixedNumber = t2.FixedNumber,
                        Address = t2.Address,
                        HeadPicPath = t2.HeadPicPath,
                        Remarks = t2.Remarks,
                    }).ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t1, t2, t3, t4) => new TSMUserAccountQueryAllModel
                    {
                        ID = t1.ID,
                        TelAccount = t1.TelAccount,
                        EmailAccount = t1.EmailAccount,
                        AccountName = t1.AccountName,
                        UserInfoId = t1.UserInfoId,
                        CompanyId = t1.CompanyId,
                        Status = t1.Status,
                        ExpDate = t1.ExpDate,
                        CreateTime = t1.CreateTime,

                        CId = t2.ID,
                        JobNumber = t2.JobNumber,
                        RealName = t2.RealName,
                        FixedNumber = t2.FixedNumber,
                        Address = t2.Address,
                        HeadPicPath = t2.HeadPicPath,
                        Remarks = t2.Remarks,
                    }).ToListAsync();
                }


                //返回执行结果
                return ResponseUtil<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMDeptQueryForDispatchModel, List<TSMUserAccountQueryAllModel>>.FailResult(requestObject, null, ex.Message);
            }
        }
    }
}
