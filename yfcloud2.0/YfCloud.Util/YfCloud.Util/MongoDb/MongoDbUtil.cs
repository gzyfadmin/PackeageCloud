using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace YfCloud.Utilities.MongoDb
{
    /// <summary>
    /// MongoDb工具类
    /// </summary>
    public class MongoDbUtil
    {
        private static MongoDbConfig _config;
        private static IMongoClient _client;
        private static object lockObj = new object();

        private static IMongoClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock(lockObj)
                    {
                        if (_client == null)
                        {
                            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
                            _config = new MongoDbConfig();
                            _client = new MongoClient(new MongoClientSettings
                            {
                                Server = new MongoServerAddress(_config.Host, _config.Port)
                            });
                        }
                    }
                    ;
                }
                return _client;
            }
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName">MongoDb Name ,default "SystemLog"</param>
        /// <param name="t"></param>
        public static void AddDoc<T>(T t, string dbName = "SystemLog")
        {
            Add(new List<T> { t }, dbName);
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dbName">MongoDb Name ,default "SystemLog"</param>
        public static void AddDoc<T>(List<T> list, string dbName = "SystemLog")
        {
            Add(list, dbName);
        }

        private static void Add<T>(List<T> list,string dbName)
        {
            try
            {
                //获取数据库
                var db = Client.GetDatabase(dbName);
                var name = typeof(T).Name;
                //获取集合
                var collection = db.GetCollection<T>(name);
                if (collection == null)
                {
                    db.CreateCollection(name);
                    collection = db.GetCollection<T>(name);
                }
                collection.InsertMany(list);
            }
            catch(Exception EX)
            {
                //异常不处理
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="sort">排序</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static List<T> GetDoc<T>(Expression<Func<T,bool>> expression,  int? pageIndex,int pageSize, SortDefinition<T> sort,ref long  totalNum, string dbName = "SystemLog")
        {
            try
            {
                //获取数据库
                var db = Client.GetDatabase(dbName);
                var name = typeof(T).Name;
                //获取集合
                var collection = db.GetCollection<T>(name);
                if (collection == null)
                {
                    db.CreateCollection(name);
                    collection = db.GetCollection<T>(name);
                }

                if (pageIndex != null)//说明分页
                {

                    int skips = (pageIndex.Value - 1) * pageSize;
                    totalNum = collection.CountDocuments<T>(expression);
                    return  collection.Find<T>(expression).Sort(sort).Skip(skips).Limit(pageSize).ToList();
                }
                else
                {
                    
                    return collection.Find(expression).Sort(sort).ToList();
                }

                

            }
            catch(Exception ex)
            {
                //异常不处理
                return null;
            }
        }


        /// <summary>
        /// 查询记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询表达式</param>
        /// <param name="sort"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static List<T> GetDoc<T>(Expression<Func<T, bool>> expression, SortDefinition<T> sort=null, string dbName = "SystemLog")
        {
            try
            {
                //获取数据库
                var db = Client.GetDatabase(dbName);
                var name = typeof(T).Name;
                //获取集合
                var collection = db.GetCollection<T>(name);
                if (collection == null)
                {
                    db.CreateCollection(name);
                    collection = db.GetCollection<T>(name);
                }

                return collection.Find(expression).Sort(sort).ToList();
            }
            catch (Exception ex)
            {
                //异常不处理
                return null;
            }
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        public static void UpdateOne<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition, string dbName = "SystemLog")
        {
            //获取数据库
            var db = Client.GetDatabase(dbName);
            var name = typeof(T).Name;
            //获取集合
            var collection = db.GetCollection<T>(name);
            if (collection == null)
            {
                db.CreateCollection(name);
                collection = db.GetCollection<T>(name);
            }

            var res = collection.UpdateOne<T>(expression, updateDefinition);
        }
    }
}
