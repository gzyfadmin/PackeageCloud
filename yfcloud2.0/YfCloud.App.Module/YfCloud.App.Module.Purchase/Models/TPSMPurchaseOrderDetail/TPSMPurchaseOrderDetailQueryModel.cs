///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail
{
    /// <summary>
    /// TPSMPurchaseOrderDetail Query Model
    /// </summary>
    public class TPSMPurchaseOrderDetailQueryModel
    {
        /// <summary>
        /// 主键值增长
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
        /// 供应商ID
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

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
        /// 采购基本单位ID
        /// </summary>
        public int? PurchaseUnitId { get; set; }

        /// <summary>
        /// 采购计量单位名称
        /// </summary>
        public string PurchaseUnitName { get; set; }

        /// <summary>
        /// 采购换算率
        /// </summary>
        public decimal? PurchaseRate { get; set; }

        /// <summary>
        /// 仓库基本单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库基本单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 仓库单位数量
        /// </summary>
        public decimal WarehouseNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 仓库单位数量 * 换算率
                    //仓库单位 = 基本单位数量 / 换算率
                    return decimal.Round(BaseUnitNum / (decimal)WarehouseRate, 4);
                }
                return BaseUnitNum;
            }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal PurchaseNum { get; set; }

        /// <summary>
        /// 基本单位数量
        /// </summary>
        public decimal BaseUnitNum
        {
            get
            {
                if (PurchaseUnitId != null && PurchaseRate != null)
                {
                    //基本单位 = 销售单位 * 换算率
                    //销售单位 = 基本单位 / 换算率
                    return decimal.Round(PurchaseNum * (decimal)PurchaseRate, 4);
                }
                return PurchaseNum;
            }
        }

        /// <summary>
        /// 采购金额
        /// </summary>
        public decimal? PurchaseAmount { get; set; }

        /// <summary>
        /// 交期
        /// </summary>
        public DateTime? DeliveryPeriod { get; set; }

        /// <summary>
        /// 可转单数量（采购转入库单，采购单位）
        /// </summary>
        public decimal TransferNum { get; set; }

        /// <summary>
        /// 可转单数量（基本单位）
        /// </summary>
        public decimal BasicTransferNum {
            get
            {
                if (PurchaseUnitId != null && PurchaseRate != null)
                {
                    //基本单位 = 销售单位 * 换算率
                    //销售单位 = 基本单位 / 换算率
                    return decimal.Round(TransferNum * (decimal)PurchaseRate, 4);
                }
                return TransferNum;
            }
        }

        /// <summary>
        /// 可转单数量（采购转入库单，仓库单位）
        /// </summary>
        public decimal TransferWareNum { get; set; }

        /// <summary>
        /// 应收数量
        /// </summary>
        public decimal ShouldNum
        {
            get;set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 生产单位，可转数量（生产采购申请单转生产采购单）
        /// </summary>
        public decimal ProdTransNum { get; set; }

        /// <summary>
        /// 生产单位ID
        /// </summary>
        public decimal? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产单位换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }

        /// <summary>
        /// 采购单位，可转数量（生产采购申请单转生产采购单）
        /// </summary>
        public decimal PurchaseTranNnum
        {
            get
            {
                var baseProdTransNum = ProdTransNum;
                decimal tempNum = PurchaseNum;

                if (AuditStatus == 2)
                {
                    tempNum = 0;
                }

                if (ProduceRate != null && ProduceUnitId != null)
                {
                    //基本单位 = 生产单位 * 换算率
                    //生产单位 = 基本单位 / 换算率
                    baseProdTransNum = decimal.Round(ProdTransNum * (decimal)ProduceRate, 4);
                }

                if(PurchaseRate != null && PurchaseUnitId != null)
                {
                    //基本单位 = 采购单位 * 换算率
                    //采购单位 = 基本单位 / 换算率

                    return decimal.Round(baseProdTransNum / (decimal)PurchaseRate, 4) + tempNum;

                }
                return baseProdTransNum + tempNum;
            }
        }

        /// <summary>
        /// 主单审核状态
        /// </summary>
        public byte? AuditStatus { get; set; }
    }
}
