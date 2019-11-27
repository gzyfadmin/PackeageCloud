///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserAccountAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    /// <summary>
    /// T_SM_UserAccount Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMUserAccountDbModel))]
    public class TSMUserAccountAddModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int Id { get; set; }        
        
        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength:11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string TelAccount { get; set; }        
        
        /// <summary>
        /// 邮箱账号
        /// </summary>       
        [StringLength(maximumLength:30)]
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string EmailAccount { get; set; }        
        
        /// <summary>
        /// 账号密码
        /// </summary>       
        [Required]
        [StringLength(maximumLength:100)]
        public string Passwd { get; set; }        
        
        /// <summary>
        /// 账户姓名
        /// </summary>       
        [Required]
        [StringLength(maximumLength:20)]
        public string AccountName { get; set; }        
        
        /// <summary>
        /// 用户详细信息
        /// </summary>       
        public int? UserInfoId { get; set; }        
        
        /// <summary>
        /// 企业ID
        /// </summary>       
        public int? CompanyId { get; set; }        
        
        /// <summary>
        /// 账号状态（0无效，1有效，2过期)
        /// </summary>       
        public byte? Status { get; set; }        
        
        /// <summary>
        /// 有效期
        /// </summary>       
        public DateTime? ExpDate { get; set; }        
        
        /// <summary>
        /// 创建时间
        /// </summary>       
        public DateTime? CreateTime { get; set; }        
        
    }
}
