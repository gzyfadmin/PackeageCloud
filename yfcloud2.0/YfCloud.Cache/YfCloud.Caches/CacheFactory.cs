using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Caches
{
    /// <summary>
    /// 缓存工厂
    /// </summary>
    public static class CacheFactory
    {
        /// <summary>
        /// 获取缓存实例
        /// </summary>
        /// <param name="cacheType">Cache Type</param>
        /// <returns></returns>
        public static ICache Instance(CacheType cacheType)
        {
            switch (cacheType)
            {
                case CacheType.Redis:
                    var config = new RedisConfigs();
                    return new RedisCache(config);
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// 缓存数据库类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// Redis
        /// </summary>
        Redis = 0
    }
}
