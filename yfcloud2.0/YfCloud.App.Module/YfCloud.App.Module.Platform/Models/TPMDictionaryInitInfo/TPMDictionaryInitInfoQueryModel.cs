///////////////////////////////////////////////////////////////////////////////////////
// File: TPMDictionaryInitInfo.cs
// Author: www.cloudyf.com
// Create Time:2019/9/23
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Platform.Models.TPMDictionaryInitInfo
{
    /// <summary>
    /// TPMDictionaryInitInfo Query Model
    /// </summary>
    public class TPMDictionaryInitInfoQueryModel
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 初始化值
        /// </summary>
        public string TypeValues { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
        /// 修改人ID
        /// </summary>
        public int? UpdateId { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        
    }
}
