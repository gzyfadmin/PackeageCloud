using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.App.Module.LogStatus.Service
{
    public interface ICheckLoginStatusService
    {
        void RefresUserStatus(CurrentUser currentUser);
    }
}
