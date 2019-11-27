///////////////////////////////////////////////////////////////////////////////////////
// File: TMMBOMTempMain.cs
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
    /// T_MM_BOMTempMain Db Model
    /// </summary>
    [SugarTable("T_MM_BOMTempMain")]
    public class TMMBOMTempMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 包型ID
        /// </summary>
        [SugarColumn(ColumnName = "PackageId")]
        public int PackageId { get; set; }
        
        /// <summary>
        /// 纸格编号
        /// </summary>
        [SugarColumn(ColumnName = "PagerCode")]
        public string PagerCode { get; set; }
        
        /// <summary>
        /// 出格师傅ID
        /// </summary>
        [SugarColumn(ColumnName = "Maker")]
        public int? Maker { get; set; }
        
        /// <summary>
        /// 正面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "FrontImgPath")]
        public string FrontImgPath { get; set; }
        
        /// <summary>
        /// 侧面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "SideImgPath")]
        public string SideImgPath { get; set; }
        
        /// <summary>
        /// 背面图片路径
        /// </summary>
        [SugarColumn(ColumnName = "BackImgPath")]
        public string BackImgPath { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [SugarColumn(ColumnName = "TempName")]
        public string TempName { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }


    }
}
