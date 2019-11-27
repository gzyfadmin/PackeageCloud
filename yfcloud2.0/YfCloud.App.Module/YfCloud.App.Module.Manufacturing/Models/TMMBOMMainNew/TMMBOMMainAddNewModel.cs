using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew
{
    [UseAutoMapper(typeof(TMMBOMMainDbModel))]
    public class TMMBOMMainAddNewModel
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
        /// 纸格编号
        /// </summary>
        [StringLength(maximumLength: 30)]
        public string PagerCode { get; set; }

        /// <summary>
        /// 出格师傅ID
        /// </summary>
        public int? Maker { get; set; }

        /// <summary>
        /// 正面图片路径
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string FrontImgPath { get; set; }

        /// <summary>
        /// 侧面图片路径
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string SideImgPath { get; set; }

        /// <summary>
        /// 背面图片路径
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string BackImgPath { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMBOMDetailAddNewModel> ChildList { get; set; }
    }
}
