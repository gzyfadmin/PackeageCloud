///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMProductionDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionMain
{
    /// <summary>
    /// T_WM_ProductionMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMProductionMainDbModel))]
    public class TWMProductionMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 出库类型 1 生产出库
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
        /// 制单人ID
        /// </summary>
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 发货员ID
        /// </summary>
        public int? SendId { get; set; }
        
        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }
        
        /// <summary>
        /// 源单据ID
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        
        /// <summary>
        /// 收货地址
        /// </summary>
        [StringLength(maximumLength:200)]
        public string ReceiptAddress { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMProductionDetailAddModel> ChildList { get; set; }
    }
}
