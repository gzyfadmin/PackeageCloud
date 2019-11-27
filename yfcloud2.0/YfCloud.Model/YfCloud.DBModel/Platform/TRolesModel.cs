///////////////////////////////////////////////////////////////////////////////////////
// File: TRoles.cs
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
    /// T_Roles Model
    /// </summary>
    [SugarTable("T_PM_Roles")]
    public class TRolesModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(ColumnName = "RoleName")]
        [Required]
        [StringLength(maximumLength:20)]
        public string RoleName { get; set; }
        
        /// <summary>
        /// 角色状态 
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        [Required]
        public bool Status { get; set; }
        
        /// <summary>
        /// 角色说明
        /// </summary>
        [SugarColumn(ColumnName = "RoleDesc")]
        [StringLength(maximumLength:50)]
        public string RoleDesc { get; set; }
        
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
