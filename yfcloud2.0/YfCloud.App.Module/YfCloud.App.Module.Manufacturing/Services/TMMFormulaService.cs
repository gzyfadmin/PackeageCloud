///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMFormulaService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:21:05
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
using YfCloud.App.Module.Manufacturing.Models.TMMFormula;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMFormulaService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITMMFormulaService))]
    public class TMMFormulaService : ITMMFormulaService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TMMFormulaService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMFormulaService(IDbContext dbContext, ILogger<TMMFormulaService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_MM_Formula数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TMMFormulaQueryModel>>> GetAsync(RequestGet requestObject,CurrentUser currentUser)
        {
            try
            {
                List<TMMFormulaQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMFormulaDbModel,TSMUserAccountDbModel,TSMUserAccountDbModel>(
                        (t,t0,t1) => new object[]
                        {
                           JoinType.Left , t.CreateId == t0.ID,
                           JoinType.Left , t.UpdateId == t1.ID
                        }
                    );
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMFormulaDbModel>(item.Column);
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
						(t, t0, t1) => new TMMFormulaQueryModel 
						{
							ID = t.ID,
							FormulaName = t.FormulaName,
							Formula = t.Formula,
							CompanyId = t.CompanyId,
							DeleteFlag = t.DeleteFlag,
							FrontFormula = t.FrontFormula,
                            CreateId = t.CreateId,
                            CreateName = t0.AccountName,
                            CreateTime = t.CreateTime,
                            UpdateId = t.UpdateId,
                            UpdateName = t1.AccountName,
                            UpdateTime = t.UpdateTime,
                            Remark = t.Remark
						})
                        .Where(t=> t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t, t0, t1) => new TMMFormulaQueryModel 
						{
							ID = t.ID,
							FormulaName = t.FormulaName,
							Formula = t.Formula,
							CompanyId = t.CompanyId,
							DeleteFlag = t.DeleteFlag,
							FrontFormula = t.FrontFormula,
                            CreateId = t.CreateId,
                            CreateName = t0.AccountName,
                            CreateTime = t.CreateTime,
                            UpdateId = t.UpdateId,
                            UpdateName = t1.AccountName,
                            UpdateTime = t.UpdateTime,
                            Remark = t.Remark
                        })
                        .Where(t => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag)
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TMMFormulaQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMFormulaQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_MM_Formula数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="currUser">当前操作用户</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TMMFormulaAddModel> requestObject,CurrentUser currUser)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                   
                }
                else
                {
                    var addModel = _mapper.Map<TMMFormulaDbModel>(requestObject.PostData);

                    if (_db.Instance.Queryable<TMMFormulaDbModel>().Any(p => p.FormulaName == addModel.FormulaName && p.CompanyId == currUser.CompanyID && SqlFunc.IsNull(p.DeleteFlag, false) == false))
                    {
                        throw new Exception("公式名重复");
                    }



                    addModel.CompanyId = currUser.CompanyID;
                    addModel.CreateId = currUser.UserID;
                    addModel.CreateTime = DateTime.Now;
                    result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_MM_Formula数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <param name="currUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TMMFormulaEditModel> requestObject,CurrentUser currUser)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                  
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TMMFormulaDbModel>(requestObject.PostData);
                    if (_db.Instance.Queryable<TMMFormulaDbModel>().Any(p => p.FormulaName == editModel.FormulaName && 
                    p.CompanyId == currUser.CompanyID && SqlFunc.IsNull(p.DeleteFlag, false) == false &&p.ID!=editModel.ID
                    ))
                    {
                        throw new Exception("公式名重复");
                    }


                    editModel.UpdateId = currUser.UserID;
                    editModel.UpdateTime = DateTime.Now;
                    result = await _db.Instance.Updateable(editModel)
                        .UpdateColumns(p => new { p.FormulaName, p.Formula, p.FrontFormula, p.UpdateId, p.UpdateTime, p.Remark })
                        .ExecuteCommandAsync() > 0;
                }
                
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_MM_Formula数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData、PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var ids = requestObject.PostDataList.Select(p => p.ID);
                    result = await _db.Instance.Updateable<TMMFormulaDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => ids.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    result = await _db.Instance.Updateable<TMMFormulaDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败，数据可能已删除!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        
    }
}
