///////////////////////////////////////////////////////////////////////////////////////
// File: TSMDeptAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Models.TSMDept
{
    /// <summary>
    /// T_SM_Dept Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TSMDeptDbModel))]
    public class TSMDeptEditModel: LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string DeptName { get; set; }
        
        /// <summary>
        /// 父ID
        /// </summary>
        [Required]
        public int ParentId { get; set; }
        
        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        [Range(0,9999)]
        public int SeqNumber { get; set; }
        
        
        /// <summary>
        /// 部门描述
        /// </summary>
        [StringLength(maximumLength:200)]
        public string DeptDesc { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        [Required]
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// 移动输入参数
    /// </summary>
    public class TSMDeptMoveModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 0表示上移 1表示下移
        /// </summary>
        public int Type { get; set; }
    }
}
