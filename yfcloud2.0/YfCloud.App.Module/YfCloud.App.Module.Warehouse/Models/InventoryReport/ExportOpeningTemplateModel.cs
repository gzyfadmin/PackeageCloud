using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.InventoryReport
{
    public class ExportOpeningTemplateModel
    {
        /// <summary>
        /// 仓库下拉数据
        /// </summary>
        public string[] WarehouseName { get; set; }
        /// <summary>
        /// 物资信息
        /// </summary>
        public List<MaterialFileTemplate> List { get; set; }
    }

    public class MaterialFileTemplate
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 仓库名字
        /// </summary>
        public string WareName { get; set; } = "";

        /// <summary>
        /// 基本单位
        /// </summary>
        public int BasicUnitId { get; set; }

        /// <summary>
        /// 库存单位
        /// </summary>
        public int? WareUnitId { get; set; }

        /// <summary>
        /// 库存单位名称
        /// </summary>
        public string WareUnitName { get; set; }
    }
}
