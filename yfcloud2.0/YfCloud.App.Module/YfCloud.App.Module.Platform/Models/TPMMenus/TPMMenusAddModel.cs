///////////////////////////////////////////////////////////////////////////////////////
// File: TPMMenusAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TPMMenus
{
    /// <summary>
    /// T_PM_Menus Add Model
    /// </summary>
    [UseAutoMapper(typeof(TPMMenusDbModel))]
    public class TPMMenusAddModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 上级ID(-1则为顶级菜单)
        /// </summary>
        [Required]
        public int ParentID { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:12)]
        public string MenuName { get; set; }
        
        /// <summary>
        /// 菜单路径
        /// </summary>
        [StringLength(maximumLength:100)]
        public string MenuPath { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Seq { get; set; }
        
        /// <summary>
        /// 是否菜单
        /// </summary>
        [Required]
        public bool IsMenu { get; set; }
        
        /// <summary>
        /// 菜单别名
        /// </summary>
        [StringLength(maximumLength:12)]
        public string MenuAnotherName { get; set; }
        
        /// <summary>
        /// 菜单状态
        /// </summary>
        [Required]
        public bool Status { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        [StringLength(maximumLength:50)]
        public string MenuIcon { get; set; }
        
        /// <summary>
        /// 菜单描述
        /// </summary>
        [StringLength(maximumLength:100)]
        public string MenuDesc { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [Required]
        public int CreateId { get; set; }
        
    }


    /// <summary>
    /// 删除总模型
    /// </summary>
    public class TPMenusDeleteAllModel
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 要删除的节点ID
        /// </summary>
        public int Id { get; set; }
    }
}
