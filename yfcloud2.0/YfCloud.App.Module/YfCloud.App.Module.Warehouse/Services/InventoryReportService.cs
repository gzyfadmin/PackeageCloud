using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using YfCloud.App.Module.Warehouse.Models.InventoryReport;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Warehouse.Services
{
    /// <summary>
    /// 库存统计报表服务
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IInventoryReportService))]
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<InventoryReportService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public InventoryReportService(IDbContext dbContext, ILogger<InventoryReportService> logger, IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<InventoryResultModel>>> LoadReport(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                //数据字典
                var tBMDictionary = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.DicValue);

                var warehouseDic = _db.Instance.Queryable<TBMWarehouseFileDbModel>().Where(t => SqlFunc.IsNull(t.DeleteFlag, false) != true &&
                t.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => new { Name = p.WarehouseName, Code = p.Code });

                List<InventoryResultModel> result = new List<InventoryResultModel>();

                //其他出入库
                var otherCount = _db.Instance.Queryable<TWMOtherCountDbModel>().
                    Select(p => new InAndOutModel()
                    {
                        MaterialId = p.MaterialId,
                        WarehouseId = p.WarehouseId,
                        WhNumber = p.WhNumber,
                        WhSendNumber = p.WhSendNumber
                    });

                //盘亏盘盈出入库
                var pCount = _db.Instance.Queryable<TWMProfitDeficitCountDbModel>().
                    Select(p => new InAndOutModel()
                    {
                        MaterialId = p.MaterialId,
                        WarehouseId = p.WarehouseId,
                        WhNumber = p.WhNumber,
                        WhSendNumber = p.WhSendNumber
                    });

                //销售出入库
                var SaleCount = _db.Instance.Queryable<TWMSalesCountDbModel>().
                   Select(p => new InAndOutModel()
                   {
                       MaterialId = p.MaterialId,
                       WarehouseId = p.WarehouseId,
                       WhNumber = p.WhNumber,
                       WhSendNumber = p.WhSendNumber
                   });

                //采购出入库
                var PurchaseCount = _db.Instance.Queryable<TWMPurchaseCountDbModel>().
                   Select(p => new InAndOutModel()
                   {
                       MaterialId = p.MaterialId,
                       WarehouseId = p.WarehouseId,
                       WhNumber = p.WhNumber,
                       WhSendNumber = p.WhSendNumber
                   });

                //生产出入库
                var ProductCount = _db.Instance.Queryable<TWMProductionCountDbModel>().
                   Select(p => new InAndOutModel()
                   {
                       MaterialId = p.MaterialId,
                       WarehouseId = p.WarehouseId,
                       WhNumber = p.WhNumber,
                       WhSendNumber = p.WhSendNumber
                   });

                //出入库数量
                var allCount = _db.Instance.UnionAll(otherCount, pCount, SaleCount, PurchaseCount, ProductCount).GroupBy(p => new { p.MaterialId, p.WarehouseId }).
                     Select(p => new TradeInventoryModel()
                     {
                         TradeNumber = SqlFunc.AggregateSum(p.WhNumber) - SqlFunc.AggregateSum(p.WhSendNumber),
                         MaterialId = p.MaterialId,
                         WarehouseId = p.WarehouseId
                     }).AS("t100");

                var materialFileQuery = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => p.CompanyId == currentUser.CompanyID);

                var ts = _db.Instance.Queryable(materialFileQuery, allCount, JoinType.Inner, (p1, p2) => p1.ID == p2.MaterialId);

                #region 待出库数量

                //其他待出库
                var otherToOut = _db.Instance.Queryable<TWMOtherWhSendMainDbModel, TWMOtherWhSendDetailDbModel>((t1, t2) =>
                new object[] { JoinType.Inner, t1.ID == t2.MainId }).Where((t1, t2) =>
                   t1.AuditStatus != 2 &&
                   t1.DeleteFlag == false).Select((t1, t2) => new InventoryOut { MaterialId = t2.MaterialId, Amount = t2.ActualNumber });

                //盘亏出库
                var deficitToOut = _db.Instance.Queryable<TWMDeficitMainDbModel, TWMDeficitDetailDbModel>((t1, t2) =>
               new object[] { JoinType.Inner, t1.ID == t2.MainId }).Where((t1, t2) =>
                  t1.AuditStatus != 2 &&
                  t1.DeleteFlag == false).Select((t1, t2) => new InventoryOut { MaterialId = t2.MaterialId, Amount = t2.ActualNumber });

                //销售出库
                var saleToOut = _db.Instance.Queryable<TWMSalesMainDbModel, TWMSalesDetailDbModel>((t1, t2) =>
              new object[] { JoinType.Inner, t1.ID == t2.MainId }).Where((t1, t2) =>
                 t1.AuditStatus != 2 &&
                 t1.DeleteFlag == false).Select((t1, t2) => new InventoryOut { MaterialId = t2.MaterialId, Amount = t2.ActualNum });

                //生产待出库
                var productToOut = _db.Instance.Queryable<TWMProductionMainDbModel, TWMProductionDetailDbModel>((t1, t2) =>
                new object[] { JoinType.Inner, t1.ID == t2.MainId }).Where((t1, t2) =>
                   t1.AuditStatus != 2 &&
                   t1.DeleteFlag == false).Select((t1, t2) => new InventoryOut { MaterialId = t2.MaterialId, Amount = t2.ActualNum });

                var allToOut = _db.Instance.UnionAll(otherToOut, deficitToOut, saleToOut, productToOut).AS("t101");

                var tsToOut = allToOut.ToList().GroupBy(p => p.MaterialId).Select(p => new InventoryOut() { MaterialId = p.Key, Amount = p.Sum(m => m.Amount) }).ToList()
                    .ToDictionary(p => p.MaterialId, p => new { MaterialId = p.MaterialId, Amount = p.Amount });
                #endregion

                string[] cQuery = { "warehouseid" };
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var QueryConditions1 = requestObject.QueryConditions.Where(p => !cQuery.Contains(p.Column.ToLower())).ToList();

                    if (QueryConditions1.Count() > 0)
                    {
                        var conditionals1 = SqlSugarUtil.GetConditionalModels(QueryConditions1);

                        foreach (ConditionalModel item in conditionals1)
                        {
                            item.FieldName = $"p1.{item.FieldName}";
                        }
                        ts.Where(conditionals1);
                    }

                    var QueryConditions2 = requestObject.QueryConditions.Where(p => cQuery.Contains(p.Column.ToLower())).FirstOrDefault();
                    if (QueryConditions2 != null)
                    {
                        int WarehouseId = Convert.ToInt32(QueryConditions2.Content);

                        ts = ts.Where((p1, p2) => p2.WarehouseId == WarehouseId);
                    }
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
                        ts.OrderBy($"p1.{item.Column} {item.Condition}");
                    }
                }
                #region 最新采购/生产时间
                var Purchase = _db.Instance.Queryable<TPSMPurchaseOrderDetailDbModel, TPSMPurchaseOrderMainDbModel>(
                    (t1, t2) => new object[] { JoinType.Left, t1.MainId == t2.ID })
                    .Where((t1, t2) => t2.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(t2.DeleteFlag, false) != true
                && t2.AuditStatus == 2
)
                    .GroupBy((t1, t2) => t1.MaterialId)
    .Select((t1, t2) => new { MaterialId = t1.MaterialId, OrderDate = SqlFunc.AggregateMax(t2.OrderDate) }).ToList()
    .ToDictionary(p => p.MaterialId, p => new { MaterialId = p.MaterialId, OrderDate = p.OrderDate });
                var Production = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel>(
    (t1, t2) => new object[] { JoinType.Left, t1.MainId == t2.ID })
    .Where((t1, t2) => t2.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(t2.DeleteFlag, false) != true
&& t2.AuditStatus == 2)
    .GroupBy((t1, t2) => t1.MaterialId)
    .Select((t1, t2) => new { MaterialId = t1.MaterialId, OrderDate = SqlFunc.AggregateMax(t2.OrderDate) }).ToList()
    .ToDictionary(p => p.MaterialId, p => new { MaterialId = p.MaterialId, OrderDate = p.OrderDate });
                #endregion
                int totalNum = -1;
                if (requestObject.IsPaging)
                {
                    int skipNum = requestObject.PageSize * (requestObject.PageIndex - 1);
                    totalNum = ts.Count();
                    result = await ts.Select((p1, p2) => new InventoryResultModel
                    {
                        MaterialName = p1.MaterialName,
                        BaseUnitId = p1.BaseUnitId,
                        ColorId = p1.ColorId,
                        MaterialCode = p1.MaterialCode,
                        MaterialId = p1.ID,
                        Spec = p1.Spec,
                        WarehouseAmount = p2.TradeNumber,
                        WarehouseUnitId = p1.WarehouseUnitId,
                        WarehouseRate = p1.WarehouseRate,
                        WarehouseId = p2.WarehouseId,
                        ShelfLife = p1.ShelfLife
                    }).Skip(skipNum).Take(requestObject.PageSize).ToListAsync();
                }
                else
                {
                    result = await ts.Select((p1, p2) => new InventoryResultModel
                    {
                        MaterialName = p1.MaterialName,
                        BaseUnitId = p1.BaseUnitId,
                        ColorId = p1.ColorId,
                        MaterialCode = p1.MaterialCode,
                        MaterialId = p1.ID,
                        Spec = p1.Spec,
                        WarehouseAmount = p2.TradeNumber,
                        WarehouseUnitId = p1.WarehouseUnitId,
                        WarehouseRate = p1.WarehouseRate,
                        WarehouseId = p2.WarehouseId,
                        ShelfLife = p1.ShelfLife
                    }).ToListAsync();
                }
                var TWMPrimeCountDbList = _db.Instance.Queryable<TWMPrimeCountDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList();
                result.ForEach(x =>
                {
                    #region 最新生产/采购时间
                    string Production_PurchaseDateTime = "";
                    if (Production.ContainsKey(x.MaterialId))
                    {
                        Production_PurchaseDateTime += "" + Convert.ToDateTime(Production[x.MaterialId].OrderDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Production_PurchaseDateTime += "无生产";
                    }
                    if (Purchase.ContainsKey(x.MaterialId))
                    {
                        Production_PurchaseDateTime += "/" + Convert.ToDateTime(Purchase[x.MaterialId].OrderDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Production_PurchaseDateTime += "/无采购";
                    }
                    if (Production_PurchaseDateTime != "")
                    {
                        x.Production_PurchaseDateTime = Production_PurchaseDateTime;
                    }
                    #endregion
                    if (tsToOut.ContainsKey(x.MaterialId))
                    {
                        x.WarehouseAvailabilityAmount = x.WarehouseAmount - tsToOut[x.MaterialId].Amount;
                    }
                    if (x.WarehouseUnitId.HasValue)
                    {
                        if (tBMDictionary.ContainsKey(x.WarehouseUnitId.Value))
                        {
                            x.WarehouseUnitName = tBMDictionary[x.WarehouseUnitId.Value];
                        }
                    }

                    if (warehouseDic.ContainsKey(x.WarehouseId))
                    {
                        x.WarehouseName = warehouseDic[x.WarehouseId].Name;
                        x.WarehouseCode = warehouseDic[x.WarehouseId].Code;
                    }

                    if (tBMDictionary.ContainsKey(x.BaseUnitId))
                    {
                        x.BaseUnitName = tBMDictionary[x.BaseUnitId];
                    }

                    if (string.IsNullOrEmpty(x.WarehouseUnitName))
                    {
                        x.WarehouseUnitName = x.BaseUnitName;
                    }

                    if (x.WarehouseRate == null)
                    {
                        x.WarehouseRate = 1;
                    }

                    if (x.ColorId.HasValue)
                    {
                        if (tBMDictionary.ContainsKey(x.ColorId.Value))
                        {
                            x.ColorName = tBMDictionary[x.ColorId.Value];
                        }
                    }

                    var firstEntity = TWMPrimeCountDbList.Where(p => p.WarehouseId == x.WarehouseId && p.MaterialId == x.MaterialId).FirstOrDefault();

                    if (firstEntity != null)
                    {
                        x.WarehouseAmount = x.WarehouseAmount + firstEntity.PrimeNum;
                        x.PrimeNum = firstEntity.PrimeNum;
                    }

                });

                return ResponseUtil<List<InventoryResultModel>>.SuccessResult(result, totalNum);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<InventoryResultModel>>.FailResult(null, ex.Message);
            }
        }


        /// <summary>
        /// 导出期初模板
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<ExportOpeningTemplateModel>> ExportOpeningTemplate(CurrentUser currentUser)
        {
            string error = string.Empty;
            int index = 0;

                String warehouseName = "";
                var warehouseDic = await _db.Instance.Queryable<TBMWarehouseFileDbModel>().Where(t => SqlFunc.IsNull(t.DeleteFlag, false) ==false &&
                t.CompanyId == currentUser.CompanyID).ToListAsync();


            warehouseDic.ForEach(x =>
                {
                    warehouseName += "," + x.WarehouseName;
                });

                //单位
                var unitList = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>(
                                    (t, t0) => new object[]
                                    {
                                    JoinType.Left,  t.TypeId== t0.ID,
                                    }).Where((t, t0) => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == currentUser.CompanyID
                                    && SqlFunc.IsNull(t0.DeleteFlag, false) == false && t0.TypeName== "计量单位"
                                    ).Select((t,t0)=>t).ToList();

                ExportOpeningTemplateModel result = new ExportOpeningTemplateModel();
                var materialFileQuery = await _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(p => p.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(p.DeleteFlag, false) == false)
                    .Select(p => new MaterialFileTemplate
                    {
                        BasicUnitId = p.BaseUnitId,
                        WareUnitId = p.WarehouseUnitId,
                        MaterialCode = p.MaterialCode,
                        MaterialName = p.MaterialName,
                        Spec = p.Spec
                    }).ToListAsync();


                
                materialFileQuery.ForEach((x) => {
                    index++;
                    if (x.WareUnitId != null)
                    {
                        var unitDic= unitList.Where(p => p.ID == x.WareUnitId).FirstOrDefault();
                        if (unitDic == null)
                        {
                            error+=$"物料：{x.MaterialName},代码{x.MaterialCode}的 仓库单位已经被删除，请重新设置{System.Environment.NewLine}";
                        }
                        else
                        {
                            x.WareUnitName = unitDic.DicValue;
                        }
                        
                    }
                    else
                    {
                        var unitDic = unitList.Where(p => p.ID == x.BasicUnitId).FirstOrDefault();
                        if (unitDic == null)
                        {
                            error += ($"物料：{x.MaterialName},代码{x.MaterialCode}的 基本单位已经被删除，请重新设置{System.Environment.NewLine}");
                        }
                        else
                        {
                            x.WareUnitName = unitDic.DicValue;
                        }
                        
                    }
                });

                if (error != string.Empty)
                {
                    throw new Exception(error);
                }
                result.List = materialFileQuery;
                result.WarehouseName = warehouseName.Substring(1).Split("1");
                return ResponseUtil<ExportOpeningTemplateModel>.SuccessResult(result);

        }

        /// <summary>
        /// 是否已经导入期初
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> IsImported(CurrentUser currentUser)
        {
            try
            {
                bool result = true;

                result = await _db.Instance.Queryable<TWMPrimeCountDbModel>().AnyAsync(p => p.CompanyId == currentUser.CompanyID);

                return ResponseUtil<bool>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 导入期初
        /// </summary>
        public async Task<ResponseObject<List<string>>> ImportPrime(RequestPost<ImportPrimeModel> requestObject, CurrentUser currentUser)
        {

            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostDataList == null || requestObject.PostDataList.Count() == 0)
                    return ResponseUtil<List<string>>.FailResult(null, "PostDataList必须有值");
                //合法性检查

                //仓库
                var Warehouses = _db.Instance.Queryable<TBMWarehouseFileDbModel>().
                    Where(p => p.CompanyId == currentUser.CompanyID && SqlFunc.IsNull(p.DeleteFlag, false) == false).ToList();

                //单位
                var unitList = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>(
                                    (t, t0) => new object[]
                                    {
                                    JoinType.Left,  t.TypeId== t0.ID,
                                    }).Where((t, t0) => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == currentUser.CompanyID
                                    && SqlFunc.IsNull(t0.DeleteFlag, false) == false &&t0.TypeName== "计量单位"
                                    ).ToList();

                //物料
                var MaterialList = _db.Instance.Queryable<TBMMaterialFileDbModel>().Where(t => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == currentUser.CompanyID).ToList();

                List<TWMPrimeCountDbModel> tWMPrimeCountDbModels = new List<TWMPrimeCountDbModel>();

                var importPrimeModels = requestObject.PostDataList;

                int index = 1;
                List<string> errorList = new List<string>();
                List<TWMPrimeCountDbModel> TWMPrimeCountList = new List<TWMPrimeCountDbModel>();
                foreach (var item in importPrimeModels)
                {
                    string msg = string.Empty;
                    TWMPrimeCountDbModel tWMPrimeCountDbModel = new TWMPrimeCountDbModel();
                    var material = MaterialList.Where(p => p.MaterialCode == item.MaterialCode).FirstOrDefault();

                    if (material != null)
                    {
                        tWMPrimeCountDbModel.MaterialId = material.ID;


                        int unitId = material.WarehouseUnitId == null ? material.BaseUnitId : material.WarehouseUnitId.Value;

                        TBMDictionaryDbModel unit = unitList.Where(p => p.ID == unitId).FirstOrDefault();
                        if (unit.DicValue != item.UnitName)
                        {
                            msg += $"库存单位应该为{unit.DicValue}";
                        }
                    }
                    else
                    {
                        msg += $"物料代码{item.MaterialCode}不存在;";
                    }



                    var houses = Warehouses.Where(p => p.WarehouseName == item.WarehouseName).FirstOrDefault();
                    if (houses != null)
                    {
                        tWMPrimeCountDbModel.WarehouseId = houses.ID;
                    }


                    tWMPrimeCountDbModel.PrimeNum = item.Num;
                    tWMPrimeCountDbModel.CompanyId = currentUser.CompanyID;

                    if (msg != string.Empty)
                    {
                        msg = $"第{index}行数据：{msg}";
                        errorList.Add(msg);
                    }
                    else
                    {
                        TWMPrimeCountList.Add(tWMPrimeCountDbModel);
                    }

                    index++;
                }


                if (errorList.Count() > 0)
                {
                    return ResponseUtil<List<string>>.FailResult(errorList);
                }

                if (TWMPrimeCountList.Count() > 0)
                {
                    await _db.Instance.Insertable<TWMPrimeCountDbModel>(TWMPrimeCountList).ExecuteCommandAsync();
                }

                return ResponseUtil<List<string>>.SuccessResult(null);
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<string>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<List<HistoryInventory>>> History(HistoryInventoryQuery requestObject, CurrentUser currentUser)
        {
            List<HistoryInventory> result = new List<HistoryInventory>();

            //其他出库
            var otherOut = _db.Instance.Queryable<TWMOtherWhDetailDbModel, TWMOtherWhMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WarehousingOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 1,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WarehousingDate,
                TypeName = "其他入库",
                InventoryType = t2.WarehousingType,
                WarehouseAmount = t1.ActualNumber,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });

            //
            var otherIn = _db.Instance.Queryable<TWMOtherWhSendDetailDbModel, TWMOtherWhSendMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WhSendOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 2,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WhSendDate,
                TypeName = "其他出库",
                InventoryType = t2.WhSendType,
                WarehouseAmount = t1.ActualNumber,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });


            //盘盈入库
            var profitDetail = _db.Instance.Queryable<TWMProfitDetailDbModel, TWMProfitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WarehousingOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 1,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WarehousingDate,
                TypeName = "盘盈入库",
                InventoryType = t2.WarehousingType,
                WarehouseAmount = t1.ActualNumber,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });

            //盘亏出库
            var deficit = _db.Instance.Queryable<TWMDeficitDetailDbModel, TWMDeficitMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WhSendOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 2,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WhSendDate,
                TypeName = "盘亏出库",
                InventoryType = t2.WhSendType,
                WarehouseAmount = t1.ActualNumber,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });

            //生产入库
            var productionIn = _db.Instance.Queryable<TWMProductionWhDetailDbModel, TWMProductionWhMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WarehousingOrderNo,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 1,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WarehousingDate,
                TypeName = "生产入库",
                InventoryType = t2.WarehousingType,
                WarehouseAmount = t1.ActualNum,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });

            //生产出库
            var productionOut = _db.Instance.Queryable<TWMProductionDetailDbModel, TWMProductionMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WhSendOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 2,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WhSendDate,
                TypeName = "生产出库",
                InventoryType = t2.WhSendType,
                WarehouseAmount = t1.ActualNum,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });



            //销售出库
            var saleDetail = _db.Instance.Queryable<TWMSalesDetailDbModel, TWMSalesMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WhSendOrder,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 1,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WhSendDate,
                TypeName = "销售出库",
                InventoryType = t2.WhSendType,
                WarehouseAmount = t1.ActualNum,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });

            //采购入库
            var purchaseIn = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel, TBMMaterialFileDbModel>((t1, t2, t3) => new object[] {
                JoinType.Inner,
                t1.MainId==t2.ID,
                JoinType.Inner,
                t1.MaterialId==t3.ID
            }).Where((t1, t2, t3) => t2.DeleteFlag == false && t2.AuditStatus == 2 && t2.CompanyId == currentUser.CompanyID).Select((t1, t2, t3) => new HistoryInventory
            {
                ID = t2.ID,
                OrderNO = t2.WarehousingOrderNo,
                MaterialId = t1.MaterialId,
                BaseUnitId = t3.BaseUnitId,
                OperateType = 1,
                WarehouseId = t1.WarehouseId,
                OpearateDate = t2.WarehousingDate,
                TypeName = "采购入库",
                InventoryType = t2.WarehousingType,
                WarehouseAmount = t1.ActualNum,
                WarehouseUnitId = t3.WarehouseUnitId,
                WarehouseRate = t3.WarehouseRate
            });


            //单位
            var unitList = _db.Instance.Queryable<TBMDictionaryDbModel, TBMDictionaryTypeDbModel>(
                                (t, t0) => new object[]
                                {
                                    JoinType.Left,  t.TypeId== t0.ID,
                                }).Where((t, t0) => SqlFunc.IsNull(t.DeleteFlag, false) == false && t.CompanyId == currentUser.CompanyID
                                && SqlFunc.IsNull(t0.DeleteFlag, false) == false
                                ).ToList();

            //出入库数量
            var allCountOrgin = _db.Instance.UnionAll(otherOut, otherIn, profitDetail,
                deficit, productionIn, productionOut, saleDetail, purchaseIn)
                .Where(p => p.MaterialId == requestObject.MaterialId && p.WarehouseId == requestObject.WarehouseId);

            //排序条件
            if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
            {
                foreach (var item in requestObject.OrderByConditions)
                {
                    var exp = SqlSugarUtil.GetOrderByLambda<HistoryInventory>(item.Column);
                    if (exp == null) continue;
                    if (item.Condition.ToLower() != "asc"
                                              && item.Condition.ToLower() != "desc") continue;
                    allCountOrgin.OrderBy($"{item.Column} {item.Condition}");


                }
            }

            var allCount = allCountOrgin.AS("t101");


            int totalNum = -1;
            if (requestObject.IsPaging)
            {
                int skipNum = requestObject.PageSize * (requestObject.PageIndex - 1);
                result = await allCount.Skip(skipNum).Take(requestObject.PageSize).ToListAsync();
                totalNum = allCount.Count();
            }
            else
            {
                result = await allCount.ToListAsync();
            }
            result.ForEach((x) =>
            {
                var unit = unitList.Where(p => p.ID == x.WarehouseUnitId).FirstOrDefault();
                if (unit != null)
                {
                    x.WarehouseUnitName = unit.DicValue;
                }

                var basicunit = unitList.Where(p => p.ID == x.BaseUnitId).FirstOrDefault();
                if (basicunit != null)
                {
                    x.BaseUnitName = basicunit.DicValue;
                }
            });

            return ResponseUtil<List<HistoryInventory>>.SuccessResult(result, totalNum);
        }
    }
}
