using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Services
{
    
   public interface IDomain
    {
        List<TBMDomainQueryModel> GetTBMDomainQueryModel();

    }
}
