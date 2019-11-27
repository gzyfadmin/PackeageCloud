using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew
{
    public class TMMBOMDetailQueryNewModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        public int MainId { get; set; }

        /// <summary>
        /// 配色项目ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 配色项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 部位名称ID
        /// </summary>
        public int? PartId { get; set; }

        /// <summary>
        /// 部位名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 长度值
        /// </summary>
        public decimal? LengthValue { get; set; }

        /// <summary>
        /// 宽度值
        /// </summary>
        public decimal? WidthValue { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public decimal NumValue { get; set; }

        /// <summary>
        /// 封度（宽幅）
        /// </summary>
        public decimal? WideValue { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal? LossValue { get; set; }

        /// <summary>
        /// 单用量
        /// </summary>
        public decimal SingleValue { get; set; }

        /// <summary>
        /// 单用量计算公式
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string FormulaName { get; set; }
    }

    public class TMMBOMDetailQueryExcelModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        public int MainId { get; set; }

        /// <summary>
        /// 配色项目ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 配色项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 部位名称ID
        /// </summary>
        public int? PartId { get; set; }

        /// <summary>
        /// 部位名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 长度值
        /// </summary>
        public decimal? LengthValue { get; set; }

        /// <summary>
        /// 宽度值
        /// </summary>
        public decimal? WidthValue { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public decimal NumValue { get; set; }

        /// <summary>
        /// 封度（宽幅）
        /// </summary>
        public decimal? WideValue { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal? LossValue { get; set; }

        /// <summary>
        /// 单用量
        /// </summary>
        public decimal SingleValue { get; set; }

        /// <summary>
        /// 单用量计算公式
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string FormulaName { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }
    }
}
