using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.Manufacturing.Models;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMain;
using YfCloud.App.Module.Manufacturing.Models.TMMColorSolutionMainNew;
using YfCloud.DBModel;
using YfCloud.Models;

namespace YfCloud.App.Module.Manufacturing.Services
{
    public interface ITMMColorSolutionMainNewService
    {
        /// <summary>
        /// 查询TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<DataTable>> GetAsync(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 新增TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PostAsync(RequestPost<TMMColorSolutionMainAddNewModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 修改TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseObject<bool>> PutAsync(RequestPut<TMMColorSolutionMainEditNewModel> requestObject, CurrentUser currentUser);

        /// <summary>
        /// 删除TMMColorSolutionMain数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject);

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="PackageID"></param>
        /// <param name="ColorSolutionID"></param>
        /// <returns></returns>
        TBMMaterialFileDbModel GetMaterialFileByPackageColor(int PackageID, int? ColorSolutionID);

        Task<ResponseObject<List<TMMColorSolutionMainQueryNewModel>>> GetColorSolution(RequestGet requestObject, CurrentUser currentUser);

        /// <summary>
        /// 包型配色方案树形结构
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        Task<ResponseObject<List<TMMPackageColorItem>>>  GetTMMPackageColorItem(CurrentUser current);
    }
}
