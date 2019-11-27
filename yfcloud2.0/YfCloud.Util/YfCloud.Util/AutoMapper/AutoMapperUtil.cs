using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using YfCloud.Attributes;

namespace YfCloud.Utilities.AutoMapper
{
    /// <summary>
    /// Auto Mapper Util
    /// </summary>
    public class AutoMapperUtil
    {
        /// <summary>
        /// 获取需要创建映射关系的集合
        /// </summary>
        /// <param name="AssemblyName"></param>
        /// <returns></returns>
        public static Dictionary<Type,Type> GetMapperList(string AssemblyName)
        {
            var result = new Dictionary<Type, Type>();
            var assembly = Assembly.Load(AssemblyName);
            var types = assembly.GetTypes();
            foreach(var item in types)
            {
                if (item.GetCustomAttribute(typeof(UseAutoMapperAttribute)) is UseAutoMapperAttribute att)
                {
                    result.Add(item, att.GetTargetType());
                }
            }
            return result;
        }
    }
}
