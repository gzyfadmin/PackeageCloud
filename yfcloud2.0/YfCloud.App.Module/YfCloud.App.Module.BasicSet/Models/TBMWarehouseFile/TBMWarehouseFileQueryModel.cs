///////////////////////////////////////////////////////////////////////////////////////
// File: TBMWarehouseFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.BasicSet.Models.TBMWarehouseFile
{
    /// <summary>
    /// TBMWarehouseFile Query Model
    /// </summary>
    public class TBMWarehouseFileQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        
        /// <summary>
        /// 仓库地址
        /// </summary>
        public string WarehouseAddress { get; set; }
        
        /// <summary>
        /// 负责人
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? Status { get; set; } = true;
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool? DeleteFlag { get; set; }
        
    }
}
