///////////////////////////////////////////////////////////////////////////////////////
// File: TPMSeedUsers.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PM_SeedUsers Db Model
    /// </summary>
    [SugarTable("T_PM_SeedUsers")]
    public class TPMSeedUsersDbModel
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
        [SugarColumn(ColumnName = "MobileNO")]
        public string MobileNO { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "EnterpriseName")]
        public string EnterpriseName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Address")]
        public string Address { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ChannelNo")]
        public string ChannelNo { get; set; }
        
    }
}
