using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.OrderGenerator
{
    ///// <summary>
    ///// 其他入库单号生成
    ///// </summary>
    //public  class OWRGenerator
    //{
    //    private static string _NowDate = "";
    //    private static int _sno = 1;
    //    //private static object obj = new object();

    //    /// <summary>
    //    /// 单号
    //    /// </summary>
    //    public static string No
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(_NowDate) || _NowDate != DateTime.Now.ToString("yyyyMMdd"))
    //            {
    //                _NowDate = DateTime.Now.ToString("yyyyMMdd");
    //                _sno = 1;
    //                return $"OWR{_NowDate}{_sno.ToString().PadLeft(4, '0')}";
    //            }
    //            else
    //            {
    //                _sno++;
    //                return $"OWR{_NowDate}{_sno.ToString().PadLeft(4, '0')}";
    //            }
    //        }
    //    }

    //}
}
