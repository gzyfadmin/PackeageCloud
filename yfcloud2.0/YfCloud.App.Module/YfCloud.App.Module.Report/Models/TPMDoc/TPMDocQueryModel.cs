///////////////////////////////////////////////////////////////////////////////////////
// File: TPMDoc.cs
// Author: www.cloudyf.com
// Create Time:2019/9/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TPMDoc
{
    /// <summary>
    /// TPMDoc Query Model
    /// </summary>
    public class TPMDocQueryModel
    {

        
        /// <summary>
        /// 文件名称
        /// </summary>
        public string DocName { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 文件地址
        /// </summary>
        public string DocPath { get; set; }
    }
}
