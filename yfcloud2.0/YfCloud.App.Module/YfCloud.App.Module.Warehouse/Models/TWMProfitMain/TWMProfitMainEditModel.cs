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
    /// T_WM_ProfitMain Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TWMProfitMainDbModel))]
    public class TWMProfitMainEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
                
        /// <summary>
        /// 入库日期
        /// </summary>
        [Required]
        public DateTime WarehousingDate { get; set; }
                        
        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TWMProfitDetailEditModel> ChildList { get; set; }
    }
}
