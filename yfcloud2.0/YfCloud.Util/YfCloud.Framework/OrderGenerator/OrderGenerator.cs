using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.Framework.OrderGenerator
{
    /// <summary>
    /// 单号生成
    /// </summary>
    public static class OrderGenerator
    {
        /// <summary>
        /// 生成单号
        /// </summary>
        /// <param name="orderEnum">单号类型</param>
        /// <param name="CompanyID">企业ID</param>
        /// <returns></returns>
        public static string Generator(OrderEnum orderEnum, int CompanyID)
        {
            switch (orderEnum)
            {
                case OrderEnum.OWR:
                    return OWRGenerator.CreateNo(CompanyID);
                case OrderEnum.OWS:
                    return OWSGenerator.CreateNo(CompanyID);
                case OrderEnum.IC:
                    return ICGenerator.CreateNo(CompanyID);
                case OrderEnum.IS:
                    return ISGenerator.CreateNo(CompanyID);
                case OrderEnum.IR:
                    return IRGenerator.CreateNo(CompanyID);
                case OrderEnum.SOWR:
                    return SOWRGenerator.CreateNo(CompanyID);
                case OrderEnum.SO:
                    return SOGenerator.CreateNo(CompanyID);
                case OrderEnum.PO:
                    return POGenerator.CreateNo(CompanyID);
                case OrderEnum.WO:
                    return WOGenerator.CreateNo(CompanyID);
                case OrderEnum.MO:
                    return MOGenerator.CreateNo(CompanyID);

                case OrderEnum.PMR:
                    return PMRGenerator.CreateNo(CompanyID);
                case OrderEnum.PDR:
                    return PDRGenerator.CreateNo(CompanyID);
                case OrderEnum.PR:
                    return PRGenerator.CreateNo(CompanyID);
                case OrderEnum.MR:
                    return MRGenerator.CreateNo(CompanyID);
                case OrderEnum.PDC:
                    return PDCGenerator.CreateNo(CompanyID);
                default:
                    return "";
            }
        }
    }
}
