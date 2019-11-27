using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.SystemLogin.Models.Verification
{
    /// <summary>
    /// 手机验证码类
    /// </summary>
    public class VerificationEditModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11,MinimumLength =11)]

        public string Mobile { get; set; }
    }
}
