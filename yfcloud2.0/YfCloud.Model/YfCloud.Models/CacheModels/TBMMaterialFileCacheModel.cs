using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
  public  class TBMMaterialFileCacheModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

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
        /// 物料分类ID
        /// </summary>
        public int MaterialTypeId { get; set; }

        /// <summary>
        /// 物料分类名称
        /// </summary>
        public string MaterialTypeName { get; set; }

        /// <summary>
        /// 默认仓库
        /// </summary>
        public int? DefaultWarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string DefaultWarehouseName { get; set; }

        /// <summary>
        /// 保质期（天）
        /// </summary>
        public decimal? ShelfLife { get; set; }

        /// <summary>
        /// 最高库存
        /// </summary>
        public decimal? HighInventory { get; set; }

        /// <summary>
        /// 最低库存
        /// </summary>
        public decimal? LowInventory { get; set; }

        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 生产计量单位ID
        /// </summary>
        public int? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产计量单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产与基本单位换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }

        /// <summary>
        /// 采购计量单位ID
        /// </summary>
        public int? PurchaseUnitId { get; set; }

        /// <summary>
        /// 采购计量单位名称
        /// </summary>
        public string PurchaseUnitName { get; set; }

        /// <summary>
        /// 采购与基本单位换算率
        /// </summary>
        public decimal? PurchaseRate { get; set; }

        /// <summary>
        /// 销售计量单位ID
        /// </summary>
        public int? SalesUnitId { get; set; }

        /// <summary>
        /// 销售计量单位名称
        /// </summary>
        public string SalesUnitName { get; set; }

        /// <summary>
        /// 销售与基本单位换算率
        /// </summary>
        public decimal? SalesRate { get; set; }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 配色方案ID
        /// </summary>
        public int? ColorSolutionID { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int? PackageID { get; set; }
    }
}
