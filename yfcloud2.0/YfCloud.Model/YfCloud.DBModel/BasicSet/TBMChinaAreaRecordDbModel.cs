///////////////////////////////////////////////////////////////////////////////////////
// File: TBMChinaAreaRecord.cs
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
    /// T_BM_ChinaAreaRecord Db Model
    /// </summary>
    [SugarTable("T_BM_ChinaAreaRecord")]
    public class TBMChinaAreaRecordDbModel
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Code")]
        public string Code { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Level")]
        public int Level { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ParentArea_id")]
        public int ParentAreaid { get; set; }
        
    }
}
