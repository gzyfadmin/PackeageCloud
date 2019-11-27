using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetailSum
{
    /// <summary>
    /// 
    /// </summary>
    public class TMMProductionOrderDetailQuerySum
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
        /// 基本单位数量
        /// </summary>
        public decimal BaseUnitNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    return decimal.Round(ProductionNum * (decimal)ProduceRate, 4);
                }
                return ProductionNum;
            }
        }




        /// <summary>
        /// 应收数量
        /// </summary>
        public decimal TransNum { get; set; }

        /// <summary>
        /// 应收数量(基本单位)
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
        /// 应收数量(库存)
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
        /// 仓库单位ID
        /// </summary>
        public int? WarehouseUnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库单位换算率
        /// </summary>
        public decimal? WarehouseRate
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库单位名称
        /// </summary>
        public string WarehouseName
        {
            get;
            set;
        }

        /// <summary>
        /// 生产单位
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
        /// 配色方案ID
        /// </summary>
        public int? ColorSolutionId { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 配色方案名称
        /// </summary>
        public string ColorSolutionName { get; set; }


        /// <summary>
        /// 客户商品代码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 客户商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal ProductionNum { get; set; }

        /// <summary>
        /// 生产车间
        /// </summary>
        public int? WorkshopId { get; set; }

        /// <summary>
        /// 生产车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string PrincipalName { get; set; }

        /// <summary>
        /// 生产入库单明细ID
        /// </summary>
        public int ProOrderDetailId { get; set; }
    }
}
