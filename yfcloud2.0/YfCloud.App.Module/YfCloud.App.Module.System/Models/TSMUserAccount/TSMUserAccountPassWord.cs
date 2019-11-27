using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    public class TSMUserAccountPassWord
    {
        /// <summary>
        /// 账号密码
        /// </summary>       
        public string Passwd { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPasswd { get; set; }
    }

    /// <summary>
    /// 手机
    /// </summary>
    public class TSMUserAccountMobile
    {     
        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength: 11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string Mobile { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        [RegularExpression(@"^[0-9]*$")]
        [StringLength(maximumLength: 6)]
        public string Code { get; set; }
    }

    /// <summary>
    /// 邮箱
    /// </summary>
    public class TSMUserAccountEmail
    {
        /// <summary>
        /// 邮箱账号
        /// </summary>       
        [StringLength(maximumLength: 30)]
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string Email { get; set; }
    }
}