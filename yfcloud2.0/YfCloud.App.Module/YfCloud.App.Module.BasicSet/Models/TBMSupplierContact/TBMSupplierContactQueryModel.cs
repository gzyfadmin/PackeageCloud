///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierContact.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.BasicSet.Models.TBMSupplierContact
{
    /// <summary>
    /// TBMSupplierContact Query Model
    /// </summary>
    public class TBMSupplierContactQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNumber { get; set; }
        
        /// <summary>
        /// 联系人优先级
        /// </summary>
        public byte Priority { get; set; }
        
    }
}
