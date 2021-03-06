﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMNTempDetailAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempDetail
{
    /// <summary>
    /// T_MM_BOMNTempDetail Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMBOMNTempDetailDbModel))]
    public class TMMBOMNTempDetailAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        [Required]
        public int MainId { get; set; }
        /// <summary>
        /// 物料Id
        /// </summary>
        [Required]
        public int MaterialId { get; set; }
        /// <summary>
        /// 部位名称ID
        /// </summary>
        public int? PartId { get; set; }
        /// <summary>
        /// 长度值
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? LengthValue { get; set; }
        /// <summary>
        /// 宽度值
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? WidthValue { get; set; }
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
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? LossValue { get; set; }
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
                
    }
}
