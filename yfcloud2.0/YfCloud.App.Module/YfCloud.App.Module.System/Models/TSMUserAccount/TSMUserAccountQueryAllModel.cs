using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    /// <summary>
    /// 
    /// </summary>
    public class TSMUserAccountQueryAllModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>       
        public int ID { get; set; }

        /// <summary>
        /// 账户（手机账号或者邮箱账号)
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 手机账号
        /// </summary>       
        public string TelAccount { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>       
        public string EmailAccount { get; set; }


        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DeptId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>       
        public string Passwd { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>       
        public string AccountName { get; set; }

        /// <summary>
        /// 用户详细信息
        /// </summary>       
        public int? UserInfoId { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>       
        public int? CompanyId { get; set; }

        /// <summary>
        /// 账号状态（0无效，1有效，2过期)
        /// </summary>       
        public byte? Status { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>       
        public DateTime? ExpDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>       
        public DateTime? CreateTime { get; set; }


        /// <summary>
        /// 主键自增长(从表)
        /// </summary>       
        public int CId { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

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
        /// 司龄
        /// </summary>
        public int? EntryAge { get; set; }

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
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

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
