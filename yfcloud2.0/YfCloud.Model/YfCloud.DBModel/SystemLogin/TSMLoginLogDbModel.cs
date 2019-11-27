///////////////////////////////////////////////////////////////////////////////////////
// File: TSMLoginLog.cs
// Author: www.cloudyf.com
// Create Time:2019/7/8
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_SM_LoginLog Db Model
    /// </summary>
    [SugarTable("T_SM_LoginLog")]
    public class TSMLoginLogDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int Id { get; set; }
        
        /// <summary>
        /// 账号ID
        /// </summary>
        [SugarColumn(ColumnName = "AccountId")]
        public int AccountId { get; set; }
        
        /// <summary>
        /// 登录登出时间
        /// </summary>
        [SugarColumn(ColumnName = "LogTime")]
        public DateTime? LogTime { get; set; }

        /// <summary>
        /// 登录类别 0登录 1登出
        /// </summary>
        public int LogType { get; set; } = 0;

    }
}
