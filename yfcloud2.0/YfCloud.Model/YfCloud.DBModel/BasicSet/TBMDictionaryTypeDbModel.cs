///////////////////////////////////////////////////////////////////////////////////////
// File: TBMDictionaryType.cs
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
    /// T_BM_DictionaryType Db Model
    /// </summary>
    [SugarTable("T_BM_DictionaryType")]
    public class TBMDictionaryTypeDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        [SugarColumn(ColumnName = "TypeName")]
        public string TypeName { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool? DeleteFlag { get; set; } = false;

        /// <summary>
        /// 是否是系统生成的类型名称
        /// </summary>
        [SugarColumn(ColumnName = "IsSys")]
        public bool? IsSys { get; set; } = false;
        
    }
}
