///////////////////////////////////////////////////////////////////////////////////////
// File: ITTenantsService.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20 10:14:15
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
using AutoMapper;
using YfCloud.DBModel;
using YfCloud.App.Module.Platform.Models.TTenants;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITTenantsService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITTenantsService))]
    public class TTenantsService : ITTenantsService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TTenantsService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TTenantsService(IDbContext dbContext, ILogger<TTenantsService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_Tenants数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantsModel, List<TTenantsQueryModel>>> GetAsync(RequestObject<TTenantsModel> requestObject)
        {
            try
            {
                List<TTenantsQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TTenantsModel, TSMCompanyDbModel>(
                                (t, t1) => new object[]
                                {
                                    JoinType.Left,  t.ID== t1.CompanyInfoId,
                                });


                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where(p));
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    requestObject.OrderByConditions.ForEach(p => query.OrderBy($"{p.Column} {p.Condition}"));
                }

              var  queryResult = query.Select((t, t1) => new TTenantsQueryModel
              {
                  Address = t.Address,
                  Area = t.Area,
                  CreateId = t.CreateId,
                  CreateTime = t.CreateTime,
                  FixedTele = t.FixedTele,
                  Id = t.ID,
                  Industry = t.Industry,
                  IsTrial = t.IsTrial,
                  MainBusiness = t.MainBusiness,
                  RegisteredCapital = t.RegisteredCapital,
                  Status = t.Status,
                  TemplateId = t.TemplateId,
                  TenantEngName = t.TenantEngName,
                  TenantLogo = t.TenantLogo,
                  TenantName = t1.CompanyName,
                  TenantScale = t.TenantScale,
                  TenantShortName = t.TenantShortName,
                  TrialDate = t.TrialDate,
                  ValidityPeriod = t.ValidityPeriod
              });


                //执行查询
                if (requestObject.IsPaging)
                    queryData = await queryResult.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                else
                    queryData = await queryResult.ToListAsync();
                    
                //返回执行结果
                return ResponseUtil<TTenantsModel, List<TTenantsQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TTenantsModel, List<TTenantsQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_Tenants数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantsModel,bool>> PostAsync(RequestObject<TTenantsModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 1)
                {
                    result = await _db.Instance.Insertable(requestObject.PostDataList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    result = await _db.Instance.Insertable(requestObject.PostData).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TTenantsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_Tenants数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantsPutModel, bool>> PutAsync(RequestObject<TTenantsPutModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTenantsPutModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                   // result = await _db.Instance.Updateable(requestObject.PostDataList).IgnoreColumns(p => new { p.BusinessLogo }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    _db.Instance.BeginTran();

                    int id = requestObject.PostData.ID;
                    string name = requestObject.PostData.TenantName;

                    var editModel = ExpressionGenericMapper<TTenantsPutModel,TTenantsModel>.Trans(requestObject.PostData);


                    var idList = _db.Instance.Queryable<TSMCompanyDbModel>().Where(p => p.CompanyInfoId == id).Select(p => p.ID).ToList();

                    await _db.Instance.Updateable<TSMCompanyDbModel>().Where(p => idList.Contains(p.ID)).SetColumns(p => new TSMCompanyDbModel { ID=p.ID, CompanyName = name }).ExecuteCommandAsync();

                    //单记录更新
                    result = await _db.Instance.Updateable(editModel).IgnoreColumns(p=>new {p.BusinessLogo,p.CreateId,p.CreateTime }).ExecuteCommandAsync() > 0;

                    _db.Instance.CommitTran();
                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TTenantsPutModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantsPutModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<TTenantsPutModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 修改T_Tenants数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantsEditModel, bool>> ModifyAsync(RequestObject<TTenantsEditModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;

                
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTenantsEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    var editList = _mapper.Map<List<TTenantsEditModel>, List<TTenantsModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Updateable(editList).UpdateColumns(p => new {
                       // p.TenantName, 需要修改从表的企业名称
                        p.TenantShortName,
                        p.TenantEngName,
                        p.Area,
                        p.Industry,
                        p.TenantScale,
                        p.RegisteredCapital,
                        p.MainBusiness,
                        p.FixedTele,
                        p.Address,
                        p.TenantLogo
                    }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    
                    var edit = _mapper.Map<TTenantsModel>(requestObject.PostData);
                    
                    //单记录更新
                    result = await _db.Instance.Updateable(edit).UpdateColumns(p=>new {
                        // p.TenantName, 需要修改从表的企业名称
                        p.TenantShortName,
                        p.TenantEngName,
                        p.Area,
                        p.Industry,
                        p.TenantScale,
                        p.RegisteredCapital,
                        p.MainBusiness,
                        p.FixedTele,
                        p.Address,
                        p.TenantLogo
                    }).ExecuteCommandAsync() > 0;
                }

                //返回执行结果
                if (result)
                    return ResponseUtil<TTenantsEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantsEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTenantsEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        /// <summary>
        /// 删除T_Tenants数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TTenantsModel, bool>> DeleteAsync(RequestObject<TTenantsModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
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
                    return ResponseUtil<TTenantsModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TTenantsModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_Tenants数据，通过主表主键删除数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject)
        {
            
            try
            {
                //删除主表信息
                var result = await _db.Instance.Deleteable<TTenantsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;                
                
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
        /// 删除T_Tenants数据，通过主表主键删除数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        public async Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject)
        {
            try
            {
                //删除信息
                var result = await _db.Instance.Deleteable<TTenantsModel>().In(requestObject.PostData).ExecuteCommandAsync() > 0;
                
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
    }
}
