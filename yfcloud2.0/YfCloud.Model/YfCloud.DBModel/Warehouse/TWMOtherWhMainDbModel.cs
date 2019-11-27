///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_OtherWhMain Db Model
    /// </summary>
    [SugarTable("T_WM_OtherWhMain")]
    public class TWMOtherWhMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 入库类型 0采购入库 1生产入库 2委外入库 3其他入库
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingType")]
        public int WarehousingType { get; set; }
        
        /// <summary>
        /// 入库日期
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingDate")]
        public DateTime WarehousingDate { get; set; }
        
        /// <summary>
        /// 入库单号
        /// </summary>
        [SugarColumn(ColumnName = "WarehousingOrder")]
        public string WarehousingOrder { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过 3撤销审核
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte AuditStatus { get; set; }
        
        /// <summary>
        /// 操作员ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
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
        public int? CompanyId { get; set; }

        /// <summary>
        /// 删除标识 true删除 false未删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool? DeleteFlag { get; set; } = false;
        
        /// <summary>
        /// 物料数量，明细总数量
        /// </summary>
        [SugarColumn(ColumnName = "Number")]
        public decimal Number { get; set; }
        
        /// <summary>
        /// 金额，明细金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        public decimal? Amount { get; set; }
        
    }
}
