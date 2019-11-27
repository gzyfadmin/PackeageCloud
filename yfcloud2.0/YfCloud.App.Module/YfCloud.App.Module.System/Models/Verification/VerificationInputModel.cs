using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.Verification
{
    /// <summary>
    /// 手机验证码类
    /// </summary>
    public class VerificationInputModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11)]

        public string Mobile { get; set; }
    }

    /// <summary>
    /// 邮箱验证类
    /// </summary>
    public class VerificationInputEmaiModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string Email { get; set; }
    }

    ///// <summary>
    ///// 手机验证码输出类
    ///// </summary>
    //public class VerificationOutputModel
    //{
    //    /// <summary>
    //    /// 短信验证码
    //    /// </summary>
    //    public string Code { get; set; }
    //}

    /// <summary>
    /// 手机验证码输出类
    /// </summary>
    public class VerificationModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 11)]
        public string Mobile { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        [RegularExpression(@"^[0-9]*$")]
        [StringLength(maximumLength: 6)]
        public string Code { get; set; }
    }
}
