///////////////////////////////////////////////////////////////////////////////////////
// File: TWMDeficitMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMDeficitMain
{
    /// <summary>
    /// T_WM_DeficitMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMDeficitMainDbModel))]
    public class TWMDeficitMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 出库类型 4盘亏出库
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
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMDeficitDetailAddModel> ChildList { get; set; }
    }
}
