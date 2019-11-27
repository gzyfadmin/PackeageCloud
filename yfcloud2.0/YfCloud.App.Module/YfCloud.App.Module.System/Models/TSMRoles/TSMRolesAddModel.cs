///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRolesAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel.System;

namespace YfCloud.App.Module.System.Models.TSMRoles
{
    /// <summary>
    /// T_SM_Roles Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMRolesDbModel))]
    public class TSMRolesAddModel
    {
        
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string RoleName { get; set; }
        
        /// <summary>
        /// 父ID 顶级为-1
        /// </summary>
        [Required]
        public int ParentId { get; set; }
        
        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        [StringLength(maximumLength:150)]
        public string PathLogic { get; set; }
        
        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        [Range(0,99999)]
        public int SeqNumber { get; set; }
        
        
        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(maximumLength:200)]
        public string RoleDesc { get; set; }
    }
}
