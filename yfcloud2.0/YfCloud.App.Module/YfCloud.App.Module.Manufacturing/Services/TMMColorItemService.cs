///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMColorItemService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2 16:29:37
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
using YfCloud.App.Module.Manufacturing.Models.TMMColorItem;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// ITMMColorItemService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITMMColorItemService))]
    public class TMMColorItemService : ITMMColorItemService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TMMColorItemService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMColorItemService(IDbContext dbContext, ILogger<TMMColorItemService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_MM_ColorItem数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TMMColorItemQueryModel>>> GetAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMColorItemQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMColorItemDbModel, TBMPackageDbModel, TBMDictionaryDbModel>(
                        (t, t0, t1) => new object[]
                        {
                            JoinType.Inner , t.PackageId == t0.ID,
                            JoinType.Left , t.ItemId == t1.ID
                        }
                    ).Where((t, t0, t1) => t0.CompanyId == currentUser.CompanyID);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMColorItemDbModel>(item.Column);
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
                        (t, t0, t1) => new TMMColorItemQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            PackageCode = t0.DicCode,
                            PackageName = t0.DicValue,
                            ItemId = t.ItemId,
                            ItemName = t1.DicValue
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                        (t, t0, t1) => new TMMColorItemQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            PackageCode = t0.DicCode,
                            PackageName = t0.DicValue,
                            ItemId = t.ItemId,
                            ItemName = t1.DicValue
                        })
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TMMColorItemQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMColorItemQueryModel>>.FailResult(null, ex.Message);
            }
        }


        /// <summary>
        /// 获取T_MM_ColorItem数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <param name="currentUser">当前操作用户</param>
        /// <returns></returns>
        public async Task<ResponseObject<List<TMMColorItemQueryModel>>> GetHasColorSolutionAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TMMColorItemQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数

                var query = _db.Instance.Queryable<TMMColorItemDbModel, TBMPackageDbModel, TBMDictionaryDbModel>(
                        (t, t0, t1) => new object[]
                        {
                            JoinType.Inner , t.PackageId == t0.ID,
                            JoinType.Left , t.ItemId == t1.ID
                        }
                    ).Where((t, t0, t1) => t0.CompanyId==currentUser.CompanyID && SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().Where(p1 => p1.PackageId == t.PackageId && p1.DeleteFlag == false &&
                        SqlFunc.Subqueryable<TMMColorSolutionDetailDbModel>().Where(p2 => p2.MainId == p1.ID).Any()).Any());

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
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMColorItemDbModel>(item.Column);
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
                        (t, t0, t1) => new TMMColorItemQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            PackageCode = t0.DicCode,
                            PackageName = t0.DicValue,
                            ItemId = t.ItemId,
                            ItemName = t1.DicValue
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                        (t, t0, t1) => new TMMColorItemQueryModel
                        {
                            ID = t.ID,
                            PackageId = t.PackageId,
                            PackageCode = t0.DicCode,
                            PackageName = t0.DicValue,
                            ItemId = t.ItemId,
                            ItemName = t1.DicValue
                        })
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TMMColorItemQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMColorItemQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_MM_ColorItem数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> PostAsync(RequestPost<List<TMMColorItemAddModel>> requestObject, CurrentUser currentUser)
        {
            try
            {
//#if DEBUG
//                var i = 9;
//#else
     
//                 throw new Exception();
//#endif

                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostData.Count < 1)
                    return ResponseUtil<bool>.FailResult(false, "PostData至少包含一条数据");

                //新增数据
                var addList = _mapper.Map<List<TMMColorItemAddModel>, List<TMMColorItemDbModel>>(requestObject.PostData);
                int packageId = requestObject.PostData[0].PackageId;

                foreach (var item in addList)
                {
                    if (item.ID <= 0)
                    {
                        await _db.Instance.Insertable(item).ExecuteCommandIdentityIntoEntityAsync();
                    }
                    else
                    {
                        await _db.Instance.Updateable(item).ExecuteCommandAsync();
                    }
                }

                List<int> addListID = addList.Select(p => p.ID).ToList();

                var toDelteModel = _db.Instance.Queryable<TMMColorItemDbModel>()
                    .Where(p => p.PackageId == packageId && !addListID.Contains(p.ID)).Select(p => p).ToList();

                if (toDelteModel.Count() > 0)
                {
                    var dic = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>((t, t1) => new object[]{
                    JoinType.Inner,t.TypeId==t1.ID
                }).Where((t, t1) => t1.TypeName == "配色方案" && t1.CompanyId == currentUser.CompanyID).ToList();

                    var packageList = _db.Instance.Queryable<TBMPackageDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList();

                    foreach (var itemDel in toDelteModel)
                    {
                        if (_db.Instance.Queryable<TMMBOMMainDbModel, TMMBOMDetailDbModel>((t, t1) => new object[] { JoinType.Inner, t.ID == t1.MainId })
                            .Any((t, t1) => t.PackageId == itemDel.PackageId && t1.ItemId == itemDel.ItemId)) //此包型的BOM是否包含被删除的配色项目
                        {
                         
                            bool isExis = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>((t, t1) => new object[] {
                            JoinType.Inner,t.MainId==t1.ID}).Any((t, t1) => t1.DeleteFlag == false &&
                                 t1.MRPStatus==false && t1.AuditStatus == 2 &&
                                 t.PackageId == itemDel.PackageId
                                    ); // 是否存在没有算料的生产单

                            if (isExis)
                            {
                                string itemName = dic.Where(p => p.ID == itemDel.ItemId).FirstOrDefault()?.DicValue;
                                string packageName = packageList.Where(p => p.ID == itemDel.PackageId).FirstOrDefault()?.DicValue;
                                throw new Exception($"已审核通过的生产单，正在使用包型'{packageName}'Bom的配色项目{itemName},请将生产单算料，或者删除包型Bom的配色项目'{itemName}'");
                            }

                            //删除此包型所有的配色项目
                            var deleteColorSolution1 = _db.Instance.Deleteable<TMMColorSolutionDetailDbModel>().Where(p => p.ItemId == itemDel.ItemId &&
                              SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().Where(p1 => p1.ID == p.MainId && p1.PackageId == itemDel.PackageId).Any());
                            deleteColorSolution1.ExecuteCommand();
                            //删除BOM
                            var bomDelete = _db.Instance.Deleteable<TMMBOMDetailDbModel>().Where(p => p.ItemId == itemDel.ItemId && SqlFunc.Subqueryable<TMMBOMMainDbModel>().
                              Where(p1 => p1.ID == p.MainId && p1.PackageId == itemDel.PackageId).Any());
                            bomDelete.ExecuteCommand();
                        }
                        else
                        {
                            //删除此包型所有的配色项目
                            var deleteColorSolution = _db.Instance.Deleteable<TMMColorSolutionDetailDbModel>().Where(p => p.ItemId == itemDel.ItemId &&
                              SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().Where(p1 => p1.ID == p.MainId && p1.PackageId == itemDel.PackageId).Any());
                            deleteColorSolution.ExecuteCommand();
                        }
                    }

                    _db.Instance.Deleteable(toDelteModel).ExecuteCommand();
                }

                _db.Instance.CommitTran();
                //返回执行结果
                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 删除配色项目，单个节点
        /// </summary>
        /// <param name="requestDelete"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestDelete)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestDelete.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData至少包含一条数据");


                #region 先注释
                ////删除包型的配色项目 则删除
                //var thisModel = _db.Instance.Queryable<TMMColorItemDbModel>().Where(p => p.ID == requestDelete.PostData.ID).First();

                //if (thisModel != null)
                //{
                //    //含有这个包型 和 配色项目的BOM 
                //    List<int> ColorSolutionIds = _db.Instance.Queryable<TMMBOMMainDbModel, TMMBOMDetailDbModel>((t, t1) => new object[] { JoinType.Inner, t.ID == t1.MainId })
                //        .Where((t, t1) => t.PackageId == thisModel.PackageId && t1.ItemId == thisModel.ItemId).Select((t, t1) => t.ID).Distinct().ToList();

                //    if (ColorSolutionIds.Count() > 0)
                //    {
                //        bool isExis = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>((t, t1) => new object[] {
                //        JoinType.Inner,t.MainId==t1.ID}).Any((t, t1) => t1.DeleteFlag == false &&
                //             SqlFunc.IsNull(t1.MRPStatus, false) == false &&
                //             ColorSolutionIds.Contains(t.ColorSolutionId.Value)
                //            ); // 是否存在没有算料的生成单

                //        if (isExis)
                //        {
                //            throw new Exception("存在使用此配色项目的生产单，还没有进行算料，请将生产单算料，或者删除生产");
                //        }
                //    }

                //    //删除此包型所有的配色项目
                //  var deleteColorSolution=  _db.Instance.Deleteable<TMMColorSolutionDetailDbModel>().Where(p => p.ItemId == thisModel.ItemId &&
                //    SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().Where(p1 => p1.ID == p.MainId && p1.PackageId == thisModel.PackageId).Any());

                //    //删除BOM
                //    _db.Instance.Deleteable<TMMBOMDetailDbModel>().Where(p => p.ItemId == thisModel.ItemId && SqlFunc.Subqueryable<TMMBOMMainDbModel>().
                //    Where(p1 => p1.ID == p.MainId && p1.PackageId == thisModel.PackageId).Any());
                //}

                #endregion

                var result = await _db.Instance.Deleteable<TMMColorItemDbModel>()
                    .Where(p => p.ID == requestDelete.PostData.ID)
                    .ExecuteCommandAsync() > 0;

                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 删除配色项目，所有节点
        /// </summary>
        /// <param name="requestDelete"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> DeleteAllAsync(RequestDelete<DeleteModel> requestDelete,CurrentUser currentUser)
        {
            try
            {
                //如果没有新增数据，返回错误信息
                if (requestDelete.PostData == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData至少包含一条数据");

                var toDelteModel = _db.Instance.Queryable<TMMColorItemDbModel> ()
                    .Where(p => p.PackageId == requestDelete.PostData.ID).Select(p => p).ToList();

                var dic = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>((t, t1) => new object[]{
                    JoinType.Inner,t.TypeId==t1.ID
                }).Where((t, t1) => t1.TypeName == "配色方案" && t1.CompanyId == currentUser.CompanyID).ToList();

                var packageList = _db.Instance.Queryable<TBMPackageDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList();

                foreach (var itemDel in toDelteModel)
                {
                    if (_db.Instance.Queryable<TMMBOMMainDbModel, TMMBOMDetailDbModel>((t, t1) => new object[] { JoinType.Inner, t.ID == t1.MainId })
                        .Any((t, t1) => t.PackageId == itemDel.PackageId && t1.ItemId == itemDel.ItemId)) //此包型的BOM是否包含被删除的配色项目
                    {
                        string itemName = dic.Where(p => p.ID == itemDel.ItemId).FirstOrDefault()?.DicValue;
                        string packageName = packageList.Where(p => p.ID == itemDel.PackageId).FirstOrDefault()?.DicValue;

                        bool isExis = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>((t, t1) => new object[] {
                            JoinType.Inner,t.MainId==t1.ID}).Any((t, t1) => t1.DeleteFlag == false &&
                                 SqlFunc.IsNull(t1.MRPStatus, false) == false && t1.AuditStatus == 2 &&
                                 t.PackageId == itemDel.PackageId
                                ); // 是否存在没有算料的生产单

                        if (isExis)
                        {
                            throw new Exception($"已审核通过的生产单，正在使用包型'{packageName}'Bom的配色项目{itemName},请将生产单算料，或者删除包型Bom的配色项目'{itemName}'");
                        }

                        //删除此包型所有的配色项目
                        var deleteColorSolution = _db.Instance.Deleteable<TMMColorSolutionDetailDbModel>().Where(p => p.ItemId == itemDel.ItemId &&
                          SqlFunc.Subqueryable<TMMColorSolutionMainDbModel>().Where(p1 => p1.ID == p.MainId && p1.PackageId == itemDel.PackageId).Any());

                        //删除BOM
                        _db.Instance.Deleteable<TMMBOMDetailDbModel>().Where(p => p.ItemId == itemDel.ItemId && SqlFunc.Subqueryable<TMMBOMMainDbModel>().
                        Where(p1 => p1.ID == p.MainId && p1.PackageId == itemDel.PackageId).Any());
                    }
                }


                var result = await _db.Instance.Deleteable<TMMColorItemDbModel>()
                    .Where(p => p.PackageId == requestDelete.PostData.ID)
                    .ExecuteCommandAsync() > 0;
                //返回执行结果
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

    }
}
