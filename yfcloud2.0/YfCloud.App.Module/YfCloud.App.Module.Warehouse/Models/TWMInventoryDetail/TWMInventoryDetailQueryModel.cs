///////////////////////////////////////////////////////////////////////////////////////
// File: TWMInventoryDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TWMInventoryDetail
{
    /// <summary>
    /// TWMInventoryDetail Query Model
    /// </summary>
    public class TWMInventoryDetailQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

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
        /// 盘点仓库ID
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        
        /// <summary>
        /// 账存数量
        /// </summary>
        public decimal AccountNum { get; set; }

        /// <summary>
        /// 账存基本单位数量
        /// </summary>
        public decimal AccountUnitNum
        {
            get
            {
                if(WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(AccountNum * (decimal)WarehouseRate, 4);
                }
                return AccountNum;
            }
        }
        
        /// <summary>
        /// 实存数量
        /// </summary>
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 实存基本单位数量
        /// </summary>
        public decimal ActualUnitNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ActualNum * (decimal)WarehouseRate, 4);
                }
                return ActualNum;
            }
        }
        
        /// <summary>
        /// 盘盈数量
        /// </summary>
        public decimal ProfitNum { get; set; }

        /// <summary>
        /// 盘盈基本单位数量
        /// </summary>
        public decimal ProfitUnitNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ProfitNum * (decimal)WarehouseRate, 4);
                }
                return ProfitNum;
            }
        }
        
        /// <summary>
        /// 盘亏数量
        /// </summary>
        public decimal DeficitNum { get; set; }

        /// <summary>
        /// 盘亏基本单位数量
        /// </summary>
        public decimal DeficitUnitNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(DeficitNum * (decimal)WarehouseRate, 4);
                }
                return DeficitNum;
            }
        }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
    }
}
