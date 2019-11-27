///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhSendMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendMain
{
    /// <summary>
    /// TWMOtherWhSendMain Query Model
    /// </summary>
    public class TWMOtherWhSendMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 出库类型 0销售出库 1生产出库 2裁片出库 3委外出库 4盘点出库 5其他出库
        /// </summary>
        public int WhSendType { get; set; }
        
        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime WhSendDate { get; set; }
        
        /// <summary>
        /// 出库单号
        /// </summary>
        public string WhSendOrder { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过
        /// </summary>
        public byte? AuditStatus { get; set; }
        
        /// <summary>
        /// 操作员ID
        /// </summary>
        public int? OperatorId { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }

        /// <summary>
        /// 收货员ID
        /// </summary>
        public string ReceiptName { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int? CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 是否显示编辑按钮
        /// </summary>
        public bool IsShowEdit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TWMOtherWhSendDetailQueryModel> ChildList { get; set; }
    }
}
