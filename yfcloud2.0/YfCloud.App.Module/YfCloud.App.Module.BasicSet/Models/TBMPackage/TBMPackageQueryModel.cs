///////////////////////////////////////////////////////////////////////////////////////
// File: TBMPackage.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Models.TBMPackage
{
    /// <summary>
    /// TBMPackage Query Model
    /// </summary>
    public class TBMPackageQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 包型编码
        /// </summary>
        public string DicCode { get; set; }
        
        /// <summary>
        /// 包型名称
        /// </summary>
        public string DicValue { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        
        /// <summary>
        /// 更新人ID
        /// </summary>
        public int? UpdateId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public string UpdateName { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgPath { get; set; }

    }
}
