///////////////////////////////////////////////////////////////////////////////////////
// File: TPMDictionaryInitInfoEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/23
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Models.TPMDictionaryInitInfo
{
    /// <summary>
    /// T_PM_DictionaryInitInfo Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TPMDictionaryInitInfoDbModel))]
    public class TPMDictionaryInitInfoEditModel : LogModelBase
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string TypeName { get; set; }
        
        /// <summary>
        /// 初始化值
        /// </summary>
        [Required]
        [StringLength(maximumLength:500)]
        public string TypeValues { get; set; }
        
    }
}
