namespace YfCloud.Models
{
    /// <summary>
    /// 排序规则
    /// </summary>
    public class OrderByCondition
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 排序规则，ASC 或 DESC
        /// </summary>
        public string Condition { get; set; }
    }
}
