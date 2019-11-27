///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPackageColorItem.cs
// Author: www.cloudyf.com
// Create Time:2019/11/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_PackageColorItem Db Model
    /// </summary>
    [SugarTable("T_MM_PackageColorItem")]
    public class TMMPackageColorItemDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        public int PackageId { get; set; }
        
        /// <summary>
        /// 配色项目
        /// </summary>
        [SugarColumn(ColumnName = "ItemName")]
        public string ItemName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; }
        
    }
}
