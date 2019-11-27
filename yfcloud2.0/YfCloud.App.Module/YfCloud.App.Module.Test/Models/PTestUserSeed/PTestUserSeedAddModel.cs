///////////////////////////////////////////////////////////////////////////////////////
// File: PTestUserSeedAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/30
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Test.Models.PTestUserSeed
{
    /// <summary>
    /// P_Test_UserSeed Add Model
    /// </summary>
    [UseAutoMapper(typeof(PTestUserSeedDbModel))]
    public class PTestUserSeedAddModel : LogModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public new int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength: 50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength:11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [Required]
        public string MobileNO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength:50)]
        [Required]
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength:300)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 渠道号
        /// </summary>
        [StringLength(maximumLength: 10)]
        [Required]
        public string ChannelNo { get; set; }

    }
}
