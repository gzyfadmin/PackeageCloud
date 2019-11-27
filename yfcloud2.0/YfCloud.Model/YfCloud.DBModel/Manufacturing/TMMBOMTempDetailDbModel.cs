///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMTempDetail.cs
// Author: www.cloudyf.com
// Create Time:2019/9/3
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_BOMTempDetail Db Model
    /// </summary>
    [SugarTable("T_MM_BOMTempDetail")]
    public class TMMBOMTempDetailDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "MainId")]
        public int MainId { get; set; }
        
        /// <summary>
        /// 配色项目ID
        /// </summary>
        [SugarColumn(ColumnName = "ItemId")]
        public int ItemId { get; set; }

        /// <summary>
        /// 配色项目ID
        /// </summary>
        [SugarColumn(ColumnName = "ItemName")]
        public string ItemName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [SugarColumn(ColumnName = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 部位名称ID
        /// </summary>
        [SugarColumn(ColumnName = "PartId")]
        public int? PartId { get; set; }

        /// <summary>
        /// 长度值
        /// </summary>
        [SugarColumn(ColumnName = "LengthValue")]
        public decimal? LengthValue { get; set; }

        /// <summary>
        /// 宽度值
        /// </summary>
        [SugarColumn(ColumnName = "WidthValue")]
        public decimal? WidthValue { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        [SugarColumn(ColumnName = "NumValue")]
        public decimal NumValue { get; set; }

        /// <summary>
        /// 封度（宽幅）
        /// </summary>
        [SugarColumn(ColumnName = "WideValue")]
        public decimal? WideValue { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        [SugarColumn(ColumnName = "LossValue")]
        public decimal? LossValue { get; set; }

        /// <summary>
        /// 单用量
        /// </summary>
        [SugarColumn(ColumnName = "SingleValue")]
        public decimal SingleValue { get; set; }
        
        /// <summary>
        /// 单用量计算公式
        /// </summary>
        [SugarColumn(ColumnName = "Formula")]
        public string Formula { get; set; }
        
    }
}
