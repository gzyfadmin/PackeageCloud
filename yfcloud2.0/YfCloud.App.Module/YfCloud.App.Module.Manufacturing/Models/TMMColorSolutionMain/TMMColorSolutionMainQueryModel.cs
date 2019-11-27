///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain
{
    /// <summary>
    /// TMMColorSolutionMain Query Model
    /// </summary>
    public class TMMColorSolutionMainQueryModel
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
        /// 方案编号
        /// </summary>
        public string SolutionCode { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMColorSolutionDetailQueryModel> ChildList { get; set; }
    }
}
