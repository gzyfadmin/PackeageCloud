using System;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using ServiceStack.Redis.Generic;

namespace YfCloud.Caches
{
    /// <summary>
    /// Redis
    /// </summary>
    public class RedisCache : ICache
    {
        private RedisClient _redisClient;
        private RedisConfigs _config;

        /// <summary>
        /// 客户端
        /// </summary>
        public RedisClient Client { get { return _redisClient; } }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="redisConfig"></param>
        public RedisCache(RedisConfigs redisConfig)
        {
            _redisClient = new RedisClient(redisConfig.Host, redisConfig.Port);
            _config = redisConfig;
        }

        #region Add
        public bool AddKey<T>(string strKey, T tValue, int? expiresIn = null) 
        {
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (tValue == null)
                throw new ArgumentNullException("Value");
            var expires = TimeSpan.FromMilliseconds(_config.Expired * 1000);
            if (expiresIn != null)
                expires = TimeSpan.FromMilliseconds(Convert.ToInt32(expiresIn) * 1000);
            return _redisClient.Add(strKey, tValue, expires);
        }

        public bool AddOrUpdateKey<T>(string strKey, T tValue, int? expiresIn = null)
        {
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (tValue == null)
                throw new ArgumentNullException("Value");
            var expires = TimeSpan.FromMilliseconds(_config.Expired * 1000);
            if (expiresIn != null)
                expires = TimeSpan.FromMilliseconds(Convert.ToInt32(expiresIn) * 1000);
            return _redisClient.Set(strKey, tValue, expires);
        }
        

        public bool AddToList<T>(string strKey, T tValue)
        {
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (tValue == null )
                throw new ArgumentNullException("Value");
            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            redisList.Add(tValue);
            return true;
        }

        public bool AddKey<T>(string strKey,List<T> list)
        {            
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (list == null || list.Count < 1)
                throw new ArgumentNullException("List");
            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            redisList.AddRange(list);
            return true;
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool UpdateKey<T>(string strKey, T tValue, int? expiresIn = null)
        {
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (tValue == null)
                throw new ArgumentNullException("tValue");
            var expires = TimeSpan.FromMilliseconds(_config.Expired * 1000);
            if (expiresIn != null)
                expires = TimeSpan.FromMilliseconds(Convert.ToInt32(expiresIn) * 1000);
            return _redisClient.Set(strKey,tValue,expires);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpdateKey<T>(string strKey,List<T> list)
        {
            if (string.IsNullOrEmpty(strKey))
                throw new ArgumentNullException("Key");
            if (list == null || list.Count < 1)
                throw new ArgumentNullException("List");
            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            redisList.RemoveAll();
            redisList.AddRange(list);
            return true;
        }
        #endregion

        #region Query
        /// <summary>
        /// 查询Key的值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>        
        public T GetValueByKey<T>(string strKey)
        {
            return _redisClient.Get<T>(strKey);
        }

        /// <summary>
        /// 查询Key的值
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="factory">查询不到值重新获取的方法</param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>        
        public T GetValueByKey<T>(string strKey, Func<T> factory,int? expiresIn = null)
        {
            var result = _redisClient.Get<T>(strKey);

            if (result == null && factory != null)
            {
                var computed = factory();
                if (computed != null)
                {
                    AddKey(strKey, computed, expiresIn);
                }
                return computed;
            }

            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public List<T> GetValuesByKey<T>(string strKey)
        {
            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            return redisList.GetAll();
        }

        /// <summary>
        /// 查询，根据lambda表达式筛选结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetValuesByKey<T>(string strKey, Expression<Func<T, bool>> expression)
        {
            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            //var result = redisList.GetAll() as IQueryable<T>;
            return redisList.Where(expression.Compile()).ToList();
        }

        /// <summary>
        /// 查询，根据lambda表达式筛选结果 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public (List<T>, long) GetValuesByKey<T>(string strKey, Expression<Func<T, bool>> expression,bool isPage=false, int pageIndex=0,int PageSize=0)
        {

            var redisTypedClient = _redisClient.As<T>();
            var redisList = redisTypedClient.Lists[strKey];
            //var result = redisList.GetAll() as IQueryable<T>;
            var query = redisList.Where(expression.Compile());
            long totalNum = query.LongCount();
            if (isPage)
            {
                return (query.Skip<T>((pageIndex - 1) * PageSize).Take<T>(PageSize).ToList(), totalNum);
            }
            else
            {
                return (query.ToList(), totalNum);
            }
        }

        public List<T> GetValuesByKeys<T>(List<string> keys)
        {
            return _redisClient.GetValues<T>(keys);
        }
        #endregion

        #region Delete

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <returns></returns>
        public bool RemoveKey(string strKey)
        {
            return _redisClient.Remove(strKey);
        }

        /// <summary>
        ///     根据lambada表达式删除符合条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public void RemoveEntityFromList<T>(string key, Func<T, bool> func)
        {
            IRedisTypedClient<T> iredisClient = _redisClient.As<T>();
            IRedisList<T> redisList = iredisClient.Lists[key];

            T value = redisList.Where(func).FirstOrDefault();
            if (value != null)
            {
                redisList.RemoveValue(value);
                iredisClient.Save();
            }
        }

        #endregion


        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public bool ContainsKey(string strKey)
        {
            return _redisClient.ContainsKey(strKey);
        }

        /// <summary>
        /// 加分布式并发锁 获取结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lockKey">并发锁的Key</param>
        /// <param name="waitfor">并发锁占用的时间</param>
        /// <param name="compute">返回结果的委托</param>
        /// <returns></returns>
        public  T LockRelease<T>(string lockKey, TimeSpan waitfor,Func<T> compute,string error="")
        {

            try
            {
                using (_redisClient.AcquireLock(lockKey, waitfor))
                {

                    return compute();
                }
            }
            catch (TimeoutException tex)
            {
                if(error!="")
                {
                    throw new Exception(error);
                }
                else
                {
                    throw new Exception("超时请等待");
                }
            }
        }

        /// <summary>
        /// 将字符串添加到有序集合中
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="score">分数</param>
        public bool AddItemToSortedSet(string setId, string value, double score)    
        {
            return _redisClient.AddItemToSortedSet(setId, value, score);
        }

        /// <summary>
        /// 遍历有序集合
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<byte[]> ZScan(string setId, ulong cursor, int count = 10000, string match = null)
        {
            return _redisClient.ZScan(setId, cursor, count, match).Results;
        }

        /// <summary>
        /// 有序集合自增
        /// </summary>
        /// <param name="setId">Id</param>
        /// <param name="value">值</param>
        /// <param name="incrementBy">增量</param>
        /// <returns></returns>
        public double IncrementItemInSortedSet(string setId, string value, double incrementBy)
        {
           return _redisClient.IncrementItemInSortedSet(setId, value, incrementBy);
        }
    }
}
