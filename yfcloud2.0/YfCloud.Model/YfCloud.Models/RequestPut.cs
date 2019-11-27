using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// API PUT请求对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestPut<T>
    {

        /// <summary>
        /// PUT 请求参数,用户修改
        /// </summary>
        public T PostData { get; set; }

        /// <summary>
        /// PUT请求修改前的参数
        /// </summary>
        public T PostDataEdit { get; set; }

        /// <summary>
        /// PUT请求参数，用户批量修改
        /// </summary>
        public List<T> PostDataList { get; set; }

        /// <summary>
        /// PUT请求修改前的参数数组
        /// </summary>
        public List<T> PostDataEditList { get; set; }

    }
}
