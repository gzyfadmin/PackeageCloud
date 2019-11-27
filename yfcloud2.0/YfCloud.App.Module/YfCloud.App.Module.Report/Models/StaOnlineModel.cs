using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Utilities.Date;

namespace YfCloud.App.Module.Report.Models
{
    public class StaOnlineModel
    {
        /// <summary>
        /// 在线时长
        /// </summary>
        public double TimeSpend { get; set; }

        /// <summary>
        /// 在线时长 某小时某分钟
        /// </summary>
        public string TimeStr { get{ return DateUtil.parseTimeHM(TimeSpend); } } 

        /// <summary>
        /// 排名
        /// </summary>
        public long Place { get; set; }

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int? Times { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 用户图像
        /// </summary>
        public string HeadPicPath { get; set; }
    }

    public class StaOnlineResult
    {
        /// <summary>
        /// 当前用户的本周统计
        /// </summary>
        public  StaOnlineModel CurentStaTW{ get; set; }

        /// <summary>
        /// 当前用户的上周统计
        /// </summary>
        public StaOnlineModel CurentStaLW { get; set; }
        
        /// <summary>
        /// 本周排名
        /// </summary>
        public IList<StaOnlineModel> TopPlace { get; set; }

        /// <summary>
        /// 用户总数
        /// </summary>
        public int TotalUser { get; set; }
    }
}
