using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Platform.Models.TRolePermissions;

namespace YfCloud.App.Module.Platform.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 角色信息
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色描述信息
        /// </summary>
        public string Introduction { get; set; } = "";
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; } = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif";
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 用户权限
        /// </summary>
        public List<MenuViewModel> Permissions { get; set; }
    }
}
