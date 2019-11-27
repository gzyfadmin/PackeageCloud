///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionWhDetailEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionWhDetail
{
    /// <summary>
    /// T_WM_ProductionWhDetail Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TWMProductionWhDetailDbModel))]
    public class TWMProductionWhDetailEditModel : LogModelBase
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
        /// 生产入库单详细表ID
        /// </summary>
        [Required]
        public int ProOrderDetailId { get; set; }

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
        /// 生产车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 生产场地ID
        /// </summary>
        public int SiteId { get; set; }
    }
}
