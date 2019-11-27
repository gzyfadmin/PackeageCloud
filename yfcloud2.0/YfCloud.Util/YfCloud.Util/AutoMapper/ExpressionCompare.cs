using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace YfCloud.Utilities.AutoMapper
{
    /// <summary>
    /// 比较工具类
    /// </summary>
    public class ExpressionCompare<T>
    {
        private static Func<T, T, List<String>, string> _FUNContain = null;

        private static Func<T, T, List<String>, string> _FUNIgnoreColumns = null;

        static ExpressionCompare()
        {
            _FUNContain = (x1, x2, cols) =>
            {
                string result = string.Empty;
                List<string> actualCols = new List<string>();
                IEnumerable<PropertyInfo> PropertiesList;

                if (cols != null && cols.Count() > 0)
                {
                    actualCols = cols.Select(p => p.ToLower()).ToList();

                    PropertiesList = x1.GetType().GetProperties().Where(p => cols.Contains(p.Name.ToLower()));
                }
                else
                {
                    PropertiesList = x1.GetType().GetProperties();
                }

                foreach (var item in PropertiesList)
                {
                    var fromObj = item.GetValue(x1);
                    var toObj = item.GetValue(x2);

                    if (!fromObj.Equals(toObj))// 如果值不相同
                    {
                        string fromObjString = fromObj == null ? "" : fromObj.ToString();
                        string toObjString = toObj == null ? "" : toObj.ToString();

                        result += $"【{fromObjString}】变成【{toObjString}】;";
                    }
                }

                return result;
            };
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        public static string CompareWith(T from ,T to,List<string> contains)
        {
            return _FUNContain(from,to,contains);
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="ignore">忽略</param>
        /// <returns></returns>
        public static string CompareIgnore(T from, T to, List<string> ignore)
        {
            return _FUNContain(from, to, ignore);
        }
    }

    /// <summary>
    /// 比较工具类
    /// </summary>
    public class ReflexCompare
    {

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string CompareWith<T>(T from, T to, List<string> cols = null)
        {
            string result = string.Empty;
            List<string> actualCols = new List<string>();
            IEnumerable<PropertyInfo> PropertiesList;

            if (cols != null && cols.Count() > 0)
            {
                actualCols = cols.Select(p => p.ToLower()).ToList();

                PropertiesList = from.GetType().GetProperties().Where(p => cols.Contains(p.Name.ToLower()));
            }
            else
            {
                PropertiesList = from.GetType().GetProperties();
            }

            foreach (var item in PropertiesList)
            {
                var fromObj = item.GetValue(from);
                var toObj = item.GetValue(to);

                if (!fromObj.Equals(toObj))// 如果值不相同
                {
                    string fromObjString = fromObj == null ? "" : fromObj.ToString();
                    string toObjString = toObj == null ? "" : toObj.ToString();

                    result += $"【{fromObjString}】变成【{toObjString}】;";
                }
            }

            return result;
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string CompareIgnore<T>(T from, T to, List<string> cols = null)
        {
            string result = string.Empty;
            List<string> actualCols = new List<string>();
            IEnumerable<PropertyInfo> PropertiesList;

            if (cols != null && cols.Count() > 0)
            {
                actualCols = cols.Select(p => p.ToLower()).ToList();

                PropertiesList = from.GetType().GetProperties().Where(p => cols.Contains(p.Name.ToLower()));
            }
            else
            {
                PropertiesList = from.GetType().GetProperties();
            }

            foreach (var item in PropertiesList)
            {
                var fromObj = item.GetValue(from);
                var toObj = item.GetValue(to);

                if (!fromObj.Equals(toObj))// 如果值不相同
                {
                    string fromObjString = fromObj == null ? "" : fromObj.ToString();
                    string toObjString = toObj == null ? "" : toObj.ToString();

                    result += $"【{fromObjString}】变成【{toObjString}】;";
                }
            }

            return result;
        }
    }
}
