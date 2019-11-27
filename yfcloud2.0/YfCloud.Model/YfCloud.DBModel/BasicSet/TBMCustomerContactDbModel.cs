///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerContact.cs
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
    /// T_BM_CustomerContact Db Model
    /// </summary>
    [SugarTable("T_BM_CustomerContact")]
    public class TBMCustomerContactDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        [SugarColumn(ColumnName = "CustomerId")]
        public int CustomerId { get; set; }
        
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
