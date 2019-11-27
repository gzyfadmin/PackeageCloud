using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.Caches;
using YfCloud.DBModel;
using YfCloud.Models;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.App.Module.BasicSet.Services
{
    [UseDI(ServiceLifetime.Scoped, typeof(IChinaAreaService))]
    public class ChinaAreaService : IChinaAreaService
    {
        private readonly IDbContext _db;//数据库操作实例对象

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public ChinaAreaService(IDbContext dbContext)
        {
            _db = dbContext;
        }


        /// <summary>
        /// 获取所有的地区
        /// </summary>
        /// <returns></returns>
        public List<ChinaAreaRecord> GetAllDistricts()
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            return redis.GetValueByKey<List<ChinaAreaRecord>>(CacheKeyString.ChinaArea, () => {
                return _db.Instance.Queryable<TBMChinaAreaRecordDbModel>().Select(p => new ChinaAreaRecord()
                {
                    ID = p.ID,
                    Code = p.Code,
                    Level = p.Level,
                    Name = p.Name,
                    FullName=p.FullName,
                    ParentArea_id = p.ParentAreaid

                }).ToList();
            });
        }

        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <returns></returns>

        public List<ChinaAreaChildRecord> GetAllTreeDistricts()
        {
            var redis = CacheFactory.Instance(CacheType.Redis);

            return redis.GetValueByKey<List<ChinaAreaChildRecord>>(CacheKeyString.ChinaAreaTree, () => {
                List<ChinaAreaChildRecord> resultTemp = new List<ChinaAreaChildRecord>();
                var allList = GetAllDistricts();
                resultTemp.AddRange(GetMenuTree(allList, -1));

                return resultTemp;
            });
        }

        private List<ChinaAreaChildRecord> GetMenuTree(List<ChinaAreaRecord> aimData, int pid)
        {
            List<ChinaAreaChildRecord> tree = new List<ChinaAreaChildRecord>();
            var children = aimData.Where(p => p.ParentArea_id == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentArea_id == pid).OrderBy(p => p.Code))
                {
                    ChinaAreaChildRecord node = ExpressionGenericMapper<ChinaAreaRecord, ChinaAreaChildRecord>.Trans(item);
                    node.Children = GetMenuTree(aimData, item.ID);
                    tree.Add(node);
                }
            }
            return tree;
        }

        private List<ChinaAreaRecord> _allDistricts;

        /// <summary>
        /// 获取所有的地区
        /// </summary>
        public List<ChinaAreaRecord> AllDistricts
        {
            get
            {
                if (_allDistricts == null)
                    _allDistricts = GetAllDistricts();
                return _allDistricts;
            }
        }

        /// <summary>
        /// 根据地区编码获取下级地区
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public List<ChinaAreaRecord> GetAreas(string cityCode)
        {
            var city = AllDistricts.FirstOrDefault(c => c.Code == cityCode);

            if (city == null)
                return new List<ChinaAreaRecord>();

            return AllDistricts
                .Where(c => c.ParentArea_id == city.ID)
                .ToList();
        }

        /// <summary>
        /// 根据级别获取地区
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<ChinaAreaRecord> GetDistrictsByLevel(int level)
        {
            return AllDistricts.Where(c => c.Level == level).ToList();
        }
    }
}
