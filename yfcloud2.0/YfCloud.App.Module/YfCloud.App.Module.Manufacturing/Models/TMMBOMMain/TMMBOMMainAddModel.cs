///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMain
{
    /// <summary>
    /// T_MM_BOMMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMBOMMainDbModel))]
    public class TMMBOMMainAddModel : LogModelBase
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
        [StringLength(maximumLength:30)]
        public string PagerCode { get; set; }
        
        /// <summary>
        /// 出格师傅ID
        /// </summary>
        public int? Maker { get; set; }
        
        /// <summary>
        /// 正面图片路径
        /// </summary>
        [StringLength(maximumLength:200)]
        public string FrontImgPath { get; set; }
        
        /// <summary>
        /// 侧面图片路径
        /// </summary>
        [StringLength(maximumLength:200)]
        public string SideImgPath { get; set; }
        
        /// <summary>
        /// 背面图片路径
        /// </summary>
        [StringLength(maximumLength:200)]
        public string BackImgPath { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMBOMDetailAddModel> ChildList { get; set; }
    }
}
