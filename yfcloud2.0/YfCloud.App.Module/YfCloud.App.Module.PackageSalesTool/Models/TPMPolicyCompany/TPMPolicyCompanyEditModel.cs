///////////////////////////////////////////////////////////////////////////////////////
// File: TPMPolicyCompanyEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.PackageSalesTool.Models.TPMPolicyCompany
{
    /// <summary>
    /// T_PM_PolicyCompany Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TPMPolicyCompanyDbModel))]
    public class TPMPolicyCompanyEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string UserName { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string TelNumber { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string CompanyName { get; set; }
        
        /// <summary>
        /// 所属行业
        /// </summary>
        [StringLength(maximumLength:50)]
        public string Industry { get; set; }
        
        /// <summary>
        /// 企业规模
        /// </summary>
        [StringLength(maximumLength:50)]
        public string CompanyScale { get; set; }
        
        /// <summary>
        /// 企业年产值
        /// </summary>
        [StringLength(maximumLength:50)]
        public string CompanyOV { get; set; }
        
    }
}
