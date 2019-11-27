using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// API请求对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestObject<T>
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
        /// POST, PUT, DELETE请求参数
        /// </summary>
        public T PostData { get; set; }

        /// <summary>
        /// PUT请求修改前的参数
        /// </summary>
        public T PostDataEdit { get; set; }

        /// <summary>
        /// POST, PUT, DELETE请求参数，批量执行优先级高于单条执行
        /// </summary>
        public List<T> PostDataList { get; set; }

        /// <summary>
        /// PUT请求修改前的参数数组
        /// </summary>
        public List<T> PostDataEditList { get; set; }

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
