///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPrimeCount.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_WM_PrimeCount Db Model
    /// </summary>
    [SugarTable("T_WM_PrimeCount")]
    public class TWMPrimeCountDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 期初库存
        /// </summary>
        [SugarColumn(ColumnName = "PrimeNum")]
        public decimal PrimeNum { get; set; }
        
        /// <summary>
        /// 仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int? WarehouseId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }


    }
}
