using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using YfCloud.App.Module.Report.Models;
using YfCloud.Framework;
using YfCloud.App.Module.Warehouse.Models.TPMDoc;
using YfCloud.Attributes;
using YfCloud.Caches;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.Date;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 在线排名统计
    /// </summary>
   [UseDI(ServiceLifetime.Scoped, typeof(IStaOnlineTime))]
    public class StaOnlineTimeService : IStaOnlineTime
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IStaOnlineTime> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        /// <param name="log"></param>
        /// <param name="mapper"></param>
        public StaOnlineTimeService(IDbContext db, ILogger<StaOnlineTimeService> log, IMapper mapper)
        {
            _db = db;
            _log = log;
            _mapper = mapper;
        }

        public async Task<ResponseObject<StaOnlineResult>> GetStaOnline(CurrentUser currentUser)
        {
            StaOnlineResult staOnlineResult = new StaOnlineResult();

            try
            {
                var userList = await _db.Instance.Queryable<TSMUserAccountDbModel, TSMUserInfoDbModel>(
                    (t1, t2) => new object[] { JoinType.Left, t1.UserInfoId == t2.ID }
                    ).Where((t1, t2) => t1.CompanyId == currentUser.CompanyID && t1.Status == 1).Select((t1, t2) => new { acount = t1, info = t2 }).ToListAsync();

                var redis = CacheFactory.Instance(CacheType.Redis) as RedisCache;

                string thisWeek = DateUtil.GetThisWeekString();
                string strV = string.Format(CacheKeyString.StaOnlineUser, currentUser.CompanyID, thisWeek);
                string strTimeV = string.Format(CacheKeyString.StaOnlineTimes, currentUser.CompanyID, thisWeek);

                staOnlineResult.TotalUser = userList.Count();
                staOnlineResult.TopPlace = new List<StaOnlineModel>();

                //本周
                List<byte[]> placeList = redis.ZScan(strV, 0);

                bool isFindSelf = false;
                int place = 1;
                for (int i = placeList.Count - 1; i > 0; i = i - 2)
                {

                    if (staOnlineResult.TopPlace.Count() >= 5 && isFindSelf)
                    {
                        break;
                    }

                    string userID = Encoding.UTF8.GetString(placeList[i - 1]); //姓名

                    if (staOnlineResult.TopPlace.Count() < 5)
                    {
                        StaOnlineModel staOnlineModel = new StaOnlineModel();
                        staOnlineModel.TimeSpend = Convert.ToDouble(Encoding.UTF8.GetString(placeList[i]));//时长

                        var userA= userList.Where(p => p.acount.ID.ToString() == userID).FirstOrDefault();
                        if (userA != null)
                        {
                            staOnlineModel.AccountName = userA.acount?.AccountName;
                            staOnlineModel.HeadPicPath= userA.info?.HeadPicPath;
                        }

                       
                        staOnlineModel.Place = place;

                        List<byte[]> placeOrder = redis.ZScan(strTimeV, 0, 1, userID);
                        if (placeOrder.Count() > 1)
                        {
                            staOnlineModel.Times = Convert.ToInt32(Encoding.UTF8.GetString(placeOrder[1]));//次数
                        }

                        staOnlineResult.TopPlace.Add(staOnlineModel);
                    }

                    if (!isFindSelf)
                    {
                        if (userID == currentUser.UserID.ToString())
                        {
                            isFindSelf = true;

                            StaOnlineModel staOnlineModel = new StaOnlineModel();
                            staOnlineModel.TimeSpend = Convert.ToDouble(Encoding.UTF8.GetString(placeList[i]));//时长
                            staOnlineModel.Place = place;

                            staOnlineResult.CurentStaTW = staOnlineModel;
                        }
                    }

                    place++;
                }

                if (!isFindSelf)//如果redis没有
                {
                    StaOnlineModel staOnlineModel = new StaOnlineModel();
                    staOnlineModel.TimeSpend = 0;//时长
                    staOnlineModel.Place = place;

                    staOnlineResult.CurentStaTW = staOnlineModel;
                }

                //上周

                string lastWeek = DateUtil.GetLastWeekString();
                string laststrV = string.Format(CacheKeyString.StaOnlineUser, currentUser.CompanyID, lastWeek);
                string laststrTimeV = string.Format(CacheKeyString.StaOnlineTimes, currentUser.CompanyID, lastWeek);

                long lastPlace = redis.Client.GetItemIndexInSortedSetDesc(laststrV, currentUser.UserID.ToString());

                if (lastPlace == -1)
                {
                    StaOnlineModel staOnlineModel = new StaOnlineModel();
                    staOnlineModel.TimeSpend = 0;//时长

                    staOnlineModel.Place = staOnlineResult.TotalUser;

                    staOnlineResult.CurentStaLW = staOnlineModel;
                }
                else
                {
                    StaOnlineModel staOnlineModel = new StaOnlineModel();

                    List<byte[]> placeOrder = redis.ZScan(laststrTimeV, 0, 1, currentUser.UserID.ToString());
                    if (placeOrder.Count() > 1)
                    {
                        staOnlineModel.Times = Convert.ToInt32(Encoding.UTF8.GetString(placeOrder[1]));//次数
                    }

                    staOnlineModel.Place = lastPlace + 1;

                    staOnlineResult.CurentStaLW = staOnlineModel;
                }

                return ResponseUtil<StaOnlineResult>.SuccessResult(staOnlineResult, 1);
            }
            catch (Exception ex)
            {
                return ResponseUtil<StaOnlineResult>.FailResult(null,ex.Message);
            }



        }

        public async Task<ResponseObject<TPMDocQueryModel>> GetDoc()
        {
            try
            {
                TPMDocQueryModel result = await _db.Instance.Queryable<TPMDocDbModel>().
                    Select(p => new TPMDocQueryModel() { DocName = p.DocName, DocPath = p.DocPath, Version = p.Version }).FirstAsync();

                return ResponseUtil<TPMDocQueryModel>.SuccessResult(result, 1);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TPMDocQueryModel>.FailResult(null, ex.Message);
            }
            
        }
    }
}
