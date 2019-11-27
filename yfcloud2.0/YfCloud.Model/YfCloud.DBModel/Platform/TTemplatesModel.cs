///////////////////////////////////////////////////////////////////////////////////////
// File: TTemplates.cs
// Author: www.cloudyf.com
// Create Time:2019/6/25
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_Templates Model
    /// </summary>
    [SugarTable("T_PM_Templates")]
    public class TTemplatesModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [SugarColumn(ColumnName = "TemplateName")]
        [Required]
        [StringLength(maximumLength:12)]
        public string TemplateName { get; set; }
        
        /// <summary>
        /// 模板说明
        /// </summary>
        [SugarColumn(ColumnName = "TemplateDesc")]
        [Required]
        [StringLength(maximumLength:50)]
        public string TemplateDesc { get; set; }
        
        /// <summary>
        /// 模板状态
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        [Required]
        public bool Status { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        [Required]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        [Required]
        public int CreateId { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool isDefault { get; set; }

        /// <summary>
        /// 版本类型 0专业版 1企业版 2旗舰版
        /// </summary>
        public byte VersionType { get; set; }
    }
}
