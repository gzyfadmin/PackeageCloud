///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompany.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMCompany
{
    /// <summary>
    /// TSMCompany Query Model
    /// </summary>
    public class TSMCompanyQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string ContactEmail { get; set; }
        
        /// <summary>
        /// 企业详细信息
        /// </summary>
        public int? CompanyInfoId { get; set; }
        
        /// <summary>
        /// 企业状态（0无效，1有效，2过期)
        /// </summary>
        public byte? Status { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 当前用户是否管理员
        /// </summary>
        public bool? IsAdmin { get; set; }
        
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public class TSMCompanyForCurentUserIDModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11)]

        public string Mobile { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string Email { get; set; }
    }

}
