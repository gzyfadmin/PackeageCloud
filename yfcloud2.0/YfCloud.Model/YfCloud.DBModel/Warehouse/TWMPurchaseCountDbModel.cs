using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace YfCloud.DBModel
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("T_WM_PurchaseCount")]
    public class TWMPurchaseCountDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId")]
        public int MaterialId { get; set; }

        /// <summary>
        /// 入库数量，审核通过后入账
        /// </summary>
        [SugarColumn(ColumnName = "WhNumber")]
        public decimal WhNumber { get; set; }

        /// <summary>
        /// 出库数量，审核通过后入账
        /// </summary>
        [SugarColumn(ColumnName = "WhSendNumber")]
        public decimal WhSendNumber { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseId")]
        public int WarehouseId { get; set; }
    }
}
