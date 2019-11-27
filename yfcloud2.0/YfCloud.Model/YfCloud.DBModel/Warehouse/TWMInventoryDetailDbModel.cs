///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_InventoryDetail Db Model
    /// </summary>
    [SugarTable("T_WM_InventoryDetail")]
    public class TWMInventoryDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 盘点仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 账存数量
        /// </summary>
        [SugarColumn(ColumnName = "AccountNum")]
        public decimal AccountNum { get; set; }
        
        /// <summary>
        /// 实存数量
        /// </summary>
        [SugarColumn(ColumnName = "ActualNum")]
        public decimal ActualNum { get; set; }
        
        /// <summary>
        /// 盘盈数量
        /// </summary>
        [SugarColumn(ColumnName = "ProfitNum")]
        public decimal ProfitNum { get; set; }
        
        /// <summary>
        /// 盘亏数量
        /// </summary>
        [SugarColumn(ColumnName = "DeficitNum")]
        public decimal DeficitNum { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }
        
    }
}
