///////////////////////////////////////////////////////////////////////////////////////
// File: TDataDicMain.cs
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
    /// T_DataDicMain Model
    /// </summary>
    [SugarTable("T_PM_DataDicMain")]
    public class TDataDicMainModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(ColumnName = "MenuId")]
        [Required]
        public int MenuId { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TMenusMenuName { get; set; }
        
        /// <summary>
        /// 键名称
        /// </summary>
        [SugarColumn(ColumnName = "Key")]
        [Required]
        [StringLength(maximumLength:20)]
        public string Key { get; set; }
        
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
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TDataDicDetailModel> ChildList { get; set; }
    }
}
