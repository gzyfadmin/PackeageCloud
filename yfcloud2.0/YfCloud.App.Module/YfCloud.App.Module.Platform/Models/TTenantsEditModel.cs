using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Utilities.Valid;

namespace YfCloud.App.Module.Platform.Models
{
    /// <summary>
    /// 企业修改模型
    /// </summary>
    [UseAutoMapper(typeof(TTenantsModel))]
    public class TTenantsEditModel
    {
        /// <summary>
        /// 主键自动生成
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 6)]
        [NORegularExpression("[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]", "[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]不能匹配这个正则表达式")]
        public string TenantShortName { get; set; }

        /// <summary>
        /// 企业英文名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 100)]
        [NORegularExpression("[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]", "[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]不能匹配这个正则表达式")]
        public string TenantEngName { get; set; }


        /// <summary>
        /// 所属区域
        /// </summary>
        public int? Area { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        public int? Industry { get; set; }


        /// <summary>
        /// 租户规模
        /// </summary>
        public int? TenantScale { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? RegisteredCapital { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [StringLength(maximumLength: 20)]
        public string FixedTele { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [StringLength(maximumLength: 30)]
        public string Email { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [StringLength(maximumLength: 200)]
        public string Address { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [StringLength(maximumLength: 100)]
        public string TenantLogo { get; set; }

    }
}
