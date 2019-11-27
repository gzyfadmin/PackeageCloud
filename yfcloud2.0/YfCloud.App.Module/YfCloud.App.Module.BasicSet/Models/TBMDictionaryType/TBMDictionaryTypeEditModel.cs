///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryTypeEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMDictionaryType
{
    /// <summary>
    /// T_BM_DictionaryType Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TBMDictionaryTypeDbModel))]
    public class TBMDictionaryTypeEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:6)]
        public string TypeName { get; set; }
    }
}
