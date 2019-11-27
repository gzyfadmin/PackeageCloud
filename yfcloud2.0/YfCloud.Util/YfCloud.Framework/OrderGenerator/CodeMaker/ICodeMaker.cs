using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.Framework.OrderGenerator
{
   public interface ICodeMaker
    {
        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        string MakeNo(int CompanyID);

        /// <summary>
        /// 
        /// </summary>
        string ProvideName { get; }
    }
}
