///////////////////////////////////////////////////////////////////////////////////////
// File: ITMMPickApplyMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/11 15:33:20
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////
using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyMain;
using YfCloud.App.Module.Manufacturing.Models.TMMPickApplyDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework;
using YfCloud.Framework.UnitChange;
using YfCloud.Framework.CacheManager;

namespace YfCloud.App.Module.Manufacturing.Service
{
    /// <summary>
    /// MRP算的生产领料单 用于生产出库的源单数据 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITMMPickApplyMainService))]
    public class TMMPickApplyMainService : ITMMPickApplyMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TMMPickApplyMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TMMPickApplyMainService(IDbContext dbContext, ILogger<TMMPickApplyMainService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_MM_PickApplyMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TMMPickApplyMainQueryModel>>> GetMainListAsync(RequestGet requestObject,CurrentUser currentUser)
        {
            try
            {
                List<TMMPickApplyMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TMMPickApplyMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel,TSMDeptDbModel,TMMProductionOrderMainDbModel>(
                                (t, t1, t2,t3,t4) => new object[]
                                {
                                    JoinType.Left, t.OperatorId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID,
                                    JoinType.Left,t.PickDeptID==t3.Id,
                                    JoinType.Left,t.SourceId==t4.ID
                                }).Where((t, t1, t2) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);
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
                if(requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TMMPickApplyMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t1, t2,t3,t4) => new TMMPickApplyMainQueryModel 
										{
											ID = t.ID,
											SourceId = t.SourceId,
											PickNo = t.PickNo,
											ApplyDate = t.ApplyDate,
											OperatorId = t.OperatorId,
                                            OperatorName =t1.AccountName,
                                            OperatorTime = t.OperatorTime,
											AuditId = t.AuditId,
                                            AuditName=t2.AccountName,
                                            AuditStatus = t.AuditStatus,
											AuditTime = t.AuditTime,
											CompanyId = t.CompanyId,
											DeleteFlag = t.DeleteFlag,
											TransferFlag = t.TransferFlag,
                                            PickDeptID=t.PickDeptID,
                                            PickDeptName =t3.DeptName,
                                            ProductionNO=t4.ProductionNo
                    })
                                            .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t1, t2,t3,t4) => new TMMPickApplyMainQueryModel
                    {
                        ID = t.ID,
                        SourceId = t.SourceId,
                        PickNo = t.PickNo,
                        ApplyDate = t.ApplyDate,
                        OperatorId = t.OperatorId,
                        OperatorName = t1.AccountName,
                        OperatorTime = t.OperatorTime,
                        AuditId = t.AuditId,
                        AuditName = t2.AccountName,
                        AuditStatus = t.AuditStatus,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        TransferFlag = t.TransferFlag,
                        PickDeptID = t.PickDeptID,
                        PickDeptName = t3.DeptName,
                        ProductionNO=t4.ProductionNo
                    }).ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TMMPickApplyMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TMMPickApplyMainQueryModel>>.FailResult(null, ex.Message);
            }
        }


        private async Task<TMMPickApplyMainQueryModel> GetMainData(int iMainId, CurrentUser currentUser,bool isHaveTrans)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TMMPickApplyMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMDeptDbModel>(
                               (t, t1, t2,t3) => new object[]
                               {
                                    JoinType.Left, t.OperatorId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID,
                                    JoinType.Left,t.PickDeptID==t3.Id
                               }).Where((t, t1, t2) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID)
                               .Select((t, t1, t2,t3) => new TMMPickApplyMainQueryModel
                               {
                                   ID = t.ID,
                                   SourceId = t.SourceId,
                                   PickNo = t.PickNo,
                                   ApplyDate = t.ApplyDate,
                                   OperatorId = t.OperatorId,
                                   OperatorName = t1.AccountName,
                                   OperatorTime = t.OperatorTime,
                                   AuditId = t.AuditId,
                                   AuditName = t2.AccountName,
                                   AuditStatus = t.AuditStatus,
                                   AuditTime = t.AuditTime,
                                   CompanyId = t.CompanyId,
                                   DeleteFlag = t.DeleteFlag,
                                   TransferFlag = t.TransferFlag,
                                   IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false,
                                   PickDeptID = t.PickDeptID,
                                   PickDeptName = t3.DeptName,
                               })
                                 .Where(t => t.ID == iMainId)
                                 .FirstAsync();

                var detailModels = await _db.Instance.Queryable<TMMPickApplyDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                                    (t, t0, t1, t2, t3, t4,t5) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID,
                                        JoinType.Left,t0.ProduceUnitId==t5.ID
                                    })
                                    .Select((t, t0, t1, t2, t3, t4, t5) => new TMMPickApplyDetailQueryModel
                                    {
                                        ID = t.ID,
                                        MainId = t.MainId,
                                        MaterialId = t.MaterialId,
                                        ApplyNum = t.ApplyNum,
                                        TransNum = t.TransNum,
                                        MaterialName = t0.MaterialName,
                                        MaterialCode = t0.MaterialCode,
                                        MaterialTypeId = t0.MaterialTypeId,
                                        MaterialTypeName = t1.TypeName,
                                        ColorId = t0.ColorId,
                                        ColorName = t2.DicValue,
                                        BaseUnitId = t0.BaseUnitId,
                                        BaseUnitName = t3.DicValue,
                                        WarehouseUnitId = t0.WarehouseUnitId,
                                        WarehouseUnitName = SqlFunc.IsNullOrEmpty(t4.ID) ? t3.DicValue : t4.DicValue,
                                        WarehouseRate = t0.WarehouseRate,
                                        Spec = t0.Spec,
                                        ProduceUnitId=t0.ProduceUnitId,
                                        ProduceUnitName=t5.DicValue,
                                        ProduceRate=t0.ProduceRate
                                    })
                                    .Where(t => t.MainId == iMainId).OrderBy(t=>t.ID)
                                    .ToListAsync();
                if (isHaveTrans)
                {
                    detailModels = detailModels.Where(p => p.TransNumWare > 0).ToList();
                }

                mainModel.ChildList = detailModels;
                return mainModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
        {
            var result = await GetMainData(iMainId, currentUser,false);

            return ResponseUtil<TMMPickApplyMainQueryModel>.SuccessResult(result);
        }

        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTransWareDate(int iMainId, CurrentUser currentUser)
        {
            var result = await GetMainData(iMainId, currentUser,true);

            return ResponseUtil<TMMPickApplyMainQueryModel>.SuccessResult(result);
        }

        /// <summary>
        /// 新增T_MM_PickApplyMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> PostAsync(RequestPost<TMMPickApplyMainAddModel> requestObject,CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TMMPickApplyMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TMMPickApplyMainDbModel>(requestObject.PostData);
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.AuditStatus = 0;
                mapMainModel.TransferFlag = true;

                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TMMPickApplyDetailAddModel>, List<TMMPickApplyDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                var Reresult = await GetMainData(mainId, currentUser,false);

                //返回执行结果
                return ResponseUtil<TMMPickApplyMainQueryModel>.SuccessResult(Reresult);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TMMPickApplyMainQueryModel>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 修改T_MM_PickApplyMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> PutAsync(RequestPut<TMMPickApplyMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {                
                if (requestObject.PostData == null)
                    return ResponseUtil<TMMPickApplyMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TMMPickApplyMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TMMPickApplyMainDbModel>(requestObject.PostData);
                var mainFlag = await currDb.Updateable(mainModel).UpdateColumns(p=>new { p.ApplyDate,p.PickDeptID }).ExecuteCommandAsync() > 0;

                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TMMPickApplyDetailEditModel>, 
                    List<TMMPickApplyDetailDbModel>>(requestObject.PostData.ChildList);
                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;                    
                    item.MainId = mainModel.ID;
                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();
                    detailFlag = currDb.Deleteable<TMMPickApplyDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                //提交事务
                currDb.CommitTran();

                var Reresult = await GetMainData(mainModel.ID, currentUser,false);
                //返回执行结果
                return ResponseUtil<TMMPickApplyMainQueryModel>.SuccessResult(Reresult) ;
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TMMPickApplyMainQueryModel>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 删除T_MM_PickApplyMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject,CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<bool>.FailResult(false, "PostData、PostDataList不能都为null");
                //开启事务
                currDb.BeginTran();
                //删除标识
                var mainFlag = true;
                var detailFlag = true;

                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();

                    mainFlag = await currDb.Updateable<TMMPickApplyMainDbModel>().Where(p => mainIds.Contains(p.ID) && p.CompanyId == currentUser.CompanyID).
                        SetColumns(p => new TMMPickApplyMainDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await currDb.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == requestObject.PostData.ID && p.CompanyId == currentUser.CompanyID).
                         SetColumns(p => new TMMPickApplyMainDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<bool>.FailResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<AuditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TMMPickApplyMainDbModel>()
                    .SetColumns(p => new TMMPickApplyMainDbModel
                    {
                        AuditStatus = requestObject.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestObject.PostData.ID && (p.AuditStatus == 0 || p.AuditStatus == 1))
                    .ExecuteCommandAsync() > 0;


                if (result)
                {
                  var m=  _db.Instance.Updateable<TMMPickApplyDetailDbModel>().SetColumns(p => new TMMPickApplyDetailDbModel() { TransNum = p.ApplyNum }).Where(p => p.MainId == requestObject.PostData.ID);
                  m.ExecuteCommand();
                }

                _db.Instance.CommitTran();
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "审核数据失败，该数据可能已审核!");
            }
            catch (Exception ex)
            {
                _db.Instance.RollbackTran();
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 终止生产出库
        /// </summary>
        /// <param name="requestPut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> StopWare(int ID, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TMMPickApplyMainDbModel>()
                    .SetColumns(p => new TMMPickApplyMainDbModel
                    {
                        TransferFlag = false
                    })
                    .Where(p => p.ID == ID && p.AuditStatus == 2 && p.CompanyId == currentUser.CompanyID && !p.DeleteFlag && p.TransferFlag == true)
                    .ExecuteCommandAsync() > 0;

                return result
                    ? ResponseUtil<bool>.SuccessResult(true)
                    : ResponseUtil<bool>.FailResult(false, "终止失败，该数据可能已终止或已删除");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"终止失败{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
