///////////////////////////////////////////////////////////////////////////////////////
// File: TSMLoginLog.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMLoginLog
{
    /// <summary>
    /// TSMLoginLog Query Model
    /// </summary>
    public class TSMLoginLogQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 账号ID
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// 登录登出时间
        /// </summary>
        public DateTime? LogTime { get; set; }
        
    }
}
