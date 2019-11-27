///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompanyAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TSMCompany
{
    /// <summary>
    /// T_SM_Company Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TSMCompanyDbModel))]
    public class TSMCompanyEditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int ID { get; set; }        
        
        /// <summary>
        /// 企业名称
        /// </summary>       
        [Required]
        [StringLength(maximumLength:50)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人名称
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 25)]
        public string LegalPerson { get; set; }


        /// <summary>
        /// 法人电话
        /// </summary>       
        [Required]
        [RegularExpression(@"(^(\d{3,4}-)?\d{7,8})$|[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11)]
        public string ContactNumber { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>       
        [StringLength(maximumLength:30)]
        public string ContactEmail { get; set; }        
        
        /// <summary>
        /// 企业详细信息
        /// </summary>       
        public int? CompanyInfoId { get; set; }        
        
        /// <summary>
        /// 企业状态（0无效，1有效，2过期)
        /// </summary>       
        public byte? Status { get; set; }        
    }
}
