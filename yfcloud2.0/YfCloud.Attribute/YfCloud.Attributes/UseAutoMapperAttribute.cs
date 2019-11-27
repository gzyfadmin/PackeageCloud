using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Attributes
{
    /// <summary>
    /// 标注要使用AutoMapper的类
    /// 此属性只能运用于类，并且此属性不能继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UseAutoMapperAttribute : Attribute
    {
        //Targets用于指示 哪些接口或者类 要被 "被属性修饰了的类" 进行映射
        private Type _targetType = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetType">映射后的目标类型</param>
        public UseAutoMapperAttribute(Type targetType)
        {
            _targetType = targetType;
        }

        /// <summary>
        /// Targets用于指示 哪些接口或者类 要被 "被属性修饰了的类" 进行映射
        /// </summary>
        /// <returns></returns>
        public Type GetTargetType()
        {
            return _targetType;
        }
    }
}
