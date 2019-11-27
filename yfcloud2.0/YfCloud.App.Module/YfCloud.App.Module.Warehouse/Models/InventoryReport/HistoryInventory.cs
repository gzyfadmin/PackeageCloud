using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.App.Module.Warehouse.Models.InventoryReport
{
    public class HistoryInventory
    {

        public int ID { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 操作类型(出库类型 0销售出库 1生产出库 2裁片出库 3委外出库 4盘亏出库 5其他出库);入库类型 0采购入库 1生产入库 2委外入库 3其他入库
        /// </summary>
        public int InventoryType { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 基本单位数量ID
        /// </summary>
        public decimal BaseUnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

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
                    return decimal.Round(WarehouseAmount * (decimal)WarehouseRate, 4);
                }
                return WarehouseAmount;
            }
        }

        /// <summary>
        /// 仓库数量
        /// </summary>
        public decimal WarehouseAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库与基本单位换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// 1表示入库 2表示出库
        /// </summary>

        public int OperateType { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime OpearateDate { get; set; }

    }


    public class HistoryInventoryQuery
    {
        /// <summary>
        /// Get请求参数,是否启用分页查询
        /// </summary>
        public bool IsPaging { get; set; }

        /// <summary>
        /// Get请求参数,每页显示条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Get请求参数,页码，索引从1开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }

        /// <summary>
        /// Get请求参数,排序条件
        /// </summary>
        public List<OrderByCondition> OrderByConditions { get; set; }
    }
}
