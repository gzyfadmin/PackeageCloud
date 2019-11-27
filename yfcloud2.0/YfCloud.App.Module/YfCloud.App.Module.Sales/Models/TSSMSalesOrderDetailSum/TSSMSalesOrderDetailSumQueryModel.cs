using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetailSum
{
    public class TSSMSalesOrderDetailSumQueryModel
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
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }
        /// <summary>
        /// 包型代码
        /// </summary>
        public string PackageCode { get; set; }
        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }
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
        /// 销售基本单位ID
        /// </summary>
        public int? SalesUnitId { get; set; }

        /// <summary>
        /// 销售基本单位名称
        /// </summary>
        public string SalesUnitName { get; set; }

        /// <summary>
        /// 销售换算率
        /// </summary>
        public decimal? SalesRate { get; set; }

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
        /// 生产单位ID
        /// </summary>
        public int? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产单位换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }

        /// <summary>
        /// 生产单位数量
        /// </summary>
        public decimal ProduceNum
        {
            get
            {
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    //基本单位 = 生产单位数量 * 换算率
                    //生产单位 = 基本单位数量 / 换算率
                    return decimal.Round(BaseUnitNum / (decimal)ProduceRate, 4);
                }
                return BaseUnitNum;
            }
        }

        /// <summary>
        /// 客户商品代码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 客户商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal SalesNum { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal? SalesAmount { get; set; }

        /// <summary>
        /// 可转出库单数量(销售单位)
        /// </summary>
        public decimal TransferNum { get; set; }

        /// <summary>
        /// 可转出库单数量（基本单位)
        /// </summary>
        public decimal BaseUnitNum
        {
            get
            {
                if (SalesUnitId != null && SalesRate != null)
                {
                    return TransferNum * SalesRate.Value;
                }
                return TransferNum;
            }
        }

        /// <summary>
        /// 可转出库单数量（仓位单位）
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 可转生产单数量（销售单位）
        /// </summary>
        public decimal? TransProdNum { get; set; }

        /// <summary>
        /// 可转生产单数量(基本单位)
        /// </summary>
        public decimal BaseTransProdNum
        {
            get
            {
                if (SalesUnitId != null && SalesRate != null)
                {
                    return TransProdNum.Value * SalesRate.Value;
                }
                return TransProdNum.Value;
            }
        }

        /// <summary>
        /// 可转生产单数量（生产单位）
        /// </summary>
        public decimal ProduceTransProdNum {
            get
            {
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    return BaseTransProdNum / ProduceRate.Value;
                }
                return BaseTransProdNum;
            }
        }

       
       

       

        /// <summary>
        /// 配色方案ID
        /// </summary>
        public int? ColorSolutionId { get; set; }

        /// <summary>
        /// 配色方案名称
        /// </summary>
        public string ColorSolutionName { get; set; }
    }
}
