using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetailSum
{
    public class TPSMPurchaseOrderDetailQuerySumModel
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
        /// 可转单数量（采购转入库单，采购单位）
        /// </summary>
        public decimal TransferNum { get; set; }

        /// <summary>
        /// 可转单数量（基本单位）
        /// </summary>
        public decimal BasicTransferNum
        {
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
        public decimal TransferWareNum
        {
            get
            {
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    //基本单位 = 销售单位 * 换算率
                    //销售单位 = 基本单位 / 换算率
                    return decimal.Round(BasicTransferNum / (decimal)WarehouseRate, 4);
                }
                return BasicTransferNum;
            }
        }

    }
}
