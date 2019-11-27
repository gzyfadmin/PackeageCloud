///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionWhMain.cs
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
    /// 生产入库 Model
    /// </summary>
    [SugarTable("T_WM_ProductionWhMain")]
    public class TWMProductionWhMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 入库类型 1 生产入库
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingType")]
        [Required]
        public int WarehousingType { get; set; }
        
        /// <summary>
        /// 入库日期
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingDate")]
        [Required]
        public DateTime WarehousingDate { get; set; }
        
        /// <summary>
        /// 入库单号
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingOrderNo")]
        [Required]
        [StringLength(maximumLength:30)]
        public string WarehousingOrderNo { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte? AuditStatus { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 收货员ID
        /// </summary>
        [SugarColumn(ColumnName = "ReceiptId")]
        public int? ReceiptId { get; set; }
        
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
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        [Required]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        [Required]
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 源单据ID
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        [Required]
        public int SourceId { get; set; }
        
        /// <summary>
        /// 物料数量，明细总数量
        /// </summary>
        [SugarColumn(ColumnName = "Number")]
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal Number { get; set; }
        
        /// <summary>
        /// 金额，明细总金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? Amount { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TWMProductionWhDetailDbModel> ChildList { get; set; }
    }
}
