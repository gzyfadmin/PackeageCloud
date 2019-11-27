///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    ///生产出库
    /// </summary>
    [SugarTable("T_WM_ProductionDetail")]
    public class TWMProductionDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主单ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 发货仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 实发数量
        /// </summary>
        [SugarColumn(ColumnName = "ActualNum")]
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 领料单详情ID
        /// </summary>
        [SugarColumn(ColumnName = "PickApplyDetailId")]
        public int PickApplyDetailId { get; set; }

        /// <summary>
        /// 实发数量(生产单位)
        /// </summary>
        [SugarColumn(ColumnName = "PickActualNum")]
        public decimal PickActualNum { get; set; }



        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice")]
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }
        
    }
}
