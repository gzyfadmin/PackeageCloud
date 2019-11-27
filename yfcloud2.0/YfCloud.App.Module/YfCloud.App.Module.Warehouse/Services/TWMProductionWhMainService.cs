///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMProductionWhMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16 13:42:00
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
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.UnitChange;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// 生产入库
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITWMProductionWhMainService))]
    public class TWMProductionWhMainService : ITWMProductionWhMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMProductionWhMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMProductionWhMainService(IDbContext dbContext, ILogger<TWMProductionWhMainService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_WM_ProductionWhMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMProductionWhMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMProductionWhMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMProductionWhMainDbModel, TMMWhApplyMainDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TMMProductionOrderMainDbModel>((t, t1, t2, t3, t4, t5, t6) => new object[] {
                    JoinType.Inner,t.SourceId==t1.ID,
                    JoinType.Left, t.OperatorId == t2.ID,
                    JoinType.Left, t.WhAdminId == t3.ID,
                    JoinType.Left,t.AuditId==t4.ID,
                    JoinType.Left,t.ReceiptId==t5.ID,
                    JoinType.Left,t1.SourceId==t6.ID
                }).Where((t, t1, t2, t3, t4, t5, t6) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);

                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        item.FieldName = $"t.{item.FieldName}";
                    }
                    query.Where(conditionals);
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMProductionWhMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t1, t2, t3, t4, t5, t6) => new TWMProductionWhMainQueryModel
                    {
                        ID = t.ID,
                        WarehousingType = t.WarehousingType,
                        WarehousingDate = t.WarehousingDate,
                        WarehousingOrderNo = t.WarehousingOrderNo,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t2.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t5.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t3.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t4.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        SourceCode = t1.WhApplyNo,
                        Number = t.Number,
                        Amount = t.Amount,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false,
                        ProductionNo = t6.ProductionNo,
                        ProductionID = t6.ID
                    })
                                            .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t1, t2, t3, t4, t5, t6) => new TWMProductionWhMainQueryModel
                    {
                        ID = t.ID,
                        WarehousingType = t.WarehousingType,
                        WarehousingDate = t.WarehousingDate,
                        WarehousingOrderNo = t.WarehousingOrderNo,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t2.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t5.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t3.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t4.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        SourceCode = t1.WhApplyNo,
                        Number = t.Number,
                        Amount = t.Amount,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false,
                        ProductionNo = t6.ProductionNo,
                        ProductionID = t6.ID
                    })
                                           .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TWMProductionWhMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMProductionWhMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
        {
            try
            {
                RequestGet requestGet = new RequestGet()
                {
                    IsPaging = false,
                    QueryConditions = new List<QueryCondition>() {
                    new QueryCondition(){ Column="Id", Condition= ConditionEnum.Equal, Content= iMainId.ToString() }
                }
                };

                var allMain = await GetMainListAsync(requestGet, currentUser);


                var mainModel = allMain.Data.FirstOrDefault();

                var materList = BasicCacheGet.GetMaterial(currentUser);
                var dicList = BasicCacheGet.GetDic(currentUser);

                var detailModels = await _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel,
                    TBMMaterialFileDbModel, TMMWhApplyMainDbModel, TMMWhApplyDetailDbModel,
                    TMMProductionOrderDetailDbModel, TMMColorSolutionMainDbModel, TBMPackageDbModel, TBMDictionaryDbModel, TSMUserAccountDbModel, TBMDictionaryDbModel>(
                    (t, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => new object[] {
                      JoinType.Inner , t.MainId == t0.ID,
                      JoinType.Inner , t.MaterialId == t1.ID,
                      JoinType.Inner,t0.SourceId==t2.ID,
                      JoinType.Inner,t.ProOrderDetailId==t3.ID ,
                      JoinType.Inner,t3.ProOrderDetailId== t4.ID,
                      JoinType.Left,t4.ColorSolutionId==t5.ID,
                      JoinType.Left,t4.PackageId==t6.ID,
                      JoinType.Left,t.WorkshopId==t7.ID,
                      JoinType.Left,t4.PrincipalId==t8.ID,
                      JoinType.Left,t.SiteId==t9.ID
                    }
                    ).Select((t, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => new TWMProductionWhDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialName = t1.MaterialName,
                        MaterialCode = t1.MaterialCode,
                        WarehouseId = t.WarehouseId,
                        ActualNum = t.ActualNum,
                        ProActualNum=t.ProActualNum,
                        UnitPrice = t.UnitPrice,
                        Amount = t.Amount,
                        MaterialTypeId = t1.MaterialTypeId,
                        //MaterialTypeName = t1.TypeName,
                        ColorId = t1.ColorId,
                        //ColorName = t2.DicValue,
                        BaseUnitId = t1.BaseUnitId,
                        //BaseUnitName = t3.DicValue,
                        WarehouseUnitId = t1.WarehouseUnitId,
                        //WarehouseUnitName = t4.DicValue,
                        WarehouseRate = t1.WarehouseRate,
                        ProduceRate = t1.ProduceRate,
                        ProduceUnitId = t1.ProduceUnitId,
                        //ProduceUnitName = t5.DicValue,
                        Spec = t1.Spec,
                        Remark = t.Remark,
                        SourceCode = t2.WhApplyNo,
                        TransNum = t3.TransNum,
                        PackageId = t4.PackageId,
                        ColorSolutionId = t4.ColorSolutionId,
                        ColorSolutionName = t5.SolutionCode,
                        PackageCode = t6.DicCode,
                        PackageName = t6.DicValue,
                        WorkshopName = t7.DicValue,
                        PrincipalName = t8.AccountName,
                        ProOrderDetailId = t.ProOrderDetailId,
                        SiteName = t9.DicValue
                    })
                                    .Where(t => t.MainId == iMainId)
                                    .ToListAsync();


                detailModels.ForEach((x) =>
                {

                    var thisMaterial = materList.Where(p => p.ID == x.MaterialId).FirstOrDefault();

                    if (mainModel.AuditStatus != 2)
                    {

                        x.TransNum = x.TransNum + x.ProActualNum;
                    }

                    x.MaterialTypeName = thisMaterial.MaterialTypeName;
                    x.ColorName = thisMaterial.ColorName;
                    x.BaseUnitName = thisMaterial.BaseUnitName;
                    x.WarehouseUnitName = thisMaterial.WarehouseUnitName;
                    x.ProduceUnitName = thisMaterial.ProduceUnitName;
                });


                mainModel.ChildList = detailModels;
                return ResponseUtil<TWMProductionWhMainQueryModel>.SuccessResult(mainModel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null);
            }
        }

        /// <summary>
        /// 新增T_WM_ProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> PostAsync(RequestPost<TWMProductionWhMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMProductionWhMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                mapMainModel.AuditStatus = 0;
                mapMainModel.WarehousingType = 1;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();

                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMProductionWhDetailAddModel>, List<TWMProductionWhDetailDbModel>>(requestObject.PostData.ChildList);


                var listItem = _db.Instance.Queryable<TMMWhApplyDetailDbModel>().Where(t => t.MainId == mapMainModel.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TMMWhApplyDetailDbModel> toEdit = new List<TMMWhApplyDetailDbModel>();

                foreach (var item in mapDetailModelList)
                {
                    var procudeMaterial = listItem.Where(p => p.ID == item.ProOrderDetailId).FirstOrDefault();//生产单明细

                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();//物料

                    decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, procudeMaterial.TransNum);//可转的数量（仓库单位）
                    decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);//实际转数量（生产单位）

                    if (item.ActualNum > wareTransNum)
                    {
                        throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                    }
                    else if (item.ActualNum == wareTransNum)
                    {
                        item.ProActualNum = procudeMaterial.TransNum;
                        procudeMaterial.TransNum = 0;
                    }
                    else
                    {
                        if (proNum > procudeMaterial.TransNum)
                        {
                            item.ProActualNum = procudeMaterial.TransNum;
                            procudeMaterial.TransNum = 0;
                        }
                        else
                        {
                            item.ProActualNum = proNum;
                            procudeMaterial.TransNum = procudeMaterial.TransNum - proNum;
                        }
                    }

                    toEdit.Add(procudeMaterial);
                }

                if (toEdit != null && toEdit.Count() > 0)
                {
                    _db.Instance.Updateable<TMMWhApplyDetailDbModel>(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TMMWhApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mapMainModel.SourceId))
                {
                    _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mapMainModel.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                }

                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;

                //提交事务
                currDb.CommitTran();

                var res = await GetWholeMainData(mainId, currentUser);
                //返回执行结果
                return ResponseUtil<TWMProductionWhMainQueryModel>.SuccessResult(res.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_WM_ProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMProductionWhMainQueryModel>> PutAsync(RequestPut<TWMProductionWhMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TWMProductionWhMainDbModel>(requestObject.PostData);
                //修改主表信息
                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);

                var mainFlag = await currDb.Updateable(mainModel).UpdateColumns(p => new
                {
                    p.WarehousingDate,
                    p.Number,
                    p.Amount,
                    p.WhAdminId,
                    p.ReceiptId

                }).ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */

                var mainEntity = _db.Instance.Queryable<TWMProductionWhMainDbModel>().Where(P => P.ID == requestObject.PostData.ID).First();

                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMProductionWhDetailEditModel>,
                    List<TWMProductionWhDetailDbModel>>(requestObject.PostData.ChildList);

                var listItem = _db.Instance.Queryable<TMMWhApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TMMWhApplyDetailDbModel> toEdit = new List<TMMWhApplyDetailDbModel>();


                foreach (var item in detailModels)
                {
                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    var procudeMaterial = listItem.Where(p => p.ID == item.ProOrderDetailId).FirstOrDefault();//生产单明细
                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;

                    if (item.ID <= 0) //新增
                    {

                        decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);//实际转出数量（生产）

                        decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, procudeMaterial.TransNum);//可转的数量（仓库单位）

                        if (item.ActualNum > wareTransNum)
                        {
                            throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                        }
                        else if (item.ActualNum == wareTransNum)
                        {
                            item.ProActualNum = procudeMaterial.TransNum;
                            procudeMaterial.TransNum = 0;
                        }
                        else
                        {
                            if (proNum > procudeMaterial.TransNum)
                            {
                                item.ProActualNum = procudeMaterial.TransNum;
                                procudeMaterial.TransNum = 0;
                            }
                            else
                            {
                                item.ProActualNum = proNum;
                                procudeMaterial.TransNum = procudeMaterial.TransNum - proNum;
                            }
                        }

                        toEdit.Add(procudeMaterial);


                    }
                    else //修改
                    {
                        var dModel = _db.Instance.Queryable<TWMProductionWhDetailDbModel>().Where(p => p.ID == item.ID).First();
                        if (dModel != null)
                        {
                            if (dModel.ActualNum != item.ActualNum)
                            {

                                decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);//实际转出数量（生产单位）

                                decimal TransNum = procudeMaterial.TransNum + dModel.ProActualNum; //可转数量（生产单位）

                                decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, TransNum);//可转的数量（仓库单位）

                                if (item.ActualNum > wareTransNum)
                                {
                                    throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                                }
                                else if (item.ActualNum == wareTransNum)
                                {
                                    item.ProActualNum = TransNum;
                                    procudeMaterial.TransNum = 0;
                                }
                                else
                                {
                                    if (proNum > TransNum)
                                    {
                                        item.ProActualNum = TransNum;
                                        procudeMaterial.TransNum = 0;
                                    }
                                    else
                                    {
                                        item.ProActualNum = proNum;
                                        procudeMaterial.TransNum = TransNum - proNum;
                                    }
                                }

                                toEdit.Add(procudeMaterial);
                            }
                            else
                            {
                                item.ProActualNum = dModel.ProActualNum;
                            }
                        }
                       
                    }

                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;


                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();
                    var whDetailList = currDb.Queryable<TWMProductionWhDetailDbModel>().Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID).ToList();

                    foreach (var item in whDetailList)
                    {
                        var dModel = _db.Instance.Queryable<TWMProductionWhDetailDbModel>().Where(p => p.ID == item.ID).First();
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        var procudeMaterial = listItem.Where(p => p.ID == item.ProOrderDetailId).FirstOrDefault();//生产单明细

                        procudeMaterial.TransNum = procudeMaterial.TransNum + dModel.ProActualNum;
                        toEdit.Add(procudeMaterial);
                    }

                    detailFlag = currDb.Deleteable<TWMProductionWhDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                if (toEdit != null && toEdit.Count() > 0)
                {
                    _db.Instance.Updateable<TMMWhApplyDetailDbModel>(toEdit).ExecuteCommand();
                }



                if (!_db.Instance.Queryable<TMMWhApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                {
                    _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                }
                else
                {
                    _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                }



                //提交事务
                currDb.CommitTran();

                var res = await GetWholeMainData(mainModel.ID, currentUser);
                //返回执行结果
                return ResponseUtil<TWMProductionWhMainQueryModel>.SuccessResult(res.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMProductionWhMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_WM_ProductionWhMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser)
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

                var materList = BasicCacheGet.GetMaterial(currentUser);



                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();

                    foreach (int ids in mainIds)
                    {
                        List<TMMWhApplyDetailDbModel> toEdit = new List<TMMWhApplyDetailDbModel>();
                        var mainEntity = _db.Instance.Queryable<TWMProductionMainDbModel>().Where(P => P.ID == ids).First();

                        var listItem = _db.Instance.Queryable<TMMWhApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                        var whDetailList = currDb.Queryable<TWMProductionWhDetailDbModel>().Where(p => p.MainId == ids).ToList();

                        foreach (var item in whDetailList)
                        {
                            var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                            var m = listItem.Where(p => p.ID == item.ProOrderDetailId).FirstOrDefault();
                            m.TransNum = m.TransNum + item.ProActualNum;
                            toEdit.Add(m);


                        }
                        if (toEdit != null && toEdit.Count() > 0)
                        {
                            _db.Instance.Updateable<TMMWhApplyDetailDbModel>(toEdit).ExecuteCommand();
                        }

                        if (!_db.Instance.Queryable<TMMWhApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                        {
                            _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                        }
                        else
                        {
                            _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                        }
                    }



                    mainFlag = await _db.Instance.Updateable<TWMProductionWhMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    List<TMMWhApplyDetailDbModel> toEdit = new List<TMMWhApplyDetailDbModel>();

                    var mainEntity = _db.Instance.Queryable<TWMProductionWhMainDbModel>().Where(P => P.ID == requestObject.PostData.ID).First();

                    var listItem = _db.Instance.Queryable<TMMWhApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                    var whDetailList = currDb.Queryable<TWMProductionWhDetailDbModel>().Where(p => p.MainId == requestObject.PostData.ID).ToList();

                    foreach (var item in whDetailList)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();

                        var m = listItem.Where(p => p.ID == item.ProOrderDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransNum = m.TransNum + item.ProActualNum;
                            toEdit.Add(m);
                        }


                    }

                    if (toEdit != null && toEdit.Count() > 0)
                    {
                        _db.Instance.Updateable<TMMWhApplyDetailDbModel>(toEdit).ExecuteCommand();
                    }

                    if (!_db.Instance.Queryable<TMMWhApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                    {
                        _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                    }
                    else
                    {
                        _db.Instance.Updateable<TMMWhApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                    }

                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TWMProductionWhMainDbModel>()
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

        /// <summary>
        /// 出库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMProductionAduit> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMProductionWhMainDbModel>()
                    .SetColumns(p => new TWMProductionWhMainDbModel
                    {
                        AuditStatus = requestObject.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestObject.PostData.ID && (p.AuditStatus == 0 || p.AuditStatus == 1))
                    .ExecuteCommandAsync() > 0;
                //审核通过后统计当前入库单的数量和金额
                if (result && requestObject.PostData.AuditStatus == 2)
                {
                    OtherWhCountAsync(requestObject.PostData.ID, currentUser);
                }

                _db.Instance.CommitTran();
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "审核数据失败，该数据可能已审核!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 审核通过后统计当前入库单信息
        /// </summary>
        /// <param name="iMainId"></param>
        private async void OtherWhCountAsync(int iMainId, CurrentUser currentUser)
        {
            var mainEntity = _db.Instance.Queryable<TWMProductionWhMainDbModel>().Where(p => p.ID == iMainId).First();

            //获取入库明细
            var details = await _db.Instance.Queryable<TWMProductionWhDetailDbModel>()
                .Where(p => p.MainId == iMainId)
                .ToListAsync();

            //更新入库数量
            var detailList = details.Select(p => new TWMProductionCountDbModel
            {
                MaterialId = p.MaterialId,
                WarehouseId = p.WarehouseId,
                WhNumber = p.ActualNum
            }).ToList();

            foreach (var item in detailList)
            {
                var updateFlag = _db.Instance.Updateable(item)
                    .SetColumns(p => p.WhNumber == p.WhNumber + item.WhNumber)
                    .Where(p => p.MaterialId == item.MaterialId && p.WarehouseId == item.WarehouseId)
                    .ExecuteCommand() > 0;
                if (!updateFlag)
                    _db.Instance.Insertable(item).ExecuteCommand();
            }
        }
    }
}
