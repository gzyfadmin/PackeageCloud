﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TPMMenus.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TPMMenus
{
    /// <summary>
    /// TPMMenus Query Model
    /// </summary>
    public class TPMMenusQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 上级ID(-1则为顶级菜单)
        /// </summary>
        public int ParentID { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Seq { get; set; }
        
        /// <summary>
        /// 是否菜单
        /// </summary>
        public bool IsMenu { get; set; }
        
        /// <summary>
        /// 菜单别名
        /// </summary>
        public string MenuAnotherName { get; set; }
        
        /// <summary>
        /// 菜单状态
        /// </summary>
        public bool Status { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
        
        /// <summary>
        /// 菜单描述
        /// </summary>
        public string MenuDesc { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateId { get; set; }
        
    }
}
