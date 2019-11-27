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
    /// T_SM_Company Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMCompanyDbModel))]
    public class TSMCompanyAddModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 50)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人名称
        /// </summary>        
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
        [StringLength(maximumLength: 30)]
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
        /// 版本类型 0专业版 1企业版 2旗舰版
        /// </summary>
        [Required]
        public byte VersionType { get; set; }


        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength: 11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string TelAccount { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>       
        [StringLength(maximumLength: 6)]
        [RegularExpression(@"^[0-9]*$")]
        public string VerificationCode { get; set; }
    }
}
