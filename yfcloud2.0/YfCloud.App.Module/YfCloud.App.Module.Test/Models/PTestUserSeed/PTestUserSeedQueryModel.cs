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
    /// P_Test_UserSeed Query Model
    /// </summary>
    public class PTestUserSeedQueryModel
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
        public DateTime RegisterTime { get; set; }
                
    }
}
