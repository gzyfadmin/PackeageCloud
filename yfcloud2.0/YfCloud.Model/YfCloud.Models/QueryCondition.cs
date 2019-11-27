
namespace YfCloud.Models
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 查询内容 1. 使用In时用英文逗号(",")分隔多个值.  2. 使用Between时用英文逗号(",")分隔范围值.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 查询条件
        /// 0 等于 
        /// 1 小于 
        /// 2 小于等于 
        /// 3 大于
        /// 4 大于等于
        /// 5 不等于
        /// 6 Like全匹配, like '%abc%'
        /// 7 Like前匹配, like 'abc%'
        /// 8 Like后匹配, like '%abc'
        /// 9 在集合范围内 in
        /// 10 在两个值范围之间 Between 1 and 10
        /// 11 空值 'column is null'
        /// 12 非空值 'column is not null'
        /// 13 NotIn 不在集合范围内
        /// 14 NoEqual 不等于
        /// 15 Is Null
        /// 16 Is Not
        /// 17 Not Like
        /// </summary>
        public ConditionEnum Condition { get; set; }        
    }
}
