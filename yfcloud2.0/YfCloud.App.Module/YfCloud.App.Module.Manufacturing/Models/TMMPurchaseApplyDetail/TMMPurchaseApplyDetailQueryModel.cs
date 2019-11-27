///////////////////////////////////////////////////////////////////////////////////////
// File: TMMPurchaseApplyDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMPurchaseApplyDetail
{
    /// <summary>
    /// TMMPurchaseApplyDetail Query Model
    /// </summary>
    public class TMMPurchaseApplyDetailQueryModel
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
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 基本单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 生产单位ID
        /// </summary>
        public int? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }

        /// <summary>
        /// 生产单位数量
        /// </summary>
        public decimal? ProduceNum => ApplyNum;

        /// <summary>
        /// 采购单位ID
        /// </summary>
        public int? PurchaseUnitId { get; set; }

        /// <summary>
        /// 采购单位名称
        /// </summary>
        public string PurchaseUnitName { get; set; }

        /// <summary>
        /// 采购换算率
        /// </summary>
        public decimal? PurchaseRate { get; set; }

        /// <summary>
        /// 可转单数量（生产单位）
        /// </summary>
        public decimal TransNum { get; set; }

        /// <summary>
        /// 申请数量(基本单位)
        /// </summary>
        public decimal BaseUnitNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    return decimal.Round(ApplyNum * (decimal)ProduceRate, 4);
                }
                return ApplyNum;
            }
        }

        /// <summary>
        /// 申请数量(采购单位)
        /// </summary>
        public decimal? PurchaseNum
        {
            get
            {
                //基本单位 = 采购单位数量 * 换算率
                //采购单位 = 基本单位数量 / 换算率
                if (PurchaseUnitId != null && PurchaseRate != null)
                {
                    return decimal.Round(BaseUnitNum / (decimal)PurchaseRate, 4);
                }
                return BaseUnitNum;
            }
        }

        /// <summary>
        /// 可转采购数量
        /// </summary>
        public decimal? TranPurchaseNum
        {
            get
            {
                //生产单位可转数量转成基本单位数量
                decimal baseNum;
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    baseNum = TransNum * (decimal)ProduceRate;
                }
                else
                {
                    baseNum = TransNum;
                }

                if(PurchaseUnitId != null && PurchaseRate!= null)
                {
                    return decimal.Round(baseNum / (decimal)PurchaseRate, 4);
                }
                return baseNum;
            }
        }

    }
}
