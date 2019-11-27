///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerContactAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMCustomerContact
{
    /// <summary>
    /// T_BM_CustomerContact Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMCustomerContactDbModel))]
    public class TBMCustomerContactAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        [Required]
        public int CustomerId { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Required]
        [StringLength(maximumLength:10)]
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string ContactNumber { get; set; }


        /// <summary>
        /// 联系人优先级 1第一联系人、2重要联系人、3一般联系人
        /// </summary>
        [Required]
        public byte Priority { get; set; }
                
    }
}
