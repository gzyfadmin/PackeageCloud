using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.Framework.UnitChange
{

    /// <summary>
    /// 单位转换工具类
    /// </summary>
    public  class UnitChange
    {
        private static readonly List<Func<TBMMaterialFileCacheModel, decimal, decimal>> othersToBasic;
        private static readonly List<Func<TBMMaterialFileCacheModel, decimal, decimal>> basictoOters;

         static UnitChange()
        {
            othersToBasic = new List<Func<TBMMaterialFileCacheModel, decimal, decimal>>() { ProduceToBase, PurchaseToBase, SalesToBase, WarehouseToBase };
            basictoOters = new List<Func<TBMMaterialFileCacheModel, decimal, decimal>>() { BaseToProduce, BaseToPurchase, BaseToSales, BaseToWarehouse };
        }



        /// <summary>
        /// 生产单位数量转化为基本单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">生产数量</param>
        /// <returns></returns>
        private static decimal ProduceToBase(TBMMaterialFileCacheModel tBMMaterialFileCacheModel,decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.ProduceUnitId != null && tBMMaterialFileCacheModel.ProduceRate != null)
            {
                result = decimal.Round(Num * (decimal)tBMMaterialFileCacheModel.ProduceRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 采购单位数量转化为基本单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">采购数量</param>
        /// <returns></returns>
        private static decimal PurchaseToBase(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.PurchaseUnitId != null && tBMMaterialFileCacheModel.PurchaseRate != null)
            {
                result = decimal.Round(Num * (decimal)tBMMaterialFileCacheModel.PurchaseRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 销售数量转化为基本单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">销售数量</param>
        /// <returns></returns>
        private static decimal SalesToBase(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.SalesUnitId != null && tBMMaterialFileCacheModel.SalesRate != null)
            {
                result = decimal.Round(Num * (decimal)tBMMaterialFileCacheModel.SalesRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 销售数量转化为基本单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">仓库数量</param>
        /// <returns></returns>
        private static decimal WarehouseToBase(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.WarehouseUnitId != null && tBMMaterialFileCacheModel.WarehouseRate != null)
            {
                result = decimal.Round(Num * (decimal)tBMMaterialFileCacheModel.WarehouseRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 基本单位数量转化为生产单位数量 
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">基本单位数量</param>
        /// <returns></returns>
        private static decimal BaseToProduce(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.ProduceUnitId != null && tBMMaterialFileCacheModel.ProduceRate != null)
            {
                result = decimal.Round(Num / (decimal)tBMMaterialFileCacheModel.ProduceRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 基本单位数量 转化为采购单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">基本单位数量</param>
        /// <returns></returns>
        private static decimal BaseToPurchase(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.PurchaseUnitId != null && tBMMaterialFileCacheModel.PurchaseRate != null)
            {
                result = decimal.Round(Num / (decimal)tBMMaterialFileCacheModel.PurchaseRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        ///  基本单位数量转化为销售数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">基本单位数量</param>
        /// <returns></returns>
        private static decimal BaseToSales(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.SalesUnitId != null && tBMMaterialFileCacheModel.SalesRate != null)
            {
                result = decimal.Round(Num / (decimal)tBMMaterialFileCacheModel.SalesRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 基本单位数量转化为仓库单位数量
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel"></param>
        /// <param name="Num">基本单位数量</param>
        /// <returns></returns>
        private static decimal BaseToWarehouse(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, decimal Num)
        {
            decimal result = 0;

            if (tBMMaterialFileCacheModel.WarehouseUnitId != null && tBMMaterialFileCacheModel.WarehouseRate != null)
            {
                result = decimal.Round(Num / (decimal)tBMMaterialFileCacheModel.WarehouseRate, 4);
            }
            else
            {
                result = Num;
            }

            return result;
        }

        /// <summary>
        /// 单位转化
        /// </summary>
        /// <param name="tBMMaterialFileCacheModel">物料</param>
        /// <param name="from">原单位</param>
        /// <param name="to">目的单位</param>
        /// <param name="num">原单位数量</param>
        /// <returns>转化后的单位值</returns>
        public static decimal TranserUnit(TBMMaterialFileCacheModel tBMMaterialFileCacheModel, UnitType from, UnitType to, decimal num)
        {
            if (from == to)
            {
                return num;
            }
            else
            {
                if (from == UnitType.Unit)
                {
                    int index = (int)to;
                    return basictoOters[index](tBMMaterialFileCacheModel, num);
                }
                else if (to == UnitType.Unit)
                {
                    int index = (int)from;
                    return othersToBasic[index](tBMMaterialFileCacheModel, num);
                }
                else
                {
                    int index1 = (int)from;
                    int index2 = (int)to;

                    decimal basic= othersToBasic[index1](tBMMaterialFileCacheModel, num);
                    return basictoOters[index2](tBMMaterialFileCacheModel, basic);
                }
            }
        } 

    }

    /// <summary>
    /// 单位枚举
    /// </summary>
    public enum UnitType
    {
        /// <summary>
        /// 生产
        /// </summary>
        Produce=0,

        /// <summary>
        /// 采购
        /// </summary>
        Purchase=1,

        /// <summary>
        /// 销售
        /// </summary>
        Sales=2,

        /// <summary>
        /// 库存
        /// </summary>
        Warehouse =3,

        /// <summary>
        /// 基本单位
        /// </summary>
        Unit=99
    }
}
