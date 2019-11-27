///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderBOMSum.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_ProductionOrderBOMSum Db Model
    /// </summary>
    [SugarTable("T_MM_ProductionOrderBOMSum")]
    public class TMMProductionOrderBOMSumDbModel
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
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ColorSolutionCode")]
        public string ColorSolutionCode { get; set; }
        
        /// <summary>
        /// 单用量
        /// </summary>
        [SugarColumn(ColumnName = "SingleValue")]
        public string SingleValue { get; set; }
        
        /// <summary>
        /// 总用量
        /// </summary>
        [SugarColumn(ColumnName = "TotalValue")]
        public decimal TotalValue { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseNum")]
        public decimal PurchaseNum { get; set; }
        
        /// <summary>
        /// 已转采购申请单数量
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseTransNum")]
        public decimal PurchaseTransNum { get; set; }
        
        /// <summary>
        /// 领料数量
        /// </summary>
        [SugarColumn(ColumnName = "PickNum")]
        public decimal PickNum { get; set; }
        
        /// <summary>
        /// 已转领料申请单数量
        /// </summary>
        [SugarColumn(ColumnName = "PickTransNum")]
        public decimal PickTransNum { get; set; }
        
        /// <summary>
        /// 总领料数
        /// </summary>
        [SugarColumn(ColumnName = "PickTotalNum")]
        public decimal PickTotalNum { get; set; }
        
    }
}
