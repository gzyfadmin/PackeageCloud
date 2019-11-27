using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew
{
    public class TMMBOMMainQueryNewModel
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
        /// 纸格编号
        /// </summary>
        public string PagerCode { get; set; }

        /// <summary>
        /// 出格师傅ID
        /// </summary>
        public int? Maker { get; set; }

        /// <summary>
        /// 正面图片路径
        /// </summary>
        public string FrontImgPath { get; set; }

        /// <summary>
        /// 侧面图片路径
        /// </summary>
        public string SideImgPath { get; set; }

        /// <summary>
        /// 背面图片路径
        /// </summary>
        public string BackImgPath { get; set; }


        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMBOMDetailQueryNewModel> ChildList { get; set; }
    }
}
