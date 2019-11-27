using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.InventoryReport
{
    /// <summary>
    /// 物料统计
    /// </summary>
    public class ImportPrimeModel
    {


        /// <summary>
        /// 物料代码
        /// </summary>
        [Required]
        [StringLength(maximumLength: 20)]
        public string MaterialCode { get; set; }


        /// <summary>
        /// 仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 50)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 导入数量
        /// </summary>
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal Num { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Required]
        [StringLength(maximumLength: 20)]
        public string UnitName { get; set; }
    }
}
