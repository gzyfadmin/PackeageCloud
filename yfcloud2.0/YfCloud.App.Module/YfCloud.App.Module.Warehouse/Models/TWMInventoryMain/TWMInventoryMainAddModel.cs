///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryMainAddModel.cs
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
using YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMInventoryMain
{
    /// <summary>
    /// T_WM_InventoryMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TWMInventoryMainDbModel))]
    public class TWMInventoryMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 仓库ID
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 盘点日期
        /// </summary>
        [Required]
        public DateTime InventoryDate { get; set; }
        
        /// <summary>
        /// 盘点单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:30)]
        public string InventoryOrder { get; set; }
        
        /// <summary>
        /// 操作员ID
        /// </summary>
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }

        /// <summary>
        /// 盘点员ID
        /// </summary>
        public int? InventoryId { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMInventoryDetailAddModel> ChildList { get; set; }
    }
}
