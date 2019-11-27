///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PSM_PurchaseOrderDetail Db Model
    /// </summary>
    [SugarTable("T_PSM_PurchaseOrderDetail")]
    public class TPSMPurchaseOrderDetailDbModel
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
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [SugarColumn(ColumnName = "SupplierId")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice")]
        public decimal? UnitPrice { get; set; } = 0;
        
        /// <summary>
        /// 采购数量
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseNum")]
        public decimal PurchaseNum { get; set; }

        /// <summary>
        /// 采购金额
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseAmount")]
        public decimal? PurchaseAmount { get; set; } = 0;
        
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
        
    }
}
