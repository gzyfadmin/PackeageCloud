using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.Caches;
using YfCloud.Framework.OrderGenerator;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.MongoDb;

namespace YfCloud.App.Module.Test.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public string Get()
        {
            return "OK";
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public string redis()
        {
            var redis = CacheFactory.Instance(CacheType.Redis);
            redis.AddOrUpdateKey("yyy7", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), 15);

            return redis.GetValueByKey<string>("yyy7");

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public List<LoginLog> managoDB()
        {
            LoginLog loginLog = new LoginLog();
            loginLog.LoginID = Guid.NewGuid().ToString();
            loginLog.Account = "123";
            loginLog.CompanyID = 1;
            loginLog.Description = LoginTypeEum.LoginSuccess;

            MongoDbUtil.AddDoc(loginLog);

            return MongoDbUtil.GetDoc<LoginLog>((x) => true);

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public string GetDatexx()
        {

            string nowStr = DateTime.Now.ToString("yyyyMMdd");

            return nowStr;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public string GetDate()
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string key = string.Format(CacheKeyString.LockGeneratorMO, 188);
            string datekey = string.Format(CacheKeyString.LockGeneratorMO_date, 188);

            string nowStr= DateTime.Now.ToString("yyyyMMdd");

            while (true)
            {
                DateSno dateSno = null;
                if (redis.ContainsKey(datekey))
                {
                    dateSno = redis.GetValueByKey<DateSno>(datekey); //获取redis 里面存储的日期
                }

                if (dateSno != null)
                {
                    if (dateSno.DateStr != nowStr) //不是当天
                    {

                        throw new Exception($"{dateSno.DateStr},{nowStr},now:{DateTime.Now.ToString("yyyymmdd HH:mm:ss")}");
                        dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                        redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                    }
                    else //当前 流水号+1
                    {
                        dateSno.SNO = dateSno.SNO + 1;

                        redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                    }
                }
                else
                {
                   throw new Exception($"{null},{nowStr},now:{DateTime.Now.ToString("yyyymmdd HH:mm:ss")}");
                    dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                    redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                }

                Thread.Sleep(50);
            }

            return nowStr;

        }
    }
}