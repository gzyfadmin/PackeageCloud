using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace YfCloud.Attributes
{
    /// <summary>
    /// 标注要运用DI的类 被此属性标注的类 要被注册到依赖注入容器中 并且可以指定类要映射的接口或者类
    /// 此属性只能运用于类，并且此属性不能继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UseDIAttribute : Attribute
    {
        //Targets用于指示 哪些接口或者类 要被 "被属性修饰了的类" 进行依赖注入
        private List<Type> targetTypes = new List<Type>();
        private ServiceLifetime lifetime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="argLifetime"></param>
        /// <param name="argTargets"></param>
        public UseDIAttribute(ServiceLifetime argLifetime, params Type[] argTargets)
        {
            lifetime = argLifetime;
            foreach (var argTarget in argTargets)
            {
                targetTypes.Add(argTarget);
            }
        }

        /// <summary>
        /// Targets用于指示 哪些接口或者类 要被 "被属性修饰了的类" 进行依赖注入
        /// </summary>
        /// <returns></returns>
        public List<Type> GetTargetTypes()
        {
            return targetTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        public ServiceLifetime Lifetime
        {
            get
            {
                return lifetime;
            }
        }

    }
}
