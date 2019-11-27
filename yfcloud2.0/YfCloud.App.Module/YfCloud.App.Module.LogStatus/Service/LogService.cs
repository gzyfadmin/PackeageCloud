using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using YfCloud.App.Module.LogStatus.Controllers;
using YfCloud.App.Module.LogStatus.Model;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.Framework;
using YfCloud.Framework.WebApi;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.MongoDb;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.LogStatus.Service
{
    /// <summary>
    /// 日志
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ILogService))]
    public class LogService : ILogService
    {
        private readonly IDbContext _db;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dbContext"></param>
        public LogService(IDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ResponseObject<LogQueryModel, List<OperateLog>> Get(RequestObject<LogQueryModel> requestObject, int UserId)
        {
            try
            {
                long totalNum = -1;
                SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(UserId, _db.Instance);

                List<OperateLog> result = new List<OperateLog>();

                Expression<Func<OperateLog, bool>> pression = p => p.CompanyID == sMUserInfo.CompanyId.Value;

                var queryEntiy = requestObject.PostData;
                if (queryEntiy != null)
                {
                    if (!string.IsNullOrEmpty(queryEntiy.Account))
                    {
                        pression = pression.And(p => p.Account.Contains(queryEntiy.Account));
                    }
                    if (!string.IsNullOrEmpty(queryEntiy.Path))
                    {
                        pression = pression.And(p => p.Path.Contains(queryEntiy.Path));
                    }

                    if (!string.IsNullOrEmpty(queryEntiy.RealName))
                    {
                        pression = pression.And(p => p.RealName.Contains(queryEntiy.RealName));
                    }

                    if (!string.IsNullOrEmpty(queryEntiy.IpAddress))
                    {
                        pression = pression.And(p => p.IpAddress == queryEntiy.IpAddress);
                    }

                    if (queryEntiy.CreateTimeBg != null)
                    {
                        pression = pression.And(p => p.CreateTime >= queryEntiy.CreateTimeBg.Value);
                    }

                    if (queryEntiy.CreateTimeEd != null)
                    {
                        DateTime eg = queryEntiy.CreateTimeEd.Value.AddDays(1);
                        pression = pression.And(p => p.CreateTime < eg);
                    }
                }
                var sort = Builders<OperateLog>.Sort.Descending(y => y.CreateTime);
                if (requestObject.IsPaging == true)
                {

                    result = MongoDbUtil.GetDoc<OperateLog>(pression, requestObject.PageIndex, requestObject.PageSize, sort, ref totalNum);
                }
                else
                {
                    result = MongoDbUtil.GetDoc<OperateLog>(pression, sort).ToList();
                }


                return ResponseUtil<LogQueryModel, List<OperateLog>>.SuccessResult(requestObject, result, totalNum);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<LogQueryModel, List<OperateLog>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ResponseObject<LoginLogQuery, List<LoginLog>> GetLoginLog(RequestObject<LoginLogQuery> requestObject, int UserId)
        {
            try
            {
                long totalNum = -1;
                SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(UserId, _db.Instance);

                List<LoginLog> result = new List<LoginLog>();

                Expression<Func<LoginLog, bool>> pression = p => p.CompanyID == sMUserInfo.CompanyId.Value;

                var queryEntiy = requestObject.PostData;
                if (queryEntiy != null)
                {

                    if (!string.IsNullOrEmpty(queryEntiy.Account))
                    {
                        pression = pression.And(p => p.Account.Contains(queryEntiy.Account));
                    }
                    if (queryEntiy.Description!=null)
                    {
                        pression = pression.And(p => p.Description==queryEntiy.Description);
                    }

                    if (!string.IsNullOrEmpty(queryEntiy.RealName))
                    {
                        pression = pression.And(p => p.RealName.Contains(queryEntiy.RealName));
                    }

                    if (!string.IsNullOrEmpty(queryEntiy.IpAddress))
                    {
                        pression = pression.And(p => p.IpAddress == queryEntiy.IpAddress);
                    }

                    if (queryEntiy.LoginTimeBg != null)
                    {
                        pression = pression.And(p => p.LoginTime >= queryEntiy.LoginTimeBg.Value);
                    }

                    if (queryEntiy.LoginTimeEd != null)
                    {
                        DateTime eg = queryEntiy.LoginTimeEd.Value.AddDays(1);
                        pression = pression.And(p => p.LoginTime < eg);
                    }

                    if (queryEntiy.Status != null)
                    {
                        pression = pression.And(p => p.Status == queryEntiy.Status);
                    }
                }

                var sort = Builders<LoginLog>.Sort.Descending(y => y.LoginTime);
                if (requestObject.IsPaging == true)
                {
                    
                    result = MongoDbUtil.GetDoc<LoginLog>(pression, requestObject.PageIndex, requestObject.PageSize, sort, ref totalNum);
                }
                else
                {
                    result = MongoDbUtil.GetDoc<LoginLog>(pression, sort).OrderByDescending(p => p.LoginTime).ToList();
                }

                return ResponseUtil<LoginLogQuery, List<LoginLog>>.SuccessResult(requestObject, result, totalNum);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<LoginLogQuery, List<LoginLog>>.FailResult(requestObject, null, ex.Message);
            }
        }
    }
}
