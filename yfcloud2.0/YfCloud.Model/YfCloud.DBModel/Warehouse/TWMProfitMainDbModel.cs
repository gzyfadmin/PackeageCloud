///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitMain.cs
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
    /// T_WM_ProfitMain Model
    /// </summary>
    [SugarTable("T_WM_ProfitMain")]
    public class TWMProfitMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 入库类型 4盘盈入库
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
        [SugarColumn(ColumnName = "WarehousingOrder")]
        [Required]
        [StringLength(maximumLength:20)]
        public string WarehousingOrder { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过 
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        [Required]
        public byte AuditStatus { get; set; } = 0;
        
        /// <summary>
        /// 操作员ID
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
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识 true删除 false未删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; } = false;
        
        /// <summary>
        /// 来源单号ID
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 盘盈入库总数
        /// </summary>
        [SugarColumn(ColumnName = "Number")]
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal Number { get; set; }
        
        /// <summary>
        /// 盘盈入库总金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? Amount { get; set; }
              

        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TWMProfitDetailDbModel> ChildList { get; set; }
    }
}
