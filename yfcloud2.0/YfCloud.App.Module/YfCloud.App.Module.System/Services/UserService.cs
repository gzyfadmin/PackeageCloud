using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.DBModel.System;
using YfCloud.Models;
using YfCloud.Utilities.AutoMapper;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Caches;

namespace YfCloud.App.Module.System.Services
{
    /// <summary>
    /// 用户维护
    /// </summary>
    /// <summary>
    /// ITSMRolesService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IDbContext _db;

        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="dbContext"></param>
        public UserService(IDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// 删除单条
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return await DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            return await DeleteAsync<int[]>(requestObject);
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TUserDeatail, List<TUserDeatail>>> GetUserInCurentCompany(RequestObject<TUserDeatail> requestObject, int UserID)
        {
            try
            {
                List<TUserDeatail> queryData = null;//查询结果集对象
                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                               (t, t1) => new object[]
                               {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                               }).Where((t, t1) => t.ID == UserID).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();

                if (user.CompanyId == null)
                {
                    return ResponseUtil<TUserDeatail, List<TUserDeatail>>.FailResult(requestObject, null, "当前用户没有公司");
                }

                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMUserAccountDbModel, TSMUserInfoDbModel, TSMRoleUserRelationDbModel, TSMRolesDbModel, TSMDeptUserRelationDbModel, TSMDeptDbModel>(
                              (t, t1, t2, t2_1, t3, t3_1) => new object[]
                              {
                                    JoinType.Left,  t.UserInfoId== t1.ID,
                                    JoinType.Left,t.ID==t2.UserId,
                                    JoinType.Left,t2.RoleId==t2_1.Id,
                                    JoinType.Left,t.ID==t3.UserAccountId,
                                    JoinType.Left,t3.DeptId==t3_1.Id
                              }).Where(t => t.CompanyId == user.CompanyId);

                string[] Mquery = { "id" };
                string[] cQuery = { "jobnumber", "realname", "fixednumber", "address", "headpicpath", "remarks", "account" };
                string[] cc = { "deptname" };


                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {

                    var QueryConditions1 = requestObject.QueryConditions.Where(p => !cQuery.Contains(p.Column.ToLower()) && !cc.Contains(p.Column.ToLower())).ToList();

                    var conditionals1 = SqlSugarUtil.GetConditionalModels(QueryConditions1);

                    foreach (ConditionalModel item in conditionals1)
                    {
                        item.FieldName = $"t.{item.FieldName}";
                    }
                    query.Where(conditionals1);


                    var QueryConditions2 = requestObject.QueryConditions.Where(p => cQuery.Contains(p.Column.ToLower())).ToList();
                    var conditionals2 = SqlSugarUtil.GetConditionalModels(QueryConditions2);
                    foreach (ConditionalModel item in conditionals1)
                    {
                        item.FieldName = $"t1.{item.FieldName}";
                    }
                    query.Where(conditionals2);

                    var QueryConditions3 = requestObject.QueryConditions.Where(p => cc.Contains(p.Column.ToLower())).ToList();
                    var conditionals3 = SqlSugarUtil.GetConditionalModels(QueryConditions3);
                    foreach (ConditionalModel item in conditionals3)
                    {
                        item.FieldName = $"t3_1.{item.FieldName}";
                    }
                    query.Where(conditionals3);

                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions.Where(p => !cQuery.Contains(p.Column.ToLower())))
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMUserInfoDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t {item.Column} {item.Condition}");
                    }

                    foreach (var item in requestObject.OrderByConditions.Where(p => cQuery.Contains(p.Column.ToLower())))
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMUserInfoDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t1 {item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select(
                        (t, t1, t2, t2_1, t3, t3_1) => new TUserDeatail
                        {
                            ID = t.ID,
                            DepartName = t3_1.DeptName,
                            JobNumber = t1.JobNumber,
                            RealName = t.AccountName,
                            FixedNumber = t1.FixedNumber,
                            Address = t1.Address,
                            HeadPicPath = t1.HeadPicPath,
                            Sex = t1.Sex,
                            EntryTime = t1.EntryTime,
                            WorkNo = t1.WorkNo,
                            WorkStatus = t1.WorkStatus,
                            IDCard = t1.IDCard,
                            Birthday = t1.Birthday,
                            Education = t1.Education,
                            Nation = t1.Nation,
                            Marriage = t1.Marriage,
                            RegisteredType = t1.RegisteredType,
                            HomeAddress = t1.HomeAddress,
                            EmergencyContact = t1.EmergencyContact,
                            EmergencyContactaPhone = t1.EmergencyContactaPhone,
                            EmergencyRealtionShip = t1.EmergencyRealtionShip,
                            Remarks = t1.Remarks,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                       (t, t1, t2, t2_1, t3, t3_1) => new TUserDeatail
                       {
                           ID = t.ID,
                           JobNumber = t1.JobNumber,
                           RealName = t.AccountName,
                           FixedNumber = t1.FixedNumber,
                           Address = t1.Address,
                           HeadPicPath = t1.HeadPicPath,
                           Sex = t1.Sex,
                           EntryTime = t1.EntryTime,
                           WorkNo = t1.WorkNo,
                           WorkStatus = t1.WorkStatus,
                           IDCard = t1.IDCard,
                           Birthday = t1.Birthday,
                           Education = t1.Education,
                           Nation = t1.Nation,
                           Marriage = t1.Marriage,
                           RegisteredType = t1.RegisteredType,
                           HomeAddress = t1.HomeAddress,
                           EmergencyContact = t1.EmergencyContact,
                           EmergencyContactaPhone = t1.EmergencyContactaPhone,
                           EmergencyRealtionShip = t1.EmergencyRealtionShip,
                           Remarks = t1.Remarks,
                       })
                        .ToListAsync();
                }


                //返回执行结果
                return ResponseUtil<TUserDeatail, List<TUserDeatail>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TUserDeatail, List<TUserDeatail>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> GetAsync(RequestObject<TSMUserAccountQueryAllModel> requestObject, int UserID)
        {
            try
            {
                List<TSMUserAccountQueryAllModel> queryData = null;//查询结果集对象
                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                               (t, t1) => new object[]
                               {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                               }).Where((t, t1) => t.ID == UserID).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();

                if (user.CompanyId == null)
                {
                    return ResponseUtil<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>.FailResult(requestObject, null, "当前用户没有公司");
                }

                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMUserAccountDbModel, TSMUserInfoDbModel, TSMRoleUserRelationDbModel, TSMRolesDbModel, TSMDeptUserRelationDbModel, TSMDeptDbModel>(
                              (t, t1, t2, t2_1, t3, t3_1) => new object[]
                              {
                                    JoinType.Left,  t.UserInfoId== t1.ID,
                                    JoinType.Left,t.ID==t2.UserId,
                                    JoinType.Left,t2.RoleId==t2_1.Id,
                                    JoinType.Left,t.ID==t3.UserAccountId,
                                    JoinType.Left,t3.DeptId==t3_1.Id
                              }).Where(t => t.CompanyId == user.CompanyId);


                string[] cQuery = { "jobnumber", "realname", "fixednumber", "address", "headpicpath", "remarks", "account" };



                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var itemCondition = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "keywords").FirstOrDefault();

                    if (itemCondition != null && !string.IsNullOrEmpty(itemCondition.Content))
                    {
                        query = query.Where((t, t1) => t.TelAccount.Contains(itemCondition.Content) || t.EmailAccount.Contains(itemCondition.Content));
                    }

                    requestObject.QueryConditions.Remove(itemCondition);

                    var QueryConditions1 = requestObject.QueryConditions.Where(p => !cQuery.Contains(p.Column.ToLower())).ToList();

                    var conditionals1 = SqlSugarUtil.GetConditionalModels(QueryConditions1);

                    foreach (ConditionalModel item in conditionals1)
                    {
                        item.FieldName = $"t.{item.FieldName}";
                    }
                    query.Where(conditionals1);


                    var QueryConditions2 = requestObject.QueryConditions.Where(p => cQuery.Contains(p.Column.ToLower())).ToList();
                    var conditionals2 = SqlSugarUtil.GetConditionalModels(QueryConditions2);
                    query.Where(conditionals2);

                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions.Where(p => !cQuery.Contains(p.Column.ToLower())))
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMUserInfoDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t {item.Column} {item.Condition}");
                    }

                    foreach (var item in requestObject.OrderByConditions.Where(p => cQuery.Contains(p.Column.ToLower())))
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TSMUserInfoDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"t1 {item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select(
                        (t, t1, t2, t2_1, t3, t3_1) => new TSMUserAccountQueryAllModel
                        {
                            ID = t.ID,
                            TelAccount = t.TelAccount,
                            EmailAccount = t.EmailAccount,
                            Passwd = t.Passwd,
                            Salt = t.Salt,
                            AccountName = t.AccountName,
                            UserInfoId = t.UserInfoId,
                            CompanyId = t.CompanyId,
                            Status = t.Status,
                            ExpDate = t.ExpDate,
                            CreateTime = t.CreateTime,

                            RoleId = t2.RoleId,
                            RoleName = t2_1.RoleName,
                            DeptId = t3.DeptId,
                            DeptName = t3_1.DeptName,
                            CId = t1.ID,
                            JobNumber = t1.JobNumber,
                            RealName = t1.RealName,
                            FixedNumber = t1.FixedNumber,
                            Address = t1.Address,
                            HeadPicPath = t1.HeadPicPath,
                            Sex = t1.Sex,
                            EntryTime = t1.EntryTime,
                            WorkNo = t1.WorkNo,
                            WorkStatus = t1.WorkStatus,
                            IDCard = t1.IDCard,
                            Birthday = t1.Birthday,
                            Education = t1.Education,
                            Nation = t1.Nation,
                            Marriage = t1.Marriage,
                            RegisteredType = t1.RegisteredType,
                            HomeAddress = t1.HomeAddress,
                            EmergencyContact = t1.EmergencyContact,
                            EmergencyContactaPhone = t1.EmergencyContactaPhone,
                            EmergencyRealtionShip = t1.EmergencyRealtionShip,
                            Remarks = t1.Remarks,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                       (t, t1, t2, t2_1, t3, t3_1) => new TSMUserAccountQueryAllModel
                       {
                           ID = t.ID,
                           TelAccount = t.TelAccount,
                           EmailAccount = t.EmailAccount,
                           Passwd = t.Passwd,
                           Salt = t.Salt,
                           AccountName = t.AccountName,
                           UserInfoId = t.UserInfoId,
                           CompanyId = t.CompanyId,
                           Status = t.Status,
                           ExpDate = t.ExpDate,
                           CreateTime = t.CreateTime,

                           RoleId = t2.RoleId,
                           RoleName = t2_1.RoleName,
                           DeptId = t3.DeptId,
                           DeptName = t3_1.DeptName,
                           CId = t1.ID,
                           JobNumber = t1.JobNumber,
                           RealName = t1.RealName,
                           FixedNumber = t1.FixedNumber,
                           Address = t1.Address,
                           HeadPicPath = t1.HeadPicPath,
                           Sex = t1.Sex,
                           EntryTime = t1.EntryTime,
                           WorkNo = t1.WorkNo,
                           WorkStatus = t1.WorkStatus,
                           IDCard = t1.IDCard,
                           Birthday = t1.Birthday,
                           Education = t1.Education,
                           Nation = t1.Nation,
                           Marriage = t1.Marriage,
                           RegisteredType = t1.RegisteredType,
                           HomeAddress = t1.HomeAddress,
                           EmergencyContact = t1.EmergencyContact,
                           EmergencyContactaPhone = t1.EmergencyContactaPhone,
                           EmergencyRealtionShip = t1.EmergencyRealtionShip,
                           Remarks = t1.Remarks,
                       })
                        .ToListAsync();
                }

                queryData.ForEach((x =>
                {

                    if (x.Birthday != null)
                    {
                        x.Age = DateTime.Now.Year - x.Birthday.Value.Year;
                    }
                    if (x.EntryTime != null)
                    {
                        x.EntryAge = DateTime.Now.Year - x.EntryTime.Value.Year;
                    }

                    x.Passwd = EncryptUtil.DeAESbyKey(x.Passwd, x.Salt);
                    x.Salt = null;
                }));

                //返回执行结果
                return ResponseUtil<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> GetCurentAsync(RequestObject<TSMUserAccountQueryAllModel> requestObject, int UserID)
        {
            try
            {

                if (requestObject.QueryConditions == null)
                {
                    requestObject.QueryConditions = new List<QueryCondition>();
                }
                var item = new QueryCondition() { Column = "ID", Condition = ConditionEnum.Equal, Content = UserID.ToString() };

                requestObject.QueryConditions.Add(item);

                return await GetAsync(requestObject, UserID);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 个人设置修改密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountPassWord, bool>> ModifyPassWordAsync(RequestObject<TSMUserAccountPassWord> requestObject, int UserID)
        {
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountPassWord, bool>.FailResult(requestObject, false, "PostData不能为null");

                var entity = requestObject.PostData;

                var enditModel = await _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == UserID).FirstAsync();



                string Passwd = EncryptUtil.DeAESbyKey(enditModel.Passwd, enditModel.Salt);

                if (!string.Equals(Passwd, entity.OldPasswd))
                {
                    return ResponseUtil<TSMUserAccountPassWord, bool>.FailResult(requestObject, false, "原密码不正确");
                }
                else
                {
                    enditModel.Salt = RandCodeCreate.GenerateRandomNumber(32);
                    enditModel.Passwd = EncryptUtil.EnAESBykey(entity.Passwd, enditModel.Salt);
                }

                _db.Instance.Updateable<TSMUserAccountDbModel>(enditModel).UpdateColumns(p => new { p.Salt, p.Passwd }).ExecuteCommand();

                return ResponseUtil<TSMUserAccountPassWord, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserAccountPassWord, bool>.FailResult(requestObject, false, ex.Message);
            }

        }

        /// <summary>
        /// 个人设置手机
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountMobile, int>> ModifyMobileAsync(RequestObject<TSMUserAccountMobile> requestObject, int UserID)
        {
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountMobile, int>.FailResult(requestObject, -1, "PostData不能为null");

                var entity = requestObject.PostData;

                var enditModel = await _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == UserID).FirstAsync();
                enditModel.TelAccount = entity.Mobile;

                var redis = CacheFactory.Instance(CacheType.Redis);
                string redisKey = string.Format(CacheKeyString.ChangeMobileMsgCode, enditModel.TelAccount);
                string RedisCode = redis.GetValueByKey<string>(redisKey);
                if (RedisCode == "")
                {
                    return ResponseUtil<TSMUserAccountMobile, int>.SuccessResult(requestObject, 0);
                }
                else if (RedisCode != entity.Code)
                {
                    return ResponseUtil<TSMUserAccountMobile, int>.SuccessResult(requestObject, 2);
                }
                bool isExists = _db.Instance.Queryable<TSMUserAccountDbModel>().Any(p => p.TelAccount == enditModel.TelAccount);
                if (isExists)
                {
                    return ResponseUtil<TSMUserAccountMobile, int>.SuccessResult(requestObject, 3);
                }
                _db.Instance.Updateable<TSMUserAccountDbModel>(enditModel).UpdateColumns(p => new { p.TelAccount }).ExecuteCommand();
                return ResponseUtil<TSMUserAccountMobile, int>.SuccessResult(requestObject, 1);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserAccountMobile, int>.FailResult(requestObject, -1, ex.Message);
            }

        }

        /// <summary>
        /// 个人设置邮箱
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountEmail, bool>> ModifyEmailAsync(RequestObject<TSMUserAccountEmail> requestObject, int UserID)
        {
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountEmail, bool>.FailResult(requestObject, false, "PostData不能为null");

                var entity = requestObject.PostData;

                var enditModel = await _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == UserID).FirstAsync();
                enditModel.EmailAccount = entity.Email;

                _db.Instance.Updateable<TSMUserAccountDbModel>(enditModel).UpdateColumns(p => new { p.EmailAccount }).ExecuteCommand();

                return ResponseUtil<TSMUserAccountEmail, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMUserAccountEmail, bool>.FailResult(requestObject, false, ex.Message);
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountAddAllModel, bool>> PostAsync(RequestObject<TSMUserAccountAddAllModel> requestObject, int UserID)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountAddAllModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();

                var user = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>(
                                (t, t1) => new object[]
                                {
                                    JoinType.Left,  t.CompanyId== t1.ID,
                                }).Where((t, t1) => t.ID == UserID).Select((t, t1) => new { CompanyId = t.CompanyId, Name = t1.CompanyName }).First();


                TSMUserAccountDbModel dbMain = ExpressionGenericMapper<TSMUserAccountAddAllModel, TSMUserAccountDbModel>.Trans(requestObject.PostData);
                //处理密码
                dbMain.Salt = RandCodeCreate.GenerateRandomNumber(32);
                dbMain.Passwd = EncryptUtil.EnAESBykey(dbMain.Passwd, dbMain.Salt);
                dbMain.CreateTime = DateTime.Now;
                dbMain.CompanyId = user.CompanyId.Value;

                if (string.IsNullOrWhiteSpace(dbMain.EmailAccount))
                {
                    dbMain.EmailAccount = null;
                }

                if (string.IsNullOrWhiteSpace(dbMain.TelAccount))
                {
                    dbMain.TelAccount = null;
                }

                TSMUserInfoDbModel dbDeatail = ExpressionGenericMapper<TSMUserAccountAddAllModel, TSMUserInfoDbModel>.Trans(requestObject.PostData);
                //插入主表数据
                var cId = await currDb.Insertable(dbDeatail).ExecuteReturnIdentityAsync();

                dbMain.UserInfoId = cId;
                //插入从表数据
                var mId = await currDb.Insertable(dbMain).ExecuteReturnIdentityAsync();
                if (requestObject.PostData.RoleId != null && requestObject.PostData.RoleId > 0)
                {
                    TSMRoleUserRelationDbModel itemEntity = new TSMRoleUserRelationDbModel();
                    itemEntity.RoleId = requestObject.PostData.RoleId.Value;
                    itemEntity.UserId = mId;

                    currDb.Insertable<TSMRoleUserRelationDbModel>(itemEntity).ExecuteCommand();
                }

                if (requestObject.PostData.DeptId != null && requestObject.PostData.DeptId > 0)
                {
                    TSMDeptUserRelationDbModel itemEntity = new TSMDeptUserRelationDbModel();
                    itemEntity.DeptId = requestObject.PostData.DeptId.Value;
                    itemEntity.UserAccountId = mId;
                    currDb.Insertable<TSMDeptUserRelationDbModel>(itemEntity).ExecuteCommand();
                }




                //提交事务
                currDb.CommitTran();
                //返回执行结果

                return ResponseUtil<TSMUserAccountAddAllModel, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMUserAccountAddAllModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountEditAllModel, bool>> PutAsync(RequestObject<TSMUserAccountEditAllModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TSMUserAccountEditAllModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                //开启事务
                currDb.BeginTran();

                TSMUserAccountDbModel dbMain = ExpressionGenericMapper<TSMUserAccountEditAllModel, TSMUserAccountDbModel>.Trans(requestObject.PostData);
                dbMain.Salt = RandCodeCreate.GenerateRandomNumber(32);
                dbMain.Passwd = EncryptUtil.EnAESBykey(dbMain.Passwd, dbMain.Salt);

                if (string.IsNullOrWhiteSpace(dbMain.EmailAccount))
                {
                    dbMain.EmailAccount = null;
                }

                if (string.IsNullOrWhiteSpace(dbMain.TelAccount))
                {
                    dbMain.TelAccount = null;
                }


                TSMUserInfoDbModel dbDeatail = ExpressionGenericMapper<TSMUserAccountEditAllModel, TSMUserInfoDbModel>.Trans(requestObject.PostData);
                dbDeatail.ID = requestObject.PostData.CId;

                await currDb.Updateable(dbMain).IgnoreColumns(p => new { p.CreateTime, p.CompanyId, p.UserInfoId }).ExecuteCommandAsync();


                await currDb.Updateable(dbDeatail).ExecuteCommandAsync();


                currDb.Deleteable<TSMRoleUserRelationDbModel>().Where(p => p.UserId == dbMain.ID).ExecuteCommand();
                if (requestObject.PostData.RoleId != null && requestObject.PostData.RoleId > 0)
                {
                    TSMRoleUserRelationDbModel itemEntity = new TSMRoleUserRelationDbModel();
                    itemEntity.RoleId = requestObject.PostData.RoleId.Value;
                    itemEntity.UserId = dbMain.ID;

                    currDb.Insertable<TSMRoleUserRelationDbModel>(itemEntity).ExecuteCommand();
                }

                currDb.Deleteable<TSMDeptUserRelationDbModel>().Where(p => p.UserAccountId == dbMain.ID).ExecuteCommand();
                if (requestObject.PostData.DeptId != null && requestObject.PostData.DeptId > 0)
                {
                    TSMDeptUserRelationDbModel itemEntity = new TSMDeptUserRelationDbModel();
                    itemEntity.DeptId = requestObject.PostData.DeptId.Value;
                    itemEntity.UserAccountId = dbMain.ID;

                    currDb.Insertable<TSMDeptUserRelationDbModel>(itemEntity).ExecuteCommand();
                }


                //提交事务
                currDb.CommitTran();
                //返回执行结果

                return ResponseUtil<TSMUserAccountEditAllModel, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TSMUserAccountEditAllModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        private async Task<ResponseObject<T, bool>> DeleteAsync<T>(RequestObject<T> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                currDb.BeginTran();
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


                var cides = _db.Instance.Queryable<TSMUserAccountDbModel>().In(ids).Select(p => p.UserInfoId).Distinct().ToArray();
                await _db.Instance.Deleteable<TSMUserAccountDbModel>().In(ids).ExecuteCommandAsync();
                await _db.Instance.Deleteable<TSMUserInfoDbModel>().In(cides).ExecuteCommandAsync();

                currDb.CommitTran();
                //返回执行结果

                return ResponseUtil<T, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<T, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        private async Task<ResponseObject<TSMUserAccountEditAllModel, bool>> checkData(RequestObject<TSMUserAccountEditAllModel> requestObject, int UserID)
        {
            //没有新增数据，返回错误信息
            if (requestObject.PostData == null)
                return ResponseUtil<TSMUserAccountEditAllModel, bool>.FailResult(requestObject, false, "PostData不能为null");

            if (UserID != requestObject.PostData.ID)
            {
                return ResponseUtil<TSMUserAccountEditAllModel, bool>.FailResult(requestObject, false, "只能修改自己的信息");
            }

            bool isExists = await _db.Instance.Queryable<TSMUserAccountDbModel>().AnyAsync(p => p.ID == UserID && p.UserInfoId == requestObject.PostData.CId);
            if (!isExists)
            {
                return ResponseUtil<TSMUserAccountEditAllModel, bool>.FailResult(requestObject, false, "详细信息不存在");
            }

            return null;
        }

        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserAccountEditAllModel, bool>> PersonalSetAsync(RequestObject<TSMUserAccountEditAllModel> requestObject, int UserID)
        {
            var checkResult = await checkData(requestObject, UserID);

            if (checkResult != null)
            {
                return checkResult;
            }
            else
            {
                return await PutAsync(requestObject);
            }
        }
    }
}
