///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryType.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.BasicSet.Models.TBMDictionaryType
{
    /// <summary>
    /// TBMDictionaryType Query Model
    /// </summary>
    public class TBMDictionaryTypeQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool? DeleteFlag { get; set; }
        
    }
}
