///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMInventoryMain
{
    /// <summary>
    /// TWMInventoryMain Query Model
    /// </summary>
    public class TWMInventoryMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 盘点日期
        /// </summary>
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// 盘点单号
        /// </summary>
        public string InventoryOrder { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }

        /// <summary>
        /// 仓管员姓名
        /// </summary>
        public string WhAdminName { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员姓名
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// 盘点员ID
        /// </summary>
        public int? InventoryId { get; set; }

        /// <summary>
        /// 盘点员姓名
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TWMInventoryDetailQueryModel> ChildList { get; set; }
    }
}
