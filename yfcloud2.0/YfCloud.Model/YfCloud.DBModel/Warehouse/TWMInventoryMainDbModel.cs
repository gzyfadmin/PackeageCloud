///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryMain.cs
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
    /// T_WM_InventoryMain Model
    /// </summary>
    [SugarTable("T_WM_InventoryMain")]
    public class TWMInventoryMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        [Required]
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 盘点日期
        /// </summary>
        [SugarColumn(ColumnName = "InventoryDate")]
        [Required]
        public DateTime InventoryDate { get; set; }
        
        /// <summary>
        /// 盘点单号
        /// </summary>
        [SugarColumn(ColumnName = "InventoryOrder")]
        [Required]
        [StringLength(maximumLength:30)]
        public string InventoryOrder { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte? AuditStatus { get; set; } = 0;
        
        /// <summary>
        /// 操作员ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 仓管员ID
        /// </summary>
        [SugarColumn(ColumnName = "WhAdminId")]
        public int? WhAdminId { get; set; }
        
        /// <summary>
        /// 审核员ID
        /// </summary>
        [SugarColumn(ColumnName = "AuditId")]
        public int? AuditId { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        [SugarColumn(ColumnName = "AuditTime")]
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; } = false;
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        [Required]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 盘点员ID
        /// </summary>
        [SugarColumn(ColumnName = "InventoryId")]
        public int? InventoryId { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TWMInventoryDetailDbModel> ChildList { get; set; }
    }
}
