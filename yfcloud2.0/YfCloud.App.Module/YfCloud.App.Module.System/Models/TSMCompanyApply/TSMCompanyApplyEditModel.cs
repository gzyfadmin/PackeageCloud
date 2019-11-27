///////////////////////////////////////////////////////////////////////////////////////
// File: TSMCompanyApplyAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Models.TSMCompanyApply
{
    /// <summary>
    /// T_SM_CompanyApply Edit Model
    /// </summary>
    [UseAutoMapper(typeof(TSMCompanyApplyDbModel))]
    public class TSMCompanyApplyEditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int Id { get; set; }        
        
        /// <summary>
        /// 申请状态（0未审核，1已审核，2已拒绝）
        /// </summary>       
        [Required]
        public byte ApplyStatus { get; set; }        
        
    }
}
