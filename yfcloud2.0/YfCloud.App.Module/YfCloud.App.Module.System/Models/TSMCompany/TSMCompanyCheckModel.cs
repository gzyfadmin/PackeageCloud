using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMCompany
{
    public class TSMCompanyCheckModel
    {
        /// <summary>
        /// 企业名称
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 50)]
        public string CompanyName { get; set; }
    }
}
