///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMBOMMain
{
    /// <summary>
    /// TMMBOMMain Query Model
    /// </summary>
    public class TMMBOMMainQueryModel
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
        public List<TMMBOMDetailQueryModel> ChildList { get; set; }

        ///// <summary>
        ///// 配色项目
        ///// </summary>
        //public List<ItemModel> Items { get; set; }
    }
}
