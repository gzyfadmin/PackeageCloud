using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    /// <summary>
    /// 账户验证类
    /// </summary>
    public class TSMUserAccountInputModel
    {
        /// <summary>
        /// 关键字(邮箱或者手机号)
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}|^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string KeyWords { get; set; }
    }

    ///// <summary>
    ///// 账户验证输出类
    ///// </summary>

    //public class TSMUserAccountOutPutModel
    //{
    //    /// <summary>
    //    /// true 存在，false不粗壮乃
    //    /// </summary>
    //    public bool IsExists { get; set; }
    //}
}
