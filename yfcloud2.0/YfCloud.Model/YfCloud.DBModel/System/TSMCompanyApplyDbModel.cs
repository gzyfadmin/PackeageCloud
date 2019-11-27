///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompanyApply.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_SM_CompanyApply Db Model
    /// </summary>
    [SugarTable("T_SM_CompanyApply")]
    public class TSMCompanyApplyDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int Id { get; set; }
        
        /// <summary>
        /// 申请账号ID
        /// </summary>
        [SugarColumn(ColumnName = "AccountId")]
        public int AccountId { get; set; }
        
        /// <summary>
        /// 申请公司ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        [SugarColumn(ColumnName = "ApplyTime")]
        public DateTime ApplyTime { get; set; }
        
        /// <summary>
        /// 申请状态（0未审核，1已审核，2已拒绝）
        /// </summary>
        [SugarColumn(ColumnName = "ApplyStatus")]
        public byte ApplyStatus { get; set; }
        
    }
}
