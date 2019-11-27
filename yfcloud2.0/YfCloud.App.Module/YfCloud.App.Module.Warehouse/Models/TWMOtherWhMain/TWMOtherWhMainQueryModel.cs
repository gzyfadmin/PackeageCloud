///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain
{
    /// <summary>
    /// TWMOtherWhMain Query Model
    /// </summary>
    public class TWMOtherWhMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 入库类型
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
        /// 审核状态 1待审核 2未通过 3通过 4撤销审核
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
        /// 删除标识 true删除 false未删除
        /// </summary>
        public bool? DeleteFlag { get; set; }
        
        /// <summary>
        /// 企业ID，界面不需要显示
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 入库总数量，即明细总数量
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 入库总金额,即明细总金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<TWMOtherWhDetailQueryModel> ChildList { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }
    }
}
