///////////////////////////////////////////////////////////////////////////////////////
// File: TTenants.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Utilities.Valid;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_Tenants Model
    /// </summary>
    [SugarTable("T_PM_Tenants")]
    public class TTenantsModel
    {
        /// <summary>
        /// 主键自动生成
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int ID { get; set; }



        /// <summary>
        /// 企业简称
        /// </summary>
        [SugarColumn(ColumnName = "TenantShortName")]
        [StringLength(maximumLength: 100)]
        public string TenantShortName { get; set; }

        /// <summary>
        /// 企业英文名称
        /// </summary>
        [SugarColumn(ColumnName = "TenantEngName")]
        [StringLength(maximumLength: 100)]
        public string TenantEngName { get; set; }

        /// <summary>
        /// 是否试用
        /// </summary>
        [SugarColumn(ColumnName = "IsTrial")]
        public bool IsTrial { get; set; }

        /// <summary>
        /// 试用有效期
        /// </summary>
        [SugarColumn(ColumnName = "TrialDate")]
        public DateTime TrialDate { get; set; }

        /// <summary>
        /// 租户状态
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public bool Status { get; set; }

        /// <summary>
        /// 模板权限
        /// </summary>
        [SugarColumn(ColumnName = "TemplateId")]
        public int? TemplateId { get; set; }

        /// <summary>
        /// 租户有效期
        /// </summary>
        [SugarColumn(ColumnName = "ValidityPeriod")]
        public DateTime ValidityPeriod { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        [SugarColumn(ColumnName = "Area")]
        public int? Area { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        [SugarColumn(ColumnName = "Industry")]
        public int? Industry { get; set; }

        /// <summary>
        /// 租户规模
        /// </summary>
        [SugarColumn(ColumnName = "TenantScale")]
        public int? TenantScale { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        [SugarColumn(ColumnName = "RegisteredCapital")]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? RegisteredCapital { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [SugarColumn(ColumnName = "MainBusiness")]
        [StringLength(maximumLength: 200)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [SugarColumn(ColumnName = "FixedTele")]
        [StringLength(maximumLength: 20)]
        [RegularExpression(@"^(0[0-9]{2,3}/-)?([2-9][0-9]{6,7})+(/-[0-9]{1,4})?$")]
        public string FixedTele { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [SugarColumn(ColumnName = "Address")]
        [StringLength(maximumLength: 500)]
        public string Address { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [SugarColumn(ColumnName = "TenantLogo")]
        [StringLength(maximumLength: 100)]
        public string TenantLogo { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [SugarColumn(ColumnName = "BusinessLogo")]
        public string BusinessLogo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int CreateId { get; set; }
    }
}
