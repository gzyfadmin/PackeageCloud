///////////////////////////////////////////////////////////////////////////////////////
// File: TBMWarehouseFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_BM_WarehouseFile Db Model
    /// </summary>
    [SugarTable("T_BM_WarehouseFile")]
    public class TBMWarehouseFileDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 仓库名称
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseName")]
        public string WarehouseName { get; set; }
        
        /// <summary>
        /// 仓库地址
        /// </summary>
        [SugarColumn(ColumnName = "WarehouseAddress")]
        public string WarehouseAddress { get; set; }
        
        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(ColumnName = "PrincipalId")]
        public int? PrincipalId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public bool? Status { get; set; } = true;
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool? DeleteFlag { get; set; } = false;
        
    }
}
