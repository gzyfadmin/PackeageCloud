///////////////////////////////////////////////////////////////////////////////////////
// File: TWMSalesDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_SalesDetail Db Model
    /// </summary>
    [SugarTable("T_WM_SalesDetail")]
    public class TWMSalesDetailDbModel
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
        /// 发货仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }

        /// <summary>
        /// 实发数量
        /// </summary>
        [SugarColumn(ColumnName = "ActualNum")]
        public decimal ActualNum { get; set; }

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
        /// 来源销售明细（T_SSM_SalesOrderDetail>ID）
        /// </summary>
        [SugarColumn(ColumnName = "SalesOrderDetailId")]
        public int SalesOrderDetailId { get; set; }

        /// <summary>
        /// 销售单位数量
        /// </summary>
        [SugarColumn(ColumnName = "SalesOrderActualNum")]
        public decimal SalesOrderActualNum { get; set; }
    }
}
