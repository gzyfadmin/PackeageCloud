///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMDictionaryTypeService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 17:52:10
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
using YfCloud.App.Module.BasicSet.Models.TBMDictionaryType;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.WebApi;
using YfCloud.Caches;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// ITBMDictionaryTypeService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMDictionaryTypeService))]
    public class TBMDictionaryTypeService : ITBMDictionaryTypeService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBMDictionaryTypeService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TBMDictionaryTypeService(IDbContext dbContext, ILogger<TBMDictionaryTypeService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_BM_DictionaryType数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMDictionaryTypeQueryModel, List<TBMDictionaryTypeQueryModel>>> GetAsync(RequestObject<TBMDictionaryTypeQueryModel> requestObject,int UserID)
        {
            try
            {
                var curentDB = _db.Instance;
                var smUserInfo = SMCurentUserManager.GetCurentUserID(UserID, curentDB);

                List<TBMDictionaryTypeQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = curentDB.Queryable<TBMDictionaryTypeDbModel>().Where(p=>p.CompanyId== smUserInfo.CompanyId && SqlFunc.IsNull(p.DeleteFlag, false) == false);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMDictionaryTypeDbModel>(item.Column);
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
						(t) => new TBMDictionaryTypeQueryModel 
						{
							ID = t.ID,
							TypeName = t.TypeName,
							CompanyId = t.CompanyId,
							DeleteFlag = t.DeleteFlag,
						})
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t) => new TBMDictionaryTypeQueryModel 
						{
							ID = t.ID,
							TypeName = t.TypeName,
							CompanyId = t.CompanyId,
							DeleteFlag = t.DeleteFlag,
						})
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<TBMDictionaryTypeQueryModel, List<TBMDictionaryTypeQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TBMDictionaryTypeQueryModel, List<TBMDictionaryTypeQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_BM_DictionaryType数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMDictionaryTypeAddModel,bool>> PostAsync(RequestObject<TBMDictionaryTypeAddModel> requestObject,CurrentUser currentUser)
        {
            var curentDB = _db.Instance;
            try
            {
                curentDB.BeginTran();

                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TBMDictionaryTypeAddModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //var addList = _mapper.Map<List<TBMDictionaryTypeAddModel>, List<TBMDictionaryTypeDbModel>>(requestObject.PostDataList);
                    //addList.ForEach((x) => {
                    //    x.CompanyId = sMUserInfo.CompanyId.Value;
                    //});
                    //result = await curentDB.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<TBMDictionaryTypeDbModel>(requestObject.PostData);
                    addModel.CompanyId = currentUser.CompanyID;
                    addModel.DeleteFlag = false;


                    var OldDb = curentDB.Queryable<TBMDictionaryTypeDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false && p.TypeName == addModel.TypeName&&
                    p.CompanyId== currentUser.CompanyID).First();

                    if (OldDb != null)
                    {
                        return ResponseUtil<TBMDictionaryTypeAddModel, bool>.FailResult(requestObject, false, addModel.TypeName + "已经存在，不能重复");
                    }
                    else
                    {
                        result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                    }
                }

                _db.Instance.CommitTran();
                var redis = CacheFactory.Instance(CacheType.Redis);

                string rediskey = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);
                redis.RemoveKey(rediskey);

                //返回执行结果
                if (result)
                    return ResponseUtil<TBMDictionaryTypeAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMDictionaryTypeAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                curentDB.RollbackTran();
                //返回异常结果
                return ResponseUtil<TBMDictionaryTypeAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_BM_DictionaryType数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMDictionaryTypeEditModel, bool>> PutAsync(RequestObject<TBMDictionaryTypeEditModel> requestObject,CurrentUser currentUser)
        {
            try
            {

                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBMDictionaryTypeEditModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    ////批量更新
                    //var editList = _mapper.Map<List<TBMDictionaryTypeEditModel>, List<TBMDictionaryTypeDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Updateable(editList).IgnoreColumns(p => new { p.DeleteFlag,p.CompanyId }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TBMDictionaryTypeDbModel>(requestObject.PostData);

                    var OldDb = _db.Instance.Queryable<TBMDictionaryTypeDbModel>().Where(p => p.DeleteFlag == true && p.TypeName == editModel.TypeName &&
                    p.CompanyId == currentUser.CompanyID);

                    if (OldDb != null)
                    {
                        return ResponseUtil<TBMDictionaryTypeEditModel, bool>.FailResult(requestObject, false, editModel.TypeName + "已经存在，不能重复");
                    }
                    else
                    {
                        result = await _db.Instance.Updateable(editModel).IgnoreColumns(p => new { p.DeleteFlag, p.CompanyId }).ExecuteCommandAsync() > 0;
                    }

                    
                }

                var redis = CacheFactory.Instance(CacheType.Redis);

                string rediskey = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);
                redis.RemoveKey(rediskey);

                //返回执行结果
                if (result)
                    return ResponseUtil<TBMDictionaryTypeEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMDictionaryTypeEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMDictionaryTypeEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_BM_DictionaryType数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser currentUser)
        {
            var curentDB = _db.Instance;
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "PostData、PostDataList不能都为null");

                curentDB.BeginTran();
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {

                    var ids = requestObject.PostDataList.Select(p => p.ID).ToList();
                    //批量删除
                    var idsEntity = requestObject.PostDataList.Select(p => new TBMDictionaryTypeDbModel() { ID = p.ID, DeleteFlag = true }).ToList();
                    await curentDB.Updateable<TBMDictionaryDbModel>().Where(p => ids.Contains(p.TypeId)).SetColumns(p => new TBMDictionaryDbModel { DeleteFlag=true }).ExecuteCommandAsync();

                    result = await curentDB.Updateable<TBMDictionaryTypeDbModel>(idsEntity).UpdateColumns(p => new { p.DeleteFlag }).ExecuteCommandAsync() > 0;
                }
                else
                { 

                    TBMDictionaryTypeDbModel id = new TBMDictionaryTypeDbModel() { ID = requestObject.PostData.ID, DeleteFlag = true };

                    await curentDB.Updateable<TBMDictionaryDbModel>().Where(p => id.ID == p.TypeId).SetColumns(p => new TBMDictionaryDbModel { DeleteFlag = true }).ExecuteCommandAsync();
                    result = await curentDB.Updateable<TBMDictionaryTypeDbModel>(id).UpdateColumns(p => new { p.DeleteFlag }).ExecuteCommandAsync() > 0;

                   
                }
                curentDB.CommitTran();

                var redis = CacheFactory.Instance(CacheType.Redis);

                string rediskey = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);
                redis.RemoveKey(rediskey);

                //返回执行结果
                if (result)
                    return ResponseUtil<DeleteModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                curentDB.RollbackTran();
                //返回异常结果
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
    }
}
