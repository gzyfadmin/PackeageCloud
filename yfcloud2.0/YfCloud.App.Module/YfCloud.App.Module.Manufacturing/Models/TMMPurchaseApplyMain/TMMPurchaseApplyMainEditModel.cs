///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPurchaseApplyMainEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyMain
{
    /// <summary>
    /// T_MM_PurchaseApplyMain Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMPurchaseApplyMainDbModel))]
    public class TMMPurchaseApplyMainEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 源单号（生产单ID）
        /// </summary>
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 采购申请单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string PurchaseNo { get; set; }
        
        /// <summary>
        /// 申请日期
        /// </summary>
        [Required]
        public DateTime ApplyDate { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMPurchaseApplyDetailEditModel> ChildList { get; set; }
    }
}
