///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderMainEditModel.cs
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
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailNP;

namespace YfCloud.App.Module.Sales.Models.TSSMSalesOrderMainNP
{
    /// <summary>
    /// T_SSM_SalesOrderMain Audit Model
    /// </summary>
    public class TSSMSalesOrderMainAuditNPModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte AuditStatus { get; set; }
    }
}
