///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// 生产合计
    /// </summary>
    [SugarTable("T_MM_ProductionOrderDetailSum")]
    public class TMMProductionOrderDetailDbSumModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        public int PackageId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }

        /// <summary>
        /// 配色方案ID
        /// </summary>
        [SugarColumn(ColumnName = "ColorSolutionId")]
        public int? ColorSolutionId { get; set; }

        /// <summary>
        /// 客户商品代码
        /// </summary>
        [SugarColumn(ColumnName = "GoodsCode")]
        public string GoodsCode { get; set; }

        /// <summary>
        /// 客户商品名称
        /// </summary>
        [SugarColumn(ColumnName = "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        [SugarColumn(ColumnName = "ProductionNum")]
        public decimal ProductionNum { get; set; }
    }
}
