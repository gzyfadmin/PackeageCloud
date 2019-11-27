
using System.ComponentModel;

namespace YfCloud.Models
{
    /// <summary>
    /// 查询条件枚举
    /// </summary>
    public enum ConditionEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 小于 
        /// </summary>
        LessThan = 1,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 2,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 3,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual = 4,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 5,
        /// <summary>
        /// Like全匹配, like '%abc%'
        /// </summary>
        Like = 6,
        /// <summary>
        /// Like前匹配, like 'abc%'
        /// </summary>
        LikeLeft = 7,
        /// <summary>
        /// Like后匹配, like '%abc'
        /// </summary>
        LikeRight = 8,
        /// <summary>
        /// 在集合范围内
        /// </summary>
        In = 9,
        /// <summary>
        /// 在两个值范围之间
        /// </summary>
        Between = 10,
        /// <summary>
        /// 空值
        /// </summary>
        IsNull = 11,
        /// <summary>
        /// 非空值
        /// </summary>
        IsNotNull = 12,
        /// <summary>
        /// 不在集合范围内
        /// </summary>
        NotIn=13,
        /// <summary>
        /// 不等于
        /// </summary>
        NoEqual = 14,
        /// <summary>
        /// Is Null
        /// </summary>
        IsNullOrEmpty = 15,
        /// <summary>
        /// Is Not
        /// </summary>
        IsNot = 16,
        /// <summary>
        /// Not Like
        /// </summary>
        NoLike = 17
    }
}
