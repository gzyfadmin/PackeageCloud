using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Platform.Models;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Services
{
    /// <summary>
    /// 数据字典服务接口
    /// </summary>
    public interface IDictionaryValuesService
    {
        /// <summary>
        /// 根据key查询对应的value，返回value集合
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TDataDicDetailModel, List<TDataDicDetailModel>>> GetAsync(RequestObject<TDataDicDetailModel> requestObject);
    }
}
