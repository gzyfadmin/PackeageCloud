///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMUserAccountService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:49:11
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
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.System.Models.TSMUserInfo;
using YfCloud.DBModel;
using YfCloud.Framework;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMUserAccountService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITSMUserAccountService))]
    public class TSMUserAccountService : ITSMUserAccountService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMUserAccountService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMUserAccountService(IDbContext dbContext, ILogger<TSMUserAccountService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_SM_UserAccount数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountQueryModel, List<TSMUserAccountQueryModel>>> GetAsync(RequestObject<TSMUserAccountQueryModel> requestObject)
        {
            try
            {
                List<TSMUserAccountQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMUserAccountDbModel>();
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }

                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select(
						(t) => new TSMUserAccountQueryModel 
						{
							Id = t.ID,
							TelAccount = t.TelAccount,
							EmailAccount = t.EmailAccount,
							Passwd = t.Passwd,
							AccountName = t.AccountName,
							UserInfoId = t.UserInfoId,
							CompanyId = t.CompanyId,
							Status = t.Status,
							ExpDate = t.ExpDate,
							CreateTime = t.CreateTime,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TSMUserAccountQueryModel 
						{
							Id = t.ID,
							TelAccount = t.TelAccount,
							EmailAccount = t.EmailAccount,
							Passwd = t.Passwd,
							AccountName = t.AccountName,
							UserInfoId = t.UserInfoId,
							CompanyId = t.CompanyId,
							Status = t.Status,
							ExpDate = t.ExpDate,
							CreateTime = t.CreateTime,
						})
                        .ToListAsync();
                }

                //查询用户详情信息
                var userIds = queryData.Where(p=>p.UserInfoId != null).Select(p => p.UserInfoId).ToArray();
                var userInfoList = _db.Instance.Queryable<TSMUserInfoDbModel>()
                    .In(userIds)
                    .Select(p=> new TSMUserInfoQueryModel
                            {
                                Id = p.ID,
                                Address = p.Address,
                                FixedNumber = p.FixedNumber,
                                HeadPicPath = p.HeadPicPath,
                                JobNumber = p.JobNumber,
                                RealName = p.RealName,
                                Remarks = p.Remarks
                            })
                    .ToList();
                queryData.ForEach(p => p.UserInfoDetail = userInfoList.Where(p1 => p1.Id == p.UserInfoId).FirstOrDefault());

                //返回执行结果
                return ResponseUtil<TSMUserAccountQueryModel, List<TSMUserAccountQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserAccountQueryModel, List<TSMUserAccountQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }


      

        /// <summary>
        /// 新增T_SM_UserAccount数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<string>> PostAsync(RequestPost<TSMUserAccountAddModel> requestObject)
        {
            var currDb = _db.Instance;
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<string>.FailResult(null,  "PostData,PostDataList不能都为null");
                var result = false;
                string token = string.Empty;
                currDb.BeginTran();
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                }
                else
                {
                    //注册账号信息
                    var addModel = _mapper.Map<TSMUserAccountDbModel>(requestObject.PostData);
                    addModel.Salt = RandCodeCreate.GenerateRandomNumber(32);
                    addModel.Passwd = EncryptUtil.EnAESBykey(addModel.Passwd, addModel.Salt);
                    addModel.Status = 1;
                    if (string.IsNullOrWhiteSpace(addModel.EmailAccount))
                    {
                        addModel.EmailAccount = null;
                    }

                    if (string.IsNullOrWhiteSpace(addModel.TelAccount))
                    {
                        addModel.TelAccount = null;
                    }
                    //生成用户详情记录
                    TSMUserInfoDbModel tSMUserInfoDbModel = new TSMUserInfoDbModel();
                    int cid = await _db.Instance.Insertable(tSMUserInfoDbModel).ExecuteReturnIdentityAsync();
                    addModel.UserInfoId = cid;

                    int id = _db.Instance.Insertable(addModel).ExecuteReturnIdentity();

                    var palyloads = new Dictionary<string, object>
                    {
                        { "UserID", id },
                        { "ID",Guid.NewGuid().ToString()},
                        { "CompanyID",0},
                        { "UserName",addModel.AccountName}

                    };
                    token = TokenManager.CreateTokenByHandler(palyloads, 60 * 24);

                    currDb.CommitTran();
                }
                //返回执行结果
                return ResponseUtil<string>.SuccessResult(token);
            }
            catch(Exception ex)
            {
                //返回异常结果
                currDb.RollbackTran();
                return ResponseUtil<string>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 验证手机邮箱是否存在
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountInputModel, bool>> CheckIsExistsAsync(RequestObject<TSMUserAccountInputModel> requestObject)
        {
            bool result = false;

            try
            {
               
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountInputModel, bool>.FailResult(requestObject, result, "PostData不能都为null");

                string keyWords = requestObject.PostData.KeyWords;

                var query = _db.Instance.Queryable<TSMUserAccountDbModel>().AnyAsync(p => p.EmailAccount == keyWords || p.TelAccount == keyWords);
                result = await query;

                return ResponseUtil<TSMUserAccountInputModel, bool>.SuccessResult(requestObject, result);
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserAccountInputModel, bool>.FailResult(requestObject, result, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_SM_UserAccount数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountEditModel, bool>> PutAsync(RequestObject<TSMUserAccountEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    if (requestObject.PostDataList.Any(p => string.IsNullOrWhiteSpace(p.TelAccount)&& string.IsNullOrWhiteSpace(p.EmailAccount)))
                    {
                        return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "邮箱和手机号必须填写一个");
                    }

                    //批量更新
                    var editList = _mapper.Map<List<TSMUserAccountEditModel>, List<TSMUserAccountDbModel>>(requestObject.PostDataList);
                    List<TSMUserAccountDbModel> editDBLiat = new List<TSMUserAccountDbModel>();

                    editList.ForEach(x => {
                        TSMUserAccountDbModel tempDB = null;

                        if (!string.IsNullOrWhiteSpace(x.TelAccount))
                        {
                             tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == x.TelAccount).First();
                          
                        }
                        else {
                            if (!string.IsNullOrWhiteSpace(x.EmailAccount))
                            {
                                tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == x.EmailAccount).First();
                            }
                        }

                        if (tempDB != null)
                        {
                            tempDB.Salt = RandCodeCreate.GenerateRandomNumber(32);
                            tempDB.Passwd = EncryptUtil.EnAESBykey(x.Passwd, tempDB.Salt);

                            if (string.IsNullOrWhiteSpace(tempDB.EmailAccount))
                            {
                                tempDB.EmailAccount = null;
                            }

                            if (string.IsNullOrWhiteSpace(tempDB.TelAccount))
                            {
                                tempDB.TelAccount = null;
                            }

                            editDBLiat.Add(tempDB);
                        }


                    });

                    if (editDBLiat.Count() > 0)
                    {
                        result = await _db.Instance.Updateable(editDBLiat).UpdateColumns(p=> new {p.Salt,p.Passwd }).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "用户不存在，不能修改");
                    }
                    
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(requestObject.PostData.EmailAccount) && string.IsNullOrWhiteSpace(requestObject.PostData.TelAccount))
                    {
                        return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "邮箱和手机号必须填写一个");
                    }

                    //单记录更新
                    var editModel = _mapper.Map<TSMUserAccountDbModel>(requestObject.PostData);

                    TSMUserAccountDbModel tempDB = null;
                    if (!string.IsNullOrWhiteSpace(editModel.TelAccount))
                    {
                        tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == editModel.TelAccount).First();

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(editModel.EmailAccount))
                        {
                            tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == editModel.EmailAccount).First();
                        }
                    }

                    if (tempDB != null)
                    {
                        tempDB.Salt = RandCodeCreate.GenerateRandomNumber(32);
                        tempDB.Passwd = EncryptUtil.EnAESBykey(editModel.Passwd, tempDB.Salt);

                        if (string.IsNullOrWhiteSpace(tempDB.EmailAccount))
                        {
                            tempDB.EmailAccount = null;
                        }

                        if (string.IsNullOrWhiteSpace(tempDB.TelAccount))
                        {
                            tempDB.TelAccount = null;
                        }

                        result = await _db.Instance.Updateable(tempDB).UpdateColumns(p => new { p.Salt, p.Passwd }).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "用户不存在，不能修改");
                    }

                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserAccountEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserAccountEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountPassWordModel, bool>> ModifyPassWd(RequestObject<TSMUserAccountPassWordModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    if (requestObject.PostDataList.Any(p => string.IsNullOrWhiteSpace(p.TelAccount) && string.IsNullOrWhiteSpace(p.EmailAccount)))
                    {
                        return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "邮箱和手机号必须填写一个");
                    }

                    List<TSMUserAccountDbModel> editDBLiat = new List<TSMUserAccountDbModel>();

                    foreach (var item in requestObject.PostDataList)
                    {
                        TSMUserAccountDbModel tempDB = null;

                        if (!string.IsNullOrWhiteSpace(item.TelAccount))
                        {
                            tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == item.TelAccount).First();

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(item.EmailAccount))
                            {
                                tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == item.EmailAccount).First();
                            }
                        }

                        if (tempDB != null)
                        {
                            if (EncryptUtil.DeAESbyKey(tempDB.Passwd, tempDB.Salt) != item.OldPasswd)
                            {
                                return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "原密码错误");
                            }

                            tempDB.Salt = RandCodeCreate.GenerateRandomNumber(32);
                            tempDB.Passwd = EncryptUtil.EnAESBykey(item.Passwd, tempDB.Salt);

                            if (string.IsNullOrWhiteSpace(tempDB.EmailAccount))
                            {
                                tempDB.EmailAccount = null;
                            }

                            if (string.IsNullOrWhiteSpace(tempDB.TelAccount))
                            {
                                tempDB.TelAccount = null;
                            }

                            editDBLiat.Add(tempDB);
                        }
                    }

                    if (editDBLiat.Count() > 0)
                    {
                        result = await _db.Instance.Updateable(editDBLiat).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "用户不存在，不能修改");
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(requestObject.PostData.EmailAccount) && string.IsNullOrWhiteSpace(requestObject.PostData.TelAccount))
                    {
                        return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "邮箱和手机号必须填写一个");
                    }

                    TSMUserAccountDbModel tempDB = null;
                    if (!string.IsNullOrWhiteSpace(requestObject.PostData.TelAccount))
                    {
                        tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == requestObject.PostData.TelAccount).First();

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(requestObject.PostData.EmailAccount))
                        {
                            tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == requestObject.PostData.EmailAccount).First();
                        }
                    }

                    if (tempDB != null)
                    {
                        if (EncryptUtil.DeAESbyKey(tempDB.Passwd,tempDB.Salt) != requestObject.PostData.OldPasswd)
                        {
                            return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "原密码错误");
                        }

                        tempDB.Salt = RandCodeCreate.GenerateRandomNumber(32);
                        tempDB.Passwd = EncryptUtil.EnAESBykey(requestObject.PostData.Passwd, tempDB.Salt);

                        if (string.IsNullOrWhiteSpace(tempDB.EmailAccount))
                        {
                            tempDB.EmailAccount = null;
                        }

                        if (string.IsNullOrWhiteSpace(tempDB.TelAccount))
                        {
                            tempDB.TelAccount = null;
                        }



                        result = await _db.Instance.Updateable(tempDB).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "用户不存在，不能修改");
                    }

                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserAccountPassWordModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserAccountPassWordModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserResetPassWordModel, bool>> ResetPassWd(RequestObject<TSMUserResetPassWordModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserResetPassWordModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(requestObject.PostData.EmailAccount) && string.IsNullOrWhiteSpace(requestObject.PostData.TelAccount))
                    {
                        return ResponseUtil<TSMUserResetPassWordModel, bool>.FailResult(requestObject, false, "邮箱和手机号必须填写一个");
                    }

                    TSMUserAccountDbModel tempDB = null;
                    if (!string.IsNullOrWhiteSpace(requestObject.PostData.TelAccount))
                    {
                        tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == requestObject.PostData.TelAccount).First();

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(requestObject.PostData.EmailAccount))
                        {
                            tempDB = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == requestObject.PostData.EmailAccount).First();
                        }
                    }

                    if (tempDB != null)
                    {
                     
                        tempDB.Salt = RandCodeCreate.GenerateRandomNumber(32);
                        tempDB.Passwd = EncryptUtil.EnAESBykey(requestObject.PostData.Passwd, tempDB.Salt);

                        if (string.IsNullOrWhiteSpace(tempDB.EmailAccount))
                        {
                            tempDB.EmailAccount = null;
                        }

                        if (string.IsNullOrWhiteSpace(tempDB.TelAccount))
                        {
                            tempDB.TelAccount = null;
                        }



                        result = await _db.Instance.Updateable(tempDB).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        return ResponseUtil<TSMUserResetPassWordModel, bool>.FailResult(requestObject, false, "用户不存在，不能修改");
                    }

                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserResetPassWordModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserResetPassWordModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserResetPassWordModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 删除T_SM_UserAccount数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountAddModel, bool>> DeleteAsync(RequestObject<TSMUserAccountAddModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var delList = _mapper.Map<List<TSMUserAccountAddModel>, List<TSMUserAccountDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Deleteable(delList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    var delModel = _mapper.Map<TSMUserAccountDbModel>(requestObject.PostData);
                    result = await _db.Instance.Deleteable(delModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMUserAccountAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMUserAccountAddModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMUserAccountAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_UserAccount数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_UserAccount数据，通过主表主键删除数据，批量删除
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
                var result = await _db.Instance.Deleteable<TSMUserAccountDbModel>().In(ids).ExecuteCommandAsync() > 0;
                
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
