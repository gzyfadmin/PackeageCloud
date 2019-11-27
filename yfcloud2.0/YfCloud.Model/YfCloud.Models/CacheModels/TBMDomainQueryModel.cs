///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDomain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.Models
{
    /// <summary>
    /// TBMDomain Query Model
    /// </summary>
    public class TBMDomainQueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string NameSpace { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        
    }

    /// <summary>
    /// 站点枚举
    /// </summary>
    public enum DomainEum
    {
        /// <summary>
        /// 平台
        /// </summary>
        [Description("YfCloud.App.Module.Platform")]
        Platform,

        /// <summary>
        /// 基础设置
        /// </summary>
        [Description("YfCloud.App.Module.BasicSet")]
        BasicSet,

        /// <summary>
        /// 系统管理
        /// </summary>
        [Description("YfCloud.App.Module.System")]
        System

    }
}
