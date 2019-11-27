///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionary.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.BasicSet.Models.TBMDictionary
{
    /// <summary>
    /// TBMDictionary Query Model
    /// </summary>
    public class TBMDictionaryQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 分类ID
        /// </summary>
        public int TypeId { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 字典编号
        /// </summary>
        public string DicCode { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DicValue { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreateId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        
        /// <summary>
        /// 更新人Id
        /// </summary>
        public int? UpdateId { get; set; }

        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdateName { get; set; }

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
