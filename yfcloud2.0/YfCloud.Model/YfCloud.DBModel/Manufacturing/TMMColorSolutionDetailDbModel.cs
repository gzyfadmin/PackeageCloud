///////////////////////////////////////////////////////////////////////////////////////
// File: TMMColorSolutionDetail.cs
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
    /// T_MM_ColorSolutionDetail Db Model
    /// </summary>
    [SugarTable("T_MM_ColorSolutionDetail")]
    public class TMMColorSolutionDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 配色项目ID
        /// </summary>
        [SugarColumn(ColumnName = "ItemId")]
        public int ItemId { get; set; }
        
        /// <summary>
        /// 颜色ID
        /// </summary>
        [SugarColumn(ColumnName = "ColorId")]
        public int ColorId { get; set; }
        
    }
}
