using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// API GET请求对象
    /// </summary>
    public class RequestGet
    {
        /// <summary>
        /// Get请求参数,是否启用分页查询
        /// </summary>
        public bool IsPaging { get; set; }

        /// <summary>
        /// Get请求参数,每页显示条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Get请求参数,页码，索引从1开始
        /// </summary>
        public int PageIndex { get; set; }        

        /// <summary>
        /// Get请求参数,查询条件
        /// </summary>
        public List<QueryCondition> QueryConditions { get; set; }

        /// <summary>
        /// Get请求参数,排序条件
        /// </summary>
        public List<OrderByCondition> OrderByConditions { get; set; }
    }
}
