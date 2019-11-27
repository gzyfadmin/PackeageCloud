using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// API POST请求对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestPost<T>
    {
        /// <summary>
        /// POST请求参数，用户新增
        /// </summary>
        public T PostData { get; set; }


        /// <summary>
        /// POST请求参数，用于批量新增
        /// </summary>
        public List<T> PostDataList { get; set; }

    }
}
