///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRolePermissions.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMRolePermissions
{
    /// <summary>
    /// TSMRolePermissions Query Model
    /// </summary>
    public class TSMRolePermissionsQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }
        
        /// <summary>
        /// 按钮权限
        /// </summary>
        public string ButtonIds { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
    }
}
