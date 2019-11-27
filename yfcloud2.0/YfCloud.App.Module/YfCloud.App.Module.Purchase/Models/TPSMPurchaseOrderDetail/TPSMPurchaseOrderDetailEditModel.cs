///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderDetailEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail
{
    /// <summary>
    /// T_PSM_PurchaseOrderDetail Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TPSMPurchaseOrderDetailDbModel))]
    public class TPSMPurchaseOrderDetailEditModel : LogModelBase
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 主单ID
        /// </summary>
        [Required]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>

        public int? SupplierId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PurchaseNum { get; set; }
        
        /// <summary>
        /// 采购金额
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? PurchaseAmount { get; set; }
        
        /// <summary>
        /// 交期
        /// </summary>
        public DateTime? DeliveryPeriod { get; set; }
        
        /// <summary>
        /// 可转单数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal TransferNum { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength:100)]
        public string Remark { get; set; }
        
    }
}
