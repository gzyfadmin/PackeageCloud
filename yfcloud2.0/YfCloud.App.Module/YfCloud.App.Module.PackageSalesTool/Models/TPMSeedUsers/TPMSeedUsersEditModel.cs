///////////////////////////////////////////////////////////////////////////////////////
// File: TPMSeedUsersEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.PackageSalesTool.Models.TPMSeedUsers
{
    /// <summary>
    /// T_PM_SeedUsers Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TPMSeedUsersDbModel))]
    public class TPMSeedUsersEditModel : LogModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(maximumLength:11)]
        public string MobileNO { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string EnterpriseName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength:300)]
        public string Address { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [StringLength(maximumLength:10)]
        public string ChannelNo { get; set; }
        
    }
}
