///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDomain.cs
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
    /// T_BM_Domain Db Model
    /// </summary>
    [SugarTable("T_BM_Domain")]
    public class TBMDomainDbModel
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
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "NameSpace")]
        public string NameSpace { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Url")]
        public string Url { get; set; }
        
    }
}
