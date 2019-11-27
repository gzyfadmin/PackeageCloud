using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Services
{
    /// <summary>
    /// 用户维护接口
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// 查询用户表
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> GetAsync(RequestObject<TSMUserAccountQueryAllModel> requestObject,int UserID);



        /// <summary>
        /// 新增TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountAddAllModel, bool>> PostAsync(RequestObject<TSMUserAccountAddAllModel> requestObject,int UserID);

        /// <summary>
        /// 修改TSMCompany数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountEditAllModel, bool>> PutAsync(RequestObject<TSMUserAccountEditAllModel> requestObject);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountPassWord, bool>> ModifyPassWordAsync(RequestObject<TSMUserAccountPassWord> requestObject,int UserID);

        /// <summary>
        /// 删除TSMCompany数据，通过主表主键删除主从数据
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int, bool>> DeleteAsync(RequestObject<int> requestObject);

        /// <summary>
        /// 删除TSMCompany数据，通过主表主键删除主从数据，批量删除
        /// </summary>
        /// <param name="requestObject">响应结果对象</param>
        /// <returns></returns>
        Task<ResponseObject<int[], bool>> DeleteAsync(RequestObject<int[]> requestObject);

        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountEditAllModel, bool>> PersonalSetAsync(RequestObject<TSMUserAccountEditAllModel> requestObject, int UserID);

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountEmail, bool>> ModifyEmailAsync(RequestObject<TSMUserAccountEmail> requestObject, int UserID);
        
        /// <summary>
        /// 修改手机
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountMobile, int>> ModifyMobileAsync(RequestObject<TSMUserAccountMobile> requestObject, int UserID);

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TSMUserAccountQueryAllModel, List<TSMUserAccountQueryAllModel>>> GetCurentAsync(RequestObject<TSMUserAccountQueryAllModel> requestObject, int UserID);

        /// <summary>
        /// 获取当前用户公司的员工
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<ResponseObject<TUserDeatail, List<TUserDeatail>>> GetUserInCurentCompany(RequestObject<TUserDeatail> requestObject, int UserID);
    }
}
