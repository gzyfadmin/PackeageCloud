namespace YfCloud.Models
{
    /// <summary>
    /// 更新条件
    /// </summary>
    public class UpdateCondition
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 需要更新的值
        /// </summary>
        public string Value { get; set; }
    }
}
