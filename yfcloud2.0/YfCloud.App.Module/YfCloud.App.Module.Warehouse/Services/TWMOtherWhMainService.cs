///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMOtherWhMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7 14:10:15
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
using YfCloud.DBModel;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain;
using YfCloud.App.Module.Warehouse.Models.TWMOtherWhDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// ITWMOtherWhMainService 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITWMOtherWhMainService))]
    public class TWMOtherWhMainService : ITWMOtherWhMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMOtherWhMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMOtherWhMainService(IDbContext dbContext, ILogger<TWMOtherWhMainService> logger,IMapper mapper)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取T_WM_OtherWhMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<TWMOtherWhMainQueryModel, List<TWMOtherWhMainQueryModel>>> GetMainListAsync(RequestObject<TWMOtherWhMainQueryModel> requestObject,
            CurrentUser currentUser)
        {
            try
            {
                List<TWMOtherWhMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMOtherWhMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                (t, t0, t1, t2) => new object[]
                                {
                                    JoinType.Left, t.OperatorId == t0.ID,
                                    JoinType.Left, t.ReceiptId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID
                                });
                //查询条件
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    query.Where(conditionals);
                }
                //排序条件
                if (requestObject.OrderByConditions != null && requestObject.OrderByConditions.Count > 0)
                {
                    foreach (var item in requestObject.OrderByConditions)
                    {
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMOtherWhMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t0, t1, t2) => new TWMOtherWhMainQueryModel
                                            {
                                                ID = t.ID,
                                                WarehousingType = t.WarehousingType,
                                                WarehousingDate = t.WarehousingDate,
                                                WarehousingOrder = t.WarehousingOrder,
                                                AuditStatus = t.AuditStatus,
                                                OperatorId = t.OperatorId,
                                                OperatorName = t0.AccountName,
                                                ReceiptId = t.ReceiptId,
                                                ReceiptName = t1.AccountName,
                                                AuditId = t.AuditId,
                                                AuditName = t2.AccountName,
                                                AuditTime = t.AuditTime,
                                                DeleteFlag = t.DeleteFlag,
                                                Number = t.Number,
                                                Amount = t.Amount,
                                                AllowEdit = currentUser.UserID == t.OperatorId && t.AuditStatus != 2
                                            })
                                            .Where(t => t.CompanyId == currentUser.CompanyID && !(bool)t.DeleteFlag)
                                            .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t0, t1, t2) => new TWMOtherWhMainQueryModel
                                            {
                                                ID = t.ID,
                                                WarehousingType = t.WarehousingType,
                                                WarehousingDate = t.WarehousingDate,
                                                WarehousingOrder = t.WarehousingOrder,
                                                AuditStatus = t.AuditStatus,
                                                OperatorId = t.OperatorId,
                                                OperatorName = t0.AccountName,
                                                ReceiptId = t.ReceiptId,
                                                ReceiptName = t1.AccountName,
                                                AuditId = t.AuditId,
                                                AuditName = t2.AccountName,
                                                AuditTime = t.AuditTime,
                                                DeleteFlag = t.DeleteFlag,
                                                Number = t.Number,
                                                Amount = t.Amount,
                                                AllowEdit = currentUser.UserID == t.OperatorId && t.AuditStatus != 2
                                            })
                                            .Where(t => t.CompanyId == currentUser.CompanyID && !(bool)t.DeleteFlag)
                                            .ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<TWMOtherWhMainQueryModel, List<TWMOtherWhMainQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<TWMOtherWhMainQueryModel, List<TWMOtherWhMainQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }
        
        /// <summary>
        /// 获取T_WM_OtherWhDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<int, List<TWMOtherWhDetailQueryModel>>> GetDetailListAsync(RequestObject<int> requestObject)
        {
            try
            {
                //查询结果集对象
                List<TWMOtherWhDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TWMOtherWhDetailDbModel,TBMMaterialFileDbModel ,TBMDictionaryTypeDbModel
                    ,TBMDictionaryDbModel,TBMDictionaryDbModel,TBMDictionaryDbModel>(
                        (t,t0,t1,t2,t3,t4) => new object[] 
                        {
                            JoinType.Left , t.MaterialId == t0.ID,
                            JoinType.Left , t0.MaterialTypeId == t1.ID,
                            JoinType.Left , t0.ColorId == t2.ID,
                            JoinType.Left , t0.BaseUnitId == t3.ID,
                            JoinType.Left , t0.WarehouseUnitId == t4.ID
                        }
                    );
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t,t0,t1,t2,t3,t4) => new TWMOtherWhDetailQueryModel 
											{
												ID = t.ID,
												MainId = t.MainId,
												MaterialId = t.MaterialId,
												WarehouseId = t.WarehouseId,
												BatchNumber = t.BatchNumber,
												ShouldNumber = t.ShouldNumber,
												ActualNumber = t.ActualNumber,
                                                MaterialCode = t0.MaterialCode,
                                                MaterialName = t0.MaterialName,
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
												UnitPrice = t.UnitPrice,
												Amount = t.Amount,
												ValidityPeriod = t.ValidityPeriod,
											})
                                            .Where(t => t.MainId == requestObject.PostData)
                                            .ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t,t0,t1,t2,t3,t4) => new TWMOtherWhDetailQueryModel 
											{
                                                ID = t.ID,
                                                MainId = t.MainId,
                                                MaterialId = t.MaterialId,
                                                WarehouseId = t.WarehouseId,
                                                BatchNumber = t.BatchNumber,
                                                ShouldNumber = t.ShouldNumber,
                                                ActualNumber = t.ActualNumber,
                                                MaterialCode = t0.MaterialCode,
                                                MaterialName = t0.MaterialName,
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
                                                UnitPrice = t.UnitPrice,
                                                Amount = t.Amount,
                                                ValidityPeriod = t.ValidityPeriod,
                                            })
                                            .Where(t => t.MainId == requestObject.PostData)
                                            .ToListAsync();
                }
                //返回执行结果
                return ResponseUtil<int, List<TWMOtherWhDetailQueryModel>>.SuccessResult(requestObject, queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<int, List<TWMOtherWhDetailQueryModel>>.FailResult(requestObject, null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_WM_OtherWhMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>> PostAsync(RequestObject<TWMOtherWhMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>.FailResult(requestObject, null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMOtherWhMainDbModel>(requestObject.PostData);
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNumber);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMOtherWhDetailAddModel>, List<TWMOtherWhDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;
                //提交事务
                currDb.CommitTran();
                TWMOtherWhMainQueryModel returnObject = null;
                if (result) returnObject = await GetMainData(mainId);
                    
                //返回执行结果
                return result 
                    ? ResponseUtil<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>.SuccessResult(requestObject, returnObject)
                    : ResponseUtil<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>.FailResult(requestObject, null, "新增数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMOtherWhMainAddModel, TWMOtherWhMainQueryModel>.FailResult(requestObject, null, ex.Message);
            }
        }

        private async Task<TWMOtherWhMainQueryModel> GetMainData(int iMainId)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TWMOtherWhMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                 (t, t0, t1, t2) => new object[]
                                 {
                                    JoinType.Left, t.OperatorId == t0.ID,
                                    JoinType.Left, t.ReceiptId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID
                                 })
                                 .Select((t, t0, t1, t2) => new TWMOtherWhMainQueryModel
                                 {
                                     ID = t.ID,
                                     WarehousingType = t.WarehousingType,
                                     WarehousingDate = t.WarehousingDate,
                                     WarehousingOrder = t.WarehousingOrder,
                                     AuditStatus = t.AuditStatus,
                                     OperatorId = t.OperatorId,
                                     OperatorName = t0.AccountName,
                                     ReceiptId = t.ReceiptId,
                                     ReceiptName = t1.AccountName,
                                     AuditId = t.AuditId,
                                     AuditName = t2.AccountName,
                                     AuditTime = t.AuditTime,
                                     DeleteFlag = t.DeleteFlag,
                                 })
                                 .Where(t => t.ID == iMainId)
                                 .FirstAsync();
                var detailModels = await _db.Instance.Queryable<TWMOtherWhDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                                    (t, t0, t1, t2, t3, t4) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID
                                    })
                                    .Select((t, t0, t1, t2, t3, t4) => new TWMOtherWhDetailQueryModel
                                    {
                                        ID = t.ID,
                                        MainId = t.MainId,
                                        MaterialId = t.MaterialId,
                                        WarehouseId = t.WarehouseId,
                                        BatchNumber = t.BatchNumber,
                                        ShouldNumber = t.ShouldNumber,
                                        ActualNumber = t.ActualNumber,
                                        MaterialCode = t0.MaterialCode,
                                        MaterialName = t0.MaterialName,
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
                                        UnitPrice = t.UnitPrice,
                                        Amount = t.Amount,
                                        ValidityPeriod = t.ValidityPeriod,
                                    })
                                    .Where(t => t.MainId == iMainId)
                                    .ToListAsync();
                mainModel.ChildList = detailModels;
                return mainModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 修改T_WM_OtherWhMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMOtherWhMainEditModel, bool>> PutAsync(RequestObject<TWMOtherWhMainEditModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {                
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMOtherWhMainEditModel, bool>.FailResult(requestObject, false, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMOtherWhMainEditModel, bool>.FailResult(requestObject, false, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TWMOtherWhMainDbModel>(requestObject.PostData);
                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNumber);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                var mainFlag = await currDb.Updateable(mainModel)
                    .UpdateColumns(p => new
                    {
                        p.WarehousingType,
                        p.WarehousingDate,
                        p.Number,
                        p.Amount,
                        p.ReceiptId
                    })
                    .ExecuteCommandAsync() > 0;
                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMOtherWhDetailEditModel>, 
                    List<TWMOtherWhDetailDbModel>>(requestObject.PostData.ChildList);
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
                    detailFlag = currDb.Deleteable<TWMOtherWhDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return mainFlag && detailFlag
                    ? ResponseUtil<TWMOtherWhMainEditModel, bool>.SuccessResult(requestObject, true)
                    : ResponseUtil<TWMOtherWhMainEditModel, bool>.FailResult(requestObject, false, "修改数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMOtherWhMainEditModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 其他入库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> OtherWhAuditAsync(RequestPut<OtherWarehousingAuditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMOtherWhMainDbModel>()
                    .SetColumns(p => new TWMOtherWhMainDbModel
                    {
                        AuditStatus = requestObject.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestObject.PostData.ID && (p.AuditStatus == 0 || p.AuditStatus == 1))
                    .ExecuteCommandAsync() > 0;
                //审核通过后统计当前入库单的数量和金额
                if(result && requestObject.PostData.AuditStatus == 2)
                {
                    OtherWhCountAsync(requestObject.PostData.ID);                    
                }
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "审核数据失败，该数据可能已审核!");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"审核数据发生异常{Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 审核通过后统计当前入库单信息
        /// </summary>
        /// <param name="iMainId"></param>
        private async void OtherWhCountAsync(int iMainId)
        {
            try
            {
                //获取入库明细
                var details = await _db.Instance.Queryable<TWMOtherWhDetailDbModel>()
                    .Where(p => p.MainId == iMainId)
                    .ToListAsync();
                //更新入库数量
                var detailList = details.Select(p => new TWMOtherCountDbModel
                {
                    MaterialId = p.MaterialId,
                    WarehouseId = p.WarehouseId,
                    WhNumber = p.ActualNumber                    
                }).ToList();

                _db.Instance.BeginTran();
                foreach (var item in detailList)
                {
                    //更新，当更新不成功则该仓库物料数据不存在，则新增
                    var updateFlag = _db.Instance.Updateable(item)
                        .SetColumns(p => p.WhNumber == p.WhNumber + item.WhNumber)
                        .Where(p => p.MaterialId == item.MaterialId && p.WarehouseId == item.WarehouseId)
                        .ExecuteCommand() > 0;
                    if (!updateFlag)
                        _db.Instance.Insertable(item).ExecuteCommand();
                }

                foreach(var item in details.Where(p=>p.UnitPrice > 0))
                {
                    await _db.Instance.Updateable<TBMMaterialFileDbModel>()
                        .SetColumns(p => p.LastPrice == item.UnitPrice)
                        .Where(p => p.ID == item.MaterialId)
                        .ExecuteCommandAsync();
                }

                _db.Instance.CommitTran();

            }
            catch
            {
                //异常不处理
            }
        }

        /// <summary>
        /// 其他入库单撤销审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> OtherWhCancelAuditAsync(RequestPut<OtherWarehousingAuditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                var result = await _db.Instance.Updateable<TWMOtherWhMainDbModel>()
                    .SetColumns(p => p.AuditStatus == requestObject.PostData.AuditStatus)
                    .Where(p => p.ID == requestObject.PostData.ID && p.AuditStatus == 2)
                    .ExecuteCommandAsync() > 0;
                return result ? ResponseUtil<bool>.SuccessResult(true) : ResponseUtil<bool>.FailResult(false, "撤销审核数据失败，该数据可能未审核或已撤销!");
            }
            catch (Exception ex)
            {
                return ResponseUtil<bool>.FailResult(false, $"撤销审核数据发生异常{Environment.NewLine} {ex.Message}");
            }
        }

        /// <summary>
        /// 删除T_WM_OtherWhMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<DeleteModel, bool>> DeleteAsync(RequestObject<DeleteModel> requestObject)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null && requestObject.PostDataList == null)
                    return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "PostData、PostDataList不能都为null");
                //开启事务
                currDb.BeginTran();
                //删除标识
                var mainFlag = true;
                var detailFlag = true;
                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();
                    mainFlag = await _db.Instance.Updateable<TWMOtherWhMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TWMOtherWhMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID).ExecuteCommandAsync() > 0;
                }
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                if (mainFlag && detailFlag)
                    return ResponseUtil<DeleteModel, bool>.SuccessResult(requestObject, true);
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, "删除数据失败!");
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<DeleteModel, bool>.FailResult(requestObject, false, ex.Message);
            }
        }

        /// <summary>
        /// 获取入库单主表和详情信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TWMOtherWhMainQueryModel>> GetWarehouseOrderAsync(int iMainId, CurrentUser currentUser)
        {
            var result = await GetMainData(iMainId);
            result.AllowEdit = currentUser.UserID == result.OperatorId && result.AuditStatus != 2;
            return ResponseUtil<TWMOtherWhMainQueryModel>.SuccessResult(result);
        }
    }
}
