///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhDetailAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhDetail
{
    /// <summary>
    /// T_WM_OtherWhDetail Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMOtherWhDetailDbModel))]
    public class TWMOtherWhDetailAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        /// <summary>
        /// 入库主表ID
        /// </summary>
        [Required]
        public int MainId { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        [StringLength(maximumLength:30)]
        public string BatchNumber { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal ActualNumber { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidityPeriod { get; set; }
                
    }
}
