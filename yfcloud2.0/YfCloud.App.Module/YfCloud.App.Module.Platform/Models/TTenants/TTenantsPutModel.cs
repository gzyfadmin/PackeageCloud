using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Platform.Models.TTenants
{
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class TTenantsPutModel
    {
        /// <summary>
        /// 主键自动生成
        /// </summary>
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantShortName { get; set; }

        /// <summary>
        /// 企业英文名称
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantEngName { get; set; }

        /// <summary>
        /// 是否试用
        /// </summary>
        public bool IsTrial { get; set; }

        /// <summary>
        /// 试用有效期
        /// </summary>
        public DateTime TrialDate { get; set; }

        /// <summary>
        /// 租户状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 模板权限
        /// </summary>
        public int? TemplateId { get; set; }

        /// <summary>
        /// 租户有效期
        /// </summary>
        public DateTime ValidityPeriod { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public int? Area { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        public int? Industry { get; set; }

        /// <summary>
        /// 租户规模
        /// </summary>
        public int? TenantScale { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? RegisteredCapital { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [StringLength(maximumLength: 20)]
        [RegularExpression(@"^(0[0-9]{2,3}/-)?([2-9][0-9]{6,7})+(/-[0-9]{1,4})?$")]
        public string FixedTele { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [StringLength(maximumLength: 500)]
        public string Address { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantLogo { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public string BusinessLogo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateId { get; set; }
    }
}
