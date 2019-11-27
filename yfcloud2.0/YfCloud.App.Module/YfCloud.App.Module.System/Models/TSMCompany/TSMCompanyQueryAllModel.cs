using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMCompany
{
    public class TSMCompanyQueryAllModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        public int ID { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>       
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人名称
        /// </summary>       
        public string LegalPerson { get; set; }


        /// <summary>
        /// 法人电话
        /// </summary>       
        public string ContactNumber { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>       
        public string ContactEmail { get; set; }

        /// <summary>
        /// 企业详细信息
        /// </summary>       
        public int? CompanyInfoId { get; set; }



        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 主键自动生成
        /// </summary>
        public int CId { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        public string TenantShortName { get; set; }

        /// <summary>
        /// 企业英文名称
        /// </summary>
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
        public decimal? RegisteredCapital { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string FixedTele { get; set; }



        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public string TenantLogo { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLogo { get; set; }
    }
}
