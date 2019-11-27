///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMDeficitMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/8/13 15:45:36
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
using YfCloud.App.Module.Warehouse.Models.TWMDeficitMain;
using YfCloud.App.Module.Warehouse.Models.TWMDeficitDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.App.Module.Warehouse.Services;
using YfCloud.Framework.CacheManager;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// 盘亏出库 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ITWMDeficitMainService))]
    public class TWMDeficitMainService : ITWMDeficitMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMDeficitMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly IStaticInventory _staticInventory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMDeficitMainService(IDbContext dbContext, ILogger<TWMDeficitMainService> logger, IMapper mapper, IStaticInventory staticInventory)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _staticInventory = staticInventory;
        }

        /// <summary>
        /// 获取T_WM_DeficitMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMDeficitMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMDeficitMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMDeficitMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                (t, t0, t1, t2) => new object[]
                                {
                                    JoinType.Left, t.OperatorId == t0.ID,
                                    JoinType.Left, t.ReceiptId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID
                                }).Where((t, t0, t1, t2) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMDeficitMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc"
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }

                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t0, t1, t2) => new TWMDeficitMainQueryModel
                    {
                        ID = t.ID,
                        WhSendType = t.WhSendType,
                        WhSendDate = t.WhSendDate,
                        WhSendOrder = t.WhSendOrder,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,

                        OperatorName = t0.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t1.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t2.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                    }).ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t0, t1, t2) => new TWMDeficitMainQueryModel
                    {
                        ID = t.ID,
                        WhSendType = t.WhSendType,
                        WhSendDate = t.WhSendDate,
                        WhSendOrder = t.WhSendOrder,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t0.AccountName,
                        ReceiptId = t.ReceiptId,
                        ReceiptName = t1.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t2.AccountName,
                        AuditTime = t.AuditTime,
                        CompanyId = t.CompanyId,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        Number = t.Number,
                        Amount = t.Amount,
                        IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                    }).ToListAsync();
                }

                //返回执行结果
                return ResponseUtil<List<TWMDeficitMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMDeficitMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 获取出库单所有的信息
        /// </summary>
        /// <param name="iMainId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TWMDeficitMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
        {
            var result = await GetMainData(iMainId, currentUser);

            result.IsShowEdit = (result.AuditStatus != 2 && result.OperatorId == currentUser.UserID) ? true : false;

            for (int i = 0; i < result.ChildList.Count; i++)
            {
                var thisEntity = result.ChildList[i];

                TWMStaQuery tWMStaQuery = new TWMStaQuery();

                if (result.AuditStatus != 2)
                {
                    tWMStaQuery.EditID = iMainId;
                    tWMStaQuery.OperateType = OperateEnum.Invertory;
                }

                tWMStaQuery.MaterialId = thisEntity.MaterialId;
                tWMStaQuery.WarehouseId = thisEntity.WarehouseId;

                thisEntity.AvailableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
            }

            return ResponseUtil<TWMDeficitMainQueryModel>.SuccessResult(result);

        }

        private async Task<TWMDeficitMainQueryModel> GetMainData(int iMainId, CurrentUser currentUser)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TWMDeficitMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                                 (t, t0, t1, t2) => new object[]
                                 {
                                    JoinType.Left, t.OperatorId == t0.ID,
                                    JoinType.Left, t.ReceiptId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID
                                 })
                                 .Select((t, t0, t1, t2) => new TWMDeficitMainQueryModel
                                 {
                                     ID = t.ID,
                                     WhSendType = t.WhSendType,
                                     WhSendDate = t.WhSendDate,
                                     WhSendOrder = t.WhSendOrder,
                                     AuditStatus = t.AuditStatus,
                                     OperatorId = t.OperatorId,
                                     OperatorName = t0.AccountName,
                                     ReceiptId = t.ReceiptId,
                                     ReceiptName = t1.AccountName,
                                     AuditId = t.AuditId,
                                     AuditName = t2.AccountName,
                                     AuditTime = t.AuditTime,
                                     CompanyId = t.CompanyId,
                                     DeleteFlag = t.DeleteFlag,
                                     SourceId = t.SourceId,
                                     Number = t.Number,
                                     Amount = t.Amount,
                                     IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                                 })
                                 .Where(t => t.ID == iMainId)
                                 .FirstAsync();
                var detailModels = await _db.Instance.Queryable<TWMDeficitDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                                    (t, t0, t1, t2, t3, t4) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID
                                    })
                                    .Select((t, t0, t1, t2, t3, t4) => new TWMDeficitDetailQueryModel
                                    {
                                        ID = t.ID,
                                        MainId = t.MainId,
                                        MaterialId = t.MaterialId,
                                        MaterialName = t0.MaterialName,
                                        MaterialCode = t0.MaterialCode,
                                        WarehouseId = t.WarehouseId,
                                        BatchNumber = t.BatchNumber,
                                        ActualNumber = t.ActualNumber,
                                        UnitPrice = t.UnitPrice,
                                        Amount = t.Amount,
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
                                        ValidityPeriod = t.ValidityPeriod,
                                        AccountNum = t.AccountNum,
                                        Remark = t.Remark
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
        /// 新增T_WM_DeficitMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMDeficitMainQueryModel>> PostAsync(RequestPost<TWMDeficitMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMDeficitMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMDeficitMainDbModel>(requestObject.PostData);
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNumber);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);

                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();
                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMDeficitDetailAddModel>, List<TWMDeficitDetailDbModel>>(requestObject.PostData.ChildList);
                var result = await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() > 0;

                var returnData = await GetMainData(mainId, currentUser);
                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return ResponseUtil<TWMDeficitMainQueryModel>.SuccessResult(returnData);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMDeficitMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 修改T_WM_DeficitMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMDeficitMainQueryModel>> PutAsync(RequestPut<TWMDeficitMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMDeficitMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMDeficitMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();

                var mainModel = _mapper.Map<TWMDeficitMainDbModel>(requestObject.PostData);
                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNumber);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                await currDb.Updateable(mainModel)
                   .UpdateColumns(p => new
                   {
                       p.WhSendType,
                       p.WhSendDate,
                       p.Number,
                       p.Amount,
                       p.ReceiptId
                   })
                   .ExecuteCommandAsync();

                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailModels = _mapper.Map<List<TWMDeficitDetailEditModel>,
                    List<TWMDeficitDetailDbModel>>(requestObject.PostData.ChildList);

                foreach (var item in detailModels)
                {
                    item.MainId = mainModel.ID;
                    if (item.ID <= 0)
                    {
                        await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync();
                    }
                    else
                    {
                        await currDb.Updateable(item).ExecuteCommandAsync();
                    }
                }

                //删除明细数据
                var detailIds = detailModels.Select(p => p.ID).ToList();
                currDb.Deleteable<TWMDeficitDetailDbModel>()
                   .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                   .ExecuteCommand();

                var returnData = await GetMainData(mainModel.ID, currentUser);

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                return ResponseUtil<TWMDeficitMainQueryModel>.SuccessResult(returnData);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMDeficitMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 删除T_WM_DeficitMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
        /// <param name="currentUser"></param>
        /// <returns>返回响应结果对象，包括响应代码，删除操作结果</returns>
        public async Task<ResponseObject<bool>> DeleteAsync(RequestDelete<DeleteModel> requestObject, CurrentUser currentUser)
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

                    mainFlag = await currDb.Updateable<TWMDeficitMainDbModel>().Where(p => mainIds.Contains(p.ID) && p.CompanyId == currentUser.CompanyID).
                        SetColumns(p => new TWMDeficitMainDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
                }
                else
                {
                    //单条删除
                    mainFlag = await currDb.Updateable<TWMDeficitMainDbModel>().Where(p => p.ID == requestObject.PostData.ID && p.CompanyId == currentUser.CompanyID).
                         SetColumns(p => new TWMDeficitMainDbModel { DeleteFlag = true }).ExecuteCommandAsync() >= 0;
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
        /// 其他出库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMDeficitMainAuditModel> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMDeficitMainDbModel>()
                    .SetColumns(p => new TWMDeficitMainDbModel
                    {
                        AuditStatus = requestObject.PostData.AuditStatus,
                        AuditId = currentUser.UserID,
                        AuditTime = DateTime.Now
                    })
                    .Where(p => p.ID == requestObject.PostData.ID && (p.AuditStatus == 0 || p.AuditStatus == 1))
                    .ExecuteCommandAsync() > 0;
                //审核通过后统计当前出库单的数量和金额
                if (result && requestObject.PostData.AuditStatus == 2)
                {
                    OtherWhCountAsync(requestObject.PostData.ID,currentUser);
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
        /// 审核通过后统计当前出库单信息
        /// </summary>
        /// <param name="iMainId"></param>
        private async void OtherWhCountAsync(int iMainId,CurrentUser currentUser)
        {
            //获取出库明细
            var details =  _db.Instance.Queryable<TWMDeficitDetailDbModel>()
                .Where(p => p.MainId == iMainId)
                .ToList();
            //更新出库数量
            var detailList = details.Select(p => new TWMProfitDeficitCountDbModel
            {
                MaterialId = p.MaterialId,
                WarehouseId = p.WarehouseId,
                WhSendNumber = p.ActualNumber
            }).ToList();

            var mList = BasicCacheGet.GetMaterial(currentUser);
            foreach (var item in detailList)
            {
                TWMStaQuery tWMStaQuery = new TWMStaQuery();
                tWMStaQuery.EditID = iMainId;
                tWMStaQuery.MaterialId = item.MaterialId;
                tWMStaQuery.WarehouseId = item.WarehouseId;
                tWMStaQuery.OperateType = OperateEnum.Other;

                decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                if (AvaiableNum < 0)
                {
                    var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料可用量不足");
                }

                if (item.WhSendNumber > AvaiableNum)
                {
                    var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                }

                var updateFlag = _db.Instance.Updateable(item)
                    .SetColumns(p => p.WhSendNumber == p.WhSendNumber + item.WhSendNumber)
                    .Where(p => p.MaterialId == item.MaterialId && p.WarehouseId == item.WarehouseId)
                    .ExecuteCommand() > 0;
                if (!updateFlag)
                    _db.Instance.Insertable(item).ExecuteCommand();
            }

        }
    }
}
