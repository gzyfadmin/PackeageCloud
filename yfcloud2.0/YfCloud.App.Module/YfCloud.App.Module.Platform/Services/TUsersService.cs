///////////////////////////////////////////////////////////////////////////////////////
// File: ITUsersService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25 9:37:17
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
using System.Text;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TPMUser;
using YfCloud.Framework;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITUsersService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITUsersService))]
    public class TUsersService : ITUsersService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TUsersService> _log;//日志操作实例对象
        private readonly ITRolePermissionsService _rolePermissionsService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="rolePermissionsService"></param>
        public TUsersService(IDbContext dbContext, ILogger<TUsersService> logger, ITRolePermissionsService rolePermissionsService)
        {
            _db = dbContext;
            _log = logger;
            _rolePermissionsService = rolePermissionsService;
        }

        /// <summary>
        /// 获取T_Users数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TUsersModel, List<TUsersModel>>> GetAsync(RequestObject<TUsersModel> requestObject)
        {
            try
            {
                List<TUsersModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TUsersModel, TRolesModel>(
                                (t, t0) => new object[]
                                {
                                    JoinType.Left,  t.RoleId== t0.Id,
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

                //设置多表查询返回实体类
                query.Select((t, t0) => new TUsersModel
                {
                    Id = t.Id,
                    UserName = t.UserName,
                    LoginName = t.LoginName,
                    LoginPwd =t.LoginPwd,
                    Salt=t.Salt,
                    RoleId = t.RoleId,
                    TRolesRoleName = t0.RoleName,
                    Status = t.Status,
                    CreateTime = t.CreateTime,
                    CreateId = t.CreateId
                  
                });

                //执行查询
                if (requestObject.IsPaging)
                    queryData = await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await query.ToListAsync();

                queryData.ForEach(x => { x.LoginPwd = EncryptUtil.DeAESbyKey(x.LoginPwd,Encoding.UTF8, x.Salt); });

                //返回执行结果
                return ResponseUtil<TUsersModel, List<TUsersModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TUsersModel, List<TUsersModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_Users数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TUsersModel, bool>> PostAsync(RequestObject<TUsersModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 1)
                {
                    requestObject.PostDataList.ForEach(x =>
                    {

                        x.Salt = RandCodeCreate.GenerateRandomNumber(32);
                        x.LoginPwd = EncryptUtil.EnAESBykey(x.LoginPwd, Encoding.UTF8, x.Salt);

                    });

                    result = await _db.Instance.Insertable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    requestObject.PostData.Salt = RandCodeCreate.GenerateRandomNumber(32);
                    requestObject.PostData.LoginPwd = EncryptUtil.EnAESBykey(requestObject.PostData.LoginPwd, Encoding.UTF8, requestObject.PostData.Salt);

                    result = await _db.Instance.Insertable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TUsersModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TPMUserAccountAddModel, LoginResult>> LoginAsync(RequestObject<TPMUserAccountAddModel> requestObject)
        {
            LoginResult loginResult = new LoginResult() { IsSuccess = false };
            try
            {
                //当前登录账户信息
                var curr = requestObject.PostData;

                if (string.IsNullOrWhiteSpace(curr.LoginName))
                {

                    return ResponseUtil<TPMUserAccountAddModel, LoginResult>.FailResult(requestObject, loginResult, "登录失败，账户不能为空!");
                }

                //查询数据库是否有该账户
                var dbModel = await _db.Instance.Queryable<TUsersModel>()
                    .Where(p => p.LoginName == curr.LoginName&&p.Status==true)
                    .FirstAsync();
                //如果没有该账户返回错误信息
                if (dbModel == null)
                {
                    return ResponseUtil<TPMUserAccountAddModel, LoginResult>.FailResult(requestObject, loginResult, "登录失败，账户不存在!");
                }
                //验证密码
                var currPwd = EncryptUtil.DeAESbyKey(dbModel.LoginPwd, Encoding.UTF8, dbModel.Salt);
                if (string.Equals(curr.Passwd, currPwd))
                {
                    loginResult.IsSuccess = true;
 
                    //生成Token
                    Dictionary<string, object> palyloads = new Dictionary<string, object>();
                    palyloads.Add("UserID", dbModel.Id);
                    string token = TokenManager.CreateTokenByHandler(palyloads, 60);

                    loginResult.Token = token;

                    //加载权限
                    //RequestObject<TRolePermissionsModel> roleQuery = new RequestObject<TRolePermissionsModel>() { IsPaging = false, QueryConditions = new List<QueryCondition>() };
                    //roleQuery.QueryConditions.Add(new QueryCondition() { Column = "roleID", Content = dbModel.RoleId.ToString(), Condition = ConditionEnum.Equal });
                    //var result = await _rolePermissionsService.LoadMenuByRoles(roleQuery);

                    //loginResult.Permissions = result.Data;

                    //返回验证成功信息
                    return ResponseUtil<TPMUserAccountAddModel, LoginResult>.SuccessResult(requestObject, loginResult);
                }
                else
                {
                    //返回密码验证失败的错误信息
                    return ResponseUtil<TPMUserAccountAddModel, LoginResult>.FailResult(requestObject, loginResult, "登录失败，密码错误");
                }
            }
            catch (Exception ex)
            {
                //返回异常信息
                return ResponseUtil<TPMUserAccountAddModel, LoginResult>.FailResult(requestObject, loginResult, $"登录失败，发生异常,{Environment.NewLine}{ex.Message}");
            }
        }



        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public bool CheckPassword(int UserID, string Password)
        {
            var dbModel =  _db.Instance.Queryable<TUsersModel>().Where(p => p.Id == UserID).First();
            var currPwd = EncryptUtil.DeAESbyKey(dbModel.LoginPwd, Encoding.UTF8, dbModel.Salt);

            return string.Equals(Password, currPwd);
        }

        /// <summary>
        /// 修改T_Users数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TUsersModel, bool>> PutAsync(RequestObject<TUsersModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    requestObject.PostDataList.ForEach(x =>
                    {
                        x.Salt = RandCodeCreate.GenerateRandomNumber(32);
                        x.LoginPwd = EncryptUtil.EnAESBykey(x.LoginPwd,Encoding.UTF8, x.Salt);
                    });

                    //批量更新
                    result = await _db.Instance.Updateable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    requestObject.PostData.Salt = RandCodeCreate.GenerateRandomNumber(32);
                    requestObject.PostData.LoginPwd = EncryptUtil.EnAESBykey(requestObject.PostData.LoginPwd,Encoding.UTF8, requestObject.PostData.Salt);

                    //单记录更新
                    result = await _db.Instance.Updateable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TUsersModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Users数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TUsersModel, bool>> DeleteAsync(RequestObject<TUsersModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TUsersModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TUsersModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_Users数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {

            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TUsersModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;

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
        /// 删除T_Users数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TUsersModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;

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
        /// 根据用户ID获取账户信息
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int, UserInfo>> GetInfoAsync(int iUserId)
        {
            try
            {
                var userModel = await _db.Instance.Queryable<TUsersModel, TRolesModel>
                    (
                        (t0, t1) => new object[]{ JoinType.Left, t0.RoleId == t1.Id }
                    )
                    .Where((t0,t1) => t0.Id == iUserId)
                    .Select((t0, t1) => new UserInfo
                    {
                        Roles = t1.RoleName,
                        Name = t0.UserName,
                        RoleId = t0.RoleId
                    })                    
                    .FirstAsync();
                if (userModel == null)
                    return ResponseUtil<int, UserInfo>.FailResult(new RequestObject<int>(), null, "未找到该用户信息");

                RequestObject<TRolePermissionsModel> roleQuery = new RequestObject<TRolePermissionsModel>() { IsPaging = false, QueryConditions = new List<QueryCondition>() };
                roleQuery.QueryConditions.Add(new QueryCondition() { Column = "roleID", Content = userModel.RoleId.ToString(), Condition = ConditionEnum.Equal });
                var result = await _rolePermissionsService.LoadMenuByRoles(roleQuery);
                userModel.Permissions = result.Data;

                return ResponseUtil<int, UserInfo>.SuccessResult(new RequestObject<int>(), userModel);

            }
            catch(Exception ex)
            {
                return ResponseUtil<int, UserInfo>.FailResult(new RequestObject<int>(), null, $"获取权限发生异常{Environment.NewLine} {ex.Message}");
            }
        }
    }
}
