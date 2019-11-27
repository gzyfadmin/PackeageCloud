using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Warehouse.Models;

namespace YfCloud.App.Module.Warehouse.Services
{
    /// <summary>
    /// 仓库统计接口
    /// </summary>
    public interface IStaticInventory
    {
        /// <summary>
        /// 统计物料的数量
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>
        TWMCountModel GeTWMCountModel(TWMStaQuery tWMStaQuery);
    }
}
