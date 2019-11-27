///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderBOM.cs
// Author: www.cloudyf.com
// Create Time:2019/9/10
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM
{
    /// <summary>
    /// TMMProductionOrderBOM Query Model
    /// </summary>
    public class TMMProductionOrderBOMQueryModel
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
        /// 配色项目ID
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// 配色项目
        /// </summary>
        public string ItemName { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 配色方案编号
        /// </summary>
        public string ColorSolutionCode { get; set; }

        /// <summary>
        /// 单用量
        /// </summary>
        public decimal SingleValue { get; set; }



        /// <summary>
        /// 部位ID
        /// </summary>
        public int? PartId { get; set; }

        /// <summary>
        /// 部位名称
        /// </summary>
        public string PartName { get; set; }
        
       
        
        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal ProductionNum { get; set; }
        


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
}
