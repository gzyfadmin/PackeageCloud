///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMNTempMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/9/4
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMNTempMain
{
    /// <summary>
    /// T_MM_BOMNTempMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TMMBOMNTempMainDbModel))]
    public class TMMBOMNTempMainAddModel : LogModelBase
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
        /// 模板名称
        /// </summary>
        [StringLength(maximumLength:20)]
        public string TempName { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? CompanyId { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TMMBOMNTempDetailAddModel> ChildList { get; set; }
    }
}
