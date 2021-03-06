﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew
{
    [UseAutoMapper(typeof(TMMBOMDetailDbModel))]
    public class TMMBOMDetailAddNewModel
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
        /// 配色项目ID
        /// </summary>
        [Required]
        public string ItemName { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MaterialName { get; set; }
        /// <summary>
        /// 部位名称ID
        /// </summary>
        [MaxLength(50)]
        public string PartName { get; set; }
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
        [StringLength(maximumLength: 100)]
        public string Formula { get; set; }
    }
}
