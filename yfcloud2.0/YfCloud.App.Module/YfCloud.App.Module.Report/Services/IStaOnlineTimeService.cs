using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Report.Models;
using YfCloud.App.Module.Warehouse.Models.TPMDoc;
using YfCloud.Models;

namespace YfCloud.App.Module.Report.Services
{
   public  interface IStaOnlineTime
    {
        Task<ResponseObject<StaOnlineResult>> GetStaOnline(CurrentUser currentUser);

        Task<ResponseObject<TPMDocQueryModel>> GetDoc();
    }
} 
