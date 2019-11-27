///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierFileEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierContact;

namespace YfCloud.App.Module.BasicSet.Models.TBMSupplierFile
{
    /// <summary>
    /// T_BM_SupplierFile Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TBMSupplierFileDbModel))]
    public class TBMSupplierFileEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 供应商编号
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string SupplierCode { get; set; }
        
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string SupplierName { get; set; }
        
        /// <summary>
        /// 供应商简称
        /// </summary>
        [StringLength(maximumLength:10)]
        public string ShortName { get; set; }
        
        /// <summary>
        /// 供应商类型ID
        /// </summary>
        [Required]
        public int SupplierTypeId { get; set; }
        
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
        public List<TBMSupplierContactEditModel> ChildList { get; set; }
    }
}
