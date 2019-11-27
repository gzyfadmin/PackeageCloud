///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMColorSolutionMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:29:58
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
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using YfCloud.Framework.CacheManager;
using YfCloud.Caches;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMColorSolutionMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITMMColorSolutionMainService))]
    public class TMMColorSolutionMainService : ITMMColorSolutionMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMColorSolutionMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMColorSolutionMainService(IDbContext dbContext, ILogger<TMMColorSolutionMainService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_MM_ColorSolutionMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<DataTable>> GetAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMColorSolutionMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMColorSolutionMainDbModel>().Where(t => t.CompanyId == currentUser.CompanyID && t.DeleteFlag == false);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMColorSolutionMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Where(t => !t.DeleteFlag)
                        .Select((t) => new TMMColorSolutionMainQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            SolutionCode = t.SolutionCode,
                            ImagePath = t.ImagePath,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Where(t => !t.DeleteFlag)
                        .Select((t) => new TMMColorSolutionMainQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            SolutionCode = t.SolutionCode,
                            ImagePath = t.ImagePath,
                        })
                        .ToListAsync();
                }

                List<TMMColorSolutionDetailQueryModel> detailList = null;
                if (queryData != null && queryData.Count > 0)
                {
                    detailList = await _db.Instance.Queryable<TMMColorSolutionDetailDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                            (t, t0, t1) => new object[]
                            {
                            JoinType.Left , t.ItemId == t0.ID,
                            JoinType.Left , t.ColorId == t1.ID
                            }
                        )
                        .In(t => t.MainId, queryData.Select(p => p.ID).ToArray())
                        .Select((t, t0, t1) => new TMMColorSolutionDetailQueryModel
                        {
                            ID = t.ID,
                            MainId = t.MainId,
                            ItemId = t.ItemId,
                            ItemName = t0.DicValue,
                            ColorId = t.ColorId,
                            ColorName = t1.DicValue
                        })
                        .ToListAsync();
                }
                var returnTable = GetReturnDataTable(queryData, detailList);
                //返回执行结果
                return ResponseUtil<DataTable>.SuccessResult(returnTable, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<DataTable>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<List<TMMColorSolutionMainQueryModel>>> GetColorSolution(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMColorSolutionMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMColorSolutionMainDbModel>().Where(t => t.CompanyId == currentUser.CompanyID && t.DeleteFlag == false);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMColorSolutionMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Where(t => !t.DeleteFlag)
                        .Select((t) => new TMMColorSolutionMainQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            SolutionCode = t.SolutionCode,
                            ImagePath = t.ImagePath,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Where(t => !t.DeleteFlag)
                        .Select((t) => new TMMColorSolutionMainQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            SolutionCode = t.SolutionCode,
                            ImagePath = t.ImagePath,
                        })
                        .ToListAsync();
                }


                //返回执行结果
                return ResponseUtil<List<TMMColorSolutionMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMColorSolutionMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        private DataTable GetReturnDataTable(List<TMMColorSolutionMainQueryModel> queryData, List<TMMColorSolutionDetailQueryModel> detailList)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("PackageId");
            dt.Columns.Add("SolutionCode");
            dt.Columns.Add("ImagePath");
            foreach (var item in queryData)
            {
                //添加动态列
                foreach (var item2 in detailList.Where(p => p.MainId == item.ID))
                {
                    if (!dt.Columns.Contains($"itemid{item2.ItemId}"))
                        dt.Columns.Add($"itemid{item2.ItemId}");
                    if (!dt.Columns.Contains($"itemcaption{item2.ItemId}"))
                        dt.Columns.Add($"itemcaption{item2.ItemId}");
                }
                var newRow = dt.NewRow();
                newRow["ID"] = item.ID;
                newRow["PackageId"] = item.PackageId;
                newRow["SolutionCode"] = item.SolutionCode;
                newRow["ImagePath"] = item.ImagePath;
                foreach (var item2 in detailList.Where(p => p.MainId == item.ID))
                {
                    newRow[$"itemid{item2.ItemId}"] = item2.ColorId;
                    newRow[$"itemcaption{item2.ItemId}"] = item2.ColorName;
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        /// <summary>
        /// 新增T_MM_ColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<TMMColorSolutionMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");


                var allDic = BasicCacheGet.GetDic(currentUser);
                var dicUnit = allDic.Where(p => p.TypeName == "计量单位" && p.DicValue == "个").FirstOrDefault();
                if (dicUnit == null)
                {
                    return ResponseUtil<bool>.FailResult(false, "请设置'个'的计量单位 ");
                }

                var MaterialTypeDic = allDic.Where(p => p.TypeName == "物料分类" && p.DicValue == "产品").FirstOrDefault();
                if (MaterialTypeDic == null)
                {
                    return ResponseUtil<bool>.FailResult(false, "请设置'产品'的物料分类 ");
                }

                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TMMColorSolutionMainDbModel>(requestObject.PostData);
                if (_db.Instance.Queryable<TMMColorSolutionMainDbModel>().Any(p => p.SolutionCode == requestObject.PostData.SolutionCode && p.PackageId == requestObject.PostData.PackageId
                && p.DeleteFlag == false && p.CompanyId == currentUser.CompanyID))
                {
                    throw new Exception("编号重复");
                }

                mapMainModel.CompanyId = currentUser.CompanyID;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();

                TBMPackageDbModel tBMPackageDbModel = _db.Instance.Queryable<TBMPackageDbModel>().Where(p => p.ID == requestObject.PostData.PackageId).First();

                //生成物料
                TBMMaterialFileDbModel material = new TBMMaterialFileDbModel();
                material.MaterialCode = CreateNo(currentUser.CompanyID);
                material.BaseUnitId = dicUnit.ID;
                material.MaterialTypeId = MaterialTypeDic.ID;
                material.MaterialName = tBMPackageDbModel.DicValue;
                material.DeleteFlag = false;
                material.CompanyId = currentUser.CompanyID;
                material.ColorSolutionID = mainId;
                material.PackageID = requestObject.PostData.PackageId;
                material.Remark = requestObject.PostData.SolutionCode;

                _db.Instance.Insertable(material).ExecuteReturnIdentity();

                ClearCache(currentUser);//更新缓存

                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TMMColorSolutionDetailAddModel>, List<TMMColorSolutionDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_MM_ColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TMMColorSolutionMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<bool>.FailResult(false, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TMMColorSolutionMainDbModel>(requestObject.PostData);
                if (_db.Instance.Queryable<TMMColorSolutionMainDbModel>().Any(p => p.SolutionCode == mainModel.SolutionCode && p.PackageId == mainModel.PackageId &&
                p.CompanyId == currentUser.CompanyID && p.ID != mainModel.ID && p.DeleteFlag == false))
                {
                    throw new Exception("编号重复");
                }
                var mainFlag = await currDb.Updateable(mainModel).IgnoreColumns(p => new { p.CompanyId }).ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TMMColorSolutionDetailEditModel>,
                    List<TMMColorSolutionDetailDbModel>>(requestObject.PostData.ChildList);
                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;
                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();
                    detailFlag = currDb.Deleteable<TMMColorSolutionDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_MM_ColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData、PostDataList不能都为null");
                //开启事务
                currDb.BeginTran();
                //删除标识
                var mainFlag = true;
                var detailFlag = true;
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();
                    mainFlag = await _db.Instance.Updateable<TMMColorSolutionMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TMMColorSolutionMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
                }
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        private string CreateNo(int CompanyID)
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
                string nowStrsss = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(2); //当前日期

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

        public void ClearCache(CurrentUser currentUser)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string rediskey = string.Format(CacheKeyString.Material, currentUser.CompanyID);
            redis.RemoveKey(rediskey);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="PackageID"></param>
        /// <param name="ColorSolutionID"></param>
        /// <returns></returns>
        public TBMMaterialFileDbModel GetMaterialFileByPackageColor(int PackageID, int? ColorSolutionID)
        {
            if (ColorSolutionID != null)
            {
                var resutl = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TBMMaterialFileDbModel>((t, t1) => new object[] {
                JoinType.Inner,t.ID==t1.ColorSolutionID && t.PackageId==t1.PackageID
            }).Where((t, t1) => t.PackageId == PackageID && t.ID == ColorSolutionID).Select((t, t1) => t1).First();

                return resutl;
            }
            else
            {
                var resutl = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TBMMaterialFileDbModel>((t, t1) => new object[] {
                JoinType.Inner,t.PackageId==t1.PackageID
            }).Where((t, t1) => t.PackageId == PackageID && SqlFunc.IsNullOrEmpty(t1.ColorSolutionID)).Select((t, t1) => t1).First();

                return resutl;
            }

        }
    }
}

