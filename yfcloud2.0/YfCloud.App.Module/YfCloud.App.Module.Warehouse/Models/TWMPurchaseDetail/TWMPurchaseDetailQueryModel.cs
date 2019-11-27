///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPurchaseDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail
{
    /// <summary>
    /// TWMPurchaseDetail Query Model
    /// </summary>
    public class TWMPurchaseDetailQueryModel
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
        /// 收货仓库ID
        /// </summary>
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal ActualNum { get; set; }

        /// <summary>
        /// 采购单位数量
        /// </summary>
        public decimal PurchaseActualNum { get; set; }

        /// <summary>
        /// 采购详细表ID
        /// </summary>
        public int PurchaseDetailId { get; set; }

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
        /// 采购计量单位ID
        /// </summary>
        public int? PurchaseUnitId { get; set; }

        /// <summary>
        /// 采购与基本单位换算率
        /// </summary>
        public decimal? PurchaseRate { get; set; }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }
        
        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }


        /// <summary>
        /// 基本单位实收数量
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
        /// 采购数量(采购单位)
        /// </summary>
        public decimal PurchaseNum { get; set; }


        /// <summary>
        /// 交期
        /// </summary>
        public DateTime? DeliveryPeriod { get; set; }

        /// <summary>
        /// 应发数量(采购单位)
        /// </summary>
        public decimal ShouldPurchaseNum { get; set; }

        /// <summary>
        /// 应收数量(仓库单位)
        /// </summary>
        public decimal ShouldNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(BaseUnitShouldNum / (decimal)WarehouseRate, 4);
                }
                return BaseUnitShouldNum;
            }
        }

        /// <summary>
        /// 欠料数量
        /// </summary>
        public decimal MissNum {
            get {
                if (ShouldNum == 0)
                {
                    return 0;
                }
                else
                {
                    return ShouldNum - ActualNum;
                }
            } }

        /// <summary>
        /// 应收数量（基本单位）
        /// </summary>
        public decimal BaseUnitShouldNum
        {
            get
            {
                if (PurchaseUnitId != null && PurchaseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ShouldPurchaseNum * (decimal)PurchaseRate, 4);
                }
                return ShouldPurchaseNum;
            }
        }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidDate { get; set; }
    }
}
