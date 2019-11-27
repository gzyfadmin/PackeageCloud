///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryEditModel.cs
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

namespace YfCloud.App.Module.BasicSet.Models.TBMDictionary
{
    /// <summary>
    /// T_BM_Dictionary Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TBMDictionaryDbModel))]
    public class TBMDictionaryEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
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
        [StringLength(maximumLength:500)]
        public string Remark { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool? DeleteFlag { get; set; }
        
    }
}
