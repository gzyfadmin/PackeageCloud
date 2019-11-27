///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderDetailEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail
{
    /// <summary>
    /// T_MM_ProductionOrderDetail Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TMMProductionOrderDetailDbModel))]
    public class TMMProductionOrderDetailEditModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [Required]
        public int MainId { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [Required]
        public int PackageId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 配色方案ID
        /// </summary>
        public int? ColorSolutionId { get; set; }

        /// <summary>
        /// 生产车间
        /// </summary>
        [Required]
        public int? WorkshopId { get; set; }
        
        /// <summary>
        /// 负责人ID
        /// </summary>
        public int? PrincipalId { get; set; }
        
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryPeriod { get; set; }
        
        /// <summary>
        /// 客户商品代码
        /// </summary>
        [StringLength(maximumLength:20)]
        public string GoodsCode { get; set; }
        
        /// <summary>
        /// 客户商品名称
        /// </summary>
        [StringLength(maximumLength:20)]
        public string GoodsName { get; set; }
        
        /// <summary>
        /// 生产数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal ProductionNum { get; set; }
        
    }
}
