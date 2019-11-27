using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.Models.CacheModels;
using YfCloud.Utilities.MongoDbModel;

namespace YfCloud.App.Module.Report.Services
{
    public interface INoticeService
    {
        /// <summary>
        /// 获取待处理的通知
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        ResponseObject<List<ToDoMgModel>> GetToDoModel(RequestGet requestObject, CurrentUser currentUser);

        ResponseObject<bool> Read(string Id, CurrentUser currentUser);
    }
}
