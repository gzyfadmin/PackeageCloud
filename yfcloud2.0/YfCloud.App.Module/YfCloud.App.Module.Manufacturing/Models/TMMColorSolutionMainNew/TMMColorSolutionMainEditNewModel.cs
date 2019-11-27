using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionDetailNew;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMainNew
{
    [UseAutoMapper(typeof(TMMColorSolutionMainDbModel))]
    public class TMMColorSolutionMainEditNewModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }

        /// <summary>
        /// 包型ID
        /// </summary>
        [Required]
        public int PackageId { get; set; }

        /// <summary>
        /// 方案编号
        /// </summary>
        [Required]
        public string SolutionCode { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string ImagePath { get; set; }
        /// <summary>
        /// 颜色ID
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMColorSolutionDetailEditNewModel> ChildList { get; set; }
    }
}
