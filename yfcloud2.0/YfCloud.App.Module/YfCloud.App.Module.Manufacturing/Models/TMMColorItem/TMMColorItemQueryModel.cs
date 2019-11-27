///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorItem.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorItem
{
    /// <summary>
    /// TMMColorItem Query Model
    /// </summary>
    public class TMMColorItemQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 包型代码
        /// </summary>
        public string PackageCode { get; set; }
        
        /// <summary>
        /// 配色项目ID
        /// </summary>
        public int ItemId { get; set; }
        
        /// <summary>
        /// 配色项目名称
        /// </summary>
        public string ItemName { get; set; }
    }
}
