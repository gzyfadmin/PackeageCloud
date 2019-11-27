///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMMaterialFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7 8:49:08
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.App.Module.Warehouse.Models.TBMMaterialFile;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.App.Module.BasicSet.Models.TBMDictionary;
using YfCloud.Caches;
using Microsoft.Extensions.Caching.Memory;
using YfCloud.Framework.Orm;
using YfCloud.Framework.CacheManager;
using System.Linq.Expressions;
using YfCloud.App.Module.BasicSet.Models;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// ITBMMaterialFileService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMMaterialFileService))]
    public class TBMMaterialFileService : ITBMMaterialFileService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBMMaterialFileService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly ITBMDictionaryService _dictionaryService;
        private const string cache_Key = "TBMMaterialFile_{0}";
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TBMMaterialFileService(IDbContext dbContext, ILogger<TBMMaterialFileService> logger,IMapper mapper, ITBMDictionaryService dictionaryService, IMemoryCache memoryCache)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _dictionaryService = dictionaryService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取T_BM_MaterialFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMMaterialFileQueryModel, List<TBMMaterialFileQueryModel>>>  GetNoMemory(RequestObject<TBMMaterialFileQueryModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                var tBMDictionaryDbModel = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.DicValue);

                List<TBMMaterialFileQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TBMMaterialFileDbModel, TBMDictionaryDbModel, TBMWarehouseFileDbModel>(
                                (t, t0, t1) => new object[]
                                {
                                    JoinType.Left,  t.ColorId== t0.ID,
                                    JoinType.Left,  t.DefaultWarehouseId== t1.ID,
                                }).Where((t, t0, t1) => t.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(t.DeleteFlag, false) == false);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMMaterialFileDbModel>(item.Column);
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
						(t, t0, t1) => new TBMMaterialFileQueryModel
                        {
                            ID = t.ID,
                            MaterialCode = t.MaterialCode,
                            MaterialName = t.MaterialName,
                            Spec = t.Spec,
                            ColorId = t.ColorId,
                            ColorName = t0.DicValue,
                            MaterialTypeId = t.MaterialTypeId,
                            DefaultWarehouseId = t.DefaultWarehouseId,
                            DefaultWarehouseName = t1.WarehouseName,
                            ShelfLife = t.ShelfLife,
                            HighInventory = t.HighInventory,
                            LowInventory = t.LowInventory,
                            BaseUnitId = t.BaseUnitId,
                            ProduceUnitId = t.ProduceUnitId,
                            ProduceRate = t.ProduceRate,
                            PurchaseUnitId = t.PurchaseUnitId,
                            PurchaseRate = t.PurchaseRate,
                            SalesUnitId = t.SalesUnitId,
                            SalesRate = t.SalesRate,
                            WarehouseUnitId = t.WarehouseUnitId,
                            WarehouseRate = t.WarehouseRate,
                            Remark = t.Remark,
                            Url=t.Url,
                            ColorSolutionID = t.ColorSolutionID,
                            PackageID= t.PackageID
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
						(t, t0, t1) => new TBMMaterialFileQueryModel
                        {
                            ID = t.ID,
                            MaterialCode = t.MaterialCode,
                            MaterialName = t.MaterialName,
                            Spec = t.Spec,
                            ColorId = t.ColorId,
                            ColorName = t0.DicValue,
                            MaterialTypeId = t.MaterialTypeId,
                            DefaultWarehouseId = t.DefaultWarehouseId,
                            DefaultWarehouseName = t1.WarehouseName,
                            ShelfLife = t.ShelfLife,
                            HighInventory = t.HighInventory,
                            LowInventory = t.LowInventory,
                            BaseUnitId = t.BaseUnitId,
                            ProduceUnitId = t.ProduceUnitId,
                            ProduceRate = t.ProduceRate,
                            PurchaseUnitId = t.PurchaseUnitId,
                            PurchaseRate = t.PurchaseRate,
                            SalesUnitId = t.SalesUnitId,
                            SalesRate = t.SalesRate,
                            WarehouseUnitId = t.WarehouseUnitId,
                            WarehouseRate = t.WarehouseRate,
                            Remark = t.Remark,
                            Url = t.Url,
                            ColorSolutionID = t.ColorSolutionID,
                            PackageID = t.PackageID
                        })
                        .ToListAsync();
                }

                queryData.ForEach((x) => {
                    if (tBMDictionaryDbModel.ContainsKey(x.BaseUnitId))
                    {
                        x.BaseUnitName = tBMDictionaryDbModel[x.BaseUnitId];
                    }

                    if (x.ProduceUnitId != null && tBMDictionaryDbModel.ContainsKey(x.ProduceUnitId.Value))
                    {
                        x.ProduceUnitName = tBMDictionaryDbModel[x.ProduceUnitId.Value];
                    }

                    if (x.PurchaseUnitId != null && tBMDictionaryDbModel.ContainsKey(x.PurchaseUnitId.Value))
                    {
                        x.PurchaseUnitName = tBMDictionaryDbModel[x.PurchaseUnitId.Value];
                    }

                    if (x.SalesUnitId != null && tBMDictionaryDbModel.ContainsKey(x.SalesUnitId.Value))
                    {
                        x.SalesUnitName = tBMDictionaryDbModel[x.SalesUnitId.Value];
                    }

                    if (x.WarehouseUnitId != null && tBMDictionaryDbModel.ContainsKey(x.WarehouseUnitId.Value))
                    {
                        x.WarehouseUnitName = tBMDictionaryDbModel[x.WarehouseUnitId.Value];
                    }

                    if (tBMDictionaryDbModel.ContainsKey(x.MaterialTypeId))
                    {
                        x.MaterialTypeName = tBMDictionaryDbModel[x.MaterialTypeId];
                    }

                });

                //返回执行结果
                return ResponseUtil<TBMMaterialFileQueryModel, List<TBMMaterialFileQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TBMMaterialFileQueryModel, List<TBMMaterialFileQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 获取物料缓存
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMMaterialFileQueryModel, List<TBMMaterialFileCacheModel>>> GetAsync(RequestObject<TBMMaterialFileQueryModel> requestObject, CurrentUser currentUser)
        {
            string key = string.Format(CacheKeyString.Material, currentUser.CompanyID);
            List<TBMMaterialFileCacheModel> result;

            try
            {
                //if (!_memoryCache.TryGetValue(key, out result))
                //{
                //    var tBMMaterialFileQueryList = await GetAllAsync(currentUser);
                //    result = tBMMaterialFileQueryList;
                //    _memoryCache.Set(key, tBMMaterialFileQueryList, new TimeSpan(2, 0, 0, 0));//缓存两天
                //}

                var redis = CacheFactory.Instance(CacheType.Redis);
                result = redis.GetValueByKey<List<TBMMaterialFileCacheModel>>(key, () =>
                {
                    var tBMMaterialFileQueryList = GetAllAsync(currentUser).Result;
                    return tBMMaterialFileQueryList;
                }, 24 * 60 * 60 * 2);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    Expression<Func<TBMMaterialFileCacheModel, bool>>  queryConditionLam = (x) => true;

                    var con1 = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "materialcode" && !string.IsNullOrWhiteSpace(p.Content)).FirstOrDefault();
                    if (con1!=null)
                    {
                        requestObject.QueryConditions.Remove(con1);
                        queryConditionLam = queryConditionLam.And(x => x.MaterialCode.Contains(con1.Content) || x.MaterialName.Contains(con1.Content));
                    }

                    var con2 = requestObject.QueryConditions.Where(p => p.Column.ToLower() == "materialname" && !string.IsNullOrWhiteSpace(p.Content)).FirstOrDefault();
                    if (con2 != null)
                    {
                        requestObject.QueryConditions.Remove(con2);
                        queryConditionLam = queryConditionLam.And(x => x.MaterialCode.Contains(con2.Content) || x.MaterialName.Contains(con2.Content));
                    }

                    var conditionals = SqlSugarUtil.GetConditionalFull(requestObject.QueryConditions).Where(p => !string.IsNullOrWhiteSpace(p.FieldValue)).ToList();

                    var whereConditional = ConditionalModelToExpression.BuildExpression<TBMMaterialFileCacheModel>(conditionals);
                    queryConditionLam = queryConditionLam.And(whereConditional);
                    result = result.Where(queryConditionLam.Compile()).ToList();
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMMaterialFileDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        //query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                int totalNum = 0;
                if (requestObject.IsPaging)
                {
                    totalNum = result.Count();

                    result = result.OrderBy(p => p.ID).Skip(requestObject.PageSize * (requestObject.PageIndex - 1)).Take(requestObject.PageSize).ToList();
                }
                else
                {
                    result = result.ToList();
                }

                return ResponseUtil<TBMMaterialFileQueryModel, List<TBMMaterialFileCacheModel>>.SuccessResult(requestObject,result, totalNum);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TBMMaterialFileQueryModel, List<TBMMaterialFileCacheModel>>.FailResult(requestObject, null, ex.Message);
            }             
        }

        public async Task<List<TBMMaterialFileCacheModel>> GetAllAsync( CurrentUser currentUser,bool isContainDelete=false)
        {
            try
            {
                var tBMDictionaryDbModel = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.DicValue);

                List<TBMMaterialFileCacheModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TBMMaterialFileDbModel, TBMDictionaryDbModel, TBMWarehouseFileDbModel>(
                               (t, t0, t1) => new object[]
                               {
                                    JoinType.Left,  t.ColorId== t0.ID,
                                    JoinType.Left,  t.DefaultWarehouseId== t1.ID,
                               }).Where((t,t0,t1)=>SqlFunc.IsNull(t.DeleteFlag,false)==false);

                if (isContainDelete)
                {
                    query= query.Where((t, t0, t1) => t.CompanyId == currentUser.CompanyID);
                }
                else
                {
                    query = query.Where((t, t0, t1) => t.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(t.DeleteFlag, false) == false);
                }

                //执行查询
                queryData = await query.Select(
                         (t, t0, t1) => new TBMMaterialFileCacheModel
                         {
                             ID = t.ID,
                             MaterialCode = t.MaterialCode,
                             MaterialName = t.MaterialName,
                             Spec = t.Spec,
                             ColorId = t.ColorId,
                             ColorName = t0.DicValue,
                             MaterialTypeId = t.MaterialTypeId,
                             DefaultWarehouseId = t.DefaultWarehouseId,
                             DefaultWarehouseName = t1.WarehouseName,
                             ShelfLife = t.ShelfLife,
                             HighInventory = t.HighInventory,
                             LowInventory = t.LowInventory,
                             BaseUnitId = t.BaseUnitId,
                             ProduceUnitId = t.ProduceUnitId,
                             ProduceRate = t.ProduceRate,
                             PurchaseUnitId = t.PurchaseUnitId,
                             PurchaseRate = t.PurchaseRate,
                             SalesUnitId = t.SalesUnitId,
                             SalesRate = t.SalesRate,
                             WarehouseUnitId = t.WarehouseUnitId,
                             WarehouseRate = t.WarehouseRate,
                             Remark = t.Remark,
                             Url=t.Url,
                             ColorSolutionID=t.ColorSolutionID,
                             PackageID = t.PackageID
                         })
                         .ToListAsync();

                queryData.ForEach((x) => {
                    if (tBMDictionaryDbModel.ContainsKey(x.BaseUnitId))
                    {
                        x.BaseUnitName = tBMDictionaryDbModel[x.BaseUnitId];
                    }

                    if (x.ProduceUnitId != null && tBMDictionaryDbModel.ContainsKey(x.ProduceUnitId.Value))
                    {
                        x.ProduceUnitName = tBMDictionaryDbModel[x.ProduceUnitId.Value];
                    }


                    if (x.PurchaseUnitId != null && tBMDictionaryDbModel.ContainsKey(x.PurchaseUnitId.Value))
                    {
                        x.PurchaseUnitName = tBMDictionaryDbModel[x.PurchaseUnitId.Value];
                    }



                    if (x.SalesUnitId != null && tBMDictionaryDbModel.ContainsKey(x.SalesUnitId.Value))
                    {
                        x.SalesUnitName = tBMDictionaryDbModel[x.SalesUnitId.Value];
                    }


                    if (x.WarehouseUnitId != null && tBMDictionaryDbModel.ContainsKey(x.WarehouseUnitId.Value))
                    {
                        x.WarehouseUnitName = tBMDictionaryDbModel[x.WarehouseUnitId.Value];
                    }


                    if (tBMDictionaryDbModel.ContainsKey(x.MaterialTypeId))
                    {
                        x.MaterialTypeName = tBMDictionaryDbModel[x.MaterialTypeId];
                    }

                    if (string.IsNullOrEmpty(x.ProduceUnitName))
                    {
                        x.ProduceUnitName = x.BaseUnitName;
                    }
                    if (string.IsNullOrEmpty(x.WarehouseUnitName))
                    {
                        x.WarehouseUnitName = x.BaseUnitName;
                    }
                    if (string.IsNullOrEmpty(x.SalesUnitName))
                    {
                        x.SalesUnitName = x.BaseUnitName;
                    }
                    if (string.IsNullOrEmpty(x.ProduceUnitName))
                    {
                        x.ProduceUnitName = x.BaseUnitName;
                    }

                });

                //返回执行结果
                return queryData;
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return new List<TBMMaterialFileCacheModel>();
            }
        }

        /// <summary>
        /// 新增T_BM_MaterialFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMMaterialFileAddModel,bool>> PostAsync(RequestObject<TBMMaterialFileAddModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                var dic = BasicCacheGet.GetDic(currentUser);
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TBMMaterialFileAddModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //var addList = _mapper.Map<List<TBMMaterialFileAddModel>, List<TBMMaterialFileDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<TBMMaterialFileDbModel>(requestObject.PostData);

                    var oldModel = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                    p.CompanyId == currentUser.CompanyID && p.MaterialCode == addModel.MaterialCode).First();
                    if (oldModel != null)
                    {
                       return ResponseUtil<TBMMaterialFileAddModel, bool>.FailResult(requestObject, false, addModel.MaterialCode + " 已经存在");
                    }

                    if (addModel.ColorId != null)
                    {
                        var oldModel1 = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                                          p.CompanyId == currentUser.CompanyID && p.MaterialName == addModel.MaterialName && p.ColorId == addModel.ColorId).First();
                        if (oldModel1 != null)
                        {
                            string colorName = "此颜色";

                            var colorEntity = dic.Where(p => p.ID == addModel.ColorId).FirstOrDefault();
                            if (null != colorEntity)
                            {
                                colorName = colorEntity.DicValue;
                            }

                            return ResponseUtil<TBMMaterialFileAddModel, bool>.FailResult(requestObject, false, addModel.MaterialName + $" {colorName}已经存在");
                        }
                    }

                    addModel.CompanyId = currentUser.CompanyID;

                    result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                }
                ClearCache(currentUser);

                //返回执行结果
                if (result)
                    return ResponseUtil<TBMMaterialFileAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMMaterialFileAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMMaterialFileAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_BM_MaterialFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMMaterialFileEditModel, bool>> PutAsync(RequestObject<TBMMaterialFileEditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBMMaterialFileEditModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    ////批量更新
                    //var editList = _mapper.Map<List<TBMMaterialFileEditModel>, List<TBMMaterialFileDbModel>>(requestObject.PostDataList);
                    //result = await _db.Instance.Updateable(editList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TBMMaterialFileDbModel>(requestObject.PostData);

                    var oldModel = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p =>
                    SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                    p.CompanyId == currentUser.CompanyID &&
                    p.MaterialCode == editModel.MaterialCode && p.ID != editModel.ID).First();

                    if (oldModel != null)
                    {
                        return ResponseUtil<TBMMaterialFileEditModel, bool>.FailResult(requestObject, false, editModel.MaterialCode + " 已经存在");
                    }

                    editModel.CompanyId = currentUser.CompanyID;


                    result = await _db.Instance.Updateable(editModel).IgnoreColumns(p=>new { p.CompanyId,p.PackageID,p.ColorSolutionID}).ExecuteCommandAsync() > 0;
                }
                ClearCache(currentUser);
                //返回执行结果
                if (result)
                    return ResponseUtil<TBMMaterialFileEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMMaterialFileEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMMaterialFileEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 复制物料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> Copy(TBMMaterialFileCopyModel copyModel,CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();

                foreach(var ID in copyModel.Ids)
                {
                    TBMMaterialFileDbModel source = await _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => p.ID == ID && p.CompanyId == currentUser.CompanyID).FirstAsync();
                    if (source.PackageID != null)
                    {
                        throw new Exception("包型对应的物料，不能复制");
                    }
                    if (source != null)
                    {

                        var sameName = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => p.MaterialName.StartsWith(source.MaterialName) && p.CompanyId == currentUser.CompanyID).ToList();
                        int index = 1;

                        string aimName = string.Empty;

                        int len = source.MaterialName.Length + 4;
                        foreach (var item in sameName.Where(p => p.MaterialName != source.MaterialName).OrderBy(p => p.MaterialName))
                        {
                            if (item.MaterialName.Length <= len)
                            {
                                continue;
                            }

                            string tempName = item.MaterialName.Substring(len);

                            if (int.TryParse(tempName, out index))
                            {
                                index++;
                                aimName = string.Concat(source.MaterialName, "copy", index.ToString().PadLeft(2, '0'));

                                if (sameName.Any(p => p.MaterialName == aimName))
                                {
                                    aimName = string.Empty;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (aimName == string.Empty)
                        {
                            aimName = string.Concat(source.MaterialName, "copy01");
                        }


                        var sameCode = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => p.MaterialCode.StartsWith(source.MaterialCode) && p.CompanyId == currentUser.CompanyID).ToList();
                        int indexCode = 1;

                        string aimCode = string.Empty;

                        int lenCode = source.MaterialCode.Length + 4;
                        foreach (var item in sameCode.Where(p => p.MaterialCode != source.MaterialCode).OrderBy(p => p.MaterialCode))
                        {
                            if (item.MaterialCode.Length <= lenCode)
                            {
                                continue;
                            }
                            string tempCode = item.MaterialCode.Substring(lenCode);

                            if (int.TryParse(tempCode, out indexCode))
                            {
                                indexCode++;
                                aimCode = string.Concat(source.MaterialCode, "copy", indexCode.ToString().PadLeft(2, '0'));

                                if (sameCode.Any(p => p.MaterialCode == aimCode))
                                {
                                    aimCode = string.Empty;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (aimCode == string.Empty)
                        {
                            aimCode = string.Concat(source.MaterialCode, "copy01");
                        }

                        if (aimName.Length > 50)
                        {
                            throw new Exception($"'{source.MaterialName}'物料名称复制后大于50，不允许复制");
                        }

                        if (aimCode.Length > 20)
                        {
                            throw new Exception($"'{source.MaterialCode}'物料代码复制后大于20，不允许复制");
                        }

                        source.MaterialCode = aimCode;
                        source.MaterialName = aimName;
                        source.ID = 0;

                        _db.Instance.Insertable<TBMMaterialFileDbModel>(source).ExecuteCommand();

                      
                       
                    }
                }
                _db.Instance.CommitTran();
                ClearCache(currentUser);
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_BM_MaterialFile数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = false;

                var curentDB = _db.Instance;
                //没有删除数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "PostData、PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    List<int> idList = requestObject.PostDataList.Select(p => p.ID).ToList();


                    result = await curentDB.Updateable<TBMMaterialFileDbModel>().Where(p => idList.Contains(p.ID) && p.CompanyId== currentUser.CompanyID).SetColumns(p => new TBMMaterialFileDbModel() { DeleteFlag = true }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    int id = requestObject.PostData.ID;
                    result = await curentDB.Updateable<TBMMaterialFileDbModel>().Where(p => p.ID == id && p.CompanyId == currentUser.CompanyID).SetColumns(p => new TBMMaterialFileDbModel() { DeleteFlag = true }).ExecuteCommandAsync() > 0;

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

        public void ClearCache(CurrentUser currentUser)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string rediskey = string.Format(CacheKeyString.Material, currentUser.CompanyID);
            redis.RemoveKey(rediskey);

            string key = string.Format(cache_Key, currentUser.CompanyID);
            _memoryCache.Remove(key);
        }
    }
}
