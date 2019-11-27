///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_ColorSolutionMain Model
    /// </summary>
    [SugarTable("T_MM_ColorSolutionMain")]
    public class TMMColorSolutionMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        [Required]
        public int PackageId { get; set; }

        /// <summary>
        /// 方案编号
        /// </summary>
        [SugarColumn(ColumnName = "SolutionCode")]
        [Required]
        public string SolutionCode { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [SugarColumn(ColumnName = "ImagePath")]
        [StringLength(maximumLength: 200)]
        public string ImagePath { get; set; }


        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMColorSolutionDetailDbModel> ChildList { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; } = false;

        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }
        /// <summary>
        /// 颜色ID
        /// </summary>
        [SugarColumn(ColumnName = "ColorId")]
        public int ColorId { get; set; }
    }
}
