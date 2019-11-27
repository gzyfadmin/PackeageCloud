using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Services
{

    /// <summary>
    /// 站点配置
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IDomain))]
    public class Domain : IDomain
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<IDomain> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public Domain(IDbContext dbContext, ILogger<IDomain> logger, IMapper mapper)
        {
            _db = dbContext;
        }

        /// <summary>
        /// 获取所有的站点配置信息
        /// </summary>
        /// <returns></returns>
        public List<TBMDomainQueryModel> GetTBMDomainQueryModel()
        {
          #if DEBUG


            TBMDomainQueryModel item1 = new TBMDomainQueryModel() { Name = "平台", NameSpace = "YfCloud.App.Module.Platform", Url = "http://localhost:32967" };
            TBMDomainQueryModel item2 = new TBMDomainQueryModel() { Name = "基础数据", NameSpace = "YfCloud.App.Module.BasicSet", Url = "http://localhost:38888" };

            List<TBMDomainQueryModel> tBMDomainQueryModels = new List<TBMDomainQueryModel>() {  item1,item2 };

            return tBMDomainQueryModels;

            #else

             return _db.Instance.Queryable<TBMDomainDbModel>().Select(p => new TBMDomainQueryModel() {
                 ID=p.ID,
                 Name=p.Name,
                 NameSpace=p.NameSpace,
                 Url=p.Url,
            }).ToList();

            #endif
        }
    }
}
