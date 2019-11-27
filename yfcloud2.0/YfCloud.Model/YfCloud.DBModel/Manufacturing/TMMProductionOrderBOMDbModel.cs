///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderBOM.cs
// Author: www.cloudyf.com
// Create Time:2019/9/6
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_ProductionOrderBOM Db Model
    /// </summary>
    [SugarTable("T_MM_ProductionOrderBOM")]
    public class TMMProductionOrderBOMDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 生产单ID
        /// </summary>
        [SugarColumn(ColumnName = "ProOrderId")]
        public int ProOrderId { get; set; }

        /// <summary>
        /// 配色方案ID
        /// </summary>
        [SugarColumn(ColumnName = "ColorSolutionId")]
        public int? ColorSolutionId { get; set; }

        /// <summary>
        /// 配色项目ID
        /// </summary>
        [SugarColumn(ColumnName = "ItemId")]
        public int? ItemId { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        public int PackageId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 部位ID
        /// </summary>
        [SugarColumn(ColumnName = "PartId")]
        public int? PartId { get; set; }
        
        /// <summary>
        /// 长度值
        /// </summary>
        [SugarColumn(ColumnName = "LengthValue")]
        public decimal? LengthValue { get; set; }
        
        /// <summary>
        /// 宽度值
        /// </summary>
        [SugarColumn(ColumnName = "WidthValue")]
        public decimal? WidthValue { get; set; }
        
        /// <summary>
        /// 件数
        /// </summary>
        [SugarColumn(ColumnName = "NumValue")]
        public decimal NumValue { get; set; }
        
        /// <summary>
        /// 封度（宽幅）
        /// </summary>
        [SugarColumn(ColumnName = "WideValue")]
        public decimal? WideValue { get; set; }
        
        /// <summary>
        /// 损耗
        /// </summary>
        [SugarColumn(ColumnName = "LossValue")]
        public decimal? LossValue { get; set; }
        
        /// <summary>
        /// 单用量
        /// </summary>
        [SugarColumn(ColumnName = "SingleValue")]
        public decimal SingleValue { get; set; }
        
        /// <summary>
        /// 单用量计算公式
        /// </summary>
        [SugarColumn(ColumnName = "Formula")]
        public string Formula { get; set; }
        
        /// <summary>
        /// 总用量
        /// </summary>
        [SugarColumn(ColumnName = "TotalValue")]
        public decimal TotalValue { get; set; }
        
        /// <summary>
        /// 生产数量
        /// </summary>
        [SugarColumn(ColumnName = "ProductionNum")]
        public decimal ProductionNum { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseNum")]
        public decimal PurchaseNum { get; set; } = 0;

        /// <summary>
        /// 已转采购申请单数量
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseTransNum")]
        public decimal PurchaseTransNum { get; set; } = 0;

        /// <summary>
        /// 领料数量
        /// </summary>
        [SugarColumn(ColumnName = "PickNum")]
        public decimal PickNum { get; set; } = 0;

        /// <summary>
        /// 已转领料申请单数量
        /// </summary>
        [SugarColumn(ColumnName = "PickTransNum")]
        public decimal PickTransNum { get; set; } = 0;

        /// <summary>
        /// 总领料数
        /// </summary>
        [SugarColumn(ColumnName = "PickTotalNum")]
        public decimal PickTotalNum { get; set; } = 0;
        
    }
}
