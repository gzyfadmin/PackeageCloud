///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderDetail.cs
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
    /// T_SSM_SalesOrderDetail Db Model
    /// </summary>
    [SugarTable("T_SSM_SalesOrderDetail")]
    public class TSSMSalesOrderDetailDbModel
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主单ID
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
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice")]
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 销售数量
        /// </summary>
        [SugarColumn(ColumnName = "SalesNum")]
        public decimal SalesNum { get; set; }
        
        /// <summary>
        /// 销售金额
        /// </summary>
        [SugarColumn(ColumnName = "SalesAmount")]
        public decimal? SalesAmount { get; set; }
        
        /// <summary>
        /// 交期
        /// </summary>
        [SugarColumn(ColumnName = "DeliveryPeriod")]
        public DateTime? DeliveryPeriod { get; set; }
        
        /// <summary>
        /// 可转单数量
        /// </summary>
        [SugarColumn(ColumnName = "TransferNum")]
        public decimal TransferNum { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 可转生产单数量
        /// </summary>
        [SugarColumn(ColumnName = "TransProdNum")]
        public decimal TransProdNum { get; set; }

    }
}
