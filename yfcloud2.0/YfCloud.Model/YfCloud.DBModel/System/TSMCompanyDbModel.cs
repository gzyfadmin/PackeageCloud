///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompany.cs
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
    /// T_SM_Company Db Model
    /// </summary>
    [SugarTable("T_SM_Company")]
    public class TSMCompanyDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        [SugarColumn(ColumnName = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人姓名
        /// </summary>
        [SugarColumn(ColumnName = "LegalPerson")]
        public string LegalPerson { get; set; }

        /// <summary>
        /// 法人电话
        /// </summary>
        [SugarColumn(ColumnName = "ContactNumber")]
        public string ContactNumber { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnName = "ContactEmail")]
        public string ContactEmail { get; set; }

        /// <summary>
        /// 公司类型
        /// </summary>
        [SugarColumn(ColumnName = "EnterpriseType")]
        [StringLength(maximumLength: 50)]
        [Required]
        public string EnterpriseType { get; set; }

        /// <summary>
        /// 企业详细信息
        /// </summary>
        [SugarColumn(ColumnName = "CompanyInfoId")]
        public int? CompanyInfoId { get; set; }
        
        /// <summary>
        /// 企业状态（0无效，1有效，2过期)
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public byte? Status { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 管理员ID，对应用户表ID
        /// </summary>
        [SugarColumn(ColumnName = "AdminId")]
        public int AdminId { get; set; }


    }
}
