///////////////////////////////////////////////////////////////////////////////////////
// File: ITBMWarehouseFileService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/5 13:37:22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Framework;
using YfCloud.Models;
using YfCloud.App.Module.BasicSet.Models.TBMWarehouseFile;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using YfCloud.DBModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.WebApi;

namespace YfCloud.App.Module.BasicSet.Service
{
    /// <summary>
    /// ITBMWarehouseFileService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITBMWarehouseFileService))]
    public class TBMWarehouseFileService : ITBMWarehouseFileService
    {
        private readonly IDbContext _db;//数据库操作实例对象
        private readonly ILogger<TBMWarehouseFileService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TBMWarehouseFileService(IDbContext dbContext, ILogger<TBMWarehouseFileService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取仓库编码
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetMaxID(int UserID)
        {
            var curentDB = _db.Instance;
            var smUserInfo = SMCurentUserManager.GetCurentUserID(UserID, curentDB);
            var result = curentDB.Queryable<TBMWarehouseFileDbModel>().Where(p => p.CompanyId == smUserInfo.CompanyId).Max(p => p.Code);
            int resultInt = Convert.ToInt32(result) + 1;

            return resultInt.ToString().PadLeft(4, '0'); //不足6位 补H
        }
        
        /// <summary>
        /// 获取T_BM_WarehouseFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，查询操作结果</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMWarehouseFileQueryModel, List<TBMWarehouseFileQueryModel>>> GetAsync(RequestObject<TBMWarehouseFileQueryModel> requestObject,int UserID)
        {
            try
            {
                var curentDB = _db.Instance;
                var smUserInfo = SMCurentUserManager.GetCurentUserID(UserID, curentDB);
                List<TBMWarehouseFileQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = curentDB.Queryable<TBMWarehouseFileDbModel, TSMUserAccountDbModel, TSMUserInfoDbModel>(
                                (t, t0, t1) => new object[]
                                {
                                    JoinType.Left,  t.PrincipalId== t0.ID,
                                    JoinType.Left,t0.UserInfoId==t1.ID
                                }).Where((t, t0, t1) => SqlFunc.IsNull(t.DeleteFlag, false) != true && t.CompanyId == smUserInfo.CompanyId);
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);

                    foreach (ConditionalModel item in conditionals)
                    {
                        item.FieldName = $"t.{item.FieldName}";
                    }
                    query.Where(conditionals);
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TBMWarehouseFileDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t0, t1) => new TBMWarehouseFileQueryModel
                        {
                            ID = t.ID,
                            Code = t.Code,
                            WarehouseName = t.WarehouseName,
                            WarehouseAddress = t.WarehouseAddress,
                            PrincipalId = t.PrincipalId,
                            RealName = t.PrincipalId == null ? "" : t0.AccountName,
                            Status = t.Status,
                            Remark = t.Remark,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                        })
                        .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t0, t1) => new TBMWarehouseFileQueryModel
                        {
                            ID = t.ID,
                            Code = t.Code,
                            WarehouseName = t.WarehouseName,
                            WarehouseAddress = t.WarehouseAddress,
                            PrincipalId = t.PrincipalId,
                            RealName = t.PrincipalId == null ? "" : t0.AccountName,
                            Status = t.Status,
                            Remark = t.Remark,
                            CompanyId = t.CompanyId,
                            DeleteFlag = t.DeleteFlag,
                        })
                        .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<TBMWarehouseFileQueryModel, List<TBMWarehouseFileQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TBMWarehouseFileQueryModel, List<TBMWarehouseFileQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_BM_WarehouseFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，新增操作结果</param>
        /// <param name="UserID">操作人ID</param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMWarehouseFileAddModel,bool>> PostAsync(RequestObject<TBMWarehouseFileAddModel> requestObject,int UserID)
        {
            try
            {
                var curentDB = _db.Instance;

               SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(UserID, curentDB);
                //如果没有新增数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<TBMWarehouseFileAddModel, bool>.FailResult(requestObject, false, "PostData,PostDataList不能都为null");
                var result = false;
                //批量新增的优先级高于单条数据新增，且只会执行一个新增操作
                if(requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    var addList = _mapper.Map<List<TBMWarehouseFileAddModel>, List<TBMWarehouseFileDbModel>>(requestObject.PostDataList);
                    foreach (var item in addList)
                    {
                        item.CompanyId = sMUserInfo.CompanyId.Value;
                    }

                    result = await curentDB.Insertable(addList).ExecuteCommandAsync() > 0;
                }
                else
                {
                    var addModel = _mapper.Map<TBMWarehouseFileDbModel>(requestObject.PostData);
                    addModel.CompanyId = sMUserInfo.CompanyId.Value;
                    addModel.DeleteFlag = false;

                    var oldModel = curentDB.Queryable<TBMWarehouseFileDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag,false) == false && p.WarehouseName == addModel.WarehouseName &&
                    p.CompanyId == sMUserInfo.CompanyId.Value).First();

                    if (oldModel != null)
                    {
                       return  ResponseUtil<TBMWarehouseFileAddModel, bool>.FailResult(requestObject, false, addModel.WarehouseName + " 已经存在");
                    }

                    var oldModelCode = curentDB.Queryable<TBMWarehouseFileDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false && p.Code == addModel.Code &&
                   p.CompanyId == sMUserInfo.CompanyId.Value).First();

                    if (oldModelCode != null)
                    {
                        return ResponseUtil<TBMWarehouseFileAddModel, bool>.FailResult(requestObject, false, "编号:"+addModel.Code + " 已经存在");
                    }

                    addModel.CompanyId = sMUserInfo.CompanyId.Value;

                    result = await _db.Instance.Insertable(addModel).ExecuteCommandAsync() > 0;
                }
                //返回执行结果
                if (result)
                    return ResponseUtil<TBMWarehouseFileAddModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMWarehouseFileAddModel, bool>.FailResult(requestObject, false, "新增数据失败!");
            }
            catch(Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMWarehouseFileAddModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_BM_WarehouseFile数据
        /// </summary>
        /// <param name="requestObject">返回响应结果对象，包括响应代码，修改操作结果</param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TBMWarehouseFileEditModel, bool>> PutAsync(RequestObject<TBMWarehouseFileEditModel> requestObject,int userID)
        {
            try
            {
                var curentDB = _db.Instance;

                SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(userID, curentDB);
                //执行结果
                var result = false;
                //没有修改信息，返回错误信息
                if (requestObject.PostDataList == null && requestObject.PostData == null)
                    return ResponseUtil<TBMWarehouseFileEditModel, bool>.FailResult(requestObject, false, "PostData不能都为null");
                //批量更新优先级高于单记录更新
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量更新
                    var editList = _mapper.Map<List<TBMWarehouseFileEditModel>, List<TBMWarehouseFileDbModel>>(requestObject.PostDataList);
                    result = await _db.Instance.Updateable<TBMWarehouseFileDbModel>(editList).IgnoreColumns(p=> new { p.DeleteFlag,p.Code,p.CompanyId }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单记录更新
                    var editModel = _mapper.Map<TBMWarehouseFileDbModel>(requestObject.PostData);

                    var oldModel = _db.Instance.Queryable<TBMWarehouseFileDbModel>().Where(p => SqlFunc.IsNull(p.DeleteFlag, false) == false &&
                    p.WarehouseName == editModel.WarehouseName && 
                    p.CompanyId == sMUserInfo.CompanyId.Value
                    &&p.ID!=editModel.ID
                    ).First();

                    if (oldModel != null)
                    {
                       return ResponseUtil<TBMWarehouseFileEditModel, bool>.FailResult(requestObject, false, editModel.WarehouseName + " 已经存在");
                    }

                    result = await _db.Instance.Updateable(editModel).IgnoreColumns(p => new { p.DeleteFlag, p.Code, p.CompanyId }).ExecuteCommandAsync() > 0;
                }
                
                //返回执行结果
                if (result)
                    return ResponseUtil<TBMWarehouseFileEditModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<TBMWarehouseFileEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<TBMWarehouseFileEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_BM_WarehouseFile数据
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject, CurrentUser user)
        {
            try
            {
                //执行结果
                var result = false;
                //没有删除数据，返回错误信息
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "PostData、PostDataList不能都为null");
                //批量删除的优先级高于单记录删除
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var ids = requestObject.PostDataList.Select(p => new TBMWarehouseFileDbModel() { ID =p.ID,DeleteFlag=true }).ToList();
                    result = await _db.Instance.Updateable<TBMWarehouseFileDbModel>(ids).UpdateColumns(p=>new {p.DeleteFlag }).ExecuteCommandAsync() > 0;
                }
                else
                {
                    TBMWarehouseFileDbModel itemDb = new TBMWarehouseFileDbModel() { ID = requestObject.PostData.ID, DeleteFlag = true };
                    //单记录删除
                    result = await _db.Instance.Updateable<TBMWarehouseFileDbModel>(itemDb).UpdateColumns(p => new { p.DeleteFlag }).ExecuteCommandAsync() > 0;
                }



                //返回执行结果
                if (result)
                    return ResponseUtil<DeleteModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //返回异常结果
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }
        
    }
}
