using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMCompany
{
    /// <summary>
    /// 个人企业编辑
    /// </summary>
    public class TSMCompanyAllEditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 50)]
        public string CompanyName { get; set; } //?中文名

        /// <summary>
        /// 法人名称
        /// </summary>       
        [StringLength(maximumLength: 25)]
        public string LegalPerson { get; set; }


        /// <summary>
        /// 法人电话
        /// </summary>       
        [RegularExpression(@"(^(\d{3,4}-)?\d{7,8})$|[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11)]
        public string ContactNumber { get; set; }

        /// <summary>
        /// 企业详细信息
        /// </summary>       
        public int? CompanyInfoId { get; set; }

        /// <summary>
        /// 公司类型
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string EnterpriseType { get; set; }

        /// <summary>
        /// 主键自动生成
        /// </summary>
        [Required]
        public int CId { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantShortName { get; set; }//?公司简称

        /// <summary>
        /// 企业英文名称
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantEngName { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantLogo { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLogo { get; set; }



    }
}
