using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using YfCloud.Attributes;
using YfCloud.DBModel.System;

namespace YfCloud.App.Module.System.Models.TSMRoles
{
    /// <summary>
    /// 模型编辑权限
    /// </summary>
    [UseAutoMapper(typeof(TSMRoleUserRelationDbModel))]
    public class TSMRoleUserRelationEditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        [Range(1,int.MaxValue)]
        public int RoleId { get; set; }

        /// <summary>
        /// 员工账号ID
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
