using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models
{
    /// <summary>
    /// 仓库统计模型
    /// </summary>
    public class TWMCountModel
    {

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int? WarehouseId { get; set; }

        /// <summary>
        /// 期初数量
        /// </summary>
        public decimal PrimeNum { get; set; }

        /// <summary>
        /// 待入库数量
        /// </summary>
        public decimal WaitWarehousingNum { get; set; }

        ///// <summary>
        ///// 已入库数量
        ///// </summary>
        //public decimal WarehousingNum { get; set; }

            /// <summary>
            /// 出入库数量
            /// </summary>
       public decimal InAndOut { get; set; }

        /// <summary>
        /// 待出库
        /// </summary>
        public decimal WaitOutWhNum { get; set; }

        ///// <summary>
        ///// 已出库数量
        ///// </summary>
        //public decimal OutWhNum { get; set; }

        /// <summary>
        /// 账目库存数量 （期初+入库-出库)
        /// </summary>
        public decimal AccountNum { get; set; }

        /// <summary>
        /// 可用数量 (期初+入库-出库-待出库数量)
        /// </summary>
        public decimal AvaiableNum { get; set; }
    }

    /// <summary>
    /// 统计查询模型
    /// </summary>
    public class TWMStaQuery
    {

        /// <summary>
        /// 出库时必填
        /// </summary>
       public int? EditID { get; set; }

        /// <summary>
        /// 出库时必填
        /// 0 其他出库
        /// 1 盘亏出库
        /// 2 销售出库
        /// 4 生产出库
        /// </summary>
        public OperateEnum? OperateType { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int? WarehouseId { get; set; }

    }

    /// <summary>
    /// 出库菜单场景()
    /// </summary>
    public enum OperateEnum
    {

        /// <summary>
        /// 其他出库菜单
        /// </summary>
        Other = 0,
        /// <summary>
        /// 盘点出库菜单
        /// </summary>
        Invertory = 1,

        /// <summary>
        /// 销售出库菜单
        /// </summary>
        Sale = 2,

        /// <summary>
        ///采购入库菜单 
        /// </summary>
        Purchase=3,

        /// <summary>
        /// 生产出库
        /// </summary>
        Product=4,
    }
}
