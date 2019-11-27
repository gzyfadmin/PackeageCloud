using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetailNew;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMainNew
{
    public class TMMColorSolutionMainQueryNewModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// 方案编号
        /// </summary>
        public string SolutionCode { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        public int ColorId { get; set; }
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMColorSolutionDetailQueryNewModel> ChildList { get; set; }
    }
}
