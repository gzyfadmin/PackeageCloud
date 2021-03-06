﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMProductionDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionMain
{
    /// <summary>
    /// TWMProductionMain Query Model
    /// </summary>
    public class TWMProductionMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 出库类型 1 生产出库
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
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 发货员ID
        /// </summary>
        public int? SendId { get; set; }

        /// <summary>
        /// 发货员
        /// </summary>
        public string SendName { get; set; }

        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }

        /// <summary>
        /// 仓管员
        /// </summary>
        public string WhAdminName { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 源单据ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 源单据号码
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 物料数量，明细总数量
        /// </summary>
        public decimal Number { get; set; }
        
        /// <summary>
        /// 金额，明细总金额
        /// </summary>
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TWMProductionDetailQueryModel> ChildList { get; set; }
    }
}
