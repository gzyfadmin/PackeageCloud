///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionWhDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/16
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_ProductionWhDetail Db Model
    /// </summary>
    [SugarTable("T_WM_ProductionWhDetail")]
    public class TWMProductionWhDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 主单ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }

        /// <summary>
        /// 收货仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }

        /// <summary>
        /// 生产入库单详细表ID
        /// </summary>
        [SugarColumn(ColumnName = "ProOrderDetailId")]
        public int ProOrderDetailId { get; set; }


        /// <summary>
        /// 实收数量
        /// </summary>
        [SugarColumn(ColumnName = "ActualNum")]
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 实发数量(生产单位)
        /// </summary>
        [SugarColumn(ColumnName = "ProActualNum")]
        public decimal ProActualNum { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 生产车间ID
        /// </summary>
        [SugarColumn(ColumnName = "WorkshopId")]
        public int WorkshopId { get; set; }

        /// <summary>
        /// 生产场地ID
        /// </summary>
        [SugarColumn(ColumnName = "SiteId")]
        public int SiteId { get; set; }
    }
}
