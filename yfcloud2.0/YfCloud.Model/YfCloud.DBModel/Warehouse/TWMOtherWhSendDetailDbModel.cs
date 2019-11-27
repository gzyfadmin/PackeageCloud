///////////////////////////////////////////////////////////////////////////////////////
// File: TWMOtherWhSendDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/12
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_OtherWhSendDetail Db Model
    /// </summary>
    [SugarTable("T_WM_OtherWhSendDetail")]
    public class TWMOtherWhSendDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 入库主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 批号
        /// </summary>
        [SugarColumn(ColumnName = "BatchNumber")]
        public string BatchNumber { get; set; }
        
        /// <summary>
        /// 实出数量
        /// </summary>
        [SugarColumn(ColumnName = "ActualNumber")]
        public decimal ActualNumber { get; set; }
        
        /// <summary>
        /// 价格
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice")]
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 金额
        /// </summary>
        [SugarColumn(ColumnName = "Amount")]
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// 有效期
        /// </summary>
        [SugarColumn(ColumnName = "ValidityPeriod")]
        public DateTime? ValidityPeriod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

    }
}
