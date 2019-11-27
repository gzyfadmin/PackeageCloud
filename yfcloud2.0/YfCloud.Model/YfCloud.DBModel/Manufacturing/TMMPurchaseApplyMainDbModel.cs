///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPurchaseApplyMain.cs
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
    /// T_MM_PurchaseApplyMain Db Model
    /// </summary>
    [SugarTable("T_MM_PurchaseApplyMain")]
    public class TMMPurchaseApplyMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 源单号（生产单ID）
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 采购申请单号
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseNo")]
        public string PurchaseNo { get; set; }
        
        /// <summary>
        /// 申请日期
        /// </summary>
        [SugarColumn(ColumnName = "ApplyDate")]
        public DateTime ApplyDate { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 制单时间
        /// </summary>
        [SugarColumn(ColumnName = "OperatorTime")]
        public DateTime OperatorTime { get; set; }
        
        /// <summary>
        /// 审核人ID
        /// </summary>
        [SugarColumn(ColumnName = "AuditId")]
        public int? AuditId { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte? AuditStatus { get; set; }
        
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
        /// 删除标识 false未删除 true已删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 转单标识 false不可转 true可转
        /// </summary>
        [SugarColumn(ColumnName = "TransferFlag")]
        public bool TransferFlag { get; set; }
        
    }
}
