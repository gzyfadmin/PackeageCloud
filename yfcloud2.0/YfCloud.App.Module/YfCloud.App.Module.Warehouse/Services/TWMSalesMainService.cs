///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMSalesMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22 8:49:16
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
using YfCloud.App.Module.Warehouse.Models.TWMSalesMain;
using YfCloud.App.Module.Warehouse.Models.TWMSalesDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitMain;
using YfCloud.Framework.CacheManager;
using YfCloud.App.Module.Warehouse.Services;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.Framework.UnitChange;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// 销售出库 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITWMSalesMainService))]
    public class TWMSalesMainService : ITWMSalesMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMSalesMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly IStaticInventory _staticInventory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMSalesMainService(IDbContext dbContext, ILogger<TWMSalesMainService> logger, IMapper mapper, IStaticInventory staticInventory)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _staticInventory = staticInventory;
        }

        /// <summary>
        /// 获取T_WM_SalesMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMSalesMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMSalesMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数


                var query = _db.Instance.Queryable<TWMSalesMainDbModel, TSSMSalesOrderMainDbModel, TBMCustomerFileDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>((t, t1, t2, t3, t4, t5, t6) => new object[] {
                    JoinType.Inner,t.SourceId==t1.ID,
                    JoinType.Inner,t1.CustomerId==t2.ID,
                    JoinType.Left, t.OperatorId == t3.ID,
                    JoinType.Left, t.SendId == t4.ID,
                    JoinType.Left, t.WhAdminId == t5.ID,
                    JoinType.Left,t.AuditId==t6.ID
                }).Where((t, t1, t2, t3, t4, t5, t6) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);


                List<string> cQuery = new List<string>() { "customerid" };
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
                            item.FieldName = $"t1.{item.FieldName}";
                        }

                        query.Where(conditionals);
                    }


                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMSalesMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t1, t2, t3, t4, t5, t6) => new TWMSalesMainQueryModel
                    {
                        ID = t.ID,
                        WhSendType = t.WhSendType,
                        WhSendDate = t.WhSendDate,
                        WhSendOrder = t.WhSendOrder,
                        AuditStatus = t.AuditStatus,
                        CustomerName = t2.CustomerName,
                        CustomerId = t1.CustomerId,
                        OperatorId = t.OperatorId,
                        OperatorName = t3.AccountName,
                        SendId = t.SendId,
                        SendName = t4.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t5.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t6.AccountName,
                        AuditTime = t.AuditTime,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        ReceiptAddress = t.ReceiptAddress,
                        SaleOrder = t1.OrderNo,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                    }).ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t1, t2, t3, t4, t5, t6) => new TWMSalesMainQueryModel
                    {
                        ID = t.ID,
                        WhSendType = t.WhSendType,
                        WhSendDate = t.WhSendDate,
                        WhSendOrder = t.WhSendOrder,
                        AuditStatus = t.AuditStatus,
                        CustomerName = t2.CustomerName,
                        CustomerId = t1.CustomerId,
                        OperatorId = t.OperatorId,
                        OperatorName = t3.AccountName,
                        SendId = t.SendId,
                        SendName = t4.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t5.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t6.AccountName,
                        AuditTime = t.AuditTime,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        ReceiptAddress = t.ReceiptAddress,
                        SaleOrder = t1.OrderNo,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                    }).ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TWMSalesMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMSalesMainQueryModel>>.FailResult(null, ex.Message);
            }
        }


        public async Task<ResponseObject<TWMSalesMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
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




                var detailModelsSql = _db.Instance.Queryable<TWMSalesDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TWMSalesMainDbModel, TSSMSalesOrderDetailDbModel
                                     , TBMPackageDbModel, TMMColorSolutionMainDbModel>(
                                    (t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID,
                                        JoinType.Inner,t.MainId==t5.ID,
                                        JoinType.Inner,t.SalesOrderDetailId==t6.ID,
                                        JoinType.Left,t0.PackageID==t7.ID,
                                        JoinType.Left,t6.ColorSolutionId==t8.ID
                                    })
                                    .Select((t, t0, t1, t2, t3, t4, t5, t6, t7, t8) => new TWMSalesDetailQueryModel
                                    {
                                        ID = t.ID,
                                        MainId = t.MainId,
                                        MaterialId = t.MaterialId,
                                        MaterialName = t0.MaterialName,
                                        MaterialCode = t0.MaterialCode,
                                        WarehouseId = t.WarehouseId,
                                        ActualNum = t.ActualNum,
                                        SalesOrderActualNum=t.SalesOrderActualNum,
                                        SalesOrderDetailId =t.SalesOrderDetailId,
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
                                        SalesUnitId=t0.SalesUnitId,
                                        SalesRate=t0.SalesRate,
                                        WarehouseRate = t0.WarehouseRate,
                                        Spec = t0.Spec,
                                        Remark = t.Remark,
                                        SalesNum = t6.SalesNum,
                                        ShouldSaleNum = t6.TransferNum,
                                        PackageCode = t7.DicCode,
                                        PackageName = t7.DicValue,
                                        ColorSolutionName = t8.SolutionCode
                                    })
                                    .Where(t => t.MainId == iMainId).OrderBy(t => t.ID);

                var detailModels = await detailModelsSql.ToListAsync();

                //物料
                List<TBMMaterialFileCacheModel> mList = BasicCacheGet.GetMaterial(currentUser);

                detailModels.ForEach(p =>
                {
                    TBMMaterialFileCacheModel me = mList.Where(x => x.ID == p.MaterialId).FirstOrDefault();
                    if (me == null)
                    {
                        throw new Exception($"物料{p.MaterialId}，不存在");
                    }

                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.MaterialId = p.MaterialId;
                    tWMStaQuery.WarehouseId = p.WarehouseId;


                    if (mainModel.AuditStatus != 2)
                    {
                        //p.WaitNum = p.WaitNum - p.ActualNum;

                        tWMStaQuery.EditID = mainModel.ID;
                        tWMStaQuery.OperateType = OperateEnum.Sale;
                        // p.ShouldNum = p.SalesOrderActualNum + p.ActualNum;
                        p.ShouldSaleNum = p.ShouldSaleNum + p.SalesOrderActualNum;
                    }


                    p.SalesNumOnInventory = GetInveroryFromSaleNum(me, p.SalesNum);

                    p.AvailableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                });

                mainModel.ChildList = detailModels;
                return ResponseUtil<TWMSalesMainQueryModel>.SuccessResult(mainModel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null);
            }
        }



        /// <summary>
        /// 新增T_WM_SalesMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMSalesMainQueryModel>> PostAsync(RequestPost<TWMSalesMainAddModel> requestObject, CurrentUser currentUser)
        { 
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMSalesMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                mapMainModel.AuditStatus = 0;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMSalesDetailAddModel>, List<TWMSalesDetailDbModel>>(requestObject.PostData.ChildList);
                
                #region 检查出库是否超过可用量

                int? iMainId = mainId;
                var materList = BasicCacheGet.GetMaterial(currentUser);

                var details = requestObject.PostData.ChildList.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMOtherCountDbModel
                {
                    MaterialId = p.Key.MaterialId,
                    WarehouseId = p.Key.WarehouseId,
                    WhSendNumber = p.Sum(p1 => p1.ActualNum)
                }).ToList();

                foreach (var item in details)
                {
                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.EditID = iMainId;
                    tWMStaQuery.MaterialId = item.MaterialId;
                    tWMStaQuery.WarehouseId = item.WarehouseId;

                    decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                    if (AvaiableNum < 0)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料可用量不足");
                    }
                    if (item.WhSendNumber > AvaiableNum)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                    }
                }

                #endregion
                #region 处理销售单的可转单数量


                var listItem = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Where(t => t.MainId == mapMainModel.SourceId).ToList();//来源单据


                List<TSSMSalesOrderDetailDbModel> toEdit = new List<TSSMSalesOrderDetailDbModel>();

                foreach (var item in mapDetailModelList)
                {
                    var procudeMaterial = listItem.Where(p => p.ID == item.SalesOrderDetailId).FirstOrDefault();//销售合同明细

                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();//物料

                    decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);
                    decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Sales, UnitType.Warehouse, procudeMaterial.TransferNum);//可转的数量（仓库单位）

                    if (item.ActualNum > wareTransNum)
                    {
                        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{wareTransNum }");
                    }
                    else if (item.ActualNum == wareTransNum)
                    {
                        item.SalesOrderActualNum = procudeMaterial.TransferNum;
                        procudeMaterial.TransferNum = 0;
                    }
                    else
                    {
                        if (proNum > procudeMaterial.TransferNum)
                        {
                            item.SalesOrderActualNum = procudeMaterial.TransferNum;
                            procudeMaterial.TransferNum = 0;
                        }
                        else
                        {
                            item.SalesOrderActualNum = proNum;
                            procudeMaterial.TransferNum = procudeMaterial.TransferNum - proNum;
                        }
                    }

                    toEdit.Add(procudeMaterial);
                    //var m = listItem.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                    //if (m != null)
                    //{
                    //    if (m.TransferNum < proNum)
                    //    {
                    //        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{proNum }");
                    //    }
                    //    m.TransferNum = m.TransferNum - proNum;

                    //    toEdit.Add(m);
                    //}
                }

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mapMainModel.SourceId))
                {
                    _db.Instance.Updateable<TSSMSalesOrderMainDbModel>().Where(p => p.ID == mapMainModel.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                }

                #endregion

                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;

                //提交事务
                currDb.CommitTran();
                //返回执行结果

                var resultTemp = await GetWholeMainData(mainId, currentUser);

                return ResponseUtil<TWMSalesMainQueryModel>.SuccessResult(resultTemp.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_WM_SalesMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMSalesMainQueryModel>> PutAsync(RequestPut<TWMSalesMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
//#if DEBUG

//#else
//                     return null;
//#endif

                if (requestObject.PostData == null)
                    return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();

                //修改主表信息
                var mainModel = _mapper.Map<TWMSalesMainDbModel>(requestObject.PostData);
                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);

                var mainFlag = await currDb.Updateable(mainModel).UpdateColumns(p => new
                {
                    p.WhSendDate,
                    p.Number,
                    p.Amount,
                    p.SendId,
                    p.WhAdminId,
                    p.ReceiptAddress

                }).ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMSalesDetailEditModel>,
                    List<TWMSalesDetailDbModel>>(requestObject.PostData.ChildList);

                var mainEntity = _db.Instance.Queryable<TWMSalesMainDbModel>().Where(p => p.ID == mainModel.ID).First();

                var listItem = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TSSMSalesOrderDetailDbModel> toEdit = new List<TSSMSalesOrderDetailDbModel>();

                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;
                    var procudeMaterial = listItem.Where(p => p.ID == item.SalesOrderDetailId).FirstOrDefault();//销售合同明细
                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();

                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;

                    if (item.ID <= 0) //新增
                    {
                        decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);

                        decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Sales, UnitType.Warehouse, procudeMaterial.TransferNum);//可转的数量（仓库单位）
                        if (item.ActualNum > wareTransNum)
                        {
                            throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{wareTransNum }");
                        }
                        else if (item.ActualNum == wareTransNum)
                        {
                            item.SalesOrderActualNum = procudeMaterial.TransferNum;
                            procudeMaterial.TransferNum = 0;
                        }
                        else
                        {
                            if (proNum > procudeMaterial.TransferNum)
                            {
                                item.SalesOrderActualNum = procudeMaterial.TransferNum;
                                procudeMaterial.TransferNum = 0;
                            }
                            else
                            {
                                item.SalesOrderActualNum = proNum;
                                procudeMaterial.TransferNum = procudeMaterial.TransferNum - proNum;
                            }
                        }
                        toEdit.Add(procudeMaterial);
                        //decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);

                        //var m = listItem.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                        //if (m != null)
                        //{
                        //    if (m.TransferNum < proNum)
                        //    {
                        //        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{proNum }");
                        //    }
                        //    m.TransferNum = m.TransferNum - proNum;

                        //    toEdit.Add(m);
                        //}

                    }
                    else //修改
                    {
                        var dModel = _db.Instance.Queryable<TWMSalesDetailDbModel>().Where(p => p.ID == item.ID).First();
                        if (dModel != null)
                        {
                            if (dModel.ActualNum != item.ActualNum)
                            {
                                #region 

                                ////原出库数量（销售单位）
                                //decimal oldActualNumPro = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, dModel.ActualNum);
                                //decimal TransNum = procudeMaterial.TransferNum + item.SalesOrderActualNum; //可转数量（生产单位）
                                ////现出库数量(销售单位)
                                //decimal nowActualNumPro = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);


                                //if (item.ActualNum > nowActualNumPro)
                                //{
                                //    throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{nowActualNumPro }");
                                //}
                                //else if (item.ActualNum == nowActualNumPro)
                                //{
                                //    item.SalesOrderActualNum = TransNum;
                                //    procudeMaterial.TransferNum = 0;
                                //}
                                //else
                                //{
                                //    if (oldActualNumPro > TransNum)
                                //    {
                                //        item.SalesOrderActualNum = TransNum;
                                //        procudeMaterial.TransferNum = 0;
                                //    }
                                //    else
                                //    {
                                //        item.SalesOrderActualNum = oldActualNumPro;
                                //        procudeMaterial.TransferNum = TransNum - oldActualNumPro;
                                //    }
                                //}

                                #endregion

                                decimal saleNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);//实际转出数量（销售单位）

                                decimal TransNum = procudeMaterial.TransferNum + dModel.SalesOrderActualNum; //可转数量（生产单位）

                                decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Sales, UnitType.Warehouse, TransNum);//可转的数量（仓库单位）

                                if (item.ActualNum > wareTransNum)
                                {
                                    throw new Exception($"物料代码{me.MaterialCode} 入库数量不能大于{wareTransNum }");
                                }
                                else if (item.ActualNum == wareTransNum)
                                {
                                    item.SalesOrderActualNum = TransNum;
                                    procudeMaterial.TransferNum = 0;
                                }
                                else
                                {
                                    if (saleNum > TransNum)
                                    {
                                        item.SalesOrderActualNum = TransNum;
                                        procudeMaterial.TransferNum = 0;
                                    }
                                    else
                                    {
                                        item.SalesOrderActualNum = saleNum;
                                        procudeMaterial.TransferNum = TransNum - saleNum;
                                    }
                                }

                                toEdit.Add(procudeMaterial);
                                //var m = listItem.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                                //if (m != null)
                                //{
                                //    if (m.TransferNum + oldActualNumPro < nowActualNumPro)
                                //    {
                                //        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{m.TransferNum + oldActualNumPro }");
                                //    }
                                //    m.TransferNum = m.TransferNum + oldActualNumPro - nowActualNumPro;

                                //    toEdit.Add(m);
                                //}


                            }
                            else
                            {
                                item.SalesOrderActualNum = dModel.SalesOrderActualNum;
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
                    var whDetailList = currDb.Queryable<TWMSalesDetailDbModel>().Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID).ToList();

                    foreach (var item in whDetailList)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        //decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);

                        var m = listItem.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransferNum = m.TransferNum + item.SalesOrderActualNum;
                            toEdit.Add(m);
                        }
                    }

                    detailFlag = currDb.Deleteable<TWMSalesDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                #region 检查出库是否超过可用量

                int? iMainId = mainModel.ID;
                var mList = BasicCacheGet.GetMaterial(currentUser);

                var details = requestObject.PostData.ChildList.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMOtherCountDbModel
                {
                    MaterialId = p.Key.MaterialId,
                    WarehouseId = p.Key.WarehouseId,
                    WhSendNumber = p.Sum(p1 => p1.ActualNum)
                }).ToList();

                foreach (var item in details)
                {
                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.EditID = iMainId;
                    tWMStaQuery.MaterialId = item.MaterialId;
                    tWMStaQuery.WarehouseId = item.WarehouseId;

                    decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                    if (AvaiableNum < 0)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料可用量不足");
                    }
                    if (item.WhSendNumber > AvaiableNum)
                    {
                        var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                    }
                }

                #endregion

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mainEntity.SourceId))
                {
                    _db.Instance.Updateable<TSSMSalesOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                }
                else
                {
                    _db.Instance.Updateable<TSSMSalesOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == true).ExecuteCommand();
                }

                //提交事务
                currDb.CommitTran();

                var resultTemp = await GetWholeMainData(requestObject.PostData.ID, currentUser);

                return ResponseUtil<TWMSalesMainQueryModel>.SuccessResult(resultTemp.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMSalesMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_WM_SalesMain数据
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
                //var materList = BasicCacheGet.GetMaterial(currentUser);

                List<int> IDS = new List<int>();

                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    IDS = requestObject.PostDataList.Select(p => p.ID).ToList();
                    mainFlag = await _db.Instance.Updateable<TWMSalesMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => IDS.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    IDS.Add(requestObject.PostData.ID);
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TWMSalesMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
                }

                //批量删除



                foreach (int ids in IDS)
                {
                    List<TSSMSalesOrderDetailDbModel> toEdit = new List<TSSMSalesOrderDetailDbModel>(); //可转销售单详细信息
                    var mainEntity = _db.Instance.Queryable<TWMSalesMainDbModel>().Where(P => P.ID == ids).First();//可转销售单主表

                    var listItem = _db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().
                        Where(t => t.MainId == mainEntity.SourceId).ToList(); //可转销售单详细信息

                    var whDetailList = currDb.Queryable<TWMSalesDetailDbModel>().Where(p => p.MainId == ids).ToList();

                    foreach (var item in whDetailList)
                    {
                        //var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        //* decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Sales, item.ActualNum);

                        var m = listItem.Where(p => p.ID == item.SalesOrderDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            //m.TransferNum = m.TransferNum + proNum;
                            m.TransferNum = m.TransferNum + item.SalesOrderActualNum;
                            toEdit.Add(m);
                        }
                    }

                    if (toEdit.Count() > 0)
                    {
                        _db.Instance.Updateable(toEdit).ExecuteCommand();
                    }

                    if (!_db.Instance.Queryable<TSSMSalesOrderDetailDbModel>().Any(p => p.TransferNum > 0 && p.MainId == mainEntity.SourceId))
                    {
                        _db.Instance.Updateable<TSSMSalesOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == false).ExecuteCommand();
                    }
                    else
                    {
                        _db.Instance.Updateable<TSSMSalesOrderMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferStatus == true).ExecuteCommand();
                    }

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
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMSalesMainAduit> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMSalesMainDbModel>()
                    .SetColumns(p => new TWMSalesMainDbModel
                    {
                        AuditStatus = requestObject.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestObject.PostData.ID && (p.AuditStatus == 0 || p.AuditStatus == 1))
                    .ExecuteCommandAsync() > 0;



                //审核通过后统计当前出库单的数量和金额
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
        /// 审核通过后统计当前出库单信息
        /// </summary>
        /// <param name="iMainId"></param>
        private  void OtherWhCountAsync(int iMainId, CurrentUser currentUser)
        {
            var mainEntity = _db.Instance.Queryable<TWMSalesMainDbModel>().Where(p => p.ID == iMainId).First();
            //获取入库明细
            var details =  _db.Instance.Queryable<TWMSalesDetailDbModel>()
                .Where(p => p.MainId == iMainId).ToList();
            //更新出库单数量
            var detailList = details.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMSalesCountDbModel
            {
                MaterialId = p.Key.MaterialId,
                WarehouseId = p.Key.WarehouseId,
                WhSendNumber = p.Sum(p1 => p1.ActualNum)
            }).ToList();


            var mList = BasicCacheGet.GetMaterial(currentUser);
            foreach (var item in detailList)
            {
                TWMStaQuery tWMStaQuery = new TWMStaQuery();
                tWMStaQuery.EditID = iMainId;
                tWMStaQuery.MaterialId = item.MaterialId;
                tWMStaQuery.WarehouseId = item.WarehouseId;
                tWMStaQuery.OperateType = OperateEnum.Sale;

                decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;

                if (AvaiableNum <=0)
                {
                    var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量可用量不足");
                }
                if (item.WhSendNumber > AvaiableNum)
                {
                    var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                }

                var updateFlag = _db.Instance.Updateable(item)
                    .SetColumns(p => p.WhSendNumber == p.WhSendNumber + item.WhSendNumber)
                    .Where(p => p.MaterialId == item.MaterialId && p.WarehouseId == item.WarehouseId)
                    .ExecuteCommand() > 0;
                if (!updateFlag)
                    _db.Instance.Insertable(item).ExecuteCommand();
            }
        }

     

        private decimal GetInveroryFromSaleNum(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal SaleNum)
        {
            decimal result = 0;

            decimal basicNum = 0;

            if (tBMMaterialFileCacheModel.SalesUnitId != null && tBMMaterialFileCacheModel.SalesRate != null)
            {
                basicNum = decimal.Round(SaleNum / (decimal)tBMMaterialFileCacheModel.SalesRate, 4);
            }
            else
            {
                basicNum = SaleNum;
            }

            if (tBMMaterialFileCacheModel.WarehouseUnitId != null && tBMMaterialFileCacheModel.WarehouseRate != null)
            {
                result = decimal.Round(basicNum * (decimal)tBMMaterialFileCacheModel.WarehouseRate, 4);
            }
            else
            {
                result = basicNum;
            }

            return result;
        }
    }
}
