///////////////////////////////////////////////////////////////////////////////////////
// File: TBMMaterialFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/6
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_BM_MaterialFile Db Model
    /// </summary>
    [SugarTable("T_BM_MaterialFile")]
    public class TBMMaterialFileDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 物料代码
        /// </summary>
        [SugarColumn(ColumnName = "MaterialCode")]
        public string MaterialCode { get; set; }
        
        /// <summary>
        /// 物料名称
        /// </summary>
        [SugarColumn(ColumnName = "MaterialName")]
        public string MaterialName { get; set; }
        
        /// <summary>
        /// 规格
        /// </summary>
        [SugarColumn(ColumnName = "Spec")]
        public string Spec { get; set; }
        
        /// <summary>
        /// 颜色Id
        /// </summary>
        [SugarColumn(ColumnName = "ColorId")]
        public int? ColorId { get; set; }
        
        /// <summary>
        /// 物料分类
        /// </summary>
        [SugarColumn(ColumnName = "MaterialTypeId")]
        public int MaterialTypeId { get; set; }
        
        /// <summary>
        /// 默认仓库
        /// </summary>
        [SugarColumn(ColumnName = "DefaultWarehouseId")]
        public int? DefaultWarehouseId { get; set; }
        
        /// <summary>
        /// 保质期（天）
        /// </summary>
        [SugarColumn(ColumnName = "ShelfLife")]
        public decimal? ShelfLife { get; set; }
        
        /// <summary>
        /// 最高库存
        /// </summary>
        [SugarColumn(ColumnName = "HighInventory")]
        public decimal? HighInventory { get; set; }
        
        /// <summary>
        /// 最低库存
        /// </summary>
        [SugarColumn(ColumnName = "LowInventory")]
        public decimal? LowInventory { get; set; }
        
        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        [SugarColumn(ColumnName = "BaseUnitId")]
        public int BaseUnitId { get; set; }
        
        /// <summary>
        /// 生产计量单位ID
        /// </summary>
        [SugarColumn(ColumnName = "ProduceUnitId")]
        public int? ProduceUnitId { get; set; }
        
        /// <summary>
        /// 生产与基本单位换算率
        /// </summary>
        [SugarColumn(ColumnName = "ProduceRate")]
        public decimal? ProduceRate { get; set; }
        
        /// <summary>
        /// 采购计量单位ID
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseUnitId")]
        public int? PurchaseUnitId { get; set; }
        
        /// <summary>
        /// 采购与基本单位换算率
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseRate")]
        public decimal? PurchaseRate { get; set; }
        
        /// <summary>
        /// 销售计量单位ID
        /// </summary>
        [SugarColumn(ColumnName = "SalesUnitId")]
        public int? SalesUnitId { get; set; }
        
        /// <summary>
        /// 销售与基本单位换算率
        /// </summary>
        [SugarColumn(ColumnName = "SalesRate")]
        public decimal? SalesRate { get; set; }
        
        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseUnitId")]
        public int? WarehouseUnitId { get; set; }
        
        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseRate")]
        public decimal? WarehouseRate { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 最后一次入库价格（审核通过后）
        /// </summary>
        [SugarColumn(ColumnName = "LastPrice")]
        public decimal? LastPrice { get; set; } = 0;


        /// <summary>
        /// 网址
        /// </summary>
        [SugarColumn(ColumnName = "Url")]
        public string Url { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageID")]
        public int? PackageID { get; set; }

        /// <summary>
        /// 配色方案ID
        /// </summary>
        [SugarColumn(ColumnName = "ColorSolutionID")]
        public int? ColorSolutionID { get; set; }
    }
}
