///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierContact.cs
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
    /// T_BM_SupplierContact Db Model
    /// </summary>
    [SugarTable("T_BM_SupplierContact")]
    public class TBMSupplierContactDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 供应商ID
        /// </summary>
        [SugarColumn(ColumnName = "SupplierId")]
        public int SupplierId { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [SugarColumn(ColumnName = "ContactName")]
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        [SugarColumn(ColumnName = "ContactNumber")]
        public string ContactNumber { get; set; }
        
        /// <summary>
        /// 联系人优先级
        /// </summary>
        [SugarColumn(ColumnName = "Priority")]
        public byte Priority { get; set; }
        
    }
}
