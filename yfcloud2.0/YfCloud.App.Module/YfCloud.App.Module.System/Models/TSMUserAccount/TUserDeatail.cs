using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    public class TUserDeatail
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 真实姓名
        public string RealName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string FixedNumber { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadPicPath { get; set; }

        /// <summary>
        /// 1 男 0女
        /// </summary>
        public byte? Sex { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 1 离职 2在职
        /// </summary>
        public byte? WorkStatus { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 婚姻状态 1未婚、2已离、3离异
        /// </summary>
        public string Marriage { get; set; }

        /// <summary>
        /// 户口类型
        /// </summary>
        public string RegisteredType { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string EmergencyContactaPhone { get; set; }

        /// <summary>
        /// 与紧急联系人的关系
        /// </summary>
        public string EmergencyRealtionShip { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
