///////////////////////////////////////////////////////////////////////////////////////
// File: TTenantPermissions.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_TenantPermissions Model
    /// </summary>
    [SugarTable("T_PM_TenantPermissions")]
    public class TTenantPermissionsModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 租户名称
        /// </summary>
        [SugarColumn(ColumnName = "TenantId")]
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TTenantsTenantName { get; set; }
        
        /// <summary>
        /// 菜单ID
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
        
        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TUsersUserName { get; set; }
    }
}
