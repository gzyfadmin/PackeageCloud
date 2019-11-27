using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 采购统计一览表
    /// </summary>
    public class PurchaseCountListModel
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
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 采购员ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 采购员姓名
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal PurchaseNum { get; set; }


        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? PurchaseAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
    }


    public class PurchaseListModel
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
        public List<PurchaseCountListModel> List { get; set; }
    }
}
