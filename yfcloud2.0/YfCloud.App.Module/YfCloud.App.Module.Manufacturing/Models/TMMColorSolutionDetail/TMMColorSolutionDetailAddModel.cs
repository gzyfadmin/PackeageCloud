﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionDetailAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail
{
    /// <summary>
    /// T_MM_ColorSolutionDetail Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMColorSolutionDetailDbModel))]
    public class TMMColorSolutionDetailAddModel : LogModelBase
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
        public int ItemId { get; set; }
        /// <summary>
        /// 颜色ID
        /// </summary>
        [Required]
        public int ColorId { get; set; }
                
    }
}
