///////////////////////////////////////////////////////////////////////////////////////
// File: TWMProfitDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Warehouse.Models.TWMProfitDetail
{
    /// <summary>
    /// TWMProfitDetail Query Model
    /// </summary>
    public class TWMProfitDetailQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 入库主表ID
        /// </summary>
        public int MainId { get; set; }
        
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }
                
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
        /// 仓库ID
        /// </summary>
        public int WarehouseId { get; set; }
        
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }
        
        /// <summary>
        /// 实盈数量
        /// </summary>
        public decimal ActualNumber { get; set; }

        /// <summary>
        /// 基本单位数量
        /// </summary>
        public decimal BaseUnitNumber
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ActualNumber * (decimal)WarehouseRate, 4);
                }
                return ActualNumber;
            }
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidityPeriod { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 账存数量
        /// </summary>
        public decimal AccountNum { get; set; }

    }
}
