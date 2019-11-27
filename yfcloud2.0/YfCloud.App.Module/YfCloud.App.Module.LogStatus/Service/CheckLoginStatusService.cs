using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using YfCloud.Attributes;
using YfCloud.Caches;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.Date;
using YfCloud.Utilities.MongoDb;

namespace YfCloud.App.Module.LogStatus.Service
{
    /// <summary>
    /// 时间服务
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ICheckLoginStatusService))]
    public class CheckLoginStatusService: ICheckLoginStatusService
    {
        //private readonly static Timer _timer;
        //private static object _lockObj = new object();

        //private readonly static RedisSubHelp _redisSubHelp;

        /// <summary>
        /// 启动
        /// </summary>
        public static void Run()
        {
           
        }

        static CheckLoginStatusService()
        {
            Task.Factory.StartNew(() => {

                try
                {
                    var redisSubHelp = new RedisSubHelp((a, b) =>
                    {
                        if (b.StartsWith(CacheKeyString.UserLoginAllKeyPre))
                        {
                            LogWrite.Write("redisLog.txt", "redis key:" + b);
                            try
                            {
                                string[] strArray = b.Split("#");
                                string ID = strArray[3];
                                int companyID = Convert.ToInt32(strArray[2]);
                                string userID = strArray[1].ToString();
                                LoginLog loginLog = MongoDbUtil.GetDoc<LoginLog>(p => p.LoginID == ID, null).FirstOrDefault();

                                DateTime now = DateTime.Now;
                                TimeSpan spendSpan = now.Subtract(loginLog.LoginTime);
                                if (loginLog != null)
                                {
                                    UpdateDefinition<LoginLog> update = Builders<LoginLog>.
                                        Update.Set(y => y.Seconds, spendSpan.TotalSeconds).Set(y => y.TimeSpan, DateUtil.parseTimeSeconds(spendSpan.TotalSeconds, 0))
                                        .Set(y => y.Status, LoginStatusEum.LogOut);
                                    MongoDbUtil.UpdateOne<LoginLog>(p => p.LoginID == ID, update);
                                }

                                var redis = CacheFactory.Instance(CacheType.Redis) as RedisCache;
                                string thisWeek = DateUtil.GetThisWeekString();
                                string strV = string.Format(CacheKeyString.StaOnlineUser, companyID, thisWeek);

                                string strTimeV= string.Format(CacheKeyString.StaOnlineTimes, companyID, thisWeek); 

                                //累加登陆时长
                                redis.IncrementItemInSortedSet(strV, userID, spendSpan.TotalSeconds);
                                redis.Client.ExpireAt(strV, DateUtil.ToUnixEpochDate(DateTime.Now.AddDays(15)));

                                redis.IncrementItemInSortedSet(strTimeV, userID, 1);
                                redis.Client.ExpireAt(strV, DateUtil.ToUnixEpochDate(DateTime.Now.AddDays(15)));

                            }
                            catch(Exception ex) {
                                LogWrite.Write("redisLog", "错误:" + ex.Message);
                            }
                        }
                    });
                    redisSubHelp.SubscribeToChannels("__keyevent@0__:expired");
                }
                catch(Exception ex) {
                    LogWrite.Write("redisLog.txt", "最外层线程:" + ex.Message);
                }
            });

        }

        //private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    lock (_lockObj)
        //    {
        //        try
        //        {
        //            var redis = CacheFactory.Instance(CacheType.Redis);
        //            List<UserStatus> statusList = redis.GetValuesByKey<UserStatus>(CacheKeyString.UserLoginAllKey);

        //            DateTime now = DateTime.Now;

        //            foreach (var item in statusList)
        //            {
        //                TimeSpan timeSpan = now.Subtract(item.LastRefreshTime);
        //                if (timeSpan.TotalSeconds > 100)//超100秒没刷新，说明离线
        //                {
        //                    LoginLog loginLog = MongoDbUtil.GetDoc<LoginLog>(p => p.LoginID == item.ID,null).FirstOrDefault();

        //                    if (loginLog != null)
        //                    {
        //                        TimeSpan spendSpan= now.Subtract(loginLog.LoginTime);

        //                        UpdateDefinition<LoginLog> update = Builders<LoginLog>.
        //                            Update.Set(y => y.Seconds, timeSpan.TotalSeconds).Set(y => y.TimeSpan, DateUtil.parseTimeSeconds(spendSpan.TotalSeconds,0))
        //                            .Set(y=>y.Status, LoginStatusEum.LogOut) ;
        //                        MongoDbUtil.UpdateOne<LoginLog>(p => p.LoginID == item.ID, update);

        //                        //删除redis里面的值
        //                        redis.RemoveEntityFromList<UserStatus>(CacheKeyString.UserLoginAllKey, p => p.ID == loginLog.LoginID);
        //                    }
        //                }
        //            }
        //            //
        //        }
        //        catch {

        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        public  void RefresUserStatus(CurrentUser currentUser)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            UserStatus userStatus = new UserStatus() { ID = currentUser.ID, LastRefreshTime = DateTime.Now };
            string redisKey = string.Format(CacheKeyString.UserLoginAllKey,currentUser.UserID,currentUser.CompanyID,currentUser.ID);
            redis.AddOrUpdateKey<UserStatus>(redisKey, userStatus,90);
        }
    }
}
