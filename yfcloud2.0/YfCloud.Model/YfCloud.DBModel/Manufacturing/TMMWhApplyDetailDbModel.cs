///////////////////////////////////////////////////////////////////////////////////////
// File: TMMWhApplyDetail.cs
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
    /// T_MM_WhApplyDetail Db Model
    /// </summary>
    [SugarTable("T_MM_WhApplyDetail")]
    public class TMMWhApplyDetailDbModel
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
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
        /// 生产单详细表ID
        /// </summary>
        [SugarColumn(ColumnName = "ProOrderDetailId")]
        public int ProOrderDetailId { get; set; }


        /// <summary>
        /// 申请数量
        /// </summary>
        [SugarColumn(ColumnName = "ApplyNum")]
        public decimal ApplyNum { get; set; }

        /// <summary>
        /// 可转单数量
        /// </summary>
        [SugarColumn(ColumnName = "TransNum")]
        public decimal TransNum { get; set; }

        /// <summary>
        /// 生产车间ID
        /// </summary>
        [SugarColumn(ColumnName = "WorkshopId")]
        public int? WorkshopId { get; set; }
    }
}
