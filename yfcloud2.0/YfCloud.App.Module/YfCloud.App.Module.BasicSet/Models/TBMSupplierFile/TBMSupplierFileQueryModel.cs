///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.BasicSet.Models.TBMSupplierContact;

namespace YfCloud.App.Module.BasicSet.Models.TBMSupplierFile
{
    /// <summary>
    /// TBMSupplierFile Query Model
    /// </summary>
    public class TBMSupplierFileQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }
        
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        
        /// <summary>
        /// 供应商简称
        /// </summary>
        public string ShortName { get; set; }
        
        /// <summary>
        /// 供应商类型ID
        /// </summary>
        public int SupplierTypeId { get; set; }

        /// <summary>
        /// 供应商类型名称
        /// </summary>
        public string SupplierTypeName { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        public string LegalPerson { get; set; }
        
        /// <summary>
        /// 行业ID
        /// </summary>
        public int? IndustryId { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string Address { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TBMSupplierContactQueryModel> ChildList { get; set; }
    }
}
