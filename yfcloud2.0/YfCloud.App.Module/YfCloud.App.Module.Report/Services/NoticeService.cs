using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SqlSugar;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.Caches;
using YfCloud.DBModel;
using YfCloud.DBModel.System;
using YfCloud.Framework.Orm;
using YfCloud.Models;
using YfCloud.Models.CacheModels;
using YfCloud.Utilities.MongoDb;
using YfCloud.Utilities.MongoDbModel;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.Report.Services
{
    /// <summary>
    /// 通知
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(INoticeService))]
    public class NoticeService:INoticeService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<INoticeService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        /// <param name="log"></param>
        /// <param name="mapper"></param>
        public NoticeService(IDbContext db, ILogger<NoticeService> log, IMapper mapper)
        {
            _db = db;
            _log = log;
            _mapper = mapper;
        }


        public ResponseObject<List<ToDoMgModel>> GetToDoModel(RequestGet requestObject, CurrentUser currentUser)
        {
            ResponseObject<List<ToDoMgModel>> responseObject = new ResponseObject<List<ToDoMgModel>>();
            responseObject.Code = 0;

            try
            {
                List<ToDoMgModel> result = new List<ToDoMgModel>();

                Expression<Func<ToDoMgModel, bool>> queryConditionLam = (x) => x.To==currentUser.UserID &&x.CompanyID==currentUser.CompanyID;

                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalFull(requestObject.QueryConditions).Where(p => !string.IsNullOrWhiteSpace(p.FieldValue)).ToList();

                    var whereConditional = ConditionalModelToExpression.BuildExpression<ToDoMgModel>(conditionals);
                    queryConditionLam = queryConditionLam.And(whereConditional);
                }

                SortDefinition<ToDoMgModel> sort=null;

                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetExpression<ToDoMgModel>(item.Column);
                       
                        if (exp == null) continue;
                        if (item.Condition.ToLower() == "asc")
                        {
                            if (sort == null)
                            {

                                sort = Builders<ToDoMgModel>.Sort.Ascending(exp);
                            }
                            else
                            {
                                sort = sort.Ascending(exp);
                            }
                           
                        }
                        else if(item.Condition.ToLower() == "desc")
                        {
                            if (sort == null)
                            {

                                sort = Builders<ToDoMgModel>.Sort.Descending(exp);
                            }
                            else
                            {
                                sort = sort.Descending(exp);
                            }
                        }
                    }
                }

                

                long totalNum = 0;

                if (requestObject.IsPaging == true)
                {

                    result = MongoDbUtil.GetDoc<ToDoMgModel>(queryConditionLam, requestObject.PageIndex, requestObject.PageSize, sort, ref totalNum);
                }
                else
                {
                    result = MongoDbUtil.GetDoc<ToDoMgModel>(queryConditionLam, sort).ToList();
                }

                return ResponseUtil<List<ToDoMgModel>>.SuccessResult(result,totalNum);
               
            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ToDoMgModel>>.FailResult(null, ex.Message);
            }

        }

        public ResponseObject<bool> Read(string Id, CurrentUser currentUser)
        {
            try
            {
                ToDoMgModel loginLog = MongoDbUtil.GetDoc<ToDoMgModel>(p => p.ToDoId == Id, null).FirstOrDefault();

                if (loginLog != null)
                {
                    UpdateDefinition<ToDoMgModel> update = Builders<ToDoMgModel>.
                        Update.Set(y => y.IsRead, true);
                    MongoDbUtil.UpdateOne<ToDoMgModel>(p => p.ToDoId == Id && p.CompanyID == currentUser.CompanyID, update);
                }

                return ResponseUtil<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"{ex.Message}");
            }
        }
    }
}
