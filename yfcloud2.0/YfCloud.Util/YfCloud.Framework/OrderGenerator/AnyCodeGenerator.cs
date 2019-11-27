using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Caches;
using YfCloud.Models;

namespace YfCloud.Framework.OrderGenerator
{
    /// <summary>
    /// 编码产生器件
    /// </summary>
    public class AnyCodeGenerator
    {
        /// <summary>
        /// 编号生成
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <param name="prefix">编码前缀</param>
        /// <returns></returns>
        public static string CreateNo(int CompanyID, string prefix)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string key = string.Format(CacheKeyString.LockAutoAnyCode, prefix, CompanyID);
            string datekey = string.Format(CacheKeyString.LockAutoAnyCode_date, prefix, CompanyID);


            var result = redis.LockRelease(key, TimeSpan.FromSeconds(10), () =>
            {
                DateSno dateSno = null;
                if (redis.ContainsKey(datekey))
                {
                    dateSno = redis.GetValueByKey<DateSno>(datekey); //获取redis 里面存储的日期
                }

                string nowStr = DateTime.Now.ToString("yyyyMMdd"); //当前日期


                if (dateSno != null)
                {
                    if (dateSno.DateStr != nowStr) //不是当天
                    {
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
                    dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                    redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                }


                return prefix + dateSno.DateStr + dateSno.SNO.ToString().PadLeft(4, '0');
            });

            return result;
        }
    }
}
