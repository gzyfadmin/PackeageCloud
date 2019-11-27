///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionary.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_BM_Dictionary Db Model
    /// </summary>
    [SugarTable("T_BM_Dictionary")]
    public class TBMDictionaryDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 分类ID
        /// </summary>
        [SugarColumn(ColumnName = "TypeId")]
        public int TypeId { get; set; }
        
        /// <summary>
        /// 字典编号
        /// </summary>
        [SugarColumn(ColumnName = "DicCode")]
        public string DicCode { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        [SugarColumn(ColumnName = "DicValue")]
        public string DicValue { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int CreateId { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnName = "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        
        /// <summary>
        /// 更新人
        /// </summary>
        [SugarColumn(ColumnName = "UpdateId")]
        public int? UpdateId { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
        
    }
}
