///////////////////////////////////////////////////////////////////////////////////////
// File: TPMPolicyCompany.cs
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
    /// T_PM_PolicyCompany Db Model
    /// </summary>
    [SugarTable("T_PM_PolicyCompany")]
    public class TPMPolicyCompanyDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(ColumnName = "UserName")]
        public string UserName { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(ColumnName = "TelNumber")]
        public string TelNumber { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        [SugarColumn(ColumnName = "CompanyName")]
        public string CompanyName { get; set; }
        
        /// <summary>
        /// 所属行业
        /// </summary>
        [SugarColumn(ColumnName = "Industry")]
        public string Industry { get; set; }
        
        /// <summary>
        /// 企业规模
        /// </summary>
        [SugarColumn(ColumnName = "CompanyScale")]
        public string CompanyScale { get; set; }
        
        /// <summary>
        /// 企业年产值
        /// </summary>
        [SugarColumn(ColumnName = "CompanyOV")]
        public string CompanyOV { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        [SugarColumn(ColumnName = "RegisterTime")]
        public DateTime RegisterTime { get; set; }
        
    }
}
