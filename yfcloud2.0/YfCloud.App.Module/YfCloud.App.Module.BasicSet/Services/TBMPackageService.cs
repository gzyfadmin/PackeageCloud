///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMPackageService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5 11:12:11
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
using YfCloud.App.Module.BasicSet.Models.TBMPackage;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Caches;
using YfCloud.Framework.OrderGenerator;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.App.Module.Warehouse.Models.TBMMaterialFile;
using YfCloud.Framework.CacheManager;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// ITBMPackageService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMPackageService))]
    public class TBMPackageService : ITBMPackageService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBMPackageService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly ITBMMaterialFileService _mService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="iTBMMaterialFileService"></param>
        public TBMPackageService(IDbContext dbContext, ILogger<TBMPackageService> logger,IMapper mapper, ITBMMaterialFileService mService)

        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _mService = mService;
        }
        
        /// <summary>
        /// 获取T_BM_Package数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TBMPackageQueryModel>>> GetAsync(RequestGet requestObject,CurrentUser currentUser)
        {
            try
            {
                List<TBMPackageQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TBMPackageDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                (t,  t1, t2) => new object[]
                                {
                                    JoinType.Left,t.CreateId==t1.ID,
                                    JoinType.Left,t.UpdateId==t2.ID
                                }).Where((t, t1, t2) => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == currentUser.CompanyID);
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        item.FieldName = item.FieldName = $"t.{item.FieldName}";

                    }
                    query.Where(conditionals);
                }
                //排序条件
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMPackageDbModel>(item.Column);
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
                        .Select((t, t1, t2) => new TBMPackageQueryModel
                        {
                            ID = t.ID,
                            DicCode = t.DicCode,
                            DicValue = t.DicValue,
                            Remark = t.Remark,
                            CreateTime = t.CreateTime,
                            CreateId = t.CreateId,
                            CreateName = t1.AccountName,
                            UpdateTime = t.UpdateTime,
                            UpdateId = t.UpdateId,
                            UpdateName = t2.AccountName,
                            ImgPath = t.ImgPath,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query
                        .Where(t => t.DeleteFlag == false)
                        .Select((t, t1, t2) => new TBMPackageQueryModel
                        {
                            ID = t.ID,
                            DicCode = t.DicCode,
                            DicValue = t.DicValue,
                            Remark = t.Remark,
                            CreateTime = t.CreateTime,
                            CreateId = t.CreateId,
                            CreateName = t1.AccountName,
                            UpdateTime = t.UpdateTime,
                            UpdateId = t.UpdateId,
                            UpdateName = t2.AccountName,
                            ImgPath = t.ImgPath,
                        })
                        .ToListAsync();
                }


                //返回执行结果
                return ResponseUtil<List<TBMPackageQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TBMPackageQueryModel>>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 新增T_BM_Package数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TBMPackageAddModel> requestObject,CurrentUser currentUser)
        {
            try
            {
                var allDic = BasicCacheGet.GetDic(currentUser);
                var dicUnit= allDic.Where(p => p.TypeName == "计量单位" && p.DicValue == "个").FirstOrDefault();
                if (dicUnit == null)
                {
                    return ResponseUtil<bool>.FailResult(false, "请设置'个'的计量单位 ");
                }

                var MaterialTypeDic = allDic.Where(p => p.TypeName == "物料分类" && p.DicValue == "产品").FirstOrDefault();
                if (MaterialTypeDic == null)
                {
                    return ResponseUtil<bool>.FailResult(false, "请设置'产品'的物料分类 ");
                }


                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能都为null");
                var result = false;

                _db.Instance.BeginTran();
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                  
                }
                else
                {
                    bool isExistsName = _db.Instance.Queryable<TBMPackageDbModel>().Any(p => p.DicValue == requestObject.PostData.DicValue
                    &&p.DeleteFlag==false&&
                    p.CompanyId==currentUser.CompanyID);

                    if (isExistsName)
                    {
                        return ResponseUtil<bool>.FailResult(false, "包型名称已经存在");
                    }

                    bool isExistCode= _db.Instance.Queryable<TBMPackageDbModel>().Any(p => p.DicCode == requestObject.PostData.DicCode
                     && p.DeleteFlag == false &&
                     p.CompanyId == currentUser.CompanyID);

                    if (isExistCode)
                    {
                        return ResponseUtil<bool>.FailResult(false, "包型编码已经存在");
                    }

                  

                   

                    //生成包型
                    var addModel = _mapper.Map<TBMPackageDbModel>(requestObject.PostData);
                    addModel.CompanyId = currentUser.CompanyID;
                    addModel.DeleteFlag = false;
                    addModel.CreateId = currentUser.UserID;
                    addModel.CreateTime = DateTime.Now;
                    int packageID =  _db.Instance.Insertable(addModel).ExecuteReturnIdentity() ;

                    //生成物料
                    TBMMaterialFileDbModel material = new TBMMaterialFileDbModel();
                    material.MaterialCode = CreateNo(currentUser.CompanyID);
                    material.BaseUnitId = dicUnit.ID;
                    material.MaterialTypeId = MaterialTypeDic.ID;
                    material.MaterialName = requestObject.PostData.DicValue;
                    material.DeleteFlag = false;
                    material.CompanyId = currentUser.CompanyID;
                    material.Remark = "无配色方案";
                    material.PackageID = packageID;

                    int MaterialId = _db.Instance.Insertable(material).ExecuteReturnIdentity();

                    _db.Instance.CommitTran();

                   _mService.ClearCache(currentUser);
                }
                //返回执行结果
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch(Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        

        /// <summary>
        /// 修改T_BM_Package数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TBMPackageEditModel> requestObject,CurrentUser currentUser)
        {
            try
            {
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    
                }
                else
                {
                    bool isExistsName = _db.Instance.Queryable<TBMPackageDbModel>().Any(p => p.DicValue == requestObject.PostData.DicValue
                   && p.DeleteFlag == false &&
                   p.CompanyId == currentUser.CompanyID &&p.ID!=requestObject.PostData.ID);

                    if (isExistsName)
                    {
                        return ResponseUtil<bool>.FailResult(false, "包型名称已经存在");
                    }

                    bool isExistCode = _db.Instance.Queryable<TBMPackageDbModel>().Any(p => p.DicCode == requestObject.PostData.DicCode
                      && p.DeleteFlag == false &&
                      p.CompanyId == currentUser.CompanyID && p.ID != requestObject.PostData.ID);

                    if (isExistCode)
                    {
                        return ResponseUtil<bool>.FailResult(false, "包型编码已经存在");
                    }

                    //单记录更新
                    var editModel = _mapper.Map<TBMPackageDbModel>(requestObject.PostData);
                    editModel.UpdateId = currentUser.UserID;
                    editModel.UpdateTime = DateTime.Now;
                    result = await _db.Instance.Updateable(editModel).IgnoreColumns(p=>new {p.CompanyId,p.DeleteFlag,p.CreateId,p.CreateTime}).ExecuteCommandAsync() > 0;
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
        /// 删除T_BM_Package数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject,CurrentUser currentUser)
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
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();
                    await _db.Instance.Updateable<TBMPackageDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID) &&p.CompanyId==currentUser.CompanyID)
                        .ExecuteCommandAsync();
                }
                else
                {
                    //单条删除
                    await _db.Instance.Updateable<TBMPackageDbModel>()
                       .SetColumns(p => p.DeleteFlag == true)
                       .Where(p => p.ID == requestObject.PostData.ID && p.CompanyId == currentUser.CompanyID)
                       .ExecuteCommandAsync();
                }
                //返回执行结果
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        private  string CreateNo(int CompanyID)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string key = string.Format(CacheKeyString.LockMaterialCode, CompanyID);
            string datekey = string.Format(CacheKeyString.LockMaterialCode_date, CompanyID);


            var result = redis.LockRelease(key, TimeSpan.FromSeconds(5), () =>
            {
                DateSno dateSno = null;
                if (redis.ContainsKey(datekey))
                {
                    dateSno = redis.GetValueByKey<DateSno>(datekey); //获取redis 里面存储的日期
                }

                string nowStr = DateTime.Now.ToString("yyyyMMdd"); //当前日期
                string nowStrsss= DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(2); //当前日期

                if (dateSno != null)
                {
                    if (dateSno.DateStr != nowStr) //不是当天
                    {
                        dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                        redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                    }
                    else //当前 流水号+1
                    {
                        dateSno.SNO = dateSno.SNO + 1;

                        redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                    }
                }
                else
                {
                    dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                    redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                }


                return nowStrsss + dateSno.SNO.ToString().PadLeft(4, '0');
            });

            return result;
        }

    }
}
