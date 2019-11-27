///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMDictionaryService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 17:56:40
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
using YfCloud.App.Module.BasicSet.Models.TBMDictionary;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.BasicSet.Models.TBMDictionaryType;
using YfCloud.Framework.WebApi;
using YfCloud.Caches;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// ITBMDictionaryService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMDictionaryService))]
    public class TBMDictionaryService : ITBMDictionaryService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBMDictionaryService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TBMDictionaryService(IDbContext dbContext, ILogger<TBMDictionaryService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取公司下所有的数据字典
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public async Task<List<TBMDictionaryCacheModel>> GetAllDictionary(int CompanyID)
        {
            try
            {
                var query = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                   (t, t0, t1, t2) => new object[]
                                   {
                                    JoinType.Left,  t.TypeId== t0.ID,
                                    JoinType.Left,t.CreateId==t1.ID,
                                    JoinType.Left,t.UpdateId==t2.ID
                                   }).Where((t, t0, t1, t2) => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == CompanyID);

                var queryData = await query.Select(
                            (t, t0, t1, t2) => new TBMDictionaryCacheModel
                            {
                                ID = t.ID,
                                TypeId = t.TypeId,
                                TypeName = t0.TypeName,
                                DicCode = t.DicCode,
                                DicValue = t.DicValue,
                                Remark = t.Remark,
                                CreateTime = t.CreateTime,
                                CreateId = t.CreateId,
                                CreateName = t1.AccountName,
                                UpdateName = t.UpdateId == null ? "" : t2.AccountName,
                                UpdateTime = t.UpdateTime,
                                UpdateId = t.UpdateId,
                                CompanyId = t.CompanyId,
                                DeleteFlag = t.DeleteFlag,
                            })
                            .ToListAsync();

                return queryData;
            }
            catch (Exception ex)
            {
                return new List<TBMDictionaryCacheModel>();
            }
        }

        /// <summary>
        /// 获取T_BM_Dictionary数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TBMDictionaryQueryModel>>> GetAsync(RequestGet requestObject,CurrentUser user)
        {
            try
            {

                List<TBMDictionaryQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel,  TSMUserAccountDbModel, TSMUserAccountDbModel>(
								(t, t0,t1,t2) => new object[] 
								{
									JoinType.Left,  t.TypeId== t0.ID, 
                                    JoinType.Left,t.CreateId==t1.ID,
                                    JoinType.Left,t.UpdateId==t2.ID
								}).Where((t,t0,t1,t2)=>SqlFunc.IsNull(t.DeleteFlag,false)==false && t.CompanyId== user.CompanyID);
                string[] cQuery = { "typename" };

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var QueryConditions1 = requestObject.QueryConditions.Where(p => cQuery.Contains(p.Column.ToLower())).ToList();

                    if (QueryConditions1.Count() > 0)
                    {
                        var conditionals1 = SqlSugarUtil.GetConditionalModels(QueryConditions1);

                        foreach (ConditionalModel item in conditionals1)
                        {
                            item.FieldName = $"t0.{item.FieldName}";
                        }
                        query.Where(conditionals1);
                    }

                    var QueryConditions2 = requestObject.QueryConditions.Where(p => !cQuery.Contains(p.Column.ToLower())).ToList();

                    if (QueryConditions2.Count() > 0)
                    {
                        var conditionals2 = SqlSugarUtil.GetConditionalModels(QueryConditions2);

                        foreach (ConditionalModel item in conditionals2)
                        {
                            item.FieldName = $"t.{item.FieldName}";
                        }
                        query.Where(conditionals2);
                    }

                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMDictionaryDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query
                        .Where(t => t.DeleteFlag == false)
                        .Select((t, t0, t1, t2) => new TBMDictionaryQueryModel
                        {
                            ID = t.ID,
                            TypeId = t.TypeId,
                            TypeName = t0.TypeName,
                            DicCode = t.DicCode,
                            DicValue = t.DicValue,
                            Remark = t.Remark,
                            CreateTime = t.CreateTime,
                            CreateId = t.CreateId,
                            CreateName = t1.AccountName,
                            UpdateName = t.UpdateId == null ? "" : t2.AccountName,
                            UpdateTime = t.UpdateTime,
                            UpdateId = t.UpdateId,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                        })                        
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Where(t => t.DeleteFlag == false)
                        .Select((t, t0,t1,t2) => new TBMDictionaryQueryModel 
						{
							ID = t.ID,
							TypeId = t.TypeId,
                            TypeName = t0.TypeName,
							DicCode = t.DicCode,
							DicValue = t.DicValue,
							Remark = t.Remark,
							CreateTime = t.CreateTime,
							CreateId = t.CreateId,
                            CreateName = t1.AccountName,
                            UpdateName = t.UpdateId == null ? "" : t2.AccountName,
                            UpdateTime = t.UpdateTime,
							UpdateId = t.UpdateId,
							CompanyId = t.CompanyId,
							DeleteFlag = t.DeleteFlag,
						})                        
                        .ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TBMDictionaryQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TBMDictionaryQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_BM_Dictionary数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMDictionaryAddModel,bool>> PostAsync(RequestObject<TBMDictionaryAddModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TBMDictionaryAddModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                var result = false;

                var curentDB = _db.Instance;


                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //var addList = _mapper.Map<List<TBMDictionaryAddModel>, List<TBMDictionaryDbModel>>(requestObject.PostDataList);

                    //addList.ForEach((x) =>
                    //{
                    //    x.CreateId = UserID;
                    //    x.CreateTime = DateTime.Now;
                    //    x.DeleteFlag = false;
                    //    x.CompanyId = sMUserInfo.CompanyId.Value;
                    //});
                    //result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<TBMDictionaryDbModel>(requestObject.PostData);
                    addModel.CreateId = currentUser.UserID;
                    addModel.CreateTime = DateTime.Now;
                    addModel.DeleteFlag = false;
                    addModel.CompanyId = currentUser.CompanyID;

                    var OldDb = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag,false) == false && p.TypeId == addModel.TypeId && 
                    p.CompanyId == currentUser.CompanyID && p.DicValue== addModel.DicValue).First();
                    if (OldDb != null)
                    {
                        return ResponseUtil<TBMDictionaryAddModel, bool>.FailResult(requestObject, false,"名称:"+ addModel.DicValue + "已经存在，不能重复");
                    }
                    else
                    {
                        var dType = _db.Instance.Queryable<TBMDictionaryTypeDbModel>().Where(p => p.ID == addModel.TypeId).First();

                        if (dType != null && dType.TypeName.Contains("品牌"))
                        {
                            bool isExists = _db.Instance.Queryable<TBMDictionaryDbModel>().Any(p =>
                              SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                              p.TypeId == addModel.TypeId &&
                              p.CompanyId == currentUser.CompanyID &&
                              p.DicCode == addModel.DicCode);

                            if (isExists)
                            {
                                return ResponseUtil<TBMDictionaryAddModel, bool>.FailResult(requestObject, false,"编码:"+ addModel.DicCode + "已经存在，不能重复");
                            }

                        }

                        result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                    }
                }
                ClearCache(currentUser);
                //返回执行结果
                if (result)
                    return ResponseUtil<TBMDictionaryAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMDictionaryAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMDictionaryAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_BM_Dictionary数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <param name="currentUser"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMDictionaryEditModel, bool>> PutAsync(RequestObject<TBMDictionaryEditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = false;

                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBMDictionaryEditModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    ////批量更新
                    //var editList = _mapper.Map<List<TBMDictionaryEditModel>, List<TBMDictionaryDbModel>>(requestObject.PostDataList);
                    //editList.ForEach((x) => {
                    //    x.UpdateId = UserID;
                    //    x.UpdateTime = DateTime.Now;
                    //});
                    //result = await _db.Instance.Updateable(editList).IgnoreColumns(p=> new {p.CreateId,p.CreateTime,p.DeleteFlag,p.CompanyId }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TBMDictionaryDbModel>(requestObject.PostData);

                    var OldDb = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false && p.TypeId == editModel.TypeId &&
                  p.CompanyId == currentUser.CompanyID && p.DicValue == editModel.DicValue && p.ID != editModel.ID).First();

                    if (OldDb != null)
                    {
                        return ResponseUtil<TBMDictionaryEditModel, bool>.FailResult(requestObject, false,"名称:"+ editModel.DicValue+" 已经存在");
                    }
                    var dType = _db.Instance.Queryable<TBMDictionaryTypeDbModel>().Where(p => p.ID == editModel.TypeId).First();

                    if (dType != null && dType.TypeName.Contains("品牌"))
                    {
                        bool isExists = _db.Instance.Queryable<TBMDictionaryDbModel>().Any(p =>
                        SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                        p.TypeId == editModel.TypeId &&
                        p.CompanyId == currentUser.CompanyID &&
                        p.DicCode == editModel.DicCode &&p.ID!=editModel.ID);

                        if (isExists)
                        {
                            return ResponseUtil<TBMDictionaryEditModel, bool>.FailResult(requestObject, false, "编码:"+editModel.DicCode + "已经存在，不能重复");
                        }
                    }

                    editModel.UpdateId = currentUser.UserID;
                    editModel.UpdateTime = DateTime.Now;
                    result = await _db.Instance.Updateable(editModel).IgnoreColumns(p => new { p.CreateId, p.CreateTime,p.DeleteFlag,p.CompanyId }).ExecuteCommandAsync() > 0;

                   
                }

                ClearCache(currentUser);
                //返回执行结果
                if (result)
                    return ResponseUtil<TBMDictionaryEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMDictionaryEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMDictionaryEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }


        private void ClearCache(CurrentUser currentUser)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string rediskey = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);
            redis.RemoveKey(rediskey);
        }

        /// <summary>
        /// 删除T_BM_Dictionary数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                var curentDB = _db.Instance;
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "PostData、PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    List<int> idList = requestObject.PostDataList.Select(p=>p.ID).ToList();


                    result = await curentDB.Updateable<TBMDictionaryDbModel>().Where(p=> idList.Contains(p.ID)).SetColumns(p => new TBMDictionaryDbModel() { DeleteFlag = true }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    int id = requestObject.PostData.ID;


                    result = await curentDB.Updateable<TBMDictionaryDbModel>().Where(p => p.ID == id).SetColumns(p => new TBMDictionaryDbModel(){  DeleteFlag=true }).ExecuteCommandAsync() > 0;

                }
                ClearCache(currentUser);
                //返回执行结果
                if (result)
                    return ResponseUtil<DeleteModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        public string GerRandom()
        {
            return RandCodeCreate.GenerateRandomNumber(6);
        }
        
    }
}
