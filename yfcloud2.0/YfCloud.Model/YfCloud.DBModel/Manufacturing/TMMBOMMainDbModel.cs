///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMMain.cs
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
    /// T_MM_BOMMain Model
    /// </summary>
    [SugarTable("T_MM_BOMMain")]
    public class TMMBOMMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        [Required]
        public int PackageId { get; set; }
        
        /// <summary>
        /// 纸格编号
        /// </summary>
        [SugarColumn(ColumnName = "PagerCode")]
        [StringLength(maximumLength:30)]
        public string PagerCode { get; set; }
        
        /// <summary>
        /// 出格师傅ID
        /// </summary>
        [SugarColumn(ColumnName = "Maker")]
        public int? Maker { get; set; }
        
        /// <summary>
        /// 正面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "FrontImgPath")]
        [StringLength(maximumLength:200)]
        public string FrontImgPath { get; set; }
        
        /// <summary>
        /// 侧面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "SideImgPath")]
        [StringLength(maximumLength:200)]
        public string SideImgPath { get; set; }
        
        /// <summary>
        /// 背面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "BackImgPath")]
        [StringLength(maximumLength:200)]
        public string BackImgPath { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TMMBOMDetailDbModel> ChildList { get; set; }
    }
}
