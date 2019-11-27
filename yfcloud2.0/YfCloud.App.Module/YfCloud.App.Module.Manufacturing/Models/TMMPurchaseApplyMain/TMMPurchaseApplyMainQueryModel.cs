///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPurchaseApplyMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyMain
{
    /// <summary>
    /// TMMPurchaseApplyMain Query Model
    /// </summary>
    public class TMMPurchaseApplyMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 源单号（生产单ID）
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string SourceNo { get; set; }
        
        /// <summary>
        /// 采购申请单号
        /// </summary>
        public string PurchaseNo { get; set; }
        
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ApplyDate { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单人姓名
        /// </summary>
        public string OperatorName { get; set; }
        
        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime OperatorTime { get; set; }
        
        /// <summary>
        /// 审核人ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string AuditName { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 转单标识 false不可转 true可转
        /// </summary>
        public bool TransferFlag { get; set; }        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMPurchaseApplyDetailQueryModel> ChildList { get; set; }
    }
}
