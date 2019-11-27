///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderBOMEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/10
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM
{
    /// <summary>
    /// T_MM_ProductionOrderBOM Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMProductionOrderBOMDbModel))]
    public class TMMProductionOrderBOMEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 生产单ID
        /// </summary>
        [Required]
        public int ProOrderId { get; set; }
        
        /// <summary>
        /// 配色项目ID
        /// </summary>
        [Required]
        public int ItemId { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [Required]
        public int PackageId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 部位ID
        /// </summary>
        [Required]
        public int PartId { get; set; }
        
        /// <summary>
        /// 长度值
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal LengthValue { get; set; }
        
        /// <summary>
        /// 宽度值
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal WidthValue { get; set; }
        
        /// <summary>
        /// 件数
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal NumValue { get; set; }
        
        /// <summary>
        /// 封度（宽幅）
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? WideValue { get; set; }
        
        /// <summary>
        /// 损耗
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal LossValue { get; set; }
        
        /// <summary>
        /// 单用量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal SingleValue { get; set; }
        
        /// <summary>
        /// 单用量计算公式
        /// </summary>
        [Required]
        [StringLength(maximumLength:100)]
        public string Formula { get; set; }
        
        /// <summary>
        /// 总用量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal TotalValue { get; set; }
        
        /// <summary>
        /// 生产数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal ProductionNum { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PurchaseNum { get; set; }
        
        /// <summary>
        /// 已转采购申请单数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PurchaseTransNum { get; set; }
        
        /// <summary>
        /// 领料数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PickNum { get; set; }
        
        /// <summary>
        /// 已转领料申请单数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PickTransNum { get; set; }
        
        /// <summary>
        /// 总领料数
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PickTotalNum { get; set; }
        
    }
}
