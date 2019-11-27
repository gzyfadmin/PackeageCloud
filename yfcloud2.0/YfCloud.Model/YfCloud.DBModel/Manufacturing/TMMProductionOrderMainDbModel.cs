///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// 生产单主表
    /// </summary>
    [SugarTable("T_MM_ProductionOrderMain")]
    public class TMMProductionOrderMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 生产类型 0库存生产 1订单生产
        /// </summary>
        [SugarColumn(ColumnName = "ProductionType")]
        [Required]
        public byte ProductionType { get; set; } = 0;
        
        /// <summary>
        /// 生产单号
        /// </summary>
        [SugarColumn(ColumnName = "ProductionNo")]
        [Required]
        [StringLength(maximumLength:20)]
        public string ProductionNo { get; set; }
        
        /// <summary>
        /// 源单号ID
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        [SugarColumn(ColumnName = "CustomerId")]
        public int? CustomerId { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        [SugarColumn(ColumnName = "OrderDate")]
        [Required]
        public DateTime OrderDate { get; set; }
        
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [SugarColumn(ColumnName = "BeginDate")]
        [Required]
        public DateTime BeginDate { get; set; }
        
        /// <summary>
        /// 计划结束日期
        /// </summary>
        [SugarColumn(ColumnName = "EndDate")]
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte? AuditStatus { get; set; } = 0;
        
        /// <summary>
        /// 审核人ID
        /// </summary>
        [SugarColumn(ColumnName = "AuditId")]
        public int? AuditId { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        [SugarColumn(ColumnName = "AuditTime")]
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 制单时间
        /// </summary>
        [SugarColumn(ColumnName = "OperatorTime")]
        [Required]
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// MRP运算状态 true已运算 false未运算
        /// </summary>
        [SugarColumn(ColumnName = "MRPStatus")]
        public bool? MRPStatus { get; set; } = false;

        /// <summary>
        /// MRP运算时间
        /// </summary>
        [SugarColumn(ColumnName = "MRPTime")]
        public DateTime? MRPTime { get; set; } = null;

        /// <summary>
        /// 生产状态 0待生产 1生产中 2生产完成
        /// </summary>
        [SugarColumn(ColumnName = "ProductionStatus")]
        public byte? ProductionStatus { get; set; } = 0;

        /// <summary>
        /// 转入库单标识 true可转 false不可转
        /// </summary>
        [SugarColumn(ColumnName = "TransferFlag")]
        public bool? TransferFlag { get; set; } = false;

        /// <summary>
        /// 转入生产采购单 true可转 false不可转
        /// </summary>
        [SugarColumn(ColumnName = "IsPurchase")]
        public bool? IsPurchase { get; set; } = true;

        /// <summary>
        /// 转入领料单 true可转 false不可转
        /// </summary>
        [SugarColumn(ColumnName = "IsPick")]
        public bool? IsPick { get; set; } = true;

        /// <summary>
        /// 转入领料单 true可转 false不可转
        /// </summary>
        [SugarColumn(ColumnName = "IsPickAll")]
        public bool? IsPickAll { get; set; } = false;
        

        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识 false未删除 true删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        [Required]
        public bool DeleteFlag { get; set; } = false;        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMProductionOrderDetailDbModel> ChildList { get; set; }

        /// <summary>
        /// 领料
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMPickApplyMainDbModel> Picks { get; set; }

        /// <summary>
        /// 采购
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMPurchaseApplyMainDbModel> Purchases { get; set; }

        /// <summary>
        /// 入库申请单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMWhApplyMainDbModel> WhApplyMains { get; set; }


    }
}
