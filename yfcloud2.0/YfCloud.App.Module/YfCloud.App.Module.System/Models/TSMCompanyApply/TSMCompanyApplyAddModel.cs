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
    /// T_SM_CompanyApply Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSMCompanyApplyDbModel))]
    public class TSMCompanyApplyAddModel
    {
        /// <summary>
        /// 申请公司ID
        /// </summary>
        public int CompanyId { get; set; }
    }
}
