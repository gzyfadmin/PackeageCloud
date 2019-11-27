///////////////////////////////////////////////////////////////////////////////////////
// File: TMMFormula.cs
// Author: www.cloudyf.com
// Create Time:2019/9/2
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Manufacturing.Models.TMMFormula
{
    /// <summary>
    /// TMMFormula Query Model
    /// </summary>
    public class TMMFormulaQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 公式名称
        /// </summary>
        public string FormulaName { get; set; }
        
        /// <summary>
        /// 公式
        /// </summary>
        public string Formula { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识 0未删除 1删除
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 前端显示公式
        /// </summary>
        public string FrontFormula { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        public int? UpdateId { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        public string UpdateName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
