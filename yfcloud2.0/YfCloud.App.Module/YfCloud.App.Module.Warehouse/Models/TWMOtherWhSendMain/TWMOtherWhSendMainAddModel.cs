///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhSendMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendMain
{
    /// <summary>
    /// T_WM_OtherWhSendMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMOtherWhSendMainDbModel))]
    public class TWMOtherWhSendMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 出库类型 0销售出库 1生产出库 2裁片出库 3委外出库 4盘点出库 5其他出库
        /// </summary>
        [Required]
        public int WhSendType { get; set; }
        
        /// <summary>
        /// 出库日期
        /// </summary>
        [Required]
        public DateTime WhSendDate { get; set; }
        
        /// <summary>
        /// 出库单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:30)]
        public string WhSendOrder { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过
        /// </summary>
        public byte? AuditStatus { get; set; }
        
        
        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMOtherWhSendDetailAddModel> ChildList { get; set; }
    }
}
