using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YfCloud.Attributes;
using YfCloud.Caches;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.Framework.OrderGenerator
{
    /// <summary>
    /// 销售单号
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ICodeMaker))]
    public class SOGenerator : ICodeMaker
    {
        public string ProvideName { get { return "SO"; } }

        private readonly IDbContext _db;//数据库实例对象

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        public SOGenerator(IDbContext db)
        {
            _db = db;
        }
        /// <summary>
        /// 单号
        /// </summary>
        public static string CreateNo(int CompanyID)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string key = string.Format(CacheKeyString.LockGeneratorSO, CompanyID);
            string datekey = string.Format(CacheKeyString.LockGeneratorSO_date, CompanyID);


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


                return "SO" + dateSno.DateStr + dateSno.SNO.ToString().PadLeft(4, '0');
            });

            return result;
        }

        public string MakeNo(int CompanyID)
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            string key = string.Format(CacheKeyString.LockGeneratorSO, CompanyID);
            string datekey = string.Format(CacheKeyString.LockGeneratorSO_date, CompanyID);


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
                    string no = _db.Instance.Queryable<TSSMSalesOrderMainDbModel>().Where(p => p.CompanyId == CompanyID && p.OrderNo.StartsWith(ProvideName)).Max(p => p.OrderNo);

                    if (!string.IsNullOrEmpty(no))
                    {
                        no = no.TrimStart(ProvideName.ToArray());

                        string timeStr = no.Substring(0, 8);

                        if (timeStr == nowStr)
                        {
                            string noIndex = no.Substring(8);

                            int index = Convert.ToInt32(noIndex);

                            dateSno = new DateSno() { DateStr = nowStr, SNO = index + 1 };

                            redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                        }
                        else
                        {
                            dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                            redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);
                        }
                    }
                    else
                    {
                        dateSno = new DateSno() { DateStr = nowStr, SNO = 1 };

                        redis.UpdateKey(datekey, dateSno, 60 * 60 * 48);

                    }
                }


                return "SO" + dateSno.DateStr + dateSno.SNO.ToString().PadLeft(4, '0');
            });

            return result;
        }
    }
}
