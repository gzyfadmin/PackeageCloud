///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderMain
{
    /// <summary>
    /// T_MM_ProductionOrderMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMProductionOrderMainDbModel))]
    public class TMMProductionOrderMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 生产类型 0库存生产 1订单生产
        /// </summary>
        [Required]
        public byte ProductionType { get; set; }
        
        /// <summary>
        /// 生产单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string ProductionNo { get; set; }
        
        /// <summary>
        /// 源单号ID
        /// </summary>
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        public int? CustomerId { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }
        
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Required]
        public DateTime BeginDate { get; set; }
        
        /// <summary>
        /// 计划结束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMProductionOrderDetailAddModel> ChildList { get; set; }
    }

    public class PackageColor
    {
        public int PackageID
        {
            get;
            set;
        }

        public int? ColorSolutionID
        {
            get; set;
        }
    }
}
