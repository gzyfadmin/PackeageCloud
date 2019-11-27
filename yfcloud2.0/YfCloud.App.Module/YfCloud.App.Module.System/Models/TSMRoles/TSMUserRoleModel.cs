using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.System.Models.TSMUserAccount;

namespace YfCloud.App.Module.System.Models.TSMRoles
{
    /// <summary>
    /// 用户权限视图
    /// </summary>
    public class TSMUserRoleModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public TSMUserAccountQueryAllModel UserAccount { get; set; }
    }
}
