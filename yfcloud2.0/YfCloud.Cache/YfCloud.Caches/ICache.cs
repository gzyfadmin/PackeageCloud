
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YfCloud.Caches
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        #region Add
        /// <summary>
        /// 根据传入的key-value添加一条记录，当key已存在返回false
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="tValue"></param>
        /// <param name="expiresIn">到期，单位秒,不设置该参数则使用默认值，从config文件中读取</param>
        /// <returns></returns>
        bool AddKey<T>(string strKey, T tValue,int? expiresIn = null);

        /// <summary>
        /// 新增或修改redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey"></param>
        /// <param name="tValue"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        bool AddOrUpdateKey<T>(string strKey, T tValue, int? expiresIn = null);

        /// <summary>
        /// 在已有的Key上新增一条记录
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="tValue">Value</param>
        /// <returns></returns>
        bool AddToList<T>(string strKey, T tValue);

        /// <summary>
        /// 新增Key的值到集合上
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="list">List<typeparamref name="T"/></param>
        /// <returns></returns>
        bool AddKey<T>(string strKey, List<T> list);

        #endregion

        T GetValueByKey<T>(string strKey, Func<T> factory, int? expiresIn = null);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="tValue"></param>
        /// <returns></returns>
        bool UpdateKey<T>(string strKey, T tValue, int? expiresIn=null);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateKey<T>(string strKey, List<T> list);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>        
        T GetValueByKey<T>(string strKey);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <returns></returns>
        List<T> GetValuesByKey<T>(string strKey);

        /// <summary>
        /// 查询，根据lambda表达式筛选结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey">Key</param>
        /// <param name="expressions">筛选条件</param>
        /// <returns></returns>
        List<T> GetValuesByKey<T>(string strKey, Expression<Func<T, bool>> expression);

        List<T> GetValuesByKeys<T>(List<string> keys);
       
        /// <summary>
       /// 分页查询
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="strKey"></param>
       /// <param name="expression"></param>
       /// <param name="isPage"></param>
       /// <param name="pageIndex"></param>
       /// <param name="PageSize"></param>
       /// <returns></returns>
        (List<T>, long) GetValuesByKey<T>(string strKey, Expression<Func<T, bool>> expression, bool isPage = false, int pageIndex = 0, int PageSize = 0);

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <returns></returns>
        bool RemoveKey(string strKey);

        /// <summary>
        /// 删除key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        void RemoveEntityFromList<T>(string key, Func<T, bool> func);

        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <returns></returns>
        bool ContainsKey(string strKey);

        /// <summary>
        /// 加分布式并发锁 获取结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lockKey">并发锁的Key</param>
        /// <param name="waitfor">并发锁占用的时间</param>
        /// <param name="compute">返回结果的委托</param>
        /// <returns></returns>
        T LockRelease<T>(string lockKey, TimeSpan waitfor, Func<T> compute,string error="");

        // <summary>
        /// 将字符串添加到有序集合中
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="score">分数</param>
        bool AddItemToSortedSet(string setId, string value, double score);


        /// <summary>
        /// 遍历有序集合
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        List<byte[]> ZScan(string setId, ulong cursor, int count = 10000, string match = null);


        /// <summary>
        /// 有序集合自增
        /// </summary>
        /// <param name="setId">Id</param>
        /// <param name="value">值</param>
        /// <param name="incrementBy">增量</param>
        /// <returns></returns>
        double IncrementItemInSortedSet(string setId, string value, double incrementBy);

    }
}
