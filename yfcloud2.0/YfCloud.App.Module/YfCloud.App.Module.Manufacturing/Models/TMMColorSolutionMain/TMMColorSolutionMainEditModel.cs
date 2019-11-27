///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionMainEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain
{
    /// <summary>
    /// T_MM_ColorSolutionMain Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMColorSolutionMainDbModel))]
    public class TMMColorSolutionMainEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        [Required]
        public int PackageId { get; set; }

        /// <summary>
        /// 方案编号
        /// </summary>
        [Required]
        public string SolutionCode { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string ImagePath { get; set; }
 
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMColorSolutionDetailEditModel> ChildList { get; set; }
    }
}
