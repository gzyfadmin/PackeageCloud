///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitMainEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMProfitDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMProfitMain
{
    /// <summary>
    /// 盘盈入库单审核模型
    /// </summary>
    public class TWMProfitMainAuditModel
    {
        /// <summary>
        /// 盘盈入库单ID
        /// </summary>
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        [Required]
        public byte AuditStatus { get; set; }

    }
}
