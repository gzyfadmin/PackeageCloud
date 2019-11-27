///////////////////////////////////////////////////////////////////////////////////////
// File: TPMSeedUsers.cs
// Author: www.cloudyf.com
// Create Time:2019/10/14
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.PackageSalesTool.Models.TPMSeedUsers
{
    /// <summary>
    /// TPMSeedUsers Query Model
    /// </summary>
    public class TPMSeedUsersQueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string MobileNO { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string EnterpriseName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ChannelNo { get; set; }
        
    }
}
