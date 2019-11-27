///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserInfo.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMUserInfo
{
    /// <summary>
    /// TSMUserInfo Query Model
    /// </summary>
    public class TSMUserInfoQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        
        /// <summary>
        /// 固定电话
        /// </summary>
        public string FixedNumber { get; set; }
        
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadPicPath { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        
    }
}
