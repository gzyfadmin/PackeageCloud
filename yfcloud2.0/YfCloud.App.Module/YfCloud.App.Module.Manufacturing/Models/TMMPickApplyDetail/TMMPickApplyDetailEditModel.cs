﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPickApplyDetailEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMPickApplyDetail
{
    /// <summary>
    /// T_MM_PickApplyDetail Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMPickApplyDetailDbModel))]
    public class TMMPickApplyDetailEditModel : LogModelBase
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [Required]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 申请数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal ApplyNum { get; set; }
        
        /// <summary>
        /// 可转单数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal TransNum { get; set; }
        
    }
}
