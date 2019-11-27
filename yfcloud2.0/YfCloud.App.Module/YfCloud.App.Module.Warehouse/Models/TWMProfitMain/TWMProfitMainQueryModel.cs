///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMProfitDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProfitMain
{
    /// <summary>
    /// TWMProfitMain Query Model
    /// </summary>
    public class TWMProfitMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 入库类型 4盘盈入库
        /// </summary>
        public int WarehousingType { get; set; }

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime WarehousingDate { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string WarehousingOrder { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过 
        /// </summary>
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }

        /// <summary>
        /// 收货员姓名
        /// </summary>
        public string ReceiptName { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员姓名
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识 true删除 false未删除
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 来源单号ID
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceOrder { get; set; }

        /// <summary>
        /// 盘盈入库总数
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 盘盈入库总金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TWMProfitDetailQueryModel> ChildList { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }
    }
}
