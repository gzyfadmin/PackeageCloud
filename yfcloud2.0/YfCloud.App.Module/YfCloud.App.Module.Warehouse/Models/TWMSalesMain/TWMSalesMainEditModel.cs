///////////////////////////////////////////////////////////////////////////////////////
// File: TWMSalesMainEditModel.cs
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
using YfCloud.App.Module.Warehouse.Models.TWMSalesDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMSalesMain
{
    /// <summary>
    /// T_WM_SalesMain Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TWMSalesMainDbModel))]
    public class TWMSalesMainEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        
        /// <summary>
        /// 出库日期
        /// </summary>
        [Required]
        public DateTime WhSendDate { get; set; }

        
        /// <summary>
        /// 制单人ID
        /// </summary>
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 发货员ID
        /// </summary>
        public int? SendId { get; set; }
        
        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }
        
        
        /// <summary>
        /// 收货地址
        /// </summary>
        [StringLength(maximumLength:200)]
        public string ReceiptAddress { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMSalesDetailEditModel> ChildList { get; set; }
    }
}
