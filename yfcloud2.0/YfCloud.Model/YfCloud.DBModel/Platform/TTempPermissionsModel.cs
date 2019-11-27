///////////////////////////////////////////////////////////////////////////////////////
// File: TTempPermissions.cs
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
    /// T_TempPermissions Model
    /// </summary>
    [SugarTable("T_PM_TempPermissions")]
    public class TTempPermissionsModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" ,IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [SugarColumn(ColumnName = "TemplateId")]
        [Required]
        public int TemplateId { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TTemplatesTemplateName { get; set; }
        
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(ColumnName = "MenuId")]
        [Required]
        public int MenuId { get; set; }

        /// <summary>
        /// 菜单父ID
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int MenuParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TMenusMenuName { get; set; }
        
        /// <summary>
        /// 按钮权限
        /// </summary>
        [SugarColumn(ColumnName = "ButtonIds")]
        [Required]
        [StringLength(maximumLength:2000)]
        public string ButtonIds { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int CreateId { get; set; } 
        
    }
}
