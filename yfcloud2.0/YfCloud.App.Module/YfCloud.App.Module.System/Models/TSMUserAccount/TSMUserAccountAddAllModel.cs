using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.System.Models.TSMUserInfo;

namespace YfCloud.App.Module.System.Models.TSMUserAccount
{
    /// <summary>
    /// 主从表添加模型
    /// </summary>
    public class TSMUserAccountAddAllModel
    {

        /// <summary>
        /// 手机账号
        /// </summary>       
        [StringLength(maximumLength: 11)]
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        public string TelAccount { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>       
        [StringLength(maximumLength: 30)]
        [RegularExpression(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$")]
        public string EmailAccount { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 16)]
        public string Passwd { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DeptId { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>       
        [Required]
        [StringLength(maximumLength: 20)]
        public string AccountName { get; set; }

        /// <summary>
        /// 账号状态（0无效，1有效，2过期)
        /// </summary>       
        public byte? Status { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>       
        public DateTime? ExpDate { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [StringLength(maximumLength: 20)]
        public string JobNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(maximumLength: 20)]
        public string RealName { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [StringLength(maximumLength: 20)]
        public string FixedNumber { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string Address { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        [StringLength(maximumLength: 100)]
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
        [StringLength(maximumLength: 50)]
        public string WorkNo { get; set; }

        /// <summary>
        /// 1 离职 2在职
        /// </summary>
        public byte? WorkStatus { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [StringLength(maximumLength: 50)]
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
        [StringLength(maximumLength: 50)]
        public string Education { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string Nation { get; set; }

        /// <summary>
        /// 婚姻状态 1未婚、2已离、3离异
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string Marriage { get; set; }

        /// <summary>
        /// 户口类型
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string RegisteredType { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string HomeAddress { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        [RegularExpression(@"^[1]+[3,5,7,8,9]+\d{9}")]
        [StringLength(maximumLength: 50)]
        public string EmergencyContactaPhone { get; set; }

        /// <summary>
        /// 与紧急联系人的关系
        /// </summary>
        [StringLength(maximumLength: 50)]
        public string EmergencyRealtionShip { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string Remarks { get; set; }
    }
}
