///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitMainAddModel.cs
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
using YfCloud.App.Module.Warehouse.Models.TWMProfitDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProfitMain
{
    /// <summary>
    /// T_WM_ProfitMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMProfitMainDbModel))]
    public class TWMProfitMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 入库类型 4盘盈入库
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
        [StringLength(maximumLength:20)]
        public string WarehousingOrder { get; set; }
        
        
        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMProfitDetailAddModel> ChildList { get; set; }
    }
}
