///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail
{
    /// <summary>
    /// TMMColorSolutionDetail Query Model
    /// </summary>
    public class TMMColorSolutionDetailQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        public int MainId { get; set; }
        
        /// <summary>
        /// 配色项目ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 配色项目名称
        /// </summary>
        public string ItemName { get; set; }
        
        /// <summary>
        /// 颜色ID
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ColorName { get; set; }
        
    }
}
