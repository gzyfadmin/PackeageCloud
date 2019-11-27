///////////////////////////////////////////////////////////////////////////////////////
// File: TPMPolicyCompany.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.PackageSalesTool.Models.TPMPolicyCompany
{
    /// <summary>
    /// TPMPolicyCompany Query Model
    /// </summary>
    public class TPMPolicyCompanyQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelNumber { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        
        /// <summary>
        /// 所属行业
        /// </summary>
        public string Industry { get; set; }
        
        /// <summary>
        /// 企业规模
        /// </summary>
        public string CompanyScale { get; set; }
        
        /// <summary>
        /// 企业年产值
        /// </summary>
        public string CompanyOV { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        
    }
}
