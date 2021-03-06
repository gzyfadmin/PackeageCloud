﻿///////////////////////////////////////////////////////////////////////////////////////
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
    /// T_SM_UserAccount Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TSMUserAccountDbModel))]
    public class TSMUserAccountEditModel
    {   
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
        
    }


    /// <summary>
    /// 修改密码的
    /// </summary>
    public class TSMUserAccountPassWordModel
    {
        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength: 11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string TelAccount { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>       
        [StringLength(maximumLength: 30)]
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string EmailAccount { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 100)]
        public string Passwd { get; set; }

        /// <summary>
        /// 原来的密码
        /// </summary>    
        [Required]
        [StringLength(maximumLength: 100)]
        public string OldPasswd { get; set; }

    }

    /// <summary>
    /// 修改密码的
    /// </summary>
    public class TSMUserResetPassWordModel
    {
        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength: 11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string TelAccount { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>       
        [StringLength(maximumLength: 30)]
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string EmailAccount { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 100)]
        public string Passwd { get; set; }

    }
}
