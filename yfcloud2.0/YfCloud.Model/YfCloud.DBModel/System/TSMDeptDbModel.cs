///////////////////////////////////////////////////////////////////////////////////////
// File: TSMDept.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_SM_Dept Db Model
    /// </summary>
    [SugarTable("T_SM_Dept")]
    public class TSMDeptDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int Id { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        [SugarColumn(ColumnName = "DeptName")]
        public string DeptName { get; set; }
        
        /// <summary>
        /// 父ID
        /// </summary>
        [SugarColumn(ColumnName = "ParentId")]
        public int ParentId { get; set; }
        
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SeqNumber")]
        public int SeqNumber { get; set; }
        
        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        [SugarColumn(ColumnName = "PathLogic")]
        public string PathLogic { get; set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        [SugarColumn(ColumnName = "DeptDesc")]
        public string DeptDesc { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }
        
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
        
    }
}
