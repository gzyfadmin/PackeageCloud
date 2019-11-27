///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderBOMSum.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOMSum
{
    /// <summary>
    /// TMMProductionOrderBOMSum Query Model
    /// </summary>
    public class TMMProductionOrderBOMSumQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 生产单ID
        /// </summary>
        public int ProOrderId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 配色方案编码
        /// </summary>
        public string ColorSolutionCode { get; set; }
        
        /// <summary>
        /// 总用量
        /// </summary>
        public decimal TotalValue { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal PurchaseNum { get; set; }
        
        /// <summary>
        /// 已转采购申请单数量
        /// </summary>
        public decimal PurchaseTransNum { get; set; }
        
        /// <summary>
        /// 领料数量
        /// </summary>
        public decimal PickNum { get; set; }
        
        /// <summary>
        /// 已转领料申请单数量
        /// </summary>
        public decimal PickTransNum { get; set; }
        
        /// <summary>
        /// 总领料数
        /// </summary>
        public decimal PickTotalNum { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 生产计量单位ID
        /// </summary>
        public int? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产计量单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

    }

    public class SumQueryApiModel
    {
        /// <summary>
        /// 列表
        /// </summary>
        public MrpResultModel MainList { get; set; }

        /// <summary>
        /// 包型ID集合
        /// </summary>
        public IList<int> PackageId { get; set; }
    }
}
