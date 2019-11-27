using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOM;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderBOMSum
{
    public class MrpResultModel
    {

        /// <summary>
        /// 明细
        /// </summary>
        public List<TMMProductionOrderBOMQueryModel> deatails { get; set; }

        /// <summary>
        /// 汇总
        /// </summary>
        public List<TMMProductionOrderBOMSumQueryModel> summary { get; set; }
    }
}
