///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPickApplyDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMPickApplyDetail
{
    /// <summary>
    /// TMMPickApplyDetail Query Model
    /// </summary>
    public class TMMPickApplyDetailQueryModel
    {
        /// <summary>
        /// 主键值增长
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
        /// 申请数量
        /// </summary>
        public decimal ApplyNum { get; set; }

        /// <summary>
        /// 申请数量(基本单位)
        /// </summary>
        public decimal BaseUnitNumber
        {
            get
            {
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(ApplyNum * (decimal)ProduceRate, 4);
                }
                return ApplyNum;
            }
        }

        /// <summary>
        /// 可转单数量(生产单位)
        /// </summary>
        public decimal TransNum { get; set; }

        /// <summary>
        /// 可转单数量(基本单位)
        /// </summary>
        public decimal TransNumBasic
        {
            get
            {
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(TransNum * (decimal)ProduceRate, 4);
                }
                return TransNum;
            }
        }

        /// <summary>
        /// 可转单数量(仓库单位)
        /// </summary>
        public decimal TransNumWare
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位 * 换算率
                    //仓库单位 = 基本单位 / 换算率
                    return decimal.Round(TransNumBasic / (decimal)WarehouseRate, 4);
                }
                return TransNumBasic;
            }
        }

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
        /// 生产单位
        /// </summary>
        public int? ProduceUnitId { get; set; }


        /// <summary>
        /// 生产计量单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }


        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal AvailableNum { get; set; }

       

    }
}
