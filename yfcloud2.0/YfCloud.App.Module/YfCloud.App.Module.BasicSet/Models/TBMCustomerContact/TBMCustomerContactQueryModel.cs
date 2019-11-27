///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerContact.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.BasicSet.Models.TBMCustomerContact
{
    /// <summary>
    /// TBMCustomerContact Query Model
    /// </summary>
    public class TBMCustomerContactQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 联系人优先级 1第一联系人、2重要联系人、3一般联系人
        /// </summary>
        public byte Priority { get; set; }
        
    }
}
