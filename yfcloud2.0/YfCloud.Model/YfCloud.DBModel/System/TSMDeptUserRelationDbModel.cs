using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace YfCloud.DBModel.System
{
    /// <summary>
    /// T_SM_DeptUserRelation Db Model
    /// </summary>
    [SugarTable("T_SM_DeptUserRelation")]
    public class TSMDeptUserRelationDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int? Id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [SugarColumn(ColumnName = "DeptId")]
        public int DeptId { get; set; }

        /// <summary>
        /// 员工账号ID
        /// </summary>
        [SugarColumn(ColumnName = "UserAccountId")]
        public int UserAccountId { get; set; }

    }
}
