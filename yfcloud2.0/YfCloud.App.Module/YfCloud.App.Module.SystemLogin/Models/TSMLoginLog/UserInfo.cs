using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.SystemLogin.Models.TSMLoginLog
{
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
        public string Introduction { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// 用户权限
        /// </summary>
        public List<MenuViewModel> Permissions { get; set; }

        /// <summary>
        /// 当前用的公司
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司Log
        /// </summary>
        public string TenantLogo { get; set; }

        /// <summary>
        /// 公司英文名称
        /// </summary>
        public string TenantEngName { get; set; }
    }
}
