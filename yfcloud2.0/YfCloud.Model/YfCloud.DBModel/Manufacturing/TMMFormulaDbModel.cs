///////////////////////////////////////////////////////////////////////////////////////
// File: TMMFormula.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_MM_Formula Db Model
    /// </summary>
    [SugarTable("T_MM_Formula")]
    public class TMMFormulaDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 公式名称
        /// </summary>
        [SugarColumn(ColumnName = "FormulaName")]
        public string FormulaName { get; set; }
        
        /// <summary>
        /// 公式
        /// </summary>
        [SugarColumn(ColumnName = "Formula")]
        public string Formula { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识 0未删除 1删除
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; } = false;
        
        /// <summary>
        /// 前端显示公式
        /// </summary>
        [SugarColumn(ColumnName = "FrontFormula")]
        public string FrontFormula { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        [SugarColumn(ColumnName = "UpdateId")]
        public int? UpdateId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnName = "UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName ="Remark")]
        public string Remark { get; set; }

    }
}
