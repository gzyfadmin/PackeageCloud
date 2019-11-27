///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerFileAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerContact;

namespace YfCloud.App.Module.BasicSet.Models.TBMCustomerFile
{
    /// <summary>
    /// T_BM_CustomerFile Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMCustomerFileDbModel))]
    public class TBMCustomerFileAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 客户编号
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string CustomerCode { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string CustomerName { get; set; }
        
        /// <summary>
        /// 客户简称
        /// </summary>
        [StringLength(maximumLength:10)]
        public string ShortName { get; set; }
        
        /// <summary>
        /// 客户类型ID
        /// </summary>
        [Required]
        public int CustomerTypeId { get; set; }
        
        /// <summary>
        /// 法人
        /// </summary>
        [StringLength(maximumLength:10)]
        public string LegalPerson { get; set; }
        
        /// <summary>
        /// 行业ID
        /// </summary>
        public int? IndustryId { get; set; }
        
        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(maximumLength:20)]
        public string City { get; set; }
        
        /// <summary>
        /// 客户地址
        /// </summary>
        [StringLength(maximumLength:100)]
        public string Address { get; set; }
        
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TBMCustomerContactAddModel> ChildList { get; set; }
    }
}
