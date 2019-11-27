using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using YfCloud.App.Module.Platform.Models;
using YfCloud.Models;
using YfCloud.Framework;
using SqlSugar;
using YfCloud.Utilities.SqlSugar;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Services
{
    /// <summary>
    /// IValuesService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IDictionaryValuesService))]
    public class DictionaryValuesService : IDictionaryValuesService
    {
        private readonly IDbContext _db;//数据库操作实例对象

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public DictionaryValuesService(IDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// 根据key查询对应的value，返回value集合
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>> GetAsync(RequestObject<TDataDicDetailModel> requestObject)
        {
            try
            {
                List<TDataDicDetailModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TDataDicDetailModel, TDataDicMainModel>(
                                (t, t0) => new object[]
                                {
                                    JoinType.Left,  t.KeyId == t0.Id
                                });
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var expressionList = SqlSugarUtil.GetQueryExpressions(requestObject.QueryConditions);
                    expressionList.ForEach(p => query.Where($"t.{p}"));
                }

                //设置多表查询返回实体类
                query.Select((t, t0) => new TDataDicDetailModel
                {
                    Id = t.Id,
                    KeyId = t0.Id,
                    Value = t.Value
                });

                //执行查询
                queryData = requestObject.IsPaging
                    ? await query.ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber)
                    : await query.ToListAsync();

                //返回执行结果
                return new ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>()
                {
                    TotalNumber = totalNumber,
                    Data = queryData,
                    Info = "执行成功",
                    Code = 0,
                    IsPaging = requestObject.IsPaging,
                    PageIndex = requestObject.PageIndex,
                    PageSize = requestObject.PageSize,
                    PostData = null,
                    QueryConditions = requestObject.QueryConditions,
                    OrderByConditions = requestObject.OrderByConditions,
                    PostDataList = null
                };
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return new ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>()
                {
                    TotalNumber = -1,
                    Data = null,
                    Info = ex.Message,
                    Code = -1,
                    IsPaging = requestObject.IsPaging,
                    PageIndex = requestObject.PageIndex,
                    PageSize = requestObject.PageSize,
                    PostData = null,
                    QueryConditions = requestObject.QueryConditions,
                    OrderByConditions = requestObject.OrderByConditions,
                    PostDataList = null
                };
            }
        }
    }
}
