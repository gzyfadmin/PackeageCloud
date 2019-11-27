///////////////////////////////////////////////////////////////////////////////////////
// File: TWMSalesDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TWMSalesDetail
{
    /// <summary>
    /// TWMSalesDetail Query Model
    /// </summary>
    public class TWMSalesDetailQueryModel
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
        public int WarehouseId { get; set; }

        #region 销售单的数量


        /// <summary>
        /// 销售数量(仓库单位)
        /// </summary>
        public decimal SalesNumOnInventory { get; set; }

        /// <summary>
        /// 销售数量(销售单位)
        /// </summary>
        public decimal SalesNum { get; set; }

        #endregion 

        /// <summary>
        /// 实发数量
        /// </summary>
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 实发数量(销售单位数量)
        /// </summary>
        public decimal SalesOrderActualNum { get; set; }

        /// <summary>
        /// 来源销售明细（T_SSM_SalesOrderDetail>ID）
        /// </summary>
        public int SalesOrderDetailId { get; set; }

        /// <summary>
        /// 基本单位实发数量
        /// </summary>
        public decimal BaseUnitActualNum
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
        /// 应发数量(销售单位)
        /// </summary>
        public decimal ShouldSaleNum { get; set; }

        /// <summary>
        /// 应发数量
        /// </summary>
        public decimal ShouldNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    return decimal.Round(BaseUnitShouldNum / (decimal)WarehouseRate, 4);
                }

                return BaseUnitShouldNum;
            }
        }

        /// <summary>
        /// 欠货数量
        /// </summary>
        public decimal MissNum
        {
            get
            {
                if (ShouldNum == 0)
                {
                    return 0;
                }
                else
                {
                    return ShouldNum - ActualNum;
                }
            }
        }

        /// <summary>
        /// 基本单位应发数量
        /// </summary>
        public decimal BaseUnitShouldNum
        {
            get
            {
                if (SalesUnitId != null && SalesRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ShouldSaleNum * (decimal)SalesRate, 4);
                }
                return ShouldSaleNum;
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
        /// 销售计量单位
        /// </summary>
        public int? SalesUnitId { get; set; }

        /// <summary>
        /// 销售换算率
        /// </summary>
        public decimal? SalesRate { get; set; }
        

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal AvailableNum { get; set; }

        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 配色方案
        /// </summary>
        public string ColorSolutionName { get; set; }

        /// <summary>
        /// 包型编码
        /// </summary>
        public string PackageCode { get; set; }

    }
}
