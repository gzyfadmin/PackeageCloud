///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMDictionary
{
    /// <summary>
    /// T_BM_Dictionary Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMDictionaryDbModel))]
    public class TBMDictionaryAddModel : LogModelBase
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [Required]
        public int TypeId { get; set; }
        /// <summary>
        /// 字典编号
        /// </summary>
        [StringLength(maximumLength:20)]
        public string DicCode { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string DicValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }           
    }
}
