using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models.TMMBOMMainNew;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITMMBOMMainNewService
    {
        /// <summary>
        /// 新增TMMBOMMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMBOMMainAddNewModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<TMMBOMMainQueryNewModel>> GetWholeMainData(int requestObject);
    }
}
