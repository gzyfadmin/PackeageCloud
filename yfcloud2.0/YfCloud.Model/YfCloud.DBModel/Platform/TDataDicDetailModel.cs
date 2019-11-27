///////////////////////////////////////////////////////////////////////////////////////
// File: TDataDicDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_DataDicDetail Model
    /// </summary>
    [SugarTable("T_PM_DataDicDetail")]
    public class TDataDicDetailModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// KeyId
        /// </summary>
        [SugarColumn(ColumnName = "KeyId")]
        [Required]
        public int KeyId { get; set; }
        
        /// <summary>
        /// 键名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TDataDicMainKey { get; set; }
        
        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(ColumnName = "Value")]
        [Required]
        [StringLength(maximumLength:20)]
        public string Value { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        [Required]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        [Required]
        public int CreateId { get; set; }   
    }
}
