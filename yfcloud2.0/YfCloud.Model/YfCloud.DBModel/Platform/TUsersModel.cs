///////////////////////////////////////////////////////////////////////////////////////
// File: TUsers.cs
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
    /// T_Users Model
    /// </summary>
    [SugarTable("T_PM_Users")]
    public class TUsersModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(ColumnName = "UserName")]
        [Required]
        [StringLength(maximumLength:12)]
        public string UserName { get; set; }
        
        /// <summary>
        /// 登录账号 
        /// </summary>
        [SugarColumn(ColumnName = "LoginName")]
        [Required]
        [StringLength(maximumLength:12)]
        public string LoginName { get; set; }
        
        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(ColumnName = "LoginPwd")]
        [Required]
        [StringLength(maximumLength:16)]
        public string LoginPwd { get; set; }
       
        /// <summary>
        /// 加密的盐
        /// </summary>
        public string Salt { get; set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(ColumnName = "RoleId")]
        [Required]
        public int RoleId { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TRolesRoleName { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        [Required]
        public bool Status { get; set; }
        
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
