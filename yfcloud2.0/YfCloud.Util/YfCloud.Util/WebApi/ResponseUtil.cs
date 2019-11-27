using YfCloud.Models;

namespace YfCloud.Utilities.WebApi
{
    /// <summary>
    /// 响应结果工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public static class ResponseUtil<T,T2>
    {
        /// <summary>
        /// 返回响应正常的结果
        /// </summary>
        /// <returns></returns>
        public static ResponseObject<T,T2> SuccessResult(RequestObject<T> requestObject, T2 requestResult, long totalNumber = 0)
        {
            var result = GetResponseObject(requestObject);
            result.TotalNumber = totalNumber;
            result.Data = requestResult;
            result.Info = "执行成功";
            result.Code = 0;
            return result;
        }

        
        private static ResponseObject<T,T2> GetResponseObject(RequestObject<T> requestObject) =>
            new ResponseObject<T,T2>
            {
                IsPaging = requestObject.IsPaging,
                PageIndex = requestObject.PageIndex,
                PageSize = requestObject.PageSize,
                PostData = requestObject.PostData,
                QueryConditions = requestObject.QueryConditions,
                OrderByConditions = requestObject.OrderByConditions,
                PostDataList = requestObject.PostDataList
            };

        /// <summary>
        /// 返回响应异常的结果
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="requestResult"></param>
        /// <param name="strErrorInfo"></param>
        /// <returns></returns>
        public static ResponseObject<T,T2> FailResult(RequestObject<T> requestObject, T2 requestResult,string strErrorInfo = "")
        {
            var result = GetResponseObject(requestObject);
            result.Code = -1;
            result.Info = strErrorInfo;
            result.Data = requestResult;
            result.TotalNumber = -1;
            return result;
        }
    }

    /// <summary>
    /// 响应结果工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ResponseUtil<T>
    {
        /// <summary>
        /// 返回响应正常的结果
        /// </summary>
        /// <returns></returns>
        public static ResponseObject<T> SuccessResult(T requestResult, long totalNumber = -1)
        {
            return new ResponseObject<T>
            {
                TotalNumber = totalNumber,
                Data = requestResult,
                Info = "执行成功",
                Code = 0
            };
        }

        /// <summary>
        /// 返回响应异常的结果
        /// </summary>
        /// <param name="requestResult"></param>
        /// <param name="strErrorInfo"></param>
        /// <returns></returns>
        public static ResponseObject<T> FailResult(T requestResult, string strErrorInfo = "")
        {
            return new ResponseObject<T>
            {
                TotalNumber = -1,
                Data = requestResult,
                Info = strErrorInfo,
                Code = -1
            };
        }
    }
}
