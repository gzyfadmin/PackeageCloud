///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierContactAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMSupplierContact
{
    /// <summary>
    /// T_BM_SupplierContact Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMSupplierContactDbModel))]
    public class TBMSupplierContactAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        [Required]
        public int SupplierId { get; set; }
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
        /// 联系人优先级
        /// </summary>
        [Required]
        public byte Priority { get; set; }
                
    }
}
