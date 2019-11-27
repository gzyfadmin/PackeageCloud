///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProductionDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionDetail
{
    /// <summary>
    /// TWMProductionDetail Query Model
    /// </summary>
    public class TWMProductionDetailQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 主单ID
        /// </summary>
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }
        
        /// <summary>
        /// 发货仓库ID
        /// </summary>
        public int? WarehouseId { get; set; }
        
        /// <summary>
        /// 实发数量
        /// </summary>
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 实发数量（生产单位）
        /// </summary>
        public decimal PickActualNum { get; set; }

        /// <summary>
        /// 领料单详细ID
        /// </summary>
        public int PickApplyDetailId { get; set; }


        /// <summary>
        /// 实发数量（基本单位）
        /// </summary>
        public decimal BaseUnitActualNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (WarehouseId != null && WarehouseRate != null)
                {
                    return decimal.Round(ActualNum * (decimal)WarehouseRate, 4);
                }
                return ActualNum;
            }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 应发数量(生产)
        /// </summary>
        public decimal TransNum { get; set; }

        /// <summary>
        /// 应发数量(基本单位)
        /// </summary>
        public decimal BaseUnitTransNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    return decimal.Round(TransNum * (decimal)ProduceRate, 4);
                }
                return TransNum;
            }
        }


        /// <summary>
        /// 应发数量(库存)
        /// </summary>
        public decimal WarehouseUnitTransNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    return decimal.Round(BaseUnitTransNum / (decimal)WarehouseRate, 4);
                }
                return BaseUnitTransNum;
            }
        }

        /// <summary>
        /// 当前仓库的可用数量（仓库单位）
        /// </summary>
        public decimal AvailableNum { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料分类ID
        /// </summary>
        public int MaterialTypeId { get; set; }

        /// <summary>
        /// 物料分类名称
        /// </summary>
        public string MaterialTypeName { get; set; }

        /// <summary>
        /// 颜色Id
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

         /// <summary>
        /// 基本计量单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 生产计量单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产单位ID
        /// </summary>
        public decimal? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }

    }
}
