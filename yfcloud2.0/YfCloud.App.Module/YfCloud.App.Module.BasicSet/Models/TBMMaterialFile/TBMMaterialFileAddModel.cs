///////////////////////////////////////////////////////////////////////////////////////
// File: TBMMaterialFileAddModel.cs
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

namespace YfCloud.App.Module.Warehouse.Models.TBMMaterialFile
{
    /// <summary>
    /// T_BM_MaterialFile Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMMaterialFileDbModel))]
    public class TBMMaterialFileAddModel : LogModelBase
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string MaterialName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [StringLength(maximumLength:50)]
        public string Spec { get; set; }
        /// <summary>
        /// 颜色Id
        /// </summary>
        public int? ColorId { get; set; }
        /// <summary>
        /// 物料分类
        /// </summary>
        [Required]
        public int MaterialTypeId { get; set; }
        /// <summary>
        /// 默认仓库
        /// </summary>
        public int? DefaultWarehouseId { get; set; }
        /// <summary>
        /// 保质期（天）
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,9}(\\.\\d{1,1})?$", ErrorMessage = "整数位须小于等于9位，小数位须小于等于1位")]
        public decimal? ShelfLife { get; set; }
        /// <summary>
        /// 最高库存
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? HighInventory { get; set; }
        /// <summary>
        /// 最低库存
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? LowInventory { get; set; }
        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        [Required]
        public int BaseUnitId { get; set; }
        /// <summary>
        /// 生产计量单位ID
        /// </summary>
        public int? ProduceUnitId { get; set; }
        /// <summary>
        /// 生产与基本单位换算率
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,3}(\\.\\d{1,2})?$", ErrorMessage = "整数位须小于等于3位，小数位须小于等于2位")]
        public decimal? ProduceRate { get; set; }
        /// <summary>
        /// 采购计量单位ID
        /// </summary>
        public int? PurchaseUnitId { get; set; }
        /// <summary>
        /// 采购与基本单位换算率
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,3}(\\.\\d{1,2})?$", ErrorMessage = "整数位须小于等于3位，小数位须小于等于2位")]
        public decimal? PurchaseRate { get; set; }
        /// <summary>
        /// 销售计量单位ID
        /// </summary>
        public int? SalesUnitId { get; set; }
        /// <summary>
        /// 销售与基本单位换算率
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,3}(\\.\\d{1,2})?$", ErrorMessage = "整数位须小于等于3位，小数位须小于等于2位")]
        public decimal? SalesRate { get; set; }
        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }
        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,3}(\\.\\d{1,2})?$", ErrorMessage = "整数位须小于等于3位，小数位须小于等于2位")]
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 500)]
        public string Remark { get; set; }

        /// <summary>
        /// 网购地址，格式：名称,Url；名称,Url;
        /// </summary>
        [StringLength(maximumLength: 500)]
        public string Url { get; set; }

    }
}
