///////////////////////////////////////////////////////////////////////////////////////
// File: TWMDeficitMain.cs
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
    /// T_WM_DeficitMain Model
    /// </summary>
    [SugarTable("T_WM_DeficitMain")]
    public class TWMDeficitMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 出库类型 4盘亏出库
        /// </summary>
        [SugarColumn(ColumnName = "WhSendType")]
        [Required]
        public int WhSendType { get; set; }
        
        /// <summary>
        /// 出库日期
        /// </summary>
        [SugarColumn(ColumnName = "WhSendDate")]
        [Required]
        public DateTime WhSendDate { get; set; }
        
        /// <summary>
        /// 出库单号
        /// </summary>
        [SugarColumn(ColumnName = "WhSendOrder")]
        [Required]
        [StringLength(maximumLength:30)]
        public string WhSendOrder { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte AuditStatus { get; set; }
        
        /// <summary>
        /// 操作员ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        public int? OperatorId { get; set; }
        
        /// <summary>
        /// 收货员ID
        /// </summary>
        [SugarColumn(ColumnName = "ReceiptId")]
        public int? ReceiptId { get; set; }
        
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
        public int? CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        [Required]
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 来源单号ID
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 出库数量，明细总数量
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
        public List<TWMDeficitDetailDbModel> ChildList { get; set; }
    }
}
