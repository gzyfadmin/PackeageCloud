///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserInfoAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TSMUserInfo
{
    /// <summary>
    /// T_SM_UserInfo Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMUserInfoDbModel))]
    public class TSMUserInfoAddModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(maximumLength:20)]
        public string RealName { get; set; }
        
        /// <summary>
        /// 固定电话
        /// </summary>
        [StringLength(maximumLength:20)]
        public string FixedNumber { get; set; }
        
        /// <summary>
        /// 联系地址
        /// </summary>
        [StringLength(maximumLength:50)]
        public string Address { get; set; }
        
        /// <summary>
        /// 头像路径
        /// </summary>
        [StringLength(maximumLength:100)]
        public string HeadPicPath { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength:100)]
        public string Remarks { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        
    }
}
