using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.ProductionOrder
{
    public class ProductionOrderCountListModel
    {
        /// <summary>
        /// 生产订单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string ProductionNo { get; set; }
        /// <summary>
        /// 生产订单明细ID
        /// </summary>
        public int DetailID { get; set; }
        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 生产类型 0库存生产 1订单生产
        /// </summary>
        public byte ProductionTypeId { get; set; }
        /// <summary>
        /// 生产类型
        /// </summary>
        public string ProductionTypeName
        {
            get
            {
                return ProductionTypeId == 0 ? "库存生产" : "订单生产";
            }
        }

        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal ProductionNum { get; set; }

        /// <summary>
        /// 生产车间
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 今日入库数量
        /// </summary>
        public decimal ToDayNum { get; set; }

        /// <summary>
        /// 累计入库数量
        /// </summary>
        public decimal TotalNum { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime OrderDate { get; set; }
    } 
    public class ProductionOrderListModel
    {
        /// <summary>
        /// 总记录条数，-1为不分页的值
        /// </summary>
        public long Total_Number { get; set; }
        /// <summary>
        /// 统计生产数量
        /// </summary>
        public decimal Total_Num { get; set; }
        /// <summary>
        /// 统计入库数量
        /// </summary>
        public decimal Total_DayNum { get; set; }
        /// <summary>
        /// 统计累计数量
        /// </summary>
        public decimal Total_TotalNum { get; set; }
        /// <summary>
        /// 列表
        /// </summary> 
        public List<ProductionOrderCountListModel> List { get; set; }
    }
}
