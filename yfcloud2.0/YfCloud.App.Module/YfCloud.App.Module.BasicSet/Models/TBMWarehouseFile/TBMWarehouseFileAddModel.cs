///////////////////////////////////////////////////////////////////////////////////////
// File: TBMWarehouseFileAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.BasicSet.Models.TBMWarehouseFile
{
    /// <summary>
    /// T_BM_WarehouseFile Add Model
    /// </summary>
    [UseAutoMapper(typeof(TBMWarehouseFileDbModel))]
    public class TBMWarehouseFileAddModel : LogModelBase
    {

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength:50)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        [StringLength(maximumLength:150)]
        public string WarehouseAddress { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength:500)]
        public string Remark { get; set; }
                
    }
}
