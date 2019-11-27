///////////////////////////////////////////////////////////////////////////////////////
// File: TButtons.cs
// Author: www.cloudyf.com
// Create Time:2019/6/18
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_Buttons Model
    /// </summary>
    [SugarTable("T_PM_Buttons")]
    public class TButtonsModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 按钮名称
        /// </summary>
        [SugarColumn(ColumnName = "ButtonCaption")]
        [Required]
        [StringLength(maximumLength:8)]
        public string ButtonCaption { get; set; }
        
        /// <summary>
        /// 按钮Key
        /// </summary>
        [SugarColumn(ColumnName = "ButtonKey")]
        [Required]
        [StringLength(maximumLength:12)]
        public string ButtonKey { get; set; }
        
        /// <summary>
        /// 是否默认权限
        /// </summary>
        [SugarColumn(ColumnName = "IsDefault")]
        [Required]
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// 按钮说明
        /// </summary>
        [SugarColumn(ColumnName = "ButtonDesc")]
        [StringLength(maximumLength:50)]
        public string ButtonDesc { get; set; }
        
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
        
        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TUsersUserName { get; set; }
    }
}
