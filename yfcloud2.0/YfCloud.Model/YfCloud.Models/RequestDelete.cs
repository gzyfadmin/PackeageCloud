using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// API DELETE请求对象
    /// </summary>
    public class RequestDelete<T>
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
