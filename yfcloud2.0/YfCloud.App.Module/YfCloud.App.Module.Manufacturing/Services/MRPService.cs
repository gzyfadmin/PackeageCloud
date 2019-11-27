using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOMSum;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.OrderGenerator;
using YfCloud.Framework.UnitChange;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;
using YfCloud.App.Module.Manufacturing.Service;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetailNew;
using System.Data;

namespace YfCloud.App.Module.Manufacturing.Services
{
    /// <summary>
    /// MRP
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IMRPService))]
    public class MRPService : IMRPService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IMRPService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly ITMMProductionOrderMainService _iTMMProductionOrderMainService;
        private readonly IEnumerable<ICodeMaker> _codeMakers;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="iTMMProductionOrderMainService"></param>
        public MRPService(IDbContext dbContext, ILogger<IMRPService> logger, IMapper mapper, ITMMProductionOrderMainService iTMMProductionOrderMainService, IEnumerable<ICodeMaker> codeMakers)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _iTMMProductionOrderMainService = iTMMProductionOrderMainService;
            _codeMakers = codeMakers;
        }

        /// <summary>
        /// 自动计算
        /// </summary>
        /// <param name="orderID">生产订单ID</param>
        /// <param name="Type">1,表示有配色，2表示无配色</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>计算是否成功</returns>
        public async Task<ResponseObject<MrpResultModel>> AutoComputeMRP(int orderID, int Type, CurrentUser currentUser)
        {
            try
            {
                TMMProductionOrderMainDbModel mainEntity = _db.Instance.Queryable<TMMProductionOrderMainDbModel>().Where(p => p.ID == orderID &&
                p.CompanyId == currentUser.CompanyID && p.MRPStatus == false).First();

                if (mainEntity == null)
                {
                    return ResponseUtil<MrpResultModel>.FailResult(null, "生产订单不存在，或MRP已经算过了不能重复计算");
                }

                List<TBMMaterialFileCacheModel> MMaterialList = BasicCacheGet.GetMaterial(currentUser);

                _db.Instance.BeginTran();



                var bomResult = await _iTMMProductionOrderMainService.CreateOrderBom(mainEntity.ID, currentUser); //生成BOM清单
                if (!bomResult.Result)
                {
                    throw new Exception(bomResult.ErrorInfo);
                }

                //数据字典
                var tBMDictionary = _db.Instance.Queryable<TBMDictionaryDbModel>().Where(p => p.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.DicValue);

                //仓库
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
                var allCount = _db.Instance.UnionAll(otherCount, pCount, SaleCount, PurchaseCount, ProductCount).GroupBy(p => new { p.MaterialId }).
                     Select(p => new TradeInventoryModel()
                     {
                         TradeNumber = SqlFunc.AggregateSum(p.WhNumber) - SqlFunc.AggregateSum(p.WhSendNumber),
                         MaterialId = p.MaterialId,
                     }).AS("t100");

                var materialFileQuery = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID);

                var ts = _db.Instance.Queryable(materialFileQuery, allCount, JoinType.Inner, (p1, p2) => p1.MaterialId == p2.MaterialId).Select((p1, p2) => new InventoryOut
                {
                    MaterialId = p2.MaterialId,
                    Amount = p2.TradeNumber
                });

                //销售单 所有物料的出入库数量
                var tsout1 = ts.ToList();
                var tsOut = tsout1.GroupBy(p => p.MaterialId).Select(p => new InventoryOut() { MaterialId = p.Key, Amount = p.Sum(m => m.Amount) }).ToList();

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

                var materialFileQuery1 = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID);

                var tsToOut = _db.Instance.Queryable(materialFileQuery1, allToOut, JoinType.Inner, (p1, p2) => p1.ID == p2.MaterialId).Select((p1, p2) => new InventoryOut
                {
                    MaterialId = p2.MaterialId,
                    Amount = p2.Amount
                });



                //销售单所有物料的待出库数量
                var tsToOutSum = tsToOut.ToList().GroupBy(p => p.MaterialId).Select(p => new InventoryOut() { MaterialId = p.Key, Amount = p.Sum(m => m.Amount) }).ToList();


                #endregion

                //期初
                List<InventoryOut> prime = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel, TWMPrimeCountDbModel>((t, t1) => new object[] {
                    JoinType.Inner,t.MaterialId==t1.MaterialId
                }).Select((t, t1) => t1).Distinct().ToList().GroupBy(p => p.MaterialId).Select(p => new InventoryOut() { MaterialId = p.Key, Amount = p.Sum(m => m.PrimeNum) }).ToList(); ;


                var Allmaterial = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID).ToList();


                Dictionary<int, string> colorSolution = _db.Instance.Queryable<TMMColorSolutionMainDbModel, TBMPackageDbModel>((t1, t2) =>
                 new object[] { JoinType.Inner, t1.PackageId == t2.ID }).
                Where((t1, t2) => t2.CompanyId == currentUser.CompanyID).ToList().ToDictionary(p => p.ID, p => p.SolutionCode);

                List<int> colorIDS = colorSolution.Keys.ToList();

                //生产订单的所有物料
                List<TMMProductionOrderBOMSumDbModel> sumList = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID).ToList();


                foreach (var item in sumList)
                {
                    decimal primeAmount = 0; //期初数量
                    decimal TradeNumber = 0; //出入库数量
                    decimal toOutAmount = 0; //待出数量

                    TBMMaterialFileCacheModel materialFile = MMaterialList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    if (materialFile == null)
                    {
                        throw new Exception($"物料ID:{item.MaterialId}，不存在");
                    }


                    InventoryOut primeEntity = prime.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                    if (primeEntity != null)
                    {
                        primeAmount = primeEntity.Amount;
                    }


                    InventoryOut tradeEntity = tsOut.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                    if (tradeEntity != null)
                    {
                        TradeNumber = tradeEntity.Amount;
                    }

                    InventoryOut toOutEntity = tsToOutSum.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                    if (toOutEntity != null)
                    {
                        toOutAmount = toOutEntity.Amount;
                    }

                    decimal avaibleAmountUnit = primeAmount + TradeNumber - toOutAmount;//仓库数量

                    decimal avaibleAmount = UnitChange.TranserUnit(materialFile, UnitType.Warehouse, UnitType.Produce, avaibleAmountUnit);  //生产数量

                    if (avaibleAmount >= item.TotalValue)
                    {
                        item.PurchaseNum = 0;
                        item.PickNum = item.TotalValue;
                    }
                    else
                    {
                        if (avaibleAmount < 0)
                        {
                            avaibleAmount = 0;
                        }
                        item.PurchaseNum = item.TotalValue - avaibleAmount;
                        item.PickNum = avaibleAmount;
                    }

                    item.PurchaseTransNum = 0;
                    item.PickTransNum = 0;
                }

                if (sumList.Count() > 0)
                {
                    _db.Instance.Updateable(sumList).ExecuteCommand();
                }

                mainEntity.MRPStatus = true;
                mainEntity.MRPTime = DateTime.Now;

                _db.Instance.Updateable(mainEntity).UpdateColumns(p => new { p.MRPStatus, p.MRPTime }).ExecuteCommand();

                _db.Instance.CommitTran();

                MrpResultModel bomList;
                if (Type == 1)
                {
                    bomList = GetProcuctBomByOrderID(orderID, currentUser);
                }
                else
                {
                    bomList = GetProcuctBomByOrderID(orderID, currentUser);
                }
                return ResponseUtil<MrpResultModel>.SuccessResult(bomList);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<MrpResultModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 转领料单
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject<NOEntity>> TransPick(int orderID, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();

                NOEntity nOEntity = new NOEntity();
                TMMProductionOrderMainDbModel mainEntity = _db.Instance.Queryable<TMMProductionOrderMainDbModel>().Where(p => p.ID == orderID &&
                p.CompanyId == currentUser.CompanyID && p.IsPick == true).First();
                if (mainEntity == null)
                {
                    return ResponseUtil<NOEntity>.FailResult(null, "生产订单不存在，或者已转领料单");
                }

                if (mainEntity.MRPStatus == false)
                {
                    return ResponseUtil<NOEntity>.FailResult(null, "还没算料，无法转单");
                }

                var Allmaterial = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID).ToList();

                List<TMMPickApplyDetailDbModel> PickApplyList = new List<TMMPickApplyDetailDbModel>();
                List<TMMProductionOrderBOMSumDbModel> productionList = new List<TMMProductionOrderBOMSumDbModel>();
                foreach (var item in Allmaterial)
                {
                    if (item.PickNum > 0) //领料
                    {
                        TMMPickApplyDetailDbModel ApplyDetail = new TMMPickApplyDetailDbModel();
                        ApplyDetail.ApplyNum = item.PickNum;
                        ApplyDetail.MaterialId = item.MaterialId;
                        ApplyDetail.TransNum = item.PickNum;
                        PickApplyList.Add(ApplyDetail);

                        item.PickTransNum = item.PickNum;
                        productionList.Add(item); //BOM 已转领料单数量
                    }
                }

                if (PickApplyList.Count() > 0)
                {
                    TMMPickApplyMainDbModel tMMPickApplyMainDbModel = new TMMPickApplyMainDbModel();
                    tMMPickApplyMainDbModel.DeleteFlag = false;
                    tMMPickApplyMainDbModel.SourceId = orderID;

                    string code = _codeMakers.Where(p => p.ProvideName == OrderEnum.PMR.GetDescription()).FirstOrDefault()?.MakeNo(currentUser.CompanyID);
                    tMMPickApplyMainDbModel.PickNo = code;
                    tMMPickApplyMainDbModel.ApplyDate = DateTime.Now;
                    tMMPickApplyMainDbModel.OperatorId = currentUser.UserID;
                    tMMPickApplyMainDbModel.OperatorTime = DateTime.Now;
                    tMMPickApplyMainDbModel.AuditStatus = 2;
                    tMMPickApplyMainDbModel.CompanyId = currentUser.CompanyID;
                    tMMPickApplyMainDbModel.TransferFlag = true;


                    int mId = _db.Instance.Insertable(tMMPickApplyMainDbModel).ExecuteReturnIdentity();

                    PickApplyList.ForEach((x) =>
                    {
                        x.MainId = mId;
                    });

                    _db.Instance.Insertable(PickApplyList).ExecuteCommand();

                    nOEntity.ID = mId;
                    nOEntity.NO = tMMPickApplyMainDbModel.PickNo;

                }

                if (productionList.Count() > 0)
                {
                    _db.Instance.Updateable(productionList).UpdateColumns(p => new { p.PickTransNum }).ExecuteCommand();
                }

                mainEntity.IsPick = false;
                _db.Instance.Updateable<TMMProductionOrderMainDbModel>(mainEntity).UpdateColumns(p => new { p.IsPick }).ExecuteCommand();

                _db.Instance.CommitTran();
                return ResponseUtil<NOEntity>.SuccessResult(nOEntity);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<NOEntity>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 转采购
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject<NOEntity>> TransPurchase(int orderID, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();

                NOEntity nOEntity = new NOEntity();

                TMMProductionOrderMainDbModel mainEntity = _db.Instance.Queryable<TMMProductionOrderMainDbModel>().Where(p => p.ID == orderID &&
                p.CompanyId == currentUser.CompanyID && p.IsPurchase == true).First();

                if (mainEntity == null)
                {
                    return ResponseUtil<NOEntity>.FailResult(nOEntity, "生产订单不存在，或者已转生产采购单");
                }
                if (mainEntity.MRPStatus == false)
                {
                    return ResponseUtil<NOEntity>.FailResult(nOEntity, "还没算料，无法转单");
                }

                var Allmaterial = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().Where(p => p.ProOrderId == orderID).ToList();//生产单BOM

                List<TMMPurchaseApplyDetailDbModel> PurchaseList = new List<TMMPurchaseApplyDetailDbModel>();//生产采购申请单明细表

                List<TMMProductionOrderBOMSumDbModel> productionList = new List<TMMProductionOrderBOMSumDbModel>();//生产单BOM

                foreach (var item in Allmaterial)
                {
                    if (item.PurchaseNum > 0) //采购
                    {
                        TMMPurchaseApplyDetailDbModel purchaseDeatail = new TMMPurchaseApplyDetailDbModel();
                        purchaseDeatail.ApplyNum = item.PurchaseNum;
                        purchaseDeatail.MaterialId = item.MaterialId;
                        purchaseDeatail.TransNum = item.PurchaseNum;
                        PurchaseList.Add(purchaseDeatail);

                        item.PurchaseTransNum = item.PurchaseNum;
                        productionList.Add(item);
                    }
                }

                if (PurchaseList.Count() > 0)
                {
                    TMMPurchaseApplyMainDbModel tMMPurchaseApplyMainDbModel = new TMMPurchaseApplyMainDbModel();
                    tMMPurchaseApplyMainDbModel.DeleteFlag = false;
                    tMMPurchaseApplyMainDbModel.SourceId = orderID;
                    tMMPurchaseApplyMainDbModel.PurchaseNo = _codeMakers.Where(p => p.ProvideName == OrderEnum.PR.GetDescription()).FirstOrDefault().MakeNo(currentUser.CompanyID);
                    tMMPurchaseApplyMainDbModel.ApplyDate = DateTime.Now;
                    tMMPurchaseApplyMainDbModel.OperatorId = currentUser.UserID;
                    tMMPurchaseApplyMainDbModel.OperatorTime = DateTime.Now;
                    tMMPurchaseApplyMainDbModel.AuditStatus = 2;
                    tMMPurchaseApplyMainDbModel.CompanyId = currentUser.CompanyID;
                    tMMPurchaseApplyMainDbModel.TransferFlag = true;
                    int mId = _db.Instance.Insertable(tMMPurchaseApplyMainDbModel).ExecuteReturnIdentity();

                    PurchaseList.ForEach((x) =>
                    {
                        x.MainId = mId;
                    });

                    await _db.Instance.Insertable(PurchaseList).ExecuteCommandAsync();

                    nOEntity.NO = tMMPurchaseApplyMainDbModel.PurchaseNo;
                    nOEntity.ID = mId;


                }

                if (productionList.Count() > 0)
                {
                    _db.Instance.Updateable(productionList).UpdateColumns(p => new { p.PurchaseTransNum }).ExecuteCommand();
                }

                mainEntity.IsPurchase = false;
                _db.Instance.Updateable(mainEntity).UpdateColumns(p => new { p.IsPurchase }).ExecuteCommand();

                _db.Instance.CommitTran();

                return ResponseUtil<NOEntity>.SuccessResult(nOEntity);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<NOEntity>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取生产单对应的EXCEL数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public AllBomExcel GetProductDataForExcel(int orderID, CurrentUser currentUser)
        {
            AllBomExcel allBomExcel = new AllBomExcel();

            #region 生产单
            var query = _db.Instance.Queryable<TMMProductionOrderBOMDbModel,
                TMMColorSolutionMainDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel, TBMPackageDbModel,TBMDictionaryDbModel, 
                TBMDictionaryDbModel>((t, t1, t2, t3, t4,t5,t6) => new object[] {
                JoinType.Left,t.ColorSolutionId==t1.ID,
                JoinType.Inner,t.MaterialId==t2.ID,
                JoinType.Inner, t2.ColorId==t3.ID,
                JoinType.Inner,t4.ID==t.PackageId,
                JoinType.Left,t5.ID==t.PartId,
                JoinType.Left,t6.ID==t.ItemId
            }).
            Where((t, t1, t2, t3, t4,t5,t6) => t.ProOrderId == orderID).Select((t, t1, t2, t3, t4,t5,t6) => new TMMProductionOrderBOMQueryModel()
            {
                ID = t.ID,
                ProOrderId = t.ProOrderId,
                MaterialId = t.MaterialId,
                ColorSolutionCode = SqlFunc.IsNull(t1.SolutionCode, "无配色"),
                MaterialName = t2.MaterialName,
                SingleValue = t.SingleValue,
                PartId =t.PartId,
                PartName= t5.DicValue,
                ProductionNum = t.ProductionNum,
                ColorId = t2.ColorId,
                PackageId = t.PackageId,
                ColorName = SqlFunc.IsNull(t3.DicValue, "无色"),
                PackageName = t4.DicValue,
                ItemId=t.ItemId,
                ItemName=t6.DicValue
            }).ToList();

            allBomExcel.ProductionOrderBOMQueryModelList = query;

            #endregion

            //bom
            var PackageColor = _db.Instance.Queryable<TMMProductionOrderDetailDbModel, TMMProductionOrderMainDbModel,TBMPackageDbModel>((t, t1,t2) => new object[] {
               JoinType.Inner,t.MainId==t1.ID,
               JoinType.Inner,t.PackageId==t2.ID
           }).Where((t, t1,t2) => t.ID == orderID).Select((t, t1,t2) => new { PackageId= t.PackageId, ColorSolutionId= t.ColorSolutionId, ProductionNum=t.ProductionNum, PackageName=t2.DicValue }).ToList();

            var PackageList = PackageColor.Where(p => p.ColorSolutionId != null).Select(p => p.PackageId).ToList().Distinct();//有配色方案的包型

            var NoPackageList= PackageColor.Where(p => p.ColorSolutionId == null).Select(p => p.PackageId).ToList().Distinct();//无配色方案的包型

            var ColorSolutionIdList= PackageColor.Where(p => p.ColorSolutionId != null).Select(p =>new { p.PackageId, p.ColorSolutionId }).ToList().Distinct();

            //有配色的Bom
            var PackBom = _db.Instance.Queryable<TMMBOMDetailDbModel, TBMDictionaryDbModel, TMMFormulaDbModel, TMMPackageColorItemDbModel, TMMBOMMainDbModel>((t, t1, t2, t3,t4) => new object[] {
                    JoinType.Left,t.PartId==t1.ID,
                    JoinType.Left,t.Formula==t2.ID.ToString(),
                    JoinType.Inner,t.ItemId==t3.ID,
                    JoinType.Inner,t.MainId==t4.ID
                }).Where((t, t1, t2, t3, t4)=> PackageList.Contains(t4.PackageId));
            PackBom.OrderBy($"t3.ItemName asc");

            var queryData = PackBom.Select((t, t1, t2, t3,t4) => new TMMBOMDetailQueryExcelModel
            {
                ID = t.ID,
                MainId = t.MainId,
                ItemId = t.ItemId,
                ItemName = t3.ItemName,
                MaterialName = t.MaterialName,
                PartId = t.PartId,
                PartName = t1.DicValue,
                LengthValue = t.LengthValue,
                WidthValue = t.WidthValue,
                NumValue = t.NumValue,
                WideValue = t.WideValue,
                LossValue = t.LossValue,
                SingleValue = t.SingleValue,
                Formula = t.Formula,
                FormulaName = t2.FormulaName,
                PackageId=t4.PackageId
            }).ToList();

            allBomExcel.TMMBOMDetailQueryExcelList = queryData;

            List<PackageColorExcelModel> PackageColorExcelModelList = new List<PackageColorExcelModel>();
            foreach (var packageItem in PackageList)
            {

                var colorIdList = ColorSolutionIdList.Where(p => p.PackageId == packageItem).Select(p=>p.PackageId).ToList();

                var detailList = _db.Instance.Queryable<TMMColorSolutionDetailDbModel, TMMPackageColorItemDbModel, TBMDictionaryDbModel, TMMColorSolutionMainDbModel>(
                          (t, t0, t1, t2) => new object[]
                          {
                            JoinType.Left , t.ItemId == t0.ID,
                            JoinType.Left , t.ColorId == t1.ID,
                            JoinType.Inner,t.MainId==t2.ID
                          }
                      ).Where((t, t0, t1, t2) => colorIdList.Contains(t2.ID)).Select((t, t0, t1, t2) => new TMMColorSolutionDetailQueryExcelModel
                      {
                          ID = t.ID,
                          MainId = t.MainId,
                          ItemId = t.ItemId,
                          ItemName = t0.ItemName,
                          ColorId = t.ColorId,
                          ColorName = t1.DicValue,
                          SolutionCode=t2.SolutionCode
                      }).ToList();

                PackageColorExcelModel packageColorExcelModel = new PackageColorExcelModel();
                packageColorExcelModel.PackageId = packageItem;

                var dt = new DataTable();
                dt.Columns.Add($"数量");

                foreach (var item in detailList.Select(p=>new { p.MainId,p.SolutionCode}).Distinct())
                {
                    var num = PackageColor.Where(p => p.ColorSolutionId == item.MainId).Sum(p => p.ProductionNum);
                    //添加动态列
                    foreach (var item2 in detailList.Where(p=>p.MainId==item.MainId))
                    {
                        if (!dt.Columns.Contains($"{item2.ItemName}"))
                            dt.Columns.Add($"{item2.ItemName}");

                        var newRow = dt.NewRow();
                        newRow[$"{item2.ItemName}"] = item2.ColorId;
                        newRow["数量"] = num;
                        dt.Rows.Add(newRow);
                    }
                }

                packageColorExcelModel.dt = dt;

                PackageColorExcelModelList.Add(packageColorExcelModel);

            }

            allBomExcel.PackageColorExcelModelList = PackageColorExcelModelList;

            return allBomExcel;

        }

     

        private MrpResultModel GetProcuctBomByOrderID(int orderID, CurrentUser currentUser)
        {

            MrpResultModel result = new MrpResultModel();

            #region 明细

            List<TMMProductionOrderBOMQueryModel> deatail = new List<TMMProductionOrderBOMQueryModel>();

            List<TBMMaterialFileCacheModel> MMaterialList = BasicCacheGet.GetMaterial(currentUser);

           
                
            var query = _db.Instance.Queryable<TMMProductionOrderBOMDbModel,
                TMMColorSolutionMainDbModel, TBMMaterialFileDbModel, TBMDictionaryDbModel, TBMPackageDbModel>((t, t1, t2, t3, t4) => new object[] {
                JoinType.Left,t.ColorSolutionId==t1.ID,
                JoinType.Inner,t.MaterialId==t2.ID,
                JoinType.Inner, t2.ColorId==t3.ID,
                JoinType.Inner,t4.ID==t.PackageId

            }).Where((t, t1, t2, t3, t4) => t.ProOrderId == orderID).Select((t, t1, t2, t3, t4) => new TMMProductionOrderBOMQueryModel()
            {
                ID = t.ID,
                ProOrderId = t.ProOrderId,
                MaterialId = t.MaterialId,
                ColorSolutionCode = SqlFunc.IsNull(t1.SolutionCode, "无配色"),
                MaterialName = t2.MaterialName,
                SingleValue = t.SingleValue,
                ProductionNum = t.ProductionNum,
                ColorId = t2.ColorId,
                PackageId=t.PackageId,
                ColorName = SqlFunc.IsNull(t3.DicValue,"无色"),
                PackageName = t4.DicValue
            });

            List<TMMProductionOrderBOMQueryModel> bomList = query.ToList();

            var packBomGroup = bomList.GroupBy(p =>new  { p.PackageName,p.PackageId});
            foreach (var item in packBomGroup)
            {
                var PackageName = item.Key.PackageName;
                var packageId = item.Key.PackageId;
                var itemDeatails = item.ToList();

                var MaterialGroup = itemDeatails.GroupBy(p => new { p.MaterialName, p.MaterialId, p.ColorName,p.ColorId });

                foreach (var itemChild in MaterialGroup)
                {
                    var MaterialName = itemChild.Key.MaterialName;
                    var materialID= itemChild.Key.MaterialId;
                    var ColorName = itemChild.Key.ColorName;
                    var colorId = itemChild.Key.ColorId;

                    var singleGroup = itemChild.ToList().GroupBy(p => p.SingleValue);

                    foreach (var itemChildChild in singleGroup)
                    {
                        var single = itemChildChild.Key;
                        var list = itemChildChild.ToList();

                        TMMProductionOrderBOMQueryModel model = new TMMProductionOrderBOMQueryModel();
                        model.PackageId = packageId;
                        model.MaterialId = materialID;
                        model.MaterialName = MaterialName;
                        model.PackageName = PackageName;
                        model.ColorName = ColorName;
                        model.ColorId = colorId;
                        model.SingleValue = single;
                        model.ProductionNum = list.Sum(p => p.ProductionNum);
                        model.ColorSolutionCode = string.Join(",", list.Select(p => p.ColorSolutionCode).Distinct());
                        deatail.Add(model);
                    }
                }
            }

            deatail.ForEach((x) =>
            {
                TBMMaterialFileCacheModel item = MMaterialList.Where(p => p.ID == x.MaterialId).FirstOrDefault();
                if (item != null)
                {
                    x.MaterialName = item.MaterialName;
                    x.MaterialCode = item.MaterialCode;
                    x.BaseUnitId = item.BaseUnitId;
                    x.BaseUnitName = item.BaseUnitName;
                    x.ProduceUnitId = item.ProduceUnitId;
                    x.ProduceUnitName = item.ProduceUnitName;
                }
            });

            #endregion

            List<TMMProductionOrderBOMSumQueryModel> bomListSummary = _db.Instance.Queryable<TMMProductionOrderBOMSumDbModel>().
                Where((t) => t.ProOrderId == orderID).Select((t) => new TMMProductionOrderBOMSumQueryModel()
                {
                    ID = t.ID,
                    ProOrderId = t.ProOrderId,
                    MaterialId = t.MaterialId,
                    ColorSolutionCode = t.ColorSolutionCode,
                    TotalValue = t.TotalValue,
                    PurchaseNum = t.PurchaseNum,
                    PurchaseTransNum = t.PurchaseTransNum,
                    PickNum = t.PickNum,
                    PickTransNum = t.PickTransNum,
                    PickTotalNum = t.PickTotalNum,
                }).ToList();

            bomListSummary.ForEach((x) =>
            {
                TBMMaterialFileCacheModel item = MMaterialList.Where(p => p.ID == x.MaterialId).FirstOrDefault();
                if (item != null)
                {
                    x.MaterialName = item.MaterialName;
                    x.MaterialCode = item.MaterialCode;
                    x.BaseUnitId = item.BaseUnitId;
                    x.BaseUnitName = item.BaseUnitName;
                    x.ProduceUnitId = item.ProduceUnitId;
                    x.ProduceUnitName = item.ProduceUnitName;
                }
            });

            result.deatails = deatail;
            result.summary = bomListSummary;

            return result;
        }


        public async Task<ResponseObject<SumQueryApiModel>> GetBom(int orderID, CurrentUser currentUser)
        {
            try
            {
                SumQueryApiModel result = new SumQueryApiModel();
                List<TBMMaterialFileCacheModel> MMaterialList = BasicCacheGet.GetMaterial(currentUser);
                var bomList = GetProcuctBomByOrderID(orderID, currentUser);
                result.MainList = bomList;
                result.PackageId = _db.Instance.Queryable<TMMProductionOrderBOMDbModel>().Where(p => p.ProOrderId == orderID).Select(p => p.PackageId).Distinct().ToList();

                return ResponseUtil<SumQueryApiModel>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil<SumQueryApiModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 采购完成后重新领料
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<NOEntity>> RePick(int orderID, CurrentUser currentUser)
        {
            try
            {
                TMMProductionOrderMainDbModel tMMProductionOrderMainDbModel = _db.Instance.Queryable<TMMProductionOrderMainDbModel>().Where(p => p.ID == orderID).First();

                if (tMMProductionOrderMainDbModel.IsPickAll == true)
                {
                    throw new Exception("已全部转领料");
                }

                TMMPurchaseApplyMainDbModel purchaseEntity = await _db.Instance.Queryable<TMMPurchaseApplyMainDbModel>().Where(p => p.SourceId == orderID && p.CompanyId == currentUser.CompanyID && p.DeleteFlag == false).FirstAsync();
                NOEntity nOEntity = new NOEntity();
                if (purchaseEntity == null)
                {
                    throw new Exception("还没采购，或者无需采购");
                }

                if (purchaseEntity.TransferFlag == true)
                {
                    throw new Exception("采购流程未完成");
                }
                else
                {
                    int producePurchaseId = purchaseEntity.ID;
                    if (_db.Instance.Queryable<TPSMPurchaseOrderMainDbModel>().Any(p => p.DeleteFlag == false && p.TransferStatus == true && p.SourceId == purchaseEntity.ID))
                    {
                        throw new Exception("采购流程未完成");
                    }
                    var allTWMPurchase = _db.Instance.Queryable<TWMPurchaseDetailDbModel, TWMPurchaseMainDbModel>((t, t1) => new object[] { JoinType.Inner, t.MainId == t1.ID }).
                           Where((t, t1) => t1.DeleteFlag == false && SqlFunc.Subqueryable<TPSMPurchaseOrderMainDbModel>().
                           Where(p1 => p1.SourceId == producePurchaseId && t1.SourceId == p1.ID).Any()).Select((t, t1) => new { Main = t1, Deatail = t }).ToList();

                    if (allTWMPurchase.Any(p => p.Main.AuditStatus != 2))
                    {
                        throw new Exception("采购流程未完成");
                    }
                    else
                    {
                        var basicMe = BasicCacheGet.GetMaterial(currentUser);
                        var groupSum = allTWMPurchase.Select(p => p.Deatail).GroupBy(x => x.MaterialId);

                        List<TMMPickApplyDetailDbModel> PickApplyList = new List<TMMPickApplyDetailDbModel>();
                        foreach (var item in groupSum)
                        {

                            var key = item.Key;
                            var itemDeatails = item.ToList();
                            var firstItem = itemDeatails.FirstOrDefault();

                            decimal total = item.Sum(p => p.ActualNum);
                            var me = basicMe.Where(p => p.ID == item.Key).FirstOrDefault();

                            decimal ProduceTotal = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, total);

                            TMMPickApplyDetailDbModel ApplyDetail = new TMMPickApplyDetailDbModel();
                            ApplyDetail.ApplyNum = ProduceTotal;
                            ApplyDetail.MaterialId = key;
                            ApplyDetail.TransNum = ProduceTotal;
                            PickApplyList.Add(ApplyDetail);
                        }

                        if (PickApplyList.Count() > 0)
                        {
                            TMMPickApplyMainDbModel tMMPickApplyMainDbModel = new TMMPickApplyMainDbModel();
                            tMMPickApplyMainDbModel.DeleteFlag = false;
                            tMMPickApplyMainDbModel.SourceId = orderID;
                            //tMMPickApplyMainDbModel.PickNo = OrderGenerator.Generator(OrderEnum.PMR, currentUser.CompanyID);
                            string code = _codeMakers.Where(p => p.ProvideName == OrderEnum.PMR.GetDescription()).FirstOrDefault()?.MakeNo(currentUser.CompanyID);
                            tMMPickApplyMainDbModel.PickNo = code;
                            tMMPickApplyMainDbModel.ApplyDate = DateTime.Now;
                            tMMPickApplyMainDbModel.OperatorId = currentUser.UserID;
                            tMMPickApplyMainDbModel.OperatorTime = DateTime.Now;
                            tMMPickApplyMainDbModel.AuditStatus = 2;
                            tMMPickApplyMainDbModel.CompanyId = currentUser.CompanyID;
                            tMMPickApplyMainDbModel.TransferFlag = true;

                            int mId = _db.Instance.Insertable(tMMPickApplyMainDbModel).ExecuteReturnIdentity();

                            PickApplyList.ForEach((x) =>
                            {
                                x.MainId = mId;
                            });

                            _db.Instance.Insertable(PickApplyList).ExecuteCommand();

                            _db.Instance.Updateable<TMMProductionOrderMainDbModel>().SetColumns(p => new TMMProductionOrderMainDbModel { IsPickAll = true }).Where(p => p.ID == orderID).ExecuteCommand();

                            nOEntity.ID = tMMPickApplyMainDbModel.ID;
                            nOEntity.NO = tMMPickApplyMainDbModel.PickNo;

                        }
                    }
                }

                _db.Instance.CommitTran();
                return ResponseUtil<NOEntity>.SuccessResult(nOEntity);
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<NOEntity>.FailResult(null, ex.Message);
            }
        }

    }
}
