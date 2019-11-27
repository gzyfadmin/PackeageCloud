///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionWhMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain
{
    /// <summary>
    /// T_WM_ProductionWhMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMProductionWhMainDbModel))]
    public class TWMProductionWhMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 入库类型 1 生产入库
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
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }
        

        
        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }
        
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        

        
        
        /// <summary>
        /// 源单据ID
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        
        /// <summary>
        /// 物料数量，明细总数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal Number { get; set; }
        
        /// <summary>
        /// 金额，明细总金额
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? Amount { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMProductionWhDetailAddModel> ChildList { get; set; }
    }
}
