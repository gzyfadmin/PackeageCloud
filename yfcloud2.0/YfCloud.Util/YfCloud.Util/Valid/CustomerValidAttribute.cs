using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace YfCloud.Utilities.Valid
{
    /// <summary>
    /// 不能匹配到某个正则表达式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NORegularExpressionAttribute : RegularExpressionAttribute
    {

        /// <summary>
        /// 正则表达式
        /// </summary>
        private string patter { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="patter">正则表达式</param>
        /// <param name="errorMessage">错误信息</param>
        public NORegularExpressionAttribute(string patter,string errorMessage) :base(errorMessage)
        {
            this.patter = patter;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            Regex regex = new Regex(patter);
            return !regex.IsMatch(value.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return name + ":" + base.ErrorMessage;
        }
    }
}
