///////////////////////////////////////////////////////////////////////////////////////
// File: ITDataDicMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18 11:49:34
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
using YfCloud.App.Module.Platform.Models.TDataDicMain;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITDataDicMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITDataDicMainService))]
    public class TDataDicMainService : ITDataDicMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TDataDicMainService> _log;//日志操作实例对象
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TDataDicMainService(IDbContext dbContext, ILogger<TDataDicMainService> logger)
        {
            _db = dbContext;
            _log = logger;
        }

        /// <summary>
        /// 获取T_DataDicMain数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<TDataDicMainModel, List<TDataDicMainModel>>> GetAsync(RequestObject<TDataDicMainModel> requestObject)
        {
            try
            {
                List<TDataDicMainModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TDataDicMainModel, TPMMenusDbModel, TUsersModel>(
								(t, t1, t2) => new object[] 
								{
									JoinType.Left,  t.MenuId== t1.Id, 
									JoinType.Left,  t.CreateId== t2.Id, 
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
                query.Select((t, t1, t2) => new TDataDicMainModel 
				{
					Id = t.Id,
					MenuId = t.MenuId,
					TMenusMenuName = t1.MenuName,
					Key = t.Key,
					CreateTime = t.CreateTime,
					CreateId = t.CreateId,
					TUsersUserName = t2.UserName
				});
                
                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();
                
                //查询明细数据
                if (queryData != null && queryData.Count > 0)
                {
                    var detailQuery = _db.Instance.Queryable<TDataDicDetailModel, TDataDicMainModel, TUsersModel>(
								(t, t0, t1) => new object[] 
								{
									JoinType.Left,  t.KeyId== t0.Id, 
									JoinType.Left,  t.CreateId== t1.Id, 
								});
                    detailQuery.In(t => t.KeyId, queryData.Select(p => p.Id).ToList());
                    detailQuery.Select((t, t0, t1) => new TDataDicDetailModel 
					{
						Id = t.Id,
						KeyId = t.KeyId,
						TDataDicMainKey = t0.Key,
						Value = t.Value,
						CreateTime = t.CreateTime,
						CreateId = t.CreateId
					});
                    var sql = detailQuery.ToSql();
                    var details = await detailQuery.ToListAsync();
                    queryData.ForEach(p => p.ChildList = details.Where(p1 => p1.KeyId == p.Id).ToList());
                }
                
                //返回执行结果
                return ResponseUtil<TDataDicMainModel, List<TDataDicMainModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TDataDicMainModel, List<TDataDicMainModel>>.FailResult(requestObject, null, ex.Message);
            }
        }


        /// <summary>
        /// 获取树结构数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<TDataDicMainModel , List<DataTreeNode>>> GetTreeAsync(RequestObject<TDataDicMainModel> requestObject)
        {
            try
            {
                List<TDataDicMainModel> queryData = null;//查询结果集对象

                List<DataTreeNode> result = new List<DataTreeNode>();
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TDataDicMainModel, TPMMenusDbModel, TUsersModel, TDataDicDetailModel>(
                                (t, t1, t2,t3) => new object[]
                                {
                                    JoinType.Inner,  t.MenuId== t1.Id,
                                    JoinType.Left,  t.CreateId== t2.Id,
                                    JoinType.Left, t3.KeyId==t.Id
                                });
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where($"t.{p}"));
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }

                var queryResult = await query.Select((t, t1, t2, t3) => new { main = t, deatail = t3 }).ToListAsync();

                foreach (var item in queryResult.Select(p=>p.main.Id).Distinct().OrderBy(p=>p))
                {

                    var temp = queryResult.Where(p => p.main.Id == item).Select(p=>p.main).FirstOrDefault();

                    DataTreeNode dataTreeNode = new DataTreeNode();
                    dataTreeNode.ID = temp.Id;
                    dataTreeNode.Name = temp.Key;
                    dataTreeNode.Children = new List<DataTreeNode>();
                    dataTreeNode.Type = 1;
                    dataTreeNode.ParentID = -1;
                    dataTreeNode.MenuID = temp.MenuId;

                    var childList = queryResult.Where(p => p.main.Id == temp.Id).Select(p => p.deatail);

                    foreach (var childItem in childList.Where(p=>p.Id>0))
                    {
                        DataTreeNode cdataTreeNode = new DataTreeNode();
                        cdataTreeNode.ID = childItem.Id;
                        cdataTreeNode.Name = childItem.Value;
                        cdataTreeNode.Children = new List<DataTreeNode>();
                        dataTreeNode.ParentID = temp.Id;
                        dataTreeNode.Type = 2;

                        dataTreeNode.Children.Add(cdataTreeNode);
                    }

                    result.Add(dataTreeNode);
                }

                //返回执行结果
                return ResponseUtil<TDataDicMainModel, List<DataTreeNode>>.SuccessResult(requestObject, result, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TDataDicMainModel, List<DataTreeNode>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_DataDicMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TDataDicMainModel,bool>> PostAsync(RequestObject<TDataDicMainModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mainId = await currDb.Insertable(requestObject.PostData).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                //requestObject.PostData.ChildList.ForEach(p => p.KeyId = mainId);
                //插入从表数据
                //var result = await currDb.Insertable(requestObject.PostData.ChildList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果

                return ResponseUtil<TDataDicMainModel, bool>.SuccessResult(requestObject, true);
                //return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_DataDicMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TDataDicMainModel, bool>> PutAsync(RequestObject<TDataDicMainModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {                
                if (requestObject.PostData == null)
                    return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainFlag = await currDb.Updateable(requestObject.PostData).ExecuteCommandAsync() > 0;
                //修改明细信息
                var detailFlag = await currDb.Updateable(requestObject.PostData.ChildList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                if (mainFlag && detailFlag)
                    return ResponseUtil<TDataDicMainModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_DataDicMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<TDataDicMainModel, bool>> DeleteAsync(RequestObject<TDataDicMainModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //删除主表信息
                var mainFlag = await currDb.Deleteable(requestObject.PostData).ExecuteCommandAsync() > 0;
                //删除明细信息
                var detailFlag = await currDb.Deleteable<TDataDicDetailModel>().Where(p=>p.KeyId == requestObject.PostData.Id).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                if (mainFlag && detailFlag)
                    return ResponseUtil<TDataDicMainModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TDataDicMainModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_DataDicMain数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //开启事务
                currDb.BeginTran();
                //删除明细信息
                var detailFlag = await currDb.Deleteable<TDataDicDetailModel>().Where(p=>p.KeyId == requestObject.PostData).ExecuteCommandAsync() >= 0;
                //删除主表信息
                var mainFlag = await currDb.Deleteable<TDataDicMainModel>().In(requestObject.PostData).ExecuteCommandAsync() >= 0;                
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                if (mainFlag && detailFlag)
                    return ResponseUtil<int, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<int, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<int, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_DataDicMain数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //开启事务
                currDb.BeginTran();
                //删除明细信息
                var detailFlag = await currDb.Deleteable<TDataDicDetailModel>().In(p => p.KeyId, requestObject.PostData).ExecuteCommandAsync() > 0;
                //删除主表信息
                var mainFlag = await currDb.Deleteable<TDataDicMainModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                if (mainFlag && detailFlag)
                    return ResponseUtil<int[], bool>.SuccessResult(requestObject, true);
                return ResponseUtil<int[], bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<int[], bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
    }
}
