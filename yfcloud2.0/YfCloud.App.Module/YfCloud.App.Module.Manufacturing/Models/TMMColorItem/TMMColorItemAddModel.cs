///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorItemAddModel.cs
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

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorItem
{
    /// <summary>
    /// T_MM_ColorItem Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMColorItemDbModel))]
    public class TMMColorItemAddModel : LogModelBase
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
        /// 配色项目ID
        /// </summary>
        [Required]
        public int ItemId { get; set; }
                
    }
}
