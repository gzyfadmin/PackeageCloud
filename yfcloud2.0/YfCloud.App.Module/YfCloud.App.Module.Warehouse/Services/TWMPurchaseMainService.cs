///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMPurchaseMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26 15:11:26
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
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseMain;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.UnitChange;
using YfCloud.App.Module.Warehouse.Models.TWMSalesMain;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// 采购入库 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITWMPurchaseMainService))]
    public class TWMPurchaseMainService : ITWMPurchaseMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMPurchaseMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMPurchaseMainService(IDbContext dbContext, ILogger<TWMPurchaseMainService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_WM_PurchaseMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMPurchaseMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMPurchaseMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMPurchaseMainDbModel, TPSMPurchaseOrderMainDbModel,
                  TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>((t, t1, t3, t4, t5, t6) => new object[] {
                    JoinType.Inner,t.SourceId==t1.ID,
                    JoinType.Left, t.OperatorId == t3.ID,
                    JoinType.Left, t.ReceiptId == t4.ID,
                    JoinType.Left, t.WhAdminId == t5.ID,
                    JoinType.Left,t.AuditId==t6.ID
              }).Where((t, t1, t3, t4, t5, t6) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);

                List<string> cQuery = new List<string>() { "suppliername" };
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var mconditionals = requestObject.QueryConditions.Where(p => !cQuery.Contains(p.Column.ToLower())).ToList();
                    if (mconditionals.Count() > 0)
                    {
                        List<IConditionalModel> conditionals = SqlSugarUtil.GetConditionalModels(mconditionals);
                        foreach (ConditionalModel item in conditionals)
                        {
                            item.FieldName = $"t.{item.FieldName}";
                        }

                        query.Where(conditionals);
                    }

                    var mconditionals2 = requestObject.QueryConditions.Where(p => cQuery.Contains(p.Column.ToLower())).ToList();
                    if (mconditionals2.Count() > 0)
                    {
                        List<IConditionalModel> conditionals = SqlSugarUtil.GetConditionalModels(mconditionals2);
                        foreach (ConditionalModel item in conditionals)
                        {
                            item.FieldName = $"t2.{item.FieldName}";
                        }

                        query.Where(conditionals);
                    }


                }

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMPurchaseMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t1, t3, t4, t5, t6) => new TWMPurchaseMainQueryModel
                    {
                        ID = t.ID,
                        WarehousingType = t.WarehousingType,
                        WarehousingDate = t.WarehousingDate,
                        WarehousingOrderNo = t.WarehousingOrderNo,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t3.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t4.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t5.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t6.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        PurchaseOrder = t1.OrderNo,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false,
                    }).ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t1, t3, t4, t5, t6) => new TWMPurchaseMainQueryModel
                    {
                        ID = t.ID,
                        WarehousingType = t.WarehousingType,
                        WarehousingDate = t.WarehousingDate,
                        WarehousingOrderNo = t.WarehousingOrderNo,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t3.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t4.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t5.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t6.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        PurchaseOrder = t1.OrderNo,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false,
                    }).ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TWMPurchaseMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMPurchaseMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
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

                var detailModels = await _db.Instance.Queryable<TWMPurchaseDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TWMPurchaseMainDbModel, TPSMPurchaseOrderDetailDbModel>(
                                    (t, t0, t1, t2, t3, t4, t5, t6) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID,
                                        JoinType.Inner,t.MainId==t5.ID,
                                        JoinType.Inner,t.PurchaseDetailId==t6.ID
                                    }).Select((t, t0, t1, t2, t3, t4, t5, t6) => new TWMPurchaseDetailQueryModel
                                    {
                                        ID = t.ID,
                                        MainId = t.MainId,
                                        MaterialId = t.MaterialId,
                                        MaterialName = t0.MaterialName,
                                        MaterialCode = t0.MaterialCode,
                                        WarehouseId = t.WarehouseId,
                                        ActualNum = t.ActualNum,
                                        PurchaseActualNum=t.PurchaseActualNum,
                                        PurchaseDetailId =t.PurchaseDetailId,
                                        UnitPrice = t.UnitPrice,
                                        Amount = t.Amount,
                                        MaterialTypeId = t0.MaterialTypeId,
                                        MaterialTypeName = t1.TypeName,
                                        ColorId = t0.ColorId,
                                        ColorName = t2.DicValue,
                                        BaseUnitId = t0.BaseUnitId,
                                        BaseUnitName = t3.DicValue,
                                        WarehouseUnitId = t0.WarehouseUnitId,
                                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                                        PurchaseUnitId=t0.PurchaseUnitId,
                                        PurchaseRate =t0.PurchaseRate,
                                        WarehouseRate = t0.WarehouseRate,
                                        Spec = t0.Spec,
                                        Remark = t.Remark,
                                        PurchaseNum = t6.PurchaseNum,
                                        ShouldPurchaseNum = t6.TransferNum,
                                        ValidDate = t.ValidDate
                                    })
                                    .Where(t => t.MainId == iMainId)
                                    .ToListAsync();





                detailModels.ForEach(p =>
                {
                    if (mainModel.AuditStatus != 2)
                    {
                        p.ShouldPurchaseNum = p.PurchaseActualNum + p.ShouldPurchaseNum;
                    }
                });

                mainModel.ChildList = detailModels;
                return ResponseUtil<TWMPurchaseMainQueryModel>.SuccessResult(mainModel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null);
            }
        }


        /// <summary>
        /// 新增T_WM_PurchaseMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> PostAsync(RequestPost<TWMPurchaseMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMPurchaseMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                mapMainModel.AuditStatus = 0;

                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMPurchaseDetailAddModel>, List<TWMPurchaseDetailDbModel>>(requestObject.PostData.ChildList);
               

                #region 处理采购单的可转单数量

                var listItem = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Where(t => t.MainId == mapMainModel.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TPSMPurchaseOrderDetailDbModel> toEdit = new List<TPSMPurchaseOrderDetailDbModel>();

                foreach (var item in mapDetailModelList)
                {
                    var procudeMaterial = listItem.Where(p => p.ID == item.PurchaseDetailId).FirstOrDefault();//查询采购单明细
                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Purchase, item.ActualNum);//入库数量转化为采购单的数量
                    decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Purchase, UnitType.Warehouse, procudeMaterial.TransferNum);//可转的数量（仓库单位）

                    if (item.ActualNum > wareTransNum)
                    {
                        throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                    }
                    else if (item.ActualNum == wareTransNum)
                    {
                        item.PurchaseActualNum = procudeMaterial.TransferNum;
                        procudeMaterial.TransferNum = 0;
                    }
                    else
                    {
                        if (proNum > procudeMaterial.TransferNum)
                        {
                            item.PurchaseActualNum = procudeMaterial.TransferNum;
                            procudeMaterial.TransferNum = 0;
                        }
                        else
                        {
                            item.PurchaseActualNum = proNum;
                            procudeMaterial.TransferNum = procudeMaterial.TransferNum - proNum;
                        }
                    }
                    toEdit.Add(procudeMaterial);
                }

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mapMainModel.SourceId))
                {
                    _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == mapMainModel.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                }

                #endregion

                _db.Instance.Insertable(mapDetailModelList).ExecuteCommand();

                //提交事务
                currDb.CommitTran();

                var queryResult = await GetWholeMainData(mainId, currentUser);
                //返回执行结果
                return ResponseUtil<TWMPurchaseMainQueryModel>.SuccessResult(queryResult.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_WM_PurchaseMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMPurchaseMainQueryModel>> PutAsync(RequestPut<TWMPurchaseMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TWMPurchaseMainDbModel>(requestObject.PostData);

                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);

                var mainFlag = await currDb.Updateable(mainModel).UpdateColumns(p => new
                {
                    p.WarehousingDate,
                    p.Number,
                    p.Amount,
                    p.ReceiptId,
                    p.WhAdminId,

                }).ExecuteCommandAsync() > 0;

                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMPurchaseDetailEditModel>,
                    List<TWMPurchaseDetailDbModel>>(requestObject.PostData.ChildList);

                #region 处理可转采购单数量

                var mainEntity = _db.Instance.Queryable<TWMPurchaseMainDbModel>().Where(p => p.ID == mainModel.ID).First();

                var listItem = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TPSMPurchaseOrderDetailDbModel> toEdit = new List<TPSMPurchaseOrderDetailDbModel>();

                #endregion 

                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;

                    #region 处理可转采购单数量

                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();

                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;

                    var procudeMaterial = listItem.Where(p => p.ID == item.PurchaseDetailId).FirstOrDefault();//查询采购单明细

                    if (item.ID <= 0) //新增
                    {
                        decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Purchase, item.ActualNum);//入库数量转化为采购单的数量
                        decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Purchase, UnitType.Warehouse, procudeMaterial.TransferNum);//可转的数量（仓库单位）

                        if (item.ActualNum > wareTransNum)
                        {
                            throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                        }
                        else if (item.ActualNum == wareTransNum)
                        {
                            item.PurchaseActualNum = procudeMaterial.TransferNum;
                            procudeMaterial.TransferNum = 0;
                        }
                        else
                        {
                            if (proNum > procudeMaterial.TransferNum)
                            {
                                item.PurchaseActualNum = procudeMaterial.TransferNum;
                                procudeMaterial.TransferNum = 0;
                            }
                            else
                            {
                                item.PurchaseActualNum = proNum;
                                procudeMaterial.TransferNum = procudeMaterial.TransferNum - proNum;
                            }
                        }
                        toEdit.Add(procudeMaterial);


                    }
                    else //修改
                    {
                        var dModel = _db.Instance.Queryable<TWMPurchaseDetailDbModel>().Where(p => p.ID == item.ID).First();
                        if (dModel != null)
                        {
                            if (dModel.ActualNum != item.ActualNum)
                            {
                                decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Purchase, item.ActualNum);//入库数量转化为采购单的数量
                                decimal TransNum = procudeMaterial.TransferNum + dModel.PurchaseActualNum; //可转数量（采购单位）

                                decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Purchase, UnitType.Warehouse, TransNum);//可转的数量（仓库单位）

                                if (item.ActualNum > wareTransNum)
                                {
                                    throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                                }
                                else if (item.ActualNum == wareTransNum)
                                {
                                    item.PurchaseActualNum = TransNum;
                                    procudeMaterial.TransferNum = 0;
                                }
                                else
                                {
                                    if (proNum > TransNum)
                                    {
                                        item.PurchaseActualNum = TransNum;
                                        procudeMaterial.TransferNum = 0;
                                    }
                                    else
                                    {
                                        item.PurchaseActualNum = proNum;
                                        procudeMaterial.TransferNum = TransNum - proNum;
                                    }
                                }
                                toEdit.Add(procudeMaterial);
                            }

                            else
                            {
                                item.PurchaseActualNum = dModel.PurchaseActualNum;
                            }
                        }
                    }

                    #endregion

                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();

                    #region 处理可转采购单数量

                    var whDetailList = currDb.Queryable<TWMPurchaseDetailDbModel>().Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID).ToList();

                    foreach (var item in whDetailList)
                    {

                        var m = listItem.Where(p => p.ID == item.PurchaseDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransferNum = m.TransferNum + item.PurchaseActualNum;
                            toEdit.Add(m);
                        }
                    }

                    #endregion

                    detailFlag = currDb.Deleteable<TWMPurchaseDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                #region 处理可转采购单数量

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mainEntity.SourceId))
                {
                    _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                }
                else
                {
                    _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == true).ExecuteCommand();
                }

                #endregion

                //提交事务
                currDb.CommitTran();
                var queryResult = await GetWholeMainData(mainModel.ID, currentUser);
                //返回执行结果
                return ResponseUtil<TWMPurchaseMainQueryModel>.SuccessResult(queryResult.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMPurchaseMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_WM_PurchaseMain数据
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
                List<int> IDS = new List<int>();
                var materList = BasicCacheGet.GetMaterial(currentUser);

                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    IDS = requestObject.PostDataList.Select(p => p.ID).ToList();
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();
                    await _db.Instance.Updateable<TWMPurchaseMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync();
                }
                else
                {
                    IDS.Add(requestObject.PostData.ID);
                    //单条删除
                    await _db.Instance.Updateable<TWMPurchaseMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync();
                }

                foreach (int ids in IDS)
                {
                    List<TPSMPurchaseOrderDetailDbModel> toEdit = new List<TPSMPurchaseOrderDetailDbModel>(); //可采购售单详细信息
                    var mainEntity = _db.Instance.Queryable<TWMPurchaseMainDbModel>().Where(P => P.ID == ids).First();//可采购售单主表

                    var listItem = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().
                        Where(t => t.MainId == mainEntity.SourceId).ToList(); //可采购售单详细信息

                    var whDetailList = currDb.Queryable<TWMPurchaseDetailDbModel>().Where(p => p.MainId == ids).ToList();

                    foreach (var item in whDetailList)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Purchase, item.ActualNum);

                        var m = listItem.Where(p => p.ID == item.PurchaseDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransferNum = m.TransferNum + proNum;
                            toEdit.Add(m);
                        }
                    }

                    if (toEdit.Count() > 0)
                    {
                        _db.Instance.Updateable(toEdit).ExecuteCommand();
                    }

                    if (!_db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mainEntity.SourceId))
                    {
                        _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                    }
                    else
                    {
                        _db.Instance.Updateable<TPSMPurchaseOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == true).ExecuteCommand();
                    }

                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return ResponseUtil<bool>.SuccessResult(true);
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
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>

        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMPurchaseMainAduit> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMPurchaseMainDbModel>()
                    .SetColumns(p => new TWMPurchaseMainDbModel
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

            var mainEntity = _db.Instance.Queryable<TWMPurchaseMainDbModel>().Where(p => p.ID == iMainId).First();

            //获取入库明细
            var details = await _db.Instance.Queryable<TWMPurchaseDetailDbModel>()
                .Where(p => p.MainId == iMainId)
                .ToListAsync();
            //更新入库数量
            var detailList = details.Select(p => new TWMPurchaseCountDbModel
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
