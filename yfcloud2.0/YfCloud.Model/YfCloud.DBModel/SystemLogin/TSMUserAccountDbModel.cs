///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserAccount.cs
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
    /// T_SM_UserAccount Db Model
    /// </summary>
    [SugarTable("T_SM_UserAccount")]
    public class TSMUserAccountDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 手机账号
        /// </summary>
        [SugarColumn(ColumnName = "TelAccount")]
        public string TelAccount { get; set; }
        
        /// <summary>
        /// 邮箱账号
        /// </summary>
        [SugarColumn(ColumnName = "EmailAccount")]
        public string EmailAccount { get; set; }
        
        /// <summary>
        /// 账号密码
        /// </summary>
        [SugarColumn(ColumnName = "Passwd")]
        public string Passwd { get; set; }
        
        /// <summary>
        /// 账户姓名
        /// </summary>
        [SugarColumn(ColumnName = "AccountName")]
        public string AccountName { get; set; }
        
        /// <summary>
        /// 用户详细信息
        /// </summary>
        [SugarColumn(ColumnName = "UserInfoId")]
        public int? UserInfoId { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int? CompanyId { get; set; }
        
        /// <summary>
        /// 账号状态（0无效，1有效，2过期)
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public byte? Status { get; set; }
        
        /// <summary>
        /// 有效期
        /// </summary>
        [SugarColumn(ColumnName = "ExpDate")]
        public DateTime? ExpDate { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 密码加密秘钥
        /// </summary>
        [SugarColumn(ColumnName = "Salt")]
        public string Salt { get; set; }
        
    }
}
