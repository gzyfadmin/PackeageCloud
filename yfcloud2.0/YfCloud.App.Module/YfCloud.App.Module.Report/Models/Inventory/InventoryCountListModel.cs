using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Inventory
{
    /// <summary>
    /// 仓库一览表
    /// </summary>
    public class InventoryCountListModel
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialID { get; set; }
        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 物料分类id
        /// </summary>
        public int MaterialTypeId { get; set; }
        /// <summary>
        /// 物料分类
        /// </summary>
        public string MaterialTypeName { get; set; }
        /// <summary>
        /// 今日入库量
        /// </summary>
        public decimal ToDayStorageAmount { get; set; }
        /// <summary>
        /// 累计入库量
        /// </summary>
        public decimal TotalStorageAmount { get; set; }
        /// <summary>
        /// 今日出库量
        /// </summary>
        public decimal ToDayOutStorageAmount { get; set; }
        /// <summary>
        /// 累计出库量
        /// </summary>
        public decimal TotalOutStorageAmount { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public decimal TotalNum
        {
            get; set;
            //get
            //{
            //    return TotalStorageAmount - TotalOutStorageAmount;
            //}
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

    }
    public class InventoryCountListModel1
    {
        /// <summary>
        /// 物料id
        /// </summary>
        public int MaterialId { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// 今日入库
        /// </summary>
        public decimal ToDayinAmount { get; set; }
        /// <summary>
        /// 累计入库
        /// </summary>
        public decimal TotalinAmount { get; set; }
        /// <summary>
        /// 今日出库
        /// </summary>
        public decimal ToDayoutAmount { get; set; }
        /// <summary>
        /// 累计出库
        /// </summary>
        public decimal TotaloutAmount { get; set; }
        /// <summary>
        /// 累计入库金额
        /// </summary>
        public decimal TotalinMoneyAmount { get; set; }
        /// <summary>
        /// 累计出库金额
        /// </summary>
        public decimal TotaloutMoneyAmount { get; set; }
    }
    public class InventoryCountListModel2
    {
        /// <summary>
        /// 物料id
        /// </summary>
        public int MaterialId { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// 今日入库
        /// </summary>
        public decimal ToDayinAmount { get; set; }
        /// <summary>
        /// 累计入库
        /// </summary>
        public decimal TotalinAmount { get; set; }
        /// <summary>
        /// 今日出库
        /// </summary>
        public decimal ToDayoutAmount { get; set; }
        /// <summary>
        /// 累计出库
        /// </summary>
        public decimal TotaloutAmount { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public decimal TotalNum { get; set; }
        /// <summary>
        /// 累计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
    }

    public class InventoryListModel
    {
        /// <summary>
        /// 总记录条数，-1为不分页的值
        /// </summary>
        public long Total_Number { get; set; }
        /// <summary>
        /// 统计今日入库数量
        /// </summary>
        public decimal ToDayinNum { get; set; }
        /// <summary>
        /// 统计累计入库数量
        /// </summary>
        public decimal TotalinNum { get; set; }
        /// <summary>
        /// 统计今日出库数量
        /// </summary>
        public decimal ToDayoutNum { get; set; }
        /// <summary>
        /// 统计累计出库数量
        /// </summary>
        public decimal TotaloutNum { get; set; }

        /// <summary>
        /// 统计库存量
        /// </summary>
        public decimal TotalNum { get; set; }
        /// <summary>
        /// 统计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 列表
        /// </summary>
        public List<InventoryCountListModel> List { get; set; }
    }
}
