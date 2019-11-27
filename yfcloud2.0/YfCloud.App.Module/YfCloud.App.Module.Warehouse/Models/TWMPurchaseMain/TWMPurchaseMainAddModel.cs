///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPurchaseMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMPurchaseMain
{
    /// <summary>
    /// T_WM_PurchaseMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMPurchaseMainDbModel))]
    public class TWMPurchaseMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 入库类型 0 采购入库
        /// </summary>
        [Required]
        public int WarehousingType { get; set; }
        
        /// <summary>
        /// 入库日期
        /// </summary>
        [Required]
        public DateTime WarehousingDate { get; set; }
        
        /// <summary>
        /// 入库单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:30)]
        public string WarehousingOrderNo { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
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
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMPurchaseDetailAddModel> ChildList { get; set; }
    }
}
