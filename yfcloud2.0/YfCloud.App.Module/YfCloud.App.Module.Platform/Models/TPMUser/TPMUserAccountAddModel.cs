using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Platform.Models.TRolePermissions;

namespace YfCloud.App.Module.Platform.Models.TPMUser
{
    /// <summary>
    /// 登陆实体
    /// </summary>
    public class TPMUserAccountAddModel
    {
        /// <summary>
        /// 登陆账户
        /// </summary>       
        [StringLength(maximumLength: 12)]
        public string LoginName { get; set; }

        

        /// <summary>
        /// 账号密码
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 100)]
        public string Passwd { get; set; }
    }

    /// <summary>
    /// 登陆返回实体
    /// </summary>
    public class LoginResult
    {

        /// <summary>
        /// 是否登陆成功
        /// </summary>
        public bool IsSuccess { get; set; }

        ///// <summary>
        ///// 菜单按钮权限
        ///// </summary>
        //public List<MenuViewModel> Permissions { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Token { get; set; }
    }
}
