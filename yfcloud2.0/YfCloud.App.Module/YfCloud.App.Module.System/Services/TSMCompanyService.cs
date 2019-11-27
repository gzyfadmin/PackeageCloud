///////////////////////////////////////////////////////////////////////////////////////
// File: ITSMCompanyService.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8 13:47:58
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
using YfCloud.App.Module.System.Models.TSMCompany;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.DBModel;
using YfCloud.Framework.WebApi;
using YfCloud.Utilities.AutoMapper;
using YfCloud.Framework;
using YfCloud.Caches;

namespace YfCloud.App.Module.System.Service
{
    /// <summary>
    /// ITSMCompanyService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITSMCompanyService))]
    public class TSMCompanyService : ITSMCompanyService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TSMCompanyService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TSMCompanyService(IDbContext dbContext, ILogger<TSMCompanyService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_SM_Company数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyQueryModel, List<TSMCompanyQueryModel>>> GetAsync(RequestObject<TSMCompanyQueryModel> requestObject)
        {
            try
            {
                List<TSMCompanyQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TSMCompanyDbModel>();

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var itemCondition = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "keywords").FirstOrDefault();

                    if (itemCondition != null)
                    {
                        query = query.Where(p => p.CompanyName.Contains(itemCondition.Content) || p.ContactNumber == itemCondition.Content);
                    }

                    requestObject.QueryConditions.Remove(itemCondition);

                    if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                    {
                        var expressionList = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                        query.Where(expressionList);
                    }
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select(
                        (t) => new TSMCompanyQueryModel
                        {
                            Id = t.ID,
                            CompanyName = t.CompanyName,
                            ContactNumber = t.ContactNumber,
                            ContactEmail = t.ContactEmail,
                            CompanyInfoId = t.CompanyInfoId,
                            Status = t.Status,
                            CreateTime = t.CreateTime,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                        (t) => new TSMCompanyQueryModel
                        {
                            Id = t.ID,
                            CompanyName = t.CompanyName,
                            ContactNumber = t.ContactNumber,
                            ContactEmail = t.ContactEmail,
                            CompanyInfoId = t.CompanyInfoId,
                            Status = t.Status,
                            CreateTime = t.CreateTime,
                        })
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<TSMCompanyQueryModel, List<TSMCompanyQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TSMCompanyQueryModel, List<TSMCompanyQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 获取当前用户的企业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>> FetchCompanyInfo(TSMCompanyForCurentUserIDModel model)
        {
            RequestObject<TSMCompanyForCurentUserIDModel> request = new RequestObject<TSMCompanyForCurentUserIDModel>();

            TSMCompanyQueryModel result = null;

            request.PostData = model;
            TSMUserAccountDbModel dbModel = null;
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Mobile))
            {
                //返回查询异常结果
                return ResponseUtil<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>.FailResult(request, result, "手机号和邮箱不能同时为空");



            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.Mobile))
                {
                    dbModel = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.TelAccount == model.Mobile).First();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        dbModel = _db.Instance.Queryable<TSMUserAccountDbModel>().Where(p => p.EmailAccount == model.Email).First();
                    }
                }
                if (dbModel == null)
                {
                    return ResponseUtil<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>.FailResult(request, result, "账户不正确");
                }

                var query = await _db.Instance.Queryable<TSMCompanyDbModel>().Where(p => p.ID == dbModel.CompanyId).Select(t2 => new TSMCompanyQueryModel()
                {
                    Id = t2.ID,
                    CompanyName = t2.CompanyName,
                    CompanyInfoId = t2.CompanyInfoId,
                    ContactEmail = t2.ContactEmail,
                    ContactNumber = t2.ContactNumber,
                    CreateTime = t2.CreateTime,
                    IsAdmin = t2.AdminId == dbModel.ID ? true : false
                }).FirstAsync();


                //返回执行结果
                return ResponseUtil<TSMCompanyForCurentUserIDModel, TSMCompanyQueryModel>.SuccessResult(request, query, 1);
            }

        }


        /// <summary>
        /// 新增T_SM_Company数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<string>> PostAsync(RequestPost<TSMCompanyAddModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();

                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<string>.FailResult(null, "PostData不能都为null");
                var result = false;

                string token = string.Empty;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {

                }
                else
                {
                    #region 创建公司时，需要效验验证是否过期
                    var verification = CacheFactory.Instance(CacheType.Redis);
                    string RedisValue = verification.GetValueByKey<string>(string.Format(CacheKeyString.LoginTimes, requestObject.PostData.TelAccount));
                    if (RedisValue == "")
                    {
                        return ResponseUtil<string>.FailResult(null, "注册失败，验证码无效");
                    }
                    if (Convert.ToInt32(RedisValue) >= 3)
                    {
                        if (string.IsNullOrWhiteSpace(requestObject.PostData.VerificationCode))
                        {
                            return ResponseUtil<string>.FailResult(null, "注册失败，请输入验证码");
                        }
                        if (requestObject.PostData.VerificationCode != verification.GetValueByKey<string>(string.Format(CacheKeyString.TimePassMsgCode, requestObject.PostData.TelAccount)))
                        {
                            return ResponseUtil<string>.FailResult(null, "注册失败，输入的验证码不一致");
                        }
                    }
                    #endregion 
                    var addModel = _mapper.Map<TSMCompanyDbModel>(requestObject.PostData);
                    if (_db.Instance.Queryable<TSMCompanyDbModel>().Any(p => p.CompanyName == addModel.CompanyName))
                    {
                        throw new Exception("公司已经存在");
                    }

                    addModel.AdminId = currentUser.UserID;

                    result = _db.Instance.Insertable(addModel).ExecuteCommandIdentityIntoEntity();

                    var tTemplates = _db.Instance.Queryable<TTemplatesModel>().Where(p => p.isDefault).First();

                    TTenantsModel tenantsModel = new TTenantsModel();
                    tenantsModel.CreateId = currentUser.UserID;
                    tenantsModel.TenantShortName = "";
                    tenantsModel.Status = true;
                    tenantsModel.IsTrial = true;
                    tenantsModel.TenantEngName = "";
                    tenantsModel.ValidityPeriod = DateTime.Now.AddMonths(1);

                    if (tTemplates != null)
                    {
                        tenantsModel.TemplateId = tTemplates.Id;
                    }

                    _db.Instance.Insertable<TTenantsModel>(tenantsModel).ExecuteCommandIdentityIntoEntity();

                    addModel.CompanyInfoId = tenantsModel.ID;//从表ID

                    _db.Instance.Updateable<TSMUserAccountDbModel>().Where(p => p.ID == currentUser.UserID).SetColumns(it => new TSMUserAccountDbModel() { Status = 1, CompanyId = addModel.ID }).ExecuteCommand();

                    _db.Instance.Updateable(addModel).UpdateColumns(p => p.CompanyInfoId).ExecuteCommand();

                    #region 根据版本号查询默认权限
                    var defaultData = await _db.Instance.Queryable<TTempPermissionsModel>()
                        .Where(p => p.TemplateId == SqlFunc.Subqueryable<TTemplatesModel>()
                                                    .Where(p1 => p1.isDefault && p1.VersionType == requestObject.PostData.VersionType)
                                                    .Select(p2 => p2.Id))
                        .ToListAsync();
                    List<TTenantPermissionsModel> tenantPermissions = new List<TTenantPermissionsModel>();
                    foreach (var item in defaultData)
                    {
                        tenantPermissions.Add(new TTenantPermissionsModel
                        {
                            TenantId = addModel.ID,
                            MenuId = item.MenuId,
                            ButtonIds = item.ButtonIds,
                            CreateId = currentUser.UserID
                        });
                    }
                    await _db.Instance.Insertable(tenantPermissions).ExecuteCommandAsync();
                    #endregion

                    #region 生成系统所需的字典类型及默认值
                    var dictionaryData = await _db.Instance.Queryable<TPMDictionaryInitInfoDbModel>().ToListAsync();
                    foreach (var item in dictionaryData)
                    {
                        var typeNameModel = new TBMDictionaryTypeDbModel
                        {
                            ID = 0,
                            TypeName = item.TypeName,
                            CompanyId = addModel.ID,
                            DeleteFlag = false,
                            IsSys = true
                        };
                        await _db.Instance.Insertable(typeNameModel).ExecuteCommandIdentityIntoEntityAsync();

                        var valueList = new List<TBMDictionaryDbModel>();
                        foreach (var v in item.TypeValues.Split(","))
                        {
                            if (string.IsNullOrEmpty(v)) continue;
                            valueList.Add(new TBMDictionaryDbModel
                            {
                                ID = 0,
                                TypeId = typeNameModel.ID,
                                DicCode = "",
                                DicValue = v,
                                Remark = "",
                                CreateId = currentUser.UserID,
                                CompanyId = addModel.ID,
                                DeleteFlag = false
                            });
                        }

                        await _db.Instance.Insertable(valueList).ExecuteCommandAsync();
                    }
                    #endregion

                    var palyloads = new Dictionary<string, object>
                    {
                        { "UserID", currentUser.UserID },
                        { "ID",Guid.NewGuid().ToString()},
                        { "CompanyID",addModel.ID},
                        { "UserName",currentUser.UserName}

                    };
                    token = TokenManager.CreateTokenByHandler(palyloads, 60 * 24);
                }

                _db.Instance.CommitTran();
                //返回执行结果
                return ResponseUtil<string>.SuccessResult(token);

            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<string>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> CheckIsExists(RequestPost<TSMCompanyCheckModel> requestObject, CurrentUser currentUser)
        {
            //如果没有新增数据，返回错误信息
            if (requestObject.PostData == null && requestObject.PostDataList == null)
                return ResponseUtil<bool>.FailResult(true, "PostData不能都为null");

            try
            {
                if (_db.Instance.Queryable<TSMCompanyDbModel>().Any(p => p.CompanyName == requestObject.PostData.CompanyName))
                {
                    return ResponseUtil<bool>.SuccessResult(true);
                }
                else
                {
                    return ResponseUtil<bool>.SuccessResult(false);
                }
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        

        /// <summary>
        /// 修改T_SM_Company数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyEditModel, bool>> PutAsync(RequestObject<TSMCompanyEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMCompanyEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    var editList = _mapper.Map<List<TSMCompanyEditModel>, List<TSMCompanyDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TSMCompanyDbModel>(requestObject.PostData);
                    result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;
                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TSMCompanyEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMCompanyEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMCompanyEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_SM_Company数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyAddModel, bool>> DeleteAsync(RequestObject<TSMCompanyAddModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TSMCompanyAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var delList = _mapper.Map<List<TSMCompanyAddModel>, List<TSMCompanyDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Deleteable(delList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    var delModel = _mapper.Map<TSMCompanyDbModel>(requestObject.PostData);
                    result = await _db.Instance.Deleteable(delModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TSMCompanyAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TSMCompanyAddModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TSMCompanyAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_SM_Company数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            return DeleteAsync<int>(requestObject);
        }

        /// <summary>
        /// 删除T_SM_Company数据，通过主表主键删除数据，批量删除
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
                var result = await _db.Instance.Deleteable<TSMCompanyDbModel>().In(ids).ExecuteCommandAsync() > 0;

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
        /// 获取企业信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int, TSMCompanyQueryAllModel>> PersonalGet(int UserID)
        {
            RequestObject<int> request = new RequestObject<int>();
            TSMCompanyQueryAllModel result = null;
            try
            {
                SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(UserID, _db.Instance);

                result = await _db.Instance.Queryable<TSMCompanyDbModel, TTenantsModel>(

                             (t1, t2) => new object[]
                             {
                                    JoinType.Left,  t1.CompanyInfoId== t2.ID

                             }).Where((t1, t2) => t1.ID == sMUserInfo.CompanyId).Select((t1, t2) =>
                             new TSMCompanyQueryAllModel
                             {
                                 ID = t1.ID,
                                 CompanyName = t1.CompanyName,
                                 LegalPerson = t1.LegalPerson,
                                 ContactNumber = t1.ContactNumber,
                                 ContactEmail = t1.ContactEmail,
                                 CompanyInfoId = t1.CompanyInfoId,
                                 CId = t2.ID,
                                 TenantShortName = t2.TenantShortName,
                                 TenantEngName = t2.TenantEngName,
                                 IsTrial = t2.IsTrial,
                                 TrialDate = t2.TrialDate,
                                 TemplateId = t2.TemplateId,
                                 ValidityPeriod = t2.ValidityPeriod,
                                 Area = t2.Area,
                                 Industry = t2.Industry,
                                 TenantScale = t2.TenantScale,
                                 RegisteredCapital = t2.RegisteredCapital,
                                 MainBusiness = t2.MainBusiness,
                                 FixedTele = t2.FixedTele,
                                 Address = t2.Address,
                                 TenantLogo = t2.TenantLogo,
                                 BusinessLogo = t2.BusinessLogo,
                                 IsAdmin = t1.AdminId == UserID ? true : false

                             }).FirstAsync();

                return ResponseUtil<int, TSMCompanyQueryAllModel>.SuccessResult(request, result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<int, TSMCompanyQueryAllModel>.FailResult(request, result, ex.Message);
            }
        }

        /// <summary>
        /// 企业设置
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMCompanyAllEditModel, bool>> ModifyCurentCompany(RequestObject<TSMCompanyAllEditModel> requestObject, int UserID)
        {
            //执行结果
            var result = false;
            //没有修改信息，返回错误信息
            if (requestObject.PostDataList == null && requestObject.PostData == null)
                return ResponseUtil<TSMCompanyAllEditModel, bool>.FailResult(requestObject, result, "PostData不能都为null");

            var curentDb = _db.Instance;

            int id = requestObject.PostData.ID;
            SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(UserID, curentDb);


            if (sMUserInfo.CompanyId != id)
            {
                return ResponseUtil<TSMCompanyAllEditModel, bool>.FailResult(requestObject, result, "只能修改自己公司");
            }

            var companyDb = curentDb.Queryable<TSMCompanyDbModel>().Where(p => p.ID == id).First();
            if (companyDb.AdminId != UserID)
            {
                return ResponseUtil<TSMCompanyAllEditModel, bool>.FailResult(requestObject, result, "您不是公司管理员");
            }

            try
            {
                curentDb.BeginTran();

                var curentData = requestObject.PostData;
                var mainDb = ExpressionGenericMapper<TSMCompanyAllEditModel, TSMCompanyDbModel>.Trans(curentData);


                await curentDb.Updateable<TSMCompanyDbModel>(mainDb).UpdateColumns(p => new { p.CompanyName, p.LegalPerson, p.ContactNumber, p.EnterpriseType }).ExecuteCommandAsync();

                var cDb = ExpressionGenericMapper<TSMCompanyAllEditModel, TTenantsModel>.Trans(curentData);
                cDb.ID = curentData.CId;

                await curentDb.Updateable<TTenantsModel>(cDb).UpdateColumns(p => new { p.TenantShortName, p.TenantEngName, p.TenantLogo, p.BusinessLogo }).ExecuteCommandAsync();

                curentDb.CommitTran();

                return ResponseUtil<TSMCompanyAllEditModel, bool>.SuccessResult(requestObject, true);
            }
            catch (Exception ex)
            {
                curentDb.RollbackTran();
                return ResponseUtil<TSMCompanyAllEditModel, bool>.FailResult(requestObject, result, ex.ToString());
            }
        }
    }
}
