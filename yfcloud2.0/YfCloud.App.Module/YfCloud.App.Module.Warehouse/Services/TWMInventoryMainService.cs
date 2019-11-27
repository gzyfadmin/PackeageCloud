///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMInventoryMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13 15:42:54
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
using YfCloud.App.Module.Warehouse.Models.TWMInventoryMain;
using YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.OrderGenerator;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// ITWMInventoryMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITWMInventoryMainService))]
    public class TWMInventoryMainService : ITWMInventoryMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMInventoryMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMInventoryMainService(IDbContext dbContext, ILogger<TWMInventoryMainService> logger, IMapper mapper, IEnumerable<ICodeMaker> codeMakers)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _codeMakers = codeMakers;
        }

        /// <summary>
        /// 盘点单审核
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> InventoryAuditAsync(RequestPut<TWMInventoryAuditModel> request, CurrentUser currentUser)
        {
            try
            {
                var flag = await _db.Instance.Updateable<TWMInventoryMainDbModel>()
                    .SetColumns(p => new TWMInventoryMainDbModel
                    {
                        AuditId = currentUser.UserID,
                        AuditStatus = request.PostData.AuditStatus,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == request.PostData.ID && (SqlFunc.IsNullOrEmpty(p.AuditStatus) || p.AuditStatus != 2))
                    .ExecuteCommandAsync() > 0;

                //盘点单审核通过后，生成盘盈入库单或盘亏入库单
                if (flag && request.PostData.AuditStatus == 2)
                {
                    CreateProfitOrDeficitOrder(request.PostData.ID, currentUser);
                }

                return flag
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "审核数据失败，该数据可能已审核");

            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// 生成盘盈入库单或盘亏出库单
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currUser"></param>
        private async void CreateProfitOrDeficitOrder(int iMainId, CurrentUser currUser)
        {
            var inventoryModel = await _db.Instance.Queryable<TWMInventoryMainDbModel>().Where(p => p.ID == iMainId).FirstAsync();
            inventoryModel.ChildList = await _db.Instance.Queryable<TWMInventoryDetailDbModel>().Where(p => p.MainId == iMainId).ToListAsync();
            CreateProfitOrder(inventoryModel, currUser);
            CreateDeficitOrder(inventoryModel, currUser);
        }

        /// <summary>
        /// 生成盘盈入库单
        /// </summary>
        /// <param name="mainModel"></param>
        /// <param name="currUser"></param>
        private async void CreateProfitOrder(TWMInventoryMainDbModel mainModel, CurrentUser currUser)
        {
            try
            {
                if (mainModel.ChildList.Count(p => p.ProfitNum > 0) < 1) return;
                var profitMainModel = new TWMProfitMainDbModel
                {
                    ID = 0,
                    WarehousingType = 4,
                    WarehousingDate = mainModel.InventoryDate,
                    WarehousingOrder = _codeMakers.Where(p=>p.ProvideName== OrderEnum.IR.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID),
                    AuditStatus = 2,
                    OperatorId = mainModel.OperatorId,
                    ReceiptId = mainModel.OperatorId,
                    AuditId = mainModel.AuditId,
                    AuditTime = mainModel.AuditTime,
                    CompanyId = currUser.CompanyID,
                    DeleteFlag = false,
                    SourceId = mainModel.ID,
                    Number = mainModel.ChildList.Sum(p => p.ActualNum),
                    Amount = mainModel.ChildList.Sum(p => p.AccountNum)
                };
                var detailList = mainModel.ChildList.Where(p => p.ProfitNum > 0)
                    .Select(p => new TWMProfitDetailDbModel
                    {
                        ID = 0,
                        MainId = 0,
                        MaterialId = p.MaterialId,
                        WarehouseId = mainModel.WarehouseId,
                        BatchNumber = "",
                        ActualNumber = p.ProfitNum,
                        UnitPrice = 0,
                        Amount = 0,
                        ValidityPeriod = null,
                        AccountNum = p.AccountNum,
                        Remark = p.Remark
                    })
                    .ToList();
                //插入盘盈入库单信息
                var mainId = await _db.Instance.Insertable(profitMainModel).ExecuteReturnIdentityAsync();
                detailList.ForEach(p => p.MainId = mainId);
                await _db.Instance.Insertable(detailList).ExecuteCommandAsync();

                //更新盘盈入库统计信息
                detailList.ForEach(p =>
                {
                    //更新数据
                    var flag = _db.Instance.Updateable<TWMProfitDeficitCountDbModel>()
                    .SetColumns(p1 => new TWMProfitDeficitCountDbModel
                    {
                        MaterialId = p.MaterialId,
                        WarehouseId = mainModel.WarehouseId,
                        WhNumber =p1.WhNumber+ p.ActualNumber
                    })
                    .Where(p1 => p1.MaterialId == p.MaterialId && p1.WarehouseId == mainModel.WarehouseId)
                    .ExecuteCommand() > 0;
                    //更新失败，则认为数据不存在，新增数据
                    if (!flag)
                    {
                        _db.Instance.Insertable(new TWMProfitDeficitCountDbModel
                        {
                            MaterialId = p.MaterialId,
                            WarehouseId = mainModel.WarehouseId,
                            WhNumber = p.ActualNumber
                        }).ExecuteCommand();
                    }
                });
            }
            catch
            {

            }
        }

        /// <summary>
        /// 生成盘亏出库单
        /// </summary>
        /// <param name="mainModel"></param>
        /// <param name="currUser"></param>
        private async void CreateDeficitOrder(TWMInventoryMainDbModel mainModel, CurrentUser currUser)
        {
            try
            {
                if (mainModel.ChildList.Count(p => p.DeficitNum > 0) < 1) return;
                var profitMainModel = new TWMDeficitMainDbModel
                {
                    ID = 0,
                    WhSendType = 4,
                    WhSendDate = mainModel.InventoryDate,
                    WhSendOrder  = _codeMakers.Where(p => p.ProvideName == OrderEnum.IS.GetDescription()).FirstOrDefault()?.MakeNo(currUser.CompanyID),
                    AuditStatus = 2,
                    OperatorId = mainModel.OperatorId,
                    ReceiptId = mainModel.OperatorId,
                    AuditId = mainModel.AuditId,
                    AuditTime = mainModel.AuditTime,
                    CompanyId = currUser.CompanyID,
                    DeleteFlag = false,
                    SourceId = mainModel.ID,
                    Number = mainModel.ChildList.Sum(p => p.ActualNum),
                    Amount = mainModel.ChildList.Sum(p => p.AccountNum)
                };
                var detailList = mainModel.ChildList.Where(p => p.DeficitNum > 0)
                    .Select(p => new TWMDeficitDetailDbModel
                    {
                        ID = 0,
                        MainId = 0,
                        MaterialId = p.MaterialId,
                        WarehouseId = mainModel.WarehouseId,
                        BatchNumber = "",
                        ActualNumber = p.DeficitNum,
                        UnitPrice = 0,
                        Amount = 0,
                        ValidityPeriod = null,
                        AccountNum = p.AccountNum,
                        Remark = p.Remark
                    })
                    .ToList();
                //插入盘亏出库单信息
                var mainId = await _db.Instance.Insertable(profitMainModel).ExecuteReturnIdentityAsync();
                detailList.ForEach(p => p.MainId = mainId);
                await _db.Instance.Insertable(detailList).ExecuteCommandAsync();

                //更新盘亏出库统计信息
                detailList.ForEach(p =>
                {
                    //更新数据
                    var flag = _db.Instance.Updateable<TWMProfitDeficitCountDbModel>()
                    .SetColumns(p1 => new TWMProfitDeficitCountDbModel
                    {
                        MaterialId = p.MaterialId,
                        WarehouseId = mainModel.WarehouseId,
                        WhSendNumber = p1.WhSendNumber+ p.ActualNumber
                    })
                    .Where(p1 => p1.MaterialId == p.MaterialId && p1.WarehouseId == mainModel.WarehouseId)
                    .ExecuteCommand() > 0;
                    //更新失败则认为数据不存在，新增数据
                    if (!flag)
                    {
                        _db.Instance.Insertable(new TWMProfitDeficitCountDbModel
                        {
                            MaterialId = p.MaterialId,
                            WarehouseId = mainModel.WarehouseId,
                            WhSendNumber = p.ActualNumber
                        }).ExecuteCommand();
                    }
                });
            }
            catch
            {

            }
        }

        /// <summary>
        /// 获取盘点单主单和明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TWMInventoryMainQueryModel>> GetInventoryOrder(int id, CurrentUser currentUser)
        {
            var queryModel = await GetMainQueryModel(id);
            queryModel.AllowEdit = queryModel.OperatorId == currentUser.UserID && queryModel.AuditStatus != 2;
            return ResponseUtil<TWMInventoryMainQueryModel>.SuccessResult(queryModel);
        }

        /// <summary>
        /// 获取盘点单明细列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject<List<TWMInventoryMainQueryModel>>> GetInventoryOrderListAsync(RequestGet requestGet, CurrentUser currentUser)
        {
            try
            {
                var mainList = GetMainListAsync(requestGet, currentUser).Result;
                var mainIds = mainList.Data.Select(p => p.ID);
                if (mainIds == null || mainIds.Count() < 1)
                    return ResponseUtil<List<TWMInventoryMainQueryModel>>.SuccessResult(mainList.Data);
                foreach (var item in mainList.Data)
                {
                    item.ChildList = await _db.Instance.Queryable<TWMInventoryDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMWarehouseFileDbModel>
                    (
                        (t, t0, t1, t2, t3, t4, t5) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.WarehouseUnitId == t4.ID,
                            JoinType.Left , t.WarehouseId == t5.ID
                        }
                    )
                    .Select((t, t0, t1, t2, t3, t4, t5) => new TWMInventoryDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        WarehouseRate = t0.WarehouseRate,
                        WarehouseId = t.WarehouseId,
                        WarehouseName = t5.WarehouseName,
                        AccountNum = t.AccountNum,
                        ActualNum = t.ActualNum,
                        ProfitNum = t.ProfitNum,
                        DeficitNum = t.DeficitNum,
                        Remark = t.Remark,
                    })
                    .Where(t => t.MainId == item.ID)
                    .Take(2)
                    .ToListAsync();
                }

                return ResponseUtil<List<TWMInventoryMainQueryModel>>.SuccessResult(mainList.Data, mainList.TotalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMInventoryMainQueryModel>>.FailResult(null, $"查询数据发生异常{Environment.NewLine}{ex.Message}");
            }

        }

        /// <summary>
        /// 获取T_WM_InventoryMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMInventoryMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMInventoryMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMInventoryMainDbModel, TBMWarehouseFileDbModel, TSMUserAccountDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                        (t, t0, t1, t2, t3, t4) => new object[]
                        {
                            JoinType.Left , t.WarehouseId == t0.ID,
                            JoinType.Left , t.OperatorId == t1.ID,
                            JoinType.Left , t.WhAdminId == t2.ID,
                            JoinType.Left , t.AuditId == t3.ID,
                            JoinType.Left , t.InventoryId == t4.ID
                        }
                    ).Where((t, t0, t1, t2, t3, t4) => t.CompanyId == currentUser.CompanyID && !t.DeleteFlag);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMInventoryMainDbModel>(item.Column);
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
                        (t, t0, t1, t2, t3, t4) => new TWMInventoryMainQueryModel
                        {
                            ID = t.ID,
                            WarehouseId = t.WarehouseId,
                            WarehouseName = t0.WarehouseName,
                            InventoryDate = t.InventoryDate,
                            InventoryOrder = t.InventoryOrder,
                            AuditStatus = t.AuditStatus,
                            OperatorId = t.OperatorId,
                            OperatorName = t1.AccountName,
                            WhAdminId = t.WhAdminId,
                            WhAdminName = t2.AccountName,
                            AuditId = t.AuditId,
                            AuditName = t3.AccountName,
                            AuditTime = t.AuditTime,
                            DeleteFlag = t.DeleteFlag,
                            CompanyId = t.CompanyId,
                            InventoryId = t.InventoryId,
                            InventoryName = t4.AccountName
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select(
                        (t, t0, t1, t2, t3, t4) => new TWMInventoryMainQueryModel
                        {
                            ID = t.ID,
                            WarehouseId = t.WarehouseId,
                            WarehouseName = t0.WarehouseName,
                            InventoryDate = t.InventoryDate,
                            InventoryOrder = t.InventoryOrder,
                            AuditStatus = t.AuditStatus,
                            OperatorId = t.OperatorId,
                            OperatorName = t1.AccountName,
                            WhAdminId = t.WhAdminId,
                            WhAdminName = t2.AccountName,
                            AuditId = t.AuditId,
                            AuditName = t3.AccountName,
                            AuditTime = t.AuditTime,
                            DeleteFlag = t.DeleteFlag,
                            CompanyId = t.CompanyId,
                            InventoryId = t.InventoryId,
                            InventoryName = t4.AccountName
                        })
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TWMInventoryMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMInventoryMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取T_WM_InventoryDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMInventoryDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TWMInventoryDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TWMInventoryDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel,
                    TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMWarehouseFileDbModel>(
                        (t, t0, t1, t2, t3, t4, t5) => new object[]
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.WarehouseUnitId == t4.ID,
                            JoinType.Left , t.WarehouseId == t5.ID
                        }
                    );

                //执行查询
                queryData = await query.Select(
                    (t, t0, t1, t2, t3, t4, t5) => new TWMInventoryDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialCode = t0.MaterialCode,
                        MaterialName = t0.MaterialName,
                        MaterialTypeId = t0.MaterialTypeId,
                        MaterialTypeName = t1.DicValue,
                        ColorId = t0.ColorId,
                        ColorName = t2.DicValue,
                        Spec = t0.Spec,
                        BaseUnitId = t0.BaseUnitId,
                        BaseUnitName = t3.DicValue,
                        WarehouseUnitId = t0.WarehouseUnitId,
                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                        WarehouseRate = t0.WarehouseRate,
                        WarehouseId = t.WarehouseId,
                        WarehouseName = t5.WarehouseName,
                        AccountNum = t.AccountNum,
                        ActualNum = t.ActualNum,
                        ProfitNum = t.ProfitNum,
                        DeficitNum = t.DeficitNum,
                        Remark = t.Remark,
                    })
                    .Where(t => t.MainId == requestObject)
                    .ToListAsync();

                //返回执行结果
                return ResponseUtil<List<TWMInventoryDetailQueryModel>>.SuccessResult(queryData, queryData.Count());
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMInventoryDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_WM_InventoryMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMInventoryMainQueryModel>> PostAsync(RequestPost<TWMInventoryMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMInventoryMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMInventoryMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMInventoryDetailAddModel>, List<TWMInventoryDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                TWMInventoryMainQueryModel returnObject = null;
                if (result)
                    returnObject = await GetMainQueryModel(mainId);
                //返回执行结果
                return result
                    ? ResponseUtil<TWMInventoryMainQueryModel>.SuccessResult(returnObject)
                    : ResponseUtil<TWMInventoryMainQueryModel>.FailResult(null, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMInventoryMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        private async Task<TWMInventoryMainQueryModel> GetMainQueryModel(int iMainId)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TWMInventoryMainDbModel, TBMWarehouseFileDbModel, TSMUserAccountDbModel,
                        TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                            (t, t0, t1, t2, t3, t4) => new object[]
                            {
                                JoinType.Left , t.WarehouseId == t0.ID,
                                JoinType.Left , t.OperatorId == t1.ID,
                                JoinType.Left , t.WhAdminId == t2.ID,
                                JoinType.Left , t.AuditId == t3.ID,
                                JoinType.Left , t.InventoryId == t4.ID
                            }
                        )
                        .Select((t, t0, t1, t2, t3, t4) => new TWMInventoryMainQueryModel
                        {
                            ID = t.ID,
                            WarehouseId = t.WarehouseId,
                            WarehouseName = t0.WarehouseName,
                            InventoryDate = t.InventoryDate,
                            InventoryOrder = t.InventoryOrder,
                            AuditStatus = t.AuditStatus,
                            OperatorId = t.OperatorId,
                            OperatorName = t1.AccountName,
                            WhAdminId = t.WhAdminId,
                            WhAdminName = t2.AccountName,
                            AuditId = t.AuditId,
                            AuditName = t3.AccountName,
                            AuditTime = t.AuditTime,
                            DeleteFlag = t.DeleteFlag,
                            CompanyId = t.CompanyId,
                            InventoryId = t.InventoryId,
                            InventoryName = t4.AccountName
                        })
                        .Where(t => t.ID == iMainId)
                        .FirstAsync();
                var detailModels = await _db.Instance.Queryable<TWMInventoryDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel,
                        TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMWarehouseFileDbModel>(
                            (t, t0, t1, t2, t3, t4, t5) => new object[]
                            {
                                JoinType.Left , t.MaterialId == t0.ID,
                                JoinType.Left , t0.MaterialTypeId == t1.ID,
                                JoinType.Left , t0.ColorId == t2.ID,
                                JoinType.Left , t0.BaseUnitId == t3.ID,
                                JoinType.Left , t0.WarehouseUnitId == t4.ID,
                                JoinType.Left , t.WarehouseId == t5.ID
                            }
                        )
                        .Select((t, t0, t1, t2, t3, t4, t5) => new TWMInventoryDetailQueryModel
                        {
                            ID = t.ID,
                            MainId = t.MainId,
                            MaterialId = t.MaterialId,
                            MaterialCode = t0.MaterialCode,
                            MaterialName = t0.MaterialName,
                            MaterialTypeId = t0.MaterialTypeId,
                            MaterialTypeName = t1.DicValue,
                            ColorId = t0.ColorId,
                            ColorName = t2.DicValue,
                            Spec = t0.Spec,
                            BaseUnitId = t0.BaseUnitId,
                            BaseUnitName = t3.DicValue,
                            WarehouseUnitId = t0.WarehouseUnitId,
                            WarehouseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                            WarehouseRate = t0.WarehouseRate,
                            WarehouseId = t.WarehouseId,
                            WarehouseName = t5.WarehouseName,
                            AccountNum = t.AccountNum,
                            ActualNum = t.ActualNum,
                            ProfitNum = t.ProfitNum,
                            DeficitNum = t.DeficitNum,
                            Remark = t.Remark,
                        })
                        .Where(t => t.MainId == iMainId)
                        .ToListAsync();
                mainModel.ChildList = detailModels;
                return mainModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 修改T_WM_InventoryMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<bool>> PutAsync(RequestPut<TWMInventoryMainEditModel> requestObject, CurrentUser currentUser)
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
                var mainModel = _mapper.Map<TWMInventoryMainDbModel>(requestObject.PostData);
                var mainFlag = await currDb.Updateable(mainModel)
                    .UpdateColumns(p => new
                    {
                        p.InventoryDate,
                        p.WarehouseId,
                        p.WhAdminId,
                        p.InventoryId
                    })
                    .ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMInventoryDetailEditModel>, List<TWMInventoryDetailDbModel>>(requestObject.PostData.ChildList);
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
                    detailFlag = currDb.Deleteable<TWMInventoryDetailDbModel>()
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
        /// 删除T_WM_InventoryMain数据
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
                    mainFlag = await _db.Instance.Updateable<TWMInventoryMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TWMInventoryMainDbModel>()
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

    }
}
