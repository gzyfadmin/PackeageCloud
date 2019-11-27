using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Services
{
   public interface IChinaAreaService
    {
        List<ChinaAreaRecord> GetAllDistricts();

        List<ChinaAreaRecord> GetAreas(string cityCode);

        List<ChinaAreaRecord> GetDistrictsByLevel(int level);

        /// <summary>
        /// 树形地区
        /// </summary>
        /// <returns></returns>
        List<ChinaAreaChildRecord> GetAllTreeDistricts();
    }
}
