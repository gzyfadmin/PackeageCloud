using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetailNew;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;

namespace YfCloud.App.Module.Manufacturing.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class InAndOutModel
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int WarehouseId
        {
            get;
            set;
        }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal WhNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 出库数量
        /// </summary>
        public decimal WhSendNumber
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TradeInventoryModel
    {
        /// <summary>
        /// 出入库数量
        /// </summary>
        public decimal TradeNumber { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class InventoryResultModel
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 颜色Id
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 基本单位数量
        /// </summary>
        public decimal BaseUnitNumber
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(WarehouseAmount * (decimal)WarehouseRate, 4);
                }
                return WarehouseAmount;
            }
        }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库计量单位数量
        /// </summary>
        public decimal WarehouseAmount { get; set; }

        /// <summary>
        /// 期初库存
        /// </summary>
        public decimal PrimeNum { get; set; }

        /// <summary>
        /// 期初库存基本单位数量
        /// </summary>
        public decimal PrimeNumUnitNumber
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(PrimeNum * (decimal)WarehouseRate, 4);
                }
                return PrimeNum;
            }
        }

        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 保质期（天）
        /// </summary>
        public decimal? ShelfLife { get; set; }

        /// <summary>
        /// 生产/采购日期
        /// </summary>
        public DateTime? InDate { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class InventoryOut
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId
        {
            get;
            set;
        }

        /// <summary>
        ///数量
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }
    }

    public class NOEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string NO { get; set; }
    }


    public class AllBomExcel
    {
        

        /// <summary>
        /// 有配色方案 产品Bom
        /// </summary>
        public List<TMMBOMDetailQueryExcelModel> TMMBOMDetailQueryExcelList { get; set; }

        /// <summary>
        /// 生产单
        /// </summary>
        public List<TMMProductionOrderBOMQueryModel> ProductionOrderBOMQueryModelList { get; set; }

        /// <summary>
        /// 配色方案
        /// </summary>
        public List<PackageColorExcelModel> PackageColorExcelModelList { get; set; }
    }
}
