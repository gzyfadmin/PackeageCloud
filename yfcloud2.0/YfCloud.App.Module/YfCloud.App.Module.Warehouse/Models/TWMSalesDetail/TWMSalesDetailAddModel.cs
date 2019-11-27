///////////////////////////////////////////////////////////////////////////////////////
// File: TWMSalesDetailAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Models.TWMSalesDetail
{
    /// <summary>
    /// T_WM_SalesDetail Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMSalesDetailDbModel))]
    public class TWMSalesDetailAddModel : LogModelBase
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
        /// 发货仓库ID
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }
        /// <summary>
        /// 实发数量
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
        [StringLength(maximumLength: 100)]
        public string Remark { get; set; }

        /// <summary>
        /// 来源销售明细（T_SSM_SalesOrderDetail>ID）
        /// </summary>
        [Required]
        public int SalesOrderDetailId { get; set; }

        /// <summary>
        /// 销售单位数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal SalesOrderActualNum { get; set; }
    }
}
