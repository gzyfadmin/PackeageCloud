using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Sales
{
    /// <summary>
    /// 销售统计一览表
    /// </summary>
    public class SalesCountListModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型ID
        /// </summary>
        public int OrderTypeId { get; set; }

        /// <summary>
        /// 订单类型名称
        /// </summary>
        public string OrderTypeName { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        public int SalesManId { get; set; }

        /// <summary>
        /// 业务员姓名
        /// </summary>
        public string SalesManName { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// 包型名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal SalesNum { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? SalesAmount { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? SalesAmountCount
        {
            get
            {
                return SalesNum * SalesAmount;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
    }
    public class SalesListModel
    {
        /// <summary>
        /// 总记录条数，-1为不分页的值
        /// </summary>
        public long Total_Number { get; set; }
        /// <summary>
        /// 统计数量
        /// </summary>
        public decimal Total_Num { get; set; }
        /// <summary>
        /// 统计金额
        /// </summary>
        public decimal Total_Amount { get; set; }
        /// <summary>
        /// 列表
        /// </summary> 
        public List<SalesCountListModel> List { get; set; }
    }
}
