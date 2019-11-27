///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompanyApply.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMCompanyApply
{
    /// <summary>
    /// TSMCompanyApply Query Model
    /// </summary>
    public class TSMCompanyApplyQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 申请账号ID
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// 申请公司ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        
        /// <summary>
        /// 申请状态（0未审核，1已审核，2已拒绝）
        /// </summary>
        public byte ApplyStatus { get; set; }

        /// <summary>
        /// 申请用户姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelAccount { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// 申请公司名称
        /// </summary>
        public string CompanyName { get; set; }

    }
}
