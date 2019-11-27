using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Warehouse.Services.Inventory
{
    /// <summary>
    /// 生产出入库
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ICalcuInventory))]
    public class ProductAmount: ICalcuInventory
    {
        private readonly IDbContext _db;//数据库实例对象

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ProductAmount(IDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 出入库数量
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>

        public decimal CalcuInAndOut(TWMStaQuery tWMStaQuery)
        {
            decimal tradeNum = 0; //入库和出库的差
            tradeNum = _db.Instance.Queryable<TWMProductionCountDbModel>().Where(p => p.MaterialId == tWMStaQuery.MaterialId && p.WarehouseId == tWMStaQuery.WarehouseId).
                Sum(p => SqlFunc.IsNull(p.WhNumber, 0) - SqlFunc.IsNull(p.WhSendNumber, 0));
            
            return tradeNum;
        }

        /// <summary>
        /// 计算待入库数量
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>
        public decimal CalcuToIn(TWMStaQuery tWMStaQuery)
        {
            var details = _db.Instance.Queryable<TWMProductionWhMainDbModel, TWMProductionWhDetailDbModel, TBMMaterialFileDbModel>((t1, t2, t3) =>
             new object[] {
                   JoinType.Inner,
                   t1.ID == t2.MainId,
                   JoinType.Inner,
                   t2.MaterialId == t3.ID
             }).Where((t1, t2, t3) => t1.AuditStatus != 2 && t2.WarehouseId == tWMStaQuery.WarehouseId &&
             t2.MaterialId == tWMStaQuery.MaterialId && t1.DeleteFlag == false
             );


            //待入库数量
            decimal ToSendNum = details.Sum((t1, t2, t3) => t2.ActualNum);

            return ToSendNum;
        }

        /// <summary>
        /// 计算待出库
        /// </summary>
        /// <param name="tWMStaQuery"></param>
        /// <returns></returns>
        public decimal CalcuToOut(TWMStaQuery tWMStaQuery)
        {
            var details = _db.Instance.Queryable<TWMProductionMainDbModel, TWMProductionDetailDbModel, TBMMaterialFileDbModel>((t1, t2, t3) =>
             new object[] {
                   JoinType.Inner,
                   t1.ID == t2.MainId,
                   JoinType.Inner,
                   t2.MaterialId == t3.ID
             }).Where((t1, t2, t3) =>
             t1.AuditStatus != 2 && t2.WarehouseId == tWMStaQuery.WarehouseId
             && t2.MaterialId == tWMStaQuery.MaterialId && t1.DeleteFlag == false);

            if (tWMStaQuery.EditID != null && tWMStaQuery.OperateType == OperateEnum.Product)
            {
                details = details.Where((t1, t2, t3) => t1.ID != tWMStaQuery.EditID);
            }

            //待出库数量
            decimal ToSendNum = details.Sum((t1, t2, t3) => t2.ActualNum);

            return ToSendNum;
        }
    }
}
