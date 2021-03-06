﻿///////////////////////////////////////////////////////////////////////////////////////
// File: TSMLoginLogAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TSMLoginLog
{
    /// <summary>
    /// T_SM_LoginLog Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TSMLoginLogDbModel))]
    public class TSMLoginLogEditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int Id { get; set; }        
        
        /// <summary>
        /// 账号ID
        /// </summary>       
        [Required]
        public int AccountId { get; set; }        
        
        /// <summary>
        /// 登录登出时间
        /// </summary>       
        public DateTime? LogTime { get; set; }        
        
    }
}
