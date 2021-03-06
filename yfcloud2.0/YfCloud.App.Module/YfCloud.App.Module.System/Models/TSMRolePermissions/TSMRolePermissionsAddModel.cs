﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRolePermissionsAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel.System;

namespace YfCloud.App.Module.System.Models.TSMRolePermissions
{
    /// <summary>
    /// T_SM_RolePermissions Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMRolePermissionsDbModel))]
    public class TSMRolePermissionsAddModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int Id { get; set; }
                
        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        public int RoleId { get; set; }
        
        /// <summary>
        /// 菜单ID
        /// </summary>
        [Required]
        public int MenuId { get; set; }
        
        /// <summary>
        /// 按钮权限
        /// </summary>
        [Required]
        [StringLength(maximumLength:2000)]
        public string ButtonIds { get; set; }
        
    }
}
