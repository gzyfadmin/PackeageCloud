using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Services
{
    /// <summary>
    /// 仓库统计服务
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IStaticInventory))]
    public class StaticInventory : IStaticInventory
    {
        private readonly IEnumerable<ICalcuInventory> _calcuInventory;
        private readonly IDbContext _db;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="calcuInventory"></param>
        /// <param name="dbContext"></param>
        public StaticInventory(IEnumerable<ICalcuInventory> calcuInventory, IDbContext dbContext)
        {
            _calcuInventory = calcuInventory;
            _db = dbContext;
        }

        /// <summary>
        /// 获取统计结果
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>
        public TWMCountModel GeTWMCountModel(TWMStaQuery tWMStaQuery)
        {
            TWMCountModel tWMCountModel = new TWMCountModel() { WarehouseId = tWMStaQuery.WarehouseId, MaterialId = tWMStaQuery.MaterialId };

            tWMCountModel.PrimeNum = GetPrimeNum(tWMStaQuery);

            //待入库数量
            _calcuInventory.ToList().ForEach(x =>
            {
                tWMCountModel.WaitWarehousingNum += x.CalcuToIn(tWMStaQuery);
                tWMCountModel.WaitOutWhNum += x.CalcuToOut(tWMStaQuery);
                tWMCountModel.InAndOut += x.CalcuInAndOut(tWMStaQuery);
            });

            tWMCountModel.AccountNum = tWMCountModel.PrimeNum + tWMCountModel.InAndOut;

            tWMCountModel.AvaiableNum = tWMCountModel.AccountNum - tWMCountModel.WaitOutWhNum;

            return tWMCountModel;

        }

        //计算期初数量
        private decimal GetPrimeNum(TWMStaQuery tWMStaQuery)
        {
            decimal primeNum = 0;

            var TWMPrimeCountDbList = _db.Instance.Queryable<TWMPrimeCountDbModel>().Where(p => p.MaterialId == tWMStaQuery.MaterialId && p.WarehouseId == tWMStaQuery.WarehouseId).ToList();

            if (TWMPrimeCountDbList.FirstOrDefault() != null)
            {
                return TWMPrimeCountDbList.FirstOrDefault().PrimeNum;
            }

            return primeNum;
        }
    }
}
