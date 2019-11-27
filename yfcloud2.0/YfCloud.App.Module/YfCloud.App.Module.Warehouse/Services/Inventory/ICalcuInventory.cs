using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Warehouse.Models;

namespace YfCloud.App.Module.Warehouse.Services
{
    /// <summary>
    /// 库存场景统计接口(其他出库、其他入库、盘盈入库、盘亏出库等)
    /// </summary>
   public  interface ICalcuInventory
    {
        /// <summary>
        /// 计算待入库
        /// </summary>
        /// <returns></returns>
        decimal CalcuToIn(TWMStaQuery tWMStaQuery);

        /// <summary>
        /// 计算待出库
        /// </summary>
        /// <returns></returns>
        decimal CalcuToOut(TWMStaQuery tWMStaQuery);

        /// <summary>
        /// 计算入库和出库数量（正数代表入库，负数代表出库)
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>
        decimal CalcuInAndOut(TWMStaQuery tWMStaQuery);
    }
}
