using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YfCloud.Framework
{
    public static class EnumUtils
    {
        /// <summary>
        /// 从枚举中获取Description
        /// 说明：
        /// </summary>
        /// <param name="enumName">需要获取枚举描述的枚举</param>
        /// <returns>描述内容</returns>
        public static string GetDescription(this Enum enumName)
        {
            var _description = new StringBuilder();
            var enums = new Enum[] { enumName };
            if (IsDefinedFlags(enumName.GetType()))
            {
                enums = Enum.GetValues(enumName.GetType())
                    .OfType<Enum>()
                    .Where(t => enumName.HasFlag(t))
                    .ToArray();
            }

            foreach (var e in enums)
            {
                var _attributes = e.GetType().GetField(e.ToString()).GetDescriptAttr();

                    var hasDisAttr = _attributes != null && _attributes.Length > 0;

                _description.Append(hasDisAttr ? _attributes[0].Description : e.ToString());

                _description.Append(",");
            }

            if (_description.Length > 0)
                _description.Length--;

            return _description.ToString();
        }

        /// <summary>
        /// 判断某个枚举类型是否定义了 <see cref="FlagsAttribute"/> 属性。
        /// </summary>
        /// <param name="enumType">要判断的枚举类型。</param>
        /// <returns>如果该枚举类型定义了 <see cref="FlagsAttribute"/> 属性，则返回 true，否则返回 false。</returns>
        public static bool IsDefinedFlags(Type enumType)
        {
            return enumType.GetCustomAttribute<FlagsAttribute>() != null;
        }

        /// <summary>
        /// 获取字段Description
        /// </summary>
        /// <param name="fieldInfo">FieldInfo</param>
        /// <returns>DescriptionAttribute[] </returns>
        private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }

        /// <summary>
        /// 将枚举转换为ArrayList
        /// 说明：
        /// 若不是枚举类型，则返回NULL
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>ArrayList</returns>
        public static ArrayList ToDictionary(this Type type)
        {
            if (type.IsEnum)
            {
                ArrayList _array = new ArrayList();
                Array _enumValues = Enum.GetValues(type);
                if (IsDefinedFlags(type))
                {
                    int[] enumValues = new int[_enumValues.Length];
                    for (int i = 0; i < _enumValues.Length; i++)
                    {
                        enumValues[i] = (int)_enumValues.GetValue(i);
                    }
                    int maxValue = enumValues.Sum();
                    Enum enumInstance = null;
                    for (int i = 0; i < maxValue; i++)
                    {
                        enumInstance = (Enum)Enum.Parse(type, (i + 1).ToString());
                        _array.Add(new KeyValuePair<string, string>(enumInstance.ToString(), GetDescription(enumInstance)));
                    }
                }
                else
                {
                    foreach (Enum value in _enumValues)
                    {
                        _array.Add(new KeyValuePair<string, string>(value.ToString(), GetDescription(value)));
                    }
                }
                return _array;
            }
            return null;
        }
    }
}
