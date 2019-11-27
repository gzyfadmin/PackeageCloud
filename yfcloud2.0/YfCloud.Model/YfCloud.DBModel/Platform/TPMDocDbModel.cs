///////////////////////////////////////////////////////////////////////////////////////
// File: TPMDoc.cs
// Author: www.cloudyf.com
// Create Time:2019/9/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PM_Doc Db Model
    /// </summary>
    [SugarTable("T_PM_Doc")]
    public class TPMDocDbModel
    {
        /// <summary>
        /// 主键值增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 文件名称
        /// </summary>
        [SugarColumn(ColumnName = "DocName")]
        public string DocName { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnName = "Version")]
        public string Version { get; set; }
        
        /// <summary>
        /// 文件地址
        /// </summary>
        [SugarColumn(ColumnName = "DocPath")]
        public string DocPath { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
        /// 修改人ID
        /// </summary>
        [SugarColumn(ColumnName = "UpdateId")]
        public int? UpdateId { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        
    }
}
