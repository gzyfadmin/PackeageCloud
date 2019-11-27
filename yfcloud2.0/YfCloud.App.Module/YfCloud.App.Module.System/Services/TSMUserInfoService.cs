///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMUserInfoService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11 10:40:25
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
using YfCloud.App.Module.System.Models.TSMUserInfo;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMUserInfoService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITSMUserInfoService))]
    public class TSMUserInfoService : ITSMUserInfoService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMUserInfoService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMUserInfoService(IDbContext dbContext, ILogger<TSMUserInfoService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_SM_UserInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserInfoQueryModel, List<TSMUserInfoQueryModel>>> GetAsync(RequestObject<TSMUserInfoQueryModel> requestObject)
        {
            try
            {
                List<TSMUserInfoQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMUserInfoDbModel>();
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMUserInfoDbModel>(item.Column);
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
						(t) => new TSMUserInfoQueryModel 
						{
							Id = t.ID,
							JobNumber = t.JobNumber,
							RealName = t.RealName,
							FixedNumber = t.FixedNumber,
							Address = t.Address,
							HeadPicPath = t.HeadPicPath,
							Remarks = t.Remarks,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TSMUserInfoQueryModel 
						{
							Id = t.ID,
							JobNumber = t.JobNumber,
							RealName = t.RealName,
							FixedNumber = t.FixedNumber,
							Address = t.Address,
							HeadPicPath = t.HeadPicPath,
							Remarks = t.Remarks,
						})
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<TSMUserInfoQueryModel, List<TSMUserInfoQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserInfoQueryModel, List<TSMUserInfoQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_SM_UserInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserInfoAddModel, bool>> PostAsync(RequestObject<TSMUserInfoAddModel> requestObject)
        {
            var currDb = _db.Instance;
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                var addModel = _mapper.Map<TSMUserInfoDbModel>(requestObject.PostData);                
                var accountModel = await currDb.Queryable<TSMUserAccountDbModel>()
                    .Where(p => p.TelAccount == requestObject.PostData.Account
                    || p.EmailAccount == requestObject.PostData.Account)
                    .FirstAsync();
                if(accountModel == null)
                    return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, "账号不存在，无法保存个人信息!");
                currDb.BeginTran();
                result = await currDb.Insertable(addModel).ExecuteCommandIdentityIntoEntityAsync();
                if (result)
                {
                    accountModel.UserInfoId = addModel.ID;
                    result = await currDb.Updateable(accountModel)
                    .UpdateColumns(p => p.UserInfoId)
                    .Where(p => p.ID == accountModel.ID)
                    .ExecuteCommandAsync() > 0;
                }
                currDb.CommitTran();
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserInfoAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_SM_UserInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserInfoEditModel, bool>> PutAsync(RequestObject<TSMUserInfoEditModel> requestObject)
        {
            try
            {
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserInfoEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");

                //执行结果
                var result = false;
                var accountModel = await _db.Instance.Queryable<TSMUserAccountDbModel>()
                    .Where(p => p.TelAccount == requestObject.PostData.Account
                    || p.EmailAccount == requestObject.PostData.Account)
                    .FirstAsync();
                if (accountModel == null)
                    return ResponseUtil<TSMUserInfoEditModel, bool>.FailResult(requestObject, false, "账号不存在，无法保存个人信息!");

                //单记录更新
                var editModel = _mapper.Map<TSMUserInfoDbModel>(requestObject.PostData);
                result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserInfoEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserInfoEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserInfoEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_UserInfo数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserInfoAddModel, bool>> DeleteAsync(RequestObject<TSMUserInfoAddModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var delList = _mapper.Map<List<TSMUserInfoAddModel>, List<TSMUserInfoDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Deleteable(delList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    var delModel = _mapper.Map<TSMUserInfoDbModel>(requestObject.PostData);
                    result = await _db.Instance.Deleteable(delModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserInfoAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserInfoAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_UserInfo数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserInfo数据，通过主表主键删除数据，批量删除
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
                if (ids == null)
                {
                    ids = new int[] { requestObject.PostData.ObjToInt() };
                }
                var result = await _db.Instance.Deleteable<TSMUserInfoDbModel>().In(ids).ExecuteCommandAsync() > 0;
                
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
