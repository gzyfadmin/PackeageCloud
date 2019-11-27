///////////////////////////////////////////////////////////////////////////////////////
// File: TSMUserInfo.cs
// Author: www.cloudyf.com
// Create Time:2019/7/11
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_SM_UserInfo Db Model
    /// </summary>
    [SugarTable("T_SM_UserInfo")]
    public class TSMUserInfoDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(ColumnName = "JobNumber")]
        public string JobNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [SugarColumn(ColumnName = "RealName")]
        public string RealName { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [SugarColumn(ColumnName = "FixedNumber")]
        public string FixedNumber { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [SugarColumn(ColumnName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        [SugarColumn(ColumnName = "HeadPicPath")]
        public string HeadPicPath { get; set; }

        /// <summary>
        /// 1 男 0女
        /// </summary>
        [SugarColumn(ColumnName = "Sex")]
        public byte? Sex { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [SugarColumn(ColumnName = "EntryTime")]
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(ColumnName = "WorkNo")]
        public string WorkNo { get; set; }

        /// <summary>
        /// 1 离职 2在职
        /// </summary>
        [SugarColumn(ColumnName = "WorkStatus")]
        public byte? WorkStatus { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [SugarColumn(ColumnName = "IDCard")]
        public string IDCard { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        [SugarColumn(ColumnName = "Birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [SugarColumn(ColumnName = "Education")]
        public string Education { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [SugarColumn(ColumnName = "Nation")]
        public string Nation { get; set; }

        /// <summary>
        /// 婚姻状态 1未婚、2已离、3离异
        /// </summary>
        [SugarColumn(ColumnName = "Marriage")]
        public string Marriage { get; set; }

        /// <summary>
        /// 户口类型
        /// </summary>
        [SugarColumn(ColumnName = "RegisteredType")]
        public string RegisteredType { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        [SugarColumn(ColumnName = "HomeAddress")]
        public string HomeAddress { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        [SugarColumn(ColumnName = "EmergencyContact")]
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        [SugarColumn(ColumnName = "EmergencyContactaPhone")]
        public string EmergencyContactaPhone { get; set; }

        /// <summary>
        /// 与紧急联系人的关系
        /// </summary>
        [SugarColumn(ColumnName = "EmergencyRealtionShip")]
        public string EmergencyRealtionShip { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remarks")]
        public string Remarks { get; set; }

    }
}
