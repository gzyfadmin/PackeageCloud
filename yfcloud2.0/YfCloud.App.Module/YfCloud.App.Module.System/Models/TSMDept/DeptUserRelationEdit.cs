using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Attributes;
using YfCloud.DBModel.System;

namespace YfCloud.App.Module.System.Models.TSMDept
{
    /// <summary>
    /// 分配人员接口
    /// </summary>
    [UseAutoMapper(typeof(TSMDeptUserRelationDbModel))]
    public class DeptUserRelationEdit
    {
        /// <summary>
        /// 传0为新增
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int DeptId { get; set; }

        /// <summary>
        /// 用户账户ID
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserAccountId { get; set; }
    }
}
