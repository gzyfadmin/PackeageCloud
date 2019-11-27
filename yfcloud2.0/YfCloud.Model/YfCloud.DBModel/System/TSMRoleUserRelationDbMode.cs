using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace YfCloud.DBModel.System
{
    /// <summary>
    /// T_SM_RoleUserRelation Db Model
    /// </summary>
    [SugarTable("T_SM_RoleUserRelation")]
    public class TSMRoleUserRelationDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "UserId")]
        public int UserId { get; set; }

    }
}
