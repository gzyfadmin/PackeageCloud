///////////////////////////////////////////////////////////////////////////////////////
// File: TMMFormulaEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMFormula
{
    /// <summary>
    /// T_MM_Formula Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMFormulaDbModel))]
    public class TMMFormulaEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 公式名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string FormulaName { get; set; }
        
        /// <summary>
        /// 公式
        /// </summary>
        [Required]
        [StringLength(maximumLength:100)]
        public string Formula { get; set; }
        
        /// <summary>
        /// 前端显示公式
        /// </summary>
        [Required]
        [StringLength(maximumLength:100)]
        public string FrontFormula { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string Remark { get; set; }

    }
}
