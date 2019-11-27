using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace YfCloud.DBModel.System
{
    /// <summary>
    /// T_SM_RolePermissions Db Model
    /// </summary>
    [SugarTable("T_SM_RolePermissions")]
    public class TSMRolePermissionsDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(ColumnName = "MenuId")]
        public int MenuId { get; set; }

        /// <summary>
        /// 按钮权限
        /// </summary>
        [SugarColumn(ColumnName = "ButtonIds")]
        public string ButtonIds { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }

    }
}
