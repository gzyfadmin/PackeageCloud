namespace YfCloud.Models
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class ResponseObject<T, T2> : RequestObject<T>
    {
        /// <summary>
        /// 执行结果描述信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 执行结果代码 0 正常，-1错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 总记录条数，-1为不分页的值
        /// </summary>
        public long TotalNumber { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T2 Data { get; set; }
    }

    /// <summary>
    /// 响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseObject<T>
    {
        /// <summary>
        /// 执行结果描述信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 执行结果代码 0 正常，-1错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 总记录条数，-1为不分页的值
        /// </summary>
        public long TotalNumber { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
    }
}
