///////////////////////////////////////////////////////////////////////////////////////
// File: TSMRoles.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMRoles
{
    /// <summary>
    /// TSMRoles Query Model
    /// </summary>
    public class TSMRolesQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        
        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        public string PathLogic { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int? SeqNumber { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDesc { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class TSMRolesQueryTreeModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        public string PathLogic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SeqNumber { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDesc { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<TSMRolesQueryTreeModel> Children { get; set; }

    }
}
