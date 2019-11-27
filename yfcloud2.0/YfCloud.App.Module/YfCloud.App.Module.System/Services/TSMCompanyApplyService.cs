///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMCompanyApplyService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:50:04
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
using YfCloud.App.Module.System.Models.TSMCompanyApply;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.DBModel;
using YfCloud.Utilities.AutoMapper;
using YfCloud.DBModel.System;
using YfCloud.Caches;
using YfCloud.Models.CacheModels;
using YfCloud.Utilities.MongoDbModel;
using YfCloud.Utilities.MongoDb;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMCompanyApplyService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITSMCompanyApplyService))]
    public class TSMCompanyApplyService : ITSMCompanyApplyService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMCompanyApplyService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMCompanyApplyService(IDbContext dbContext, ILogger<TSMCompanyApplyService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_SM_CompanyApply数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyApplyQueryModel, List<TSMCompanyApplyQueryModel>>> GetAsync(RequestObject<TSMCompanyApplyQueryModel> requestObject,CurrentUser currentUser)
        {
            try
            {
                List<TSMCompanyApplyQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMCompanyApplyDbModel, TSMUserAccountDbModel, TSMUserInfoDbModel>((t,t1,t2)=>new object[] {
                    JoinType.Inner,t.AccountId==t1.ID,
                    JoinType.Inner,t1.UserInfoId==t2.ID
                }).Where((t, t1, t2)=> t.CompanyId==currentUser.CompanyID);

                var  company = _db.Instance.Queryable<TSMCompanyDbModel>().Where(p => p.ID == currentUser.CompanyID).First();

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    List<IConditionalModel> conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "telaccount")
                        {
                            item.FieldName = $"t1.{item.FieldName}";
                        }
                        else
                        {
                            if (item.ConditionalType == ConditionalType.LessThanOrEqual)
                            {
                                item.FieldValue = Convert.ToDateTime(item.FieldValue).AddDays(1).ToString("yyyyMMdd");
                            }
                            item.FieldName = $"t.{item.FieldName}";
                        }
                    }

                    query.Where(conditionals);
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
						(t, t1, t2) => new TSMCompanyApplyQueryModel 
						{
							Id = t.Id,
							AccountId = t.AccountId,
							CompanyId = t.CompanyId,
							ApplyTime = t.ApplyTime,
							ApplyStatus = t.ApplyStatus,
                            AccountName=t1.AccountName,
                            TelAccount =t1.TelAccount,
                            EmailAccount = t1.EmailAccount,
                            CompanyName= company.CompanyName
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                        (t, t1, t2) => new TSMCompanyApplyQueryModel
                        {
                            Id = t.Id,
                            AccountId = t.AccountId,
                            CompanyId = t.CompanyId,
                            ApplyTime = t.ApplyTime,
                            ApplyStatus = t.ApplyStatus,
                            AccountName = t1.AccountName,
                            TelAccount = t1.TelAccount,
                            EmailAccount = t1.EmailAccount,
                            CompanyName = company.CompanyName
                        })
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<TSMCompanyApplyQueryModel, List<TSMCompanyApplyQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMCompanyApplyQueryModel, List<TSMCompanyApplyQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_SM_CompanyApply数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TSMCompanyApplyAddModel> requestObject,CurrentUser currentUser)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    
                }
                else
                {
                    if (!_db.Instance.Queryable<TSMCompanyApplyDbModel>().Any(p => p.CompanyId == requestObject.PostData.CompanyId && p.AccountId == currentUser.UserID && p.ApplyStatus == 0))
                    {
                        var addModel = _mapper.Map<TSMCompanyApplyDbModel>(requestObject.PostData);
                        addModel.AccountId = currentUser.UserID;
                        addModel.ApplyStatus = 0;
                        addModel.ApplyTime = DateTime.Now;
                        await _db.Instance.Insertable(addModel).ExecuteCommandAsync();

                        var userAccount = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == addModel.AccountId).First();
                        //推送通知
                        PushNotice(addModel.AccountId, addModel.CompanyId, userAccount);
                    }
                }

                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        private void PushNotice(int UserId, int CompanyID, TSMUserAccountDbModel userAccount)
        {
            var permissUser = _db.Instance.Queryable<TSMUserAccountDbModel, TSMRoleUserRelationDbModel, TSMRolePermissionsDbModel, TSMRolesDbModel, TPMMenusDbModel>(
                                                (x, x1, x2, x3, x4) => new object[]
                                                {
                                    JoinType.Inner,x.ID==x1.UserId,
                                    JoinType.Inner,  x1.RoleId== x2.RoleId,
                                    JoinType.Inner,  x2.RoleId== x3.Id,
                                    JoinType.Inner,  x2.MenuId== x4.Id
                                                }).Where((x, x1, x2, x3, x4) => x.CompanyId == CompanyID && x.Status == 1 && x4.MenuPath == "/views/SysSettings/UserAuth/index.vue")
                                                .Select((x, x1, x2, x3, x4) => x).ToList();

            var adminUser = _db.Instance.Queryable<TSMUserAccountDbModel, TSMCompanyDbModel>((x, x1) => new object[] {
                JoinType.Inner,x.ID==x1.AdminId
            }).Where((x, x1) => x1.ID == CompanyID).Select((x, x1) => x).First();

            if (!permissUser.Any(p => p.ID == adminUser.ID))
            {
                permissUser.Add(adminUser);
            }
            

            var redis = CacheFactory.Instance(CacheType.Redis);

            List<ToDoMgModel> toDoMgModelList = new List<ToDoMgModel>();
            foreach (var userTtem in permissUser)
            {

                ToDoMgModel toDoModel = new ToDoMgModel();
                toDoModel.Title = "申请加入公司";
                toDoModel.Content = $"姓名：{userAccount.AccountName} 电话：{userAccount.TelAccount}，申请加入公司";
                toDoModel.CreateBy = UserId;
                toDoModel.CreateDate = DateTime.Now;
                toDoModel.IsRead = false;
                toDoModel.MessageType = MessageTypeMgEnum.ToApplytoComplany;
                toDoModel.To = userTtem.ID;
                toDoModel.ToDoId = Guid.NewGuid().ToString();
                toDoModel.CompanyID = CompanyID;
                toDoMgModelList.Add(toDoModel);
            }

            MongoDbUtil.AddDoc(toDoMgModelList);

        }
        
        /// <summary>
        /// 修改T_SM_CompanyApply数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TSMCompanyApplyEditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = true;

                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    // var editList = _mapper.Map<List<TSMCompanyApplyEditModel>, List<TSMCompanyApplyDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Updateable(editList).Where(p=>p.CompanyId== currentUser.CompanyID).UpdateColumns(p=>new { p.ApplyStatus}).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TSMCompanyApplyDbModel>(requestObject.PostData);
                 

                    var run= _db.Instance.Updateable<TSMCompanyApplyDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && p.Id == editModel.Id).SetColumns(p => p.ApplyStatus == editModel.ApplyStatus);

                    run.ExecuteCommand();

                    if (editModel.ApplyStatus == 1)
                    {
                        var editModelDB = _db.Instance.Queryable<TSMCompanyApplyDbModel>().Where(p => p.Id == editModel.Id).First();


                        //var account = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.ID == editModelDB.AccountId).First();

                        //if (_db.Instance.Queryable<TSMUserAccountDbModel>().Any(p => p.CompanyId == editModelDB.CompanyId && p.AccountName == account.AccountName))
                        //{

                        //}

                        var runSQL = _db.Instance.Updateable<TSMUserAccountDbModel>().Where(p => p.ID == editModelDB.AccountId).SetColumns(p => new TSMUserAccountDbModel { CompanyId = editModelDB.CompanyId,Status=1 });
                        runSQL.ExecuteCommand();
                    }
                }
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_CompanyApply数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyApplyAddModel, bool>> DeleteAsync(RequestObject<TSMCompanyApplyAddModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMCompanyApplyAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var delList = _mapper.Map<List<TSMCompanyApplyAddModel>, List<TSMCompanyApplyDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Deleteable(delList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    var delModel = _mapper.Map<TSMCompanyApplyDbModel>(requestObject.PostData);
                    result = await _db.Instance.Deleteable(delModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMCompanyApplyAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMCompanyApplyAddModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMCompanyApplyAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_SM_CompanyApply数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_CompanyApply数据，通过主表主键删除数据，批量删除
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
                var result = await _db.Instance.Deleteable<TSMCompanyApplyDbModel>().In(ids).ExecuteCommandAsync() > 0;
                
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
