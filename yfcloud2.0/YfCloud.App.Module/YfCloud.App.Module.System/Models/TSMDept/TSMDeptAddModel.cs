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

namespace YfCloud.App.Module.System.Models.TSMDept
{
    /// <summary>
    /// T_SM_Dept Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMDeptDbModel))]
    public class TSMDeptAddModel
    {


        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string DeptName { get; set; }
        
        /// <summary>
        /// 父ID 顶级传-1
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        [Range(0, 9999)]
        public int SeqNumber { get; set; }
        
        
        /// <summary>
        /// 部门描述
        /// </summary>
        [StringLength(maximumLength:200)]
        public string DeptDesc { get; set; }
    }
}
