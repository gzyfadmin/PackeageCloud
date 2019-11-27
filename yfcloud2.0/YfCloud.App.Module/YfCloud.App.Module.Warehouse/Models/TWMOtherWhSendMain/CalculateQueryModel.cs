using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhSendMain
{
    /// <summary>
    /// 查询可用物料
    /// </summary>
    public class CalculateQueryModel
    {
        /// <summary>
        /// 主单ID
        /// </summary>
       public int? ID
        {
            get;
            set;
        }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        public int MaterialId
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Required]
        public int houseID
        {
            get;
            set;
        }
    }
}
