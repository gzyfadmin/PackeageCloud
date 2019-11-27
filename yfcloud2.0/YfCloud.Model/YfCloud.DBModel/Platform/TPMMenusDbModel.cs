///////////////////////////////////////////////////////////////////////////////////////
// File: TPMMenus.cs
// Author: www.cloudyf.com
// Create Time:2019/7/15
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PM_Menus Db Model
    /// </summary>
    [SugarTable("T_PM_Menus")]
    public class TPMMenusDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int Id { get; set; }
        
        /// <summary>
        /// 上级ID(-1则为顶级菜单)
        /// </summary>
        [SugarColumn(ColumnName = "ParentID")]
        public int ParentID { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(ColumnName = "MenuName")]
        public string MenuName { get; set; }
        
        /// <summary>
        /// 菜单路径
        /// </summary>
        [SugarColumn(ColumnName = "MenuPath")]
        public string MenuPath { get; set; }

        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        public string LogicPath { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnName = "Seq")]
        public int Seq { get; set; }
        
        /// <summary>
        /// 是否菜单
        /// </summary>
        [SugarColumn(ColumnName = "IsMenu")]
        public bool IsMenu { get; set; }
        
        /// <summary>
        /// 菜单别名
        /// </summary>
        [SugarColumn(ColumnName = "MenuAnotherName")]
        public string MenuAnotherName { get; set; }
        
        /// <summary>
        /// 菜单状态
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public bool Status { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        [SugarColumn(ColumnName = "MenuIcon")]
        public string MenuIcon { get; set; }
        
        /// <summary>
        /// 菜单描述
        /// </summary>
        [SugarColumn(ColumnName = "MenuDesc")]
        public string MenuDesc { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int CreateId { get; set; }


        
    }
}
