///////////////////////////////////////////////////////////////////////////////////////
// File: ITPMDictionaryInitInfoService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/23 15:16:15
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
using YfCloud.App.Module.Platform.Models.TPMDictionaryInitInfo;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace YfCloud.App.Module.Platform.Service
{
    /// <summary>
    /// ITPMDictionaryInitInfoService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITPMDictionaryInitInfoService))]
    public class TPMDictionaryInitInfoService : ITPMDictionaryInitInfoService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TPMDictionaryInitInfoService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TPMDictionaryInitInfoService(IDbContext dbContext, ILogger<TPMDictionaryInitInfoService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取T_PM_DictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TPMDictionaryInitInfoQueryModel>>> GetAsync(RequestGet requestObject)
        {
            try
            {
                List<TPMDictionaryInitInfoQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TPMDictionaryInitInfoDbModel>();
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TPMDictionaryInitInfoDbModel>(item.Column);
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
						(t) => new TPMDictionaryInitInfoQueryModel 
						{
							ID = t.ID,
							TypeName = t.TypeName,
							TypeValues = t.TypeValues,
							CreateId = t.CreateId,
							CreateTime = t.CreateTime,
							UpdateId = t.UpdateId,
							UpdateTime = t.UpdateTime,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TPMDictionaryInitInfoQueryModel 
						{
							ID = t.ID,
							TypeName = t.TypeName,
							TypeValues = t.TypeValues,
							CreateId = t.CreateId,
							CreateTime = t.CreateTime,
							UpdateId = t.UpdateId,
							UpdateTime = t.UpdateTime,
						})
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TPMDictionaryInitInfoQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TPMDictionaryInitInfoQueryModel>>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 新增T_PM_DictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TPMDictionaryInitInfoAddModel> requestObject)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    var addList = _mapper.Map<List<TPMDictionaryInitInfoAddModel>, List<TPMDictionaryInitInfoDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<TPMDictionaryInitInfoDbModel>(requestObject.PostData);
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
        /// 修改T_PM_DictionaryInitInfo数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TPMDictionaryInitInfoEditModel> requestObject)
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
                    //批量更新
                    var editList = _mapper.Map<List<TPMDictionaryInitInfoEditModel>, List<TPMDictionaryInitInfoDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TPMDictionaryInitInfoDbModel>(requestObject.PostData);
                    result = await _db.Instance.Updateable(editModel).ExecuteCommandAsync() > 0;
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
        /// 删除T_PM_DictionaryInitInfo数据
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
                    result = await _db.Instance.Deleteable<TPMDictionaryInitInfoDbModel>().In(ids).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录删除
                    result = await _db.Instance.Deleteable<TPMDictionaryInitInfoDbModel>().In(requestObject.PostData.ID).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }
        
    }
}
