﻿///////////////////////////////////////////////////////////////////////////////////////
// File: PTestCompanyAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Test.Models.PTestCompany
{
    /// <summary>
    /// P_Test_Company Add Model
    /// </summary>
    [UseAutoMapper(typeof(PTestCompanyDbModel))]
    public class PTestCompanyAddModel : LogModelBase
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
