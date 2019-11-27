///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPurchaseDetailAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail
{
    /// <summary>
    /// T_WM_PurchaseDetail Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMPurchaseDetailDbModel))]
    public class TWMPurchaseDetailAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
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
        /// 收货仓库ID
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal ActualNum { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength:500)]
        public string Remark { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidDate { get; set; }

        /// <summary>
        /// 来源采购明细（T_WM_PurchaseDetail>ID）
        /// </summary>
        [Required]
        public int PurchaseDetailId { get; set; }

        /// <summary>
        /// 采购单位数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PurchaseActualNum { get; set; }
    }
}
