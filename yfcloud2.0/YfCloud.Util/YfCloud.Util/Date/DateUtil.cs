using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.Date
{
    /// <summary>
    /// 日期
    /// </summary>
   public class DateUtil
    {
        ///<summary>
        ///由秒数得到日期几天几小时。。。
        ///</summary
        ///<param name="t">秒数</param>
        ///<param name="type">0：转换后带秒，1:转换后不带秒</param>
        ///<returns>几天几小时几分几秒</returns>
        public static string parseTimeSeconds(double t, int type)
        {
            string r = "";
            int day, hour, minute, second;
            if (t >= 86400) //天,
            {
                day = Convert.ToInt16(Math.Floor(t / 86400));
                hour = Convert.ToInt16(Math.Floor((t % 86400) / 3600));
                minute = Convert.ToInt16(Math.Floor((t % 86400 % 3600) / 60));
                second = Convert.ToInt16(t % 86400 % 3600 % 60);
                if (type == 0)
                    r = day + ("天") + hour + ("时") + minute + ("分") + second + ("秒");
                else
                    r = day + ("天") + hour + ("时") + minute + ("分");

            }
            else if (t >= 3600)//时,
            {
                hour = Convert.ToInt16(Math.Floor(t / 3600));
                minute = Convert.ToInt16(Math.Floor((t % 3600) / 60));
                second = Convert.ToInt16(t % 3600 % 60);
                if (type == 0)
                    r = hour + ("时") + minute + ("分") + second + ("秒");
                else
                    r = hour + ("时") + minute + ("分");
            }
            else if (t >= 60)//分
            {
                minute = Convert.ToInt16(Math.Floor(t / 60));
                second = Convert.ToInt16(t % 60);
                r = minute + ("分") + second + ("秒");
            }
            else
            {
                second = Convert.ToInt16(t);
                r = second + ("秒");
            }
            return r;
        }

        ///<summary>
        ///由秒数得到日期几天几小时。。。
        ///</summary
        ///<param name="t">秒数</param>
        ///<param name="type">0：转换后带秒，1:转换后不带秒</param>
        ///<returns>几天几小时几分几秒</returns>
        public static string parseTimeHM(double t)
        {
            string r = "";
            int hour, minute;
            if (t >= 3600)//时,
            {
                hour = Convert.ToInt16(Math.Floor(t / 3600));
                minute = Convert.ToInt16(Math.Floor((t % 3600) / 60));
                r = hour + ("时") + minute + ("分");
            }
            else //分
            {
                minute = Convert.ToInt16(Math.Floor(t / 60));
                r = minute + ("分");
            }
            return r;
        }

        //本周周一日期，返回结果格式：2014-5-5 0:00:00

        public static DateTime GetMondayDate()
        {
            DateTime dt = DateTime.Now;

            int today = (int)dt.DayOfWeek;

            if (dt.DayOfWeek.ToString() != "Sunday")//也可以使用today!=0
            {
                return dt.AddDays(1 - today).Date;
            }
            else
            {
                return dt.AddDays(-6 - today).Date;//若今天是周日，获取到的周一日期是下周一的日期，所以要减去7天
            }
        }

        //本周日日期

        public static DateTime GetSundayDate()
        {

            DateTime dt = DateTime.Now;
            int today = (int)dt.DayOfWeek;

            if (dt.DayOfWeek.ToString() != "Sunday")//也可以使用today!=0
            {
                return dt.AddDays(7 - today).Date;
            }

            else
            {
                return dt.AddDays(-today).Date;//若今天是周日，获取到的周日日期是下周日的日期，所以要减去7天
            }
        }



        //上周一日期

        public static DateTime GetLastMondayDate()
        {

            DateTime dt = DateTime.Now;
            int today = (int)dt.DayOfWeek;

            if (dt.DayOfWeek.ToString() != "Sunday")//也可以使用today!=0
            {
                return dt.AddDays(-today - 6).Date;
            }

            else
            {
                return dt.AddDays(-today - 13).Date;//若今天是周日，获取到的上周一的日期是本周周一的日期，所以要减去7天

            }
        }



        //上周日日期

        public static DateTime GetLastSundayDate()
        {

            DateTime dt = DateTime.Now;
            int today = (int)dt.DayOfWeek;

            if (dt.DayOfWeek.ToString() != "Sunday")//也可以使用today!=0
            {
                return dt.AddDays(-today).Date;
            }

            else
            {
                return dt.AddDays(-today - 7).Date;//若今天是周日，获取到的上周日的日期是本周周日的日期，所以要减去7天
            }
        }

        /// <summary>
        /// 获取本周开始结束字符串
        /// </summary>
        /// <returns></returns>
        public static string  GetThisWeekString()
        {
            return $"{GetMondayDate().ToString("yyyyMMdd")}" + "-" + $"{GetSundayDate().ToString("yyyyMMdd")}";
        }

        /// <summary>
        /// 获取上周开始结束字符串
        /// </summary>
        /// <returns></returns>
        public static string GetLastWeekString()
        {
            return $"{GetLastMondayDate().ToString("yyyyMMdd")}" + "-" + $"{GetLastSundayDate().ToString("yyyyMMdd")}";
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
