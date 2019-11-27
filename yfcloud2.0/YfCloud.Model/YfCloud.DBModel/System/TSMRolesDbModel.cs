using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace YfCloud.DBModel.System
{
    /// <summary>
    /// T_SM_Roles Db Model
    /// </summary>
    [SugarTable("T_SM_Roles")]
    public class TSMRolesDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(ColumnName = "RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [SugarColumn(ColumnName = "ParentId")]
        public int ParentId { get; set; }

        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        [SugarColumn(ColumnName = "PathLogic")]
        public string PathLogic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SeqNumber")]
        public int? SeqNumber { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [SugarColumn(ColumnName = "RoleDesc")]
        public string RoleDesc { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }

    }
}
