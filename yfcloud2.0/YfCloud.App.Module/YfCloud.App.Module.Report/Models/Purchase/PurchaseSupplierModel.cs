﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 采购供应商模型
    /// </summary>
    public class PurchaseSupplierModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }
    }
}
