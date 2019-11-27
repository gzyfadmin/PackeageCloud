///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPickApplyDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/6
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// 生产领料申请单明细表 Db Model
    /// </summary>
    [SugarTable("T_MM_PickApplyDetail")]
    public class TMMPickApplyDetailDbModel
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 申请数量
        /// </summary>
        [SugarColumn(ColumnName = "ApplyNum")]
        public decimal ApplyNum { get; set; }
        
        /// <summary>
        /// 已转单数量
        /// </summary>
        [SugarColumn(ColumnName = "TransNum")]
        public decimal TransNum { get; set; }
        
    }
}
