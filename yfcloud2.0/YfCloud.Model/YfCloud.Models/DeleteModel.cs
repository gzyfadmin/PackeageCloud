
using System.ComponentModel.DataAnnotations;

namespace YfCloud.Models
{
    /// <summary>
    /// 删除模型
    /// </summary>
    public class DeleteModel : LogModelBase
    {
        /// <summary>
        /// 被删除数据的ID
        /// </summary>
        [Required]
        public override int ID { get; set; }

        /// <summary>
        /// 删除描述
        /// </summary>
        [Required]
        public override string _LogName { get; set; }
    }
}
