///////////////////////////////////////////////////////////////////////////////////////
// File: TBMPackageEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMPackage
{
    /// <summary>
    /// T_BM_Package Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TBMPackageDbModel))]
    public class TBMPackageEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }

        /// <summary>
        /// 包型编码
        /// </summary>
        [StringLength(maximumLength: 20)]
        [Required]
        public string DicCode { get; set; }
        /// <summary>
        /// 包型名称
        /// </summary>
        [StringLength(maximumLength: 20)]
        [Required]
        public string DicValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 500)]
        public string Remark { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(maximumLength:100)]
        public string ImgPath { get; set; }
        
    }
}
