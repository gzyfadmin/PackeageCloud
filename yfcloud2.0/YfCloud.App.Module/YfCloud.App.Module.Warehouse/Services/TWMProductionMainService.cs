///////////////////////////////////////////////////////////////////////////////////////
// File: ITWMProductionMainService.cs
// Author: www.cloudyf.com
// Create Time:2019/9/18 13:52:03
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
using YfCloud.App.Module.Warehouse.Models.TWMProductionMain;
using YfCloud.App.Module.Warehouse.Models.TWMProductionDetail;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Utilities.WebApi;
using YfCloud.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YfCloud.Framework.CacheManager;
using YfCloud.Framework.UnitChange;
using YfCloud.App.Module.Warehouse.Models.TWMProductionWhMain;
using YfCloud.App.Module.Warehouse.Models;
using YfCloud.App.Module.Warehouse.Services;

namespace YfCloud.App.Module.Warehouse.Service
{
    /// <summary>
    /// 生产出库 实现类
    /// </summary>
    [UseDI(ServiceLifetime.Scoped,typeof(ITWMProductionMainService))]
    public class TWMProductionMainService : ITWMProductionMainService
    {
        private readonly IDbContext _db;//数据库实例对象
        private readonly ILogger<TWMProductionMainService> _log;//日志操作实例对象
        private readonly IMapper _mapper;//AutoMapper实例
        private readonly IStaticInventory _staticInventory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TWMProductionMainService(IDbContext dbContext, ILogger<TWMProductionMainService> logger,IMapper mapper,IStaticInventory staticInventory)
        {
            _db = dbContext;
            _log = logger;
            _mapper = mapper;
            _staticInventory = staticInventory;
        }
        
        /// <summary>
        /// 获取T_WM_ProductionMain主表数据数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMProductionMainQueryModel>>> GetMainListAsync(RequestGet requestObject, CurrentUser currentUser)
        {
            try
            {
                List<TWMProductionMainQueryModel> queryData = null;//查询结果集对象
                RefAsync<int> totalNumber = -1;//总记录数
                var query = _db.Instance.Queryable<TWMProductionMainDbModel, TMMPickApplyMainDbModel,
                    TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel > ((t, t1, t2, t3, t4,t5) => new object[] {
                    JoinType.Inner,t.SourceId==t1.ID,
                    JoinType.Left, t.OperatorId == t2.ID,
                    JoinType.Left, t.WhAdminId == t3.ID,
                    JoinType.Left,t.AuditId==t4.ID,
                    JoinType.Left,t.SendId==t5.ID
                }).Where((t, t1, t2, t3, t4) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID);
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
                        var exp = SqlSugarUtil.GetOrderByLambda<TWMProductionMainDbModel>(item.Column);
                        if (exp == null) continue;
                        if (item.Condition.ToLower() != "asc" 
                            && item.Condition.ToLower() != "desc") continue;
                        query.OrderBy($"{item.Column} {item.Condition}");
                    }
                }
                
                //执行查询
                if (requestObject.IsPaging)
                {
                    queryData = await query.Select((t, t1, t2, t3, t4,t5) => new TWMProductionMainQueryModel 
										{
											ID = t.ID,
											WhSendType = t.WhSendType,
											WhSendDate = t.WhSendDate,
											WhSendOrder = t.WhSendOrder,
											AuditStatus = t.AuditStatus,
											OperatorId = t.OperatorId,
                                            OperatorName =t2.AccountName,
                                            SendId = t.SendId,
                                            SendName =t5.AccountName,
                                            WhAdminId = t.WhAdminId,
                                            WhAdminName=t3.AccountName,
											AuditId = t.AuditId,
                                            AuditName=t4.AccountName,
											AuditTime = t.AuditTime,
											DeleteFlag = t.DeleteFlag,
											SourceId = t.SourceId,
                                            SourceCode =t1.PickNo,
                                            Number = t.Number,
											Amount = t.Amount,
											ReceiptAddress = t.ReceiptAddress,
										}).ToPageListAsync(requestObject.PageIndex, requestObject.PageSize, totalNumber);
                }
                else
                {
                    queryData = await query.Select((t, t1, t2, t3, t4, t5) => new TWMProductionMainQueryModel
                    {
                        ID = t.ID,
                        WhSendType = t.WhSendType,
                        WhSendDate = t.WhSendDate,
                        WhSendOrder = t.WhSendOrder,
                        AuditStatus = t.AuditStatus,
                        OperatorId = t.OperatorId,
                        OperatorName = t2.AccountName,
                        SendId = t.SendId,
                        SendName = t5.AccountName,
                        WhAdminId = t.WhAdminId,
                        WhAdminName = t3.AccountName,
                        AuditId = t.AuditId,
                        AuditName = t4.AccountName,
                        AuditTime = t.AuditTime,
                        DeleteFlag = t.DeleteFlag,
                        SourceId = t.SourceId,
                        SourceCode = t1.PickNo,
                        Number = t.Number,
                        Amount = t.Amount,
                        ReceiptAddress = t.ReceiptAddress,
                    }).ToListAsync();
                }
                    
                //返回执行结果
                return ResponseUtil<List<TWMProductionMainQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMProductionMainQueryModel>>.FailResult(null, ex.Message);
            }
        }

        public async Task<ResponseObject<TWMProductionMainQueryModel>> GetWholeMainData(int iMainId, CurrentUser currentUser)
        {
            try
            {
                RequestGet requestGet = new RequestGet()
                {
                    IsPaging = false,
                    QueryConditions = new List<QueryCondition>() {
                    new QueryCondition(){ Column="Id", Condition= ConditionEnum.Equal, Content= iMainId.ToString() }
                }
                };

                var allMain = await GetMainListAsync(requestGet, currentUser);


                var mainModel = allMain.Data.FirstOrDefault();

                var materList = BasicCacheGet.GetMaterial(currentUser);
                var dicList = BasicCacheGet.GetDic(currentUser);

                var detailModels = await _db.Instance.Queryable<TWMProductionDetailDbModel , TWMProductionMainDbModel,
                    TBMMaterialFileDbModel, TMMPickApplyMainDbModel, TMMPickApplyDetailDbModel>(
                    (t, t0, t1, t2, t3) => new object[] {
                      JoinType.Inner , t.MainId == t0.ID,
                      JoinType.Inner , t.MaterialId == t1.ID,
                      JoinType.Inner,t0.SourceId==t2.ID,
                      JoinType.Inner,t.PickApplyDetailId==t3.ID
                    }
                    ).Select((t, t0, t1, t2, t3) => new TWMProductionDetailQueryModel
                    {
                        ID = t.ID,
                        MainId = t.MainId,
                        MaterialId = t.MaterialId,
                        MaterialName = t1.MaterialName,
                        MaterialCode = t1.MaterialCode,
                        WarehouseId = t.WarehouseId,
                        ActualNum = t.ActualNum,
                        PickActualNum=t.PickActualNum,
                        PickApplyDetailId =t.PickApplyDetailId,
                        UnitPrice = t.UnitPrice,
                        Amount = t.Amount,
                        MaterialTypeId = t1.MaterialTypeId,
                        //MaterialTypeName = t1.TypeName,
                        ColorId = t1.ColorId,
                        //ColorName = t2.DicValue,
                        BaseUnitId = t1.BaseUnitId,
                        //BaseUnitName = t3.DicValue,
                        WarehouseUnitId = t1.WarehouseUnitId,
                        //WarehouseUnitName = t4.DicValue,
                        WarehouseRate = t1.WarehouseRate,
                        ProduceRate = t1.ProduceRate,
                        ProduceUnitId = t1.ProduceUnitId,
                        //ProduceUnitName = t5.DicValue,
                        Spec = t1.Spec,
                        Remark = t.Remark,
                        TransNum = t3.TransNum,

                    })
                                    .Where(t => t.MainId == iMainId)
                                    .ToListAsync();


                detailModels.ForEach((x) => {

                    var thisMaterial = materList.Where(p => p.ID == x.MaterialId).FirstOrDefault();

                    if (mainModel.AuditStatus != 2)
                    {

                        x.TransNum = x.TransNum + x.PickActualNum;
                    }

                    x.MaterialTypeName = thisMaterial.MaterialTypeName;
                    x.ColorName = thisMaterial.ColorName;
                    x.BaseUnitName = thisMaterial.BaseUnitName;
                    x.WarehouseUnitName = thisMaterial.WarehouseUnitName;
                    x.ProduceUnitName = thisMaterial.ProduceUnitName;


                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.MaterialId = x.MaterialId;
                    tWMStaQuery.WarehouseId = x.WarehouseId;

                    if (mainModel.AuditStatus != 2)
                    {
                        tWMStaQuery.EditID = mainModel.ID;
                        tWMStaQuery.OperateType = OperateEnum.Product;
                    }

                    x.AvailableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                });

               
                mainModel.ChildList = detailModels;
                return ResponseUtil<TWMProductionMainQueryModel>.SuccessResult(mainModel);
            }
            catch (Exception ex)
            {
                return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null);
            }
        }

        #region 选择

        public async Task<TMMPickApplyMainQueryModel> GetTopMainData(int iMainId, CurrentUser currentUser)
        {
            try
            {
                var mainModel = await _db.Instance.Queryable<TMMPickApplyMainDbModel, TSMUserAccountDbModel, TSMUserAccountDbModel>(
                               (t, t1, t2) => new object[]
                               {
                                    JoinType.Left, t.OperatorId == t1.ID,
                                    JoinType.Left, t.AuditId == t2.ID
                               }).Where((t, t1, t2) => t.DeleteFlag == false && t.CompanyId == currentUser.CompanyID)
                               .Select((t, t1, t2) => new TMMPickApplyMainQueryModel
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
                                   IsShowEdit = (t.AuditStatus != 2 && t.OperatorId == currentUser.UserID) ? true : false
                               })
                                 .Where(t => t.ID == iMainId)
                                 .FirstAsync();

                var detailModels = await _db.Instance.Queryable<TMMPickApplyDetailDbModel, TBMMaterialFileDbModel, TBMDictionaryTypeDbModel,
                                     TBMDictionaryDbModel, TBMDictionaryDbModel, TBMDictionaryDbModel>(
                                    (t, t0, t1, t2, t3, t4) => new object[]
                                    {
                                        JoinType.Left , t.MaterialId == t0.ID,
                                        JoinType.Left , t0.MaterialTypeId == t1.ID,
                                        JoinType.Left , t0.ColorId == t2.ID,
                                        JoinType.Left , t0.BaseUnitId == t3.ID,
                                        JoinType.Left , t0.WarehouseUnitId == t4.ID
                                    }).Where((t, t0, t1, t2, t3, t4) => t.MainId == iMainId&&t.TransNum>0)
                                    .Select((t, t0, t1, t2, t3, t4) => new TMMPickApplyDetailQueryModel
                                    {
                                        MaterialId = t.MaterialId,
                                        TransNum = t.TransNum,

                                        MaterialName = t0.MaterialName,
                                        MaterialCode = t0.MaterialCode,
                                        MaterialTypeId = t0.MaterialTypeId,
                                        //MaterialTypeName = t1.TypeName,
                                        ColorId = t0.ColorId,
                                        //ColorName = t2.DicValue,
                                        BaseUnitId = t0.BaseUnitId,
                                        //BaseUnitName = t3.DicValue,
                                        WarehouseUnitId = t0.WarehouseUnitId,
                                        //WarehouseUnitName = t4.DicValue,
                                        WarehouseRate = t0.WarehouseRate,
                                        ProduceRate = t0.ProduceRate,
                                        ProduceUnitId = t0.ProduceUnitId,
                                        Spec = t0.Spec,
                                    })
                                   
                                    .ToListAsync();

                var materList = BasicCacheGet.GetMaterial(currentUser);
                var dicList = BasicCacheGet.GetDic(currentUser);

                detailModels.ForEach((x) => {

                    var thisMaterial = materList.Where(p => p.ID == x.MaterialId).FirstOrDefault();
                    x.MaterialTypeName = thisMaterial.MaterialTypeName;
                    x.ColorName = thisMaterial.ColorName;
                    x.BaseUnitName = thisMaterial.BaseUnitName;
                    x.WarehouseUnitName = thisMaterial.WarehouseUnitName;
                    x.ProduceUnitName = thisMaterial.ProduceUnitName;
                });

                mainModel.ChildList = detailModels;
                return mainModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseObject<TMMPickApplyMainQueryModel>> GetTopWholeMainData(int iMainId, CurrentUser currentUser)
        {
            var result = await GetTopMainData(iMainId, currentUser);

            return ResponseUtil<TMMPickApplyMainQueryModel>.SuccessResult(result);

        }

        #endregion

        /// <summary>
        /// 获取T_WM_ProductionDetail数据
        /// </summary>
        /// <param name="requestObject">Get请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，查询操作结果</returns>
        public async Task<ResponseObject<List<TWMProductionDetailQueryModel>>> GetDetailListAsync(int requestObject)
        {
            try
            {
                //查询结果集对象
                List<TWMProductionDetailQueryModel> queryData = null;
                //总记录数
                RefAsync<int> totalNumber = -1;
                var query = _db.Instance.Queryable<TWMProductionDetailDbModel>();
                
                //执行查询
                queryData = await query.Select((t) => new TWMProductionDetailQueryModel 
										{
											ID = t.ID,
											MainId = t.MainId,
											MaterialId = t.MaterialId,
											WarehouseId = t.WarehouseId,
											ActualNum = t.ActualNum,
											UnitPrice = t.UnitPrice,
											Amount = t.Amount,
											Remark = t.Remark,
										})
                                       .Where(t => t.MainId == requestObject)
                                       .ToListAsync();
                                            
                //返回执行结果
                return ResponseUtil<List<TWMProductionDetailQueryModel>>.SuccessResult(queryData, totalNumber);
            }
            catch (Exception ex)
            {
                //返回查询异常结果
                return ResponseUtil<List<TWMProductionDetailQueryModel>>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 新增T_WM_ProductionMain数据
        /// </summary>
        /// <param name="requestObject">Post请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，新增操作结果</returns>
        public async Task<ResponseObject<TWMProductionMainQueryModel>> PostAsync(RequestPost<TWMProductionMainAddModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {
                //没有新增数据，返回错误信息
                if (requestObject.PostData == null)
                    return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null, "PostData不能为null");
                //开启事务
                currDb.BeginTran();
                //插入主表数据
                var mapMainModel = _mapper.Map<TWMProductionMainDbModel>(requestObject.PostData);

                mapMainModel.CompanyId = currentUser.CompanyID;
                mapMainModel.OperatorId = currentUser.UserID;
                mapMainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mapMainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);
                mapMainModel.AuditStatus = 0;
                mapMainModel.WhSendType = 1;
                var mainId = await currDb.Insertable(mapMainModel).ExecuteReturnIdentityAsync();

                //更新明细表外键ID值
                requestObject.PostData.ChildList.ForEach(p => p.MainId = mainId);
                //插入从表数据
                var mapDetailModelList = _mapper.Map<List<TWMProductionDetailAddModel>, List<TWMProductionDetailDbModel>>(requestObject.PostData.ChildList);
                var materList = BasicCacheGet.GetMaterial(currentUser);
                #region 检查出库是否超过可用量

                int? iMainId = mainId;

                var details = requestObject.PostData.ChildList.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMOtherCountDbModel
                {
                    MaterialId = p.Key.MaterialId,
                    WarehouseId = p.Key.WarehouseId,
                    WhSendNumber = p.Sum(p1 => p1.ActualNum)
                }).ToList();

                foreach (var item in details)
                {
                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.EditID = iMainId;
                    tWMStaQuery.MaterialId = item.MaterialId;
                    tWMStaQuery.WarehouseId = item.WarehouseId;

                    decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                    if (AvaiableNum < 0)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料可用量不足");
                    }
                    if (item.WhSendNumber > AvaiableNum)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                    }
                }

                #endregion

                var listItem = _db.Instance.Queryable<TMMPickApplyDetailDbModel>().Where(t => t.MainId == mapMainModel.SourceId).ToList();



                List<TMMPickApplyDetailDbModel> toEdit = new List<TMMPickApplyDetailDbModel>();

                foreach (var item in mapDetailModelList)
                {
                    var procudeMaterial = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();//领料明细

                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();//物料

                    decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);
                    decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, procudeMaterial.TransNum);//可转的数量（仓库单位）

                    if (item.ActualNum > wareTransNum)
                    {
                        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{wareTransNum }");
                    }
                    else if (item.ActualNum == wareTransNum)
                    {
                        item.PickActualNum = procudeMaterial.TransNum;
                        procudeMaterial.TransNum = 0;
                    }
                    else
                    {
                        if (proNum > procudeMaterial.TransNum)
                        {
                            item.PickActualNum = procudeMaterial.TransNum;
                            procudeMaterial.TransNum = 0;
                        }
                        else
                        {
                            item.PickActualNum = proNum;
                            procudeMaterial.TransNum = procudeMaterial.TransNum - proNum;
                        }
                    }

                    toEdit.Add(procudeMaterial);

                    //var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();

                    //decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);

                    //var m = listItem.Where(p => p.MaterialId == item.MaterialId).FirstOrDefault();
                    //if (m != null)
                    //{
                    //    if (m.TransNum < proNum)
                    //    {
                    //        throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{proNum }");
                    //    }
                    //    m.TransNum = m.TransNum - proNum;

                    //    toEdit.Add(m);
                    //}
                }

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TMMPickApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mapMainModel.SourceId))
                {
                    _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mapMainModel.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                }

                await currDb.Insertable(mapDetailModelList).ExecuteCommandAsync() ;
                //提交事务
                currDb.CommitTran();

                var res = await GetWholeMainData(mainId, currentUser);
                //返回执行结果
                return ResponseUtil<TWMProductionMainQueryModel>.SuccessResult(res.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null, ex.Message);
            }
        }
        
        /// <summary>
        /// 修改T_WM_ProductionMain数据
        /// </summary>
        /// <param name="requestObject">Put请求参数</param>
        /// <returns>返回响应结果对象，包括响应代码，修改操作结果</returns>
        public async Task<ResponseObject<TWMProductionMainQueryModel>> PutAsync(RequestPut<TWMProductionMainEditModel> requestObject, CurrentUser currentUser)
        {
            var currDb = _db.Instance;//事务需要使用同一个 SqlSugarClient对象实例
            try
            {

//#if DEBUG
                
//#else
//     return null;
//#endif

                if (requestObject.PostData == null)
                    return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null, "PostData不能为null");
                if (requestObject.PostData.ChildList == null || requestObject.PostData.ChildList.Count < 1)
                    return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null, "PostData.ChildList至少包含一条数据");
                //开启事务
                currDb.BeginTran();
                //修改主表信息
                var mainModel = _mapper.Map<TWMProductionMainDbModel>(requestObject.PostData);

                mainModel.Number = requestObject.PostData.ChildList.Sum(p => p.ActualNum);
                mainModel.Amount = requestObject.PostData.ChildList.Sum(p => p.Amount);

                var mainFlag = await currDb.Updateable(mainModel).UpdateColumns(p => new
                {
                    p.WhSendDate,
                    p.Number,
                    p.Amount,
                    p.WhAdminId,
                    p.SendId,
                    p.ReceiptAddress

                }).ExecuteCommandAsync() > 0;

                /*
                 * 修改明细逻辑
                 * 1.根据主单ID查询现有明细数据
                 * 2.PostData.ChildList中明细ID <= 0的新增
                 * 3.PostData.ChildList中明细ID > 0的修改
                 * 4.删除不在PostData.CihldList中的数据
                 */
                var detailFlag = true;
                var detailModels = _mapper.Map<List<TWMProductionDetailEditModel>, 
                    List<TWMProductionDetailDbModel>>(requestObject.PostData.ChildList);

                var mainEntity = _db.Instance.Queryable<TWMProductionMainDbModel>().Where(p => p.ID == mainModel.ID).First();

                var listItem = _db.Instance.Queryable<TMMPickApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                var materList = BasicCacheGet.GetMaterial(currentUser);

                List<TMMPickApplyDetailDbModel> toEdit = new List<TMMPickApplyDetailDbModel>();

                foreach (var item in detailModels)
                {
                    if (!detailFlag) break;                    
                    item.MainId = mainModel.ID;

                    var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();

                    if (!detailFlag) break;
                    item.MainId = mainModel.ID;

                    if (item.ID <= 0) //新增
                    {
                        var procudeMaterial = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();//领料明细

                        decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);
                        decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, procudeMaterial.TransNum);//可转的数量（仓库单位）

                        if (item.ActualNum > wareTransNum)
                        {
                            throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{wareTransNum }");
                        }
                        else if (item.ActualNum == wareTransNum)
                        {
                            item.PickActualNum = procudeMaterial.TransNum;
                            procudeMaterial.TransNum = 0;
                        }
                        else
                        {
                            if (proNum > procudeMaterial.TransNum)
                            {
                                item.PickActualNum = procudeMaterial.TransNum;
                                procudeMaterial.TransNum = 0;
                            }
                            else
                            {
                                item.PickActualNum = proNum;
                                procudeMaterial.TransNum = procudeMaterial.TransNum - proNum;
                            }
                        }

                        toEdit.Add(procudeMaterial);

                    }
                    else //修改
                    {
                        var dModel = _db.Instance.Queryable<TWMProductionDetailDbModel>().Where(p => p.ID == item.ID).First();
                        if (dModel != null)
                        {
                            if (dModel.ActualNum != item.ActualNum)
                            {
                                var procudeMaterial = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();//领料明细

                                decimal proNum = UnitChange.TranserUnit(me, UnitType.Warehouse, UnitType.Produce, item.ActualNum);//实际转出数量（销售单位）

                                decimal TransNum = procudeMaterial.TransNum + dModel.PickActualNum; //可转数量（生产单位）

                                decimal wareTransNum = UnitChange.TranserUnit(me, UnitType.Produce, UnitType.Warehouse, TransNum);//可转的数量（仓库单位）

                                if (item.ActualNum > wareTransNum)
                                {
                                    throw new Exception($"物料代码{me.MaterialCode} 出库数量不能大于{wareTransNum }");
                                }
                                else if (item.ActualNum == wareTransNum)
                                {
                                    item.PickActualNum = TransNum;
                                    procudeMaterial.TransNum = 0;
                                }
                                else
                                {
                                    if (proNum > TransNum)
                                    {
                                        item.PickActualNum = TransNum;
                                        procudeMaterial.TransNum = 0;
                                    }
                                    else
                                    {
                                        item.PickActualNum = proNum;
                                        procudeMaterial.TransNum = TransNum - proNum;
                                    }
                                }

                                toEdit.Add(procudeMaterial);
                            }
                            else
                            {
                                item.PickActualNum = dModel.PickActualNum;
                            }
                        }
                    }

               

                    //新增或修改明细数据
                    detailFlag = item.ID <= 0
                        ? await currDb.Insertable(item).ExecuteCommandIdentityIntoEntityAsync()
                        : await currDb.Updateable(item).ExecuteCommandAsync() > 0;
                }

                //删除明细数据
                if (detailFlag)
                {
                    var detailIds = detailModels.Select(p => p.ID).ToList();

                    var whDetailList = currDb.Queryable<TWMProductionDetailDbModel>().Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID).ToList();

                    foreach (var item in whDetailList)
                    {

                        var m = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransNum = m.TransNum + item.PickActualNum;
                            toEdit.Add(m);
                        }


                    }

                    detailFlag = currDb.Deleteable<TWMProductionDetailDbModel>()
                        .Where(p => !detailIds.Contains(p.ID) && p.MainId == mainModel.ID)
                        .ExecuteCommand() >= 0;
                }

                if (toEdit.Count() > 0)
                {
                    _db.Instance.Updateable(toEdit).ExecuteCommand();
                }

                if (!_db.Instance.Queryable<TMMPickApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                {
                    _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                }
                else
                {
                    _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                }

                #region 检查出库是否超过可用量

                int? iMainId = mainModel.ID;

                var details = requestObject.PostData.ChildList.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMOtherCountDbModel
                {
                    MaterialId = p.Key.MaterialId,
                    WarehouseId = p.Key.WarehouseId,
                    WhSendNumber = p.Sum(p1 => p1.ActualNum)
                }).ToList();

                foreach (var item in details)
                {
                    TWMStaQuery tWMStaQuery = new TWMStaQuery();
                    tWMStaQuery.EditID = iMainId;
                    tWMStaQuery.MaterialId = item.MaterialId;
                    tWMStaQuery.WarehouseId = item.WarehouseId;

                    decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;
                    if (AvaiableNum < 0)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料可用量不足");
                    }
                    if (item.WhSendNumber > AvaiableNum)
                    {
                        var me = materList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                        throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量不能大于{AvaiableNum}");
                    }
                }

                #endregion

                //提交事务
                currDb.CommitTran();
                //返回执行结果
                var res = await GetWholeMainData(mainModel.ID, currentUser);
                //返回执行结果
                return ResponseUtil<TWMProductionMainQueryModel>.SuccessResult(res.Data);
            }
            catch (Exception ex)
            {
                //回滚事务
                currDb.RollbackTran();
                //返回异常结果
                return ResponseUtil<TWMProductionMainQueryModel>.FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 出库单审核
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseObject<bool>> AuditAsync(RequestPut<TWMProductionAduit> requestObject, CurrentUser currentUser)
        {
            try
            {
                _db.Instance.BeginTran();
                //更新审核状态
                var result = await _db.Instance.Updateable<TWMProductionMainDbModel>()
                    .SetColumns(p => new TWMProductionMainDbModel
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
                    OtherWhCountAsync(requestObject.PostData.ID, currentUser);
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
        private  void OtherWhCountAsync(int iMainId, CurrentUser currentUser)
        {
            var mainEntity = _db.Instance.Queryable<TWMProductionMainDbModel>().Where(p => p.ID == iMainId).First();

            //获取出库明细
            var details =  _db.Instance.Queryable<TWMProductionDetailDbModel>()
                .Where(p => p.MainId == iMainId)
                .ToList();

            //更新出库数量
            var detailList = details.GroupBy(p => new { p.MaterialId, p.WarehouseId }).Select(p => new TWMProductionCountDbModel
            {
                MaterialId = p.Key.MaterialId,
                WarehouseId = p.Key.WarehouseId,
                WhSendNumber = p.Sum(p1 => p1.ActualNum)
            }).ToList();



            var mList = BasicCacheGet.GetMaterial(currentUser);
            foreach (var item in detailList)
            {
                TWMStaQuery tWMStaQuery = new TWMStaQuery();
                tWMStaQuery.EditID = iMainId;
                tWMStaQuery.MaterialId = item.MaterialId;
                tWMStaQuery.WarehouseId = item.WarehouseId;
                tWMStaQuery.OperateType = OperateEnum.Product;

                decimal AvaiableNum = _staticInventory.GeTWMCountModel(tWMStaQuery).AvaiableNum;

                if (AvaiableNum <= 0)
                {
                    var me = mList.Where(p => p.ID == item.MaterialId).FirstOrDefault();
                    throw new Exception($"物料代码:{me.MaterialCode},物料名称{me.MaterialName}的物料出库数量可用量不足");
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

        /// <summary>
        /// 删除T_WM_ProductionMain数据
        /// </summary>
        /// <param name="requestObject">Delete请求参数</param>
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
                var materList = BasicCacheGet.GetMaterial(currentUser);

                if (requestObject.PostDataList != null && requestObject.PostDataList.Count > 0)
                {
                    //批量删除
                    var mainIds = requestObject.PostDataList.Select(p => p.ID).ToList();

                    foreach (int ids in mainIds)
                    {
                        List<TMMPickApplyDetailDbModel> toEdit = new List<TMMPickApplyDetailDbModel>();
                        var mainEntity = _db.Instance.Queryable<TWMProductionMainDbModel>().Where(P => P.ID == ids).First();

                        var listItem = _db.Instance.Queryable<TMMPickApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                        var whDetailList = currDb.Queryable<TWMProductionDetailDbModel>().Where(p => p.MainId == ids).ToList();

                        foreach (var item in whDetailList)
                        {

                            var m = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();
                            if (m != null)
                            {
                                m.TransNum = m.TransNum + item.PickActualNum;
                                toEdit.Add(m);
                            }
                        }
                        if (toEdit != null && toEdit.Count() > 0)
                        {
                            _db.Instance.Updateable<TMMPickApplyDetailDbModel>(toEdit).ExecuteCommand();
                        }

                        if (!_db.Instance.Queryable<TMMPickApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                        {
                            _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                        }
                        else
                        {
                            _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                        }
                    }



                    mainFlag = await _db.Instance.Updateable<TWMProductionMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => mainIds.Contains(p.ID))
                        .ExecuteCommandAsync() > 0;
                }
                else
                {
                    List<TMMPickApplyDetailDbModel> toEdit = new List<TMMPickApplyDetailDbModel>();

                    var mainEntity = _db.Instance.Queryable<TWMProductionMainDbModel>().Where(P => P.ID == requestObject.PostData.ID).First();

                    var listItem = _db.Instance.Queryable<TMMPickApplyDetailDbModel>().Where(t => t.MainId == mainEntity.SourceId).ToList();

                    var whDetailList = currDb.Queryable<TWMProductionDetailDbModel>().Where(p => p.MainId == requestObject.PostData.ID).ToList();

                    foreach (var item in whDetailList)
                    {
                        var m = listItem.Where(p => p.ID == item.PickApplyDetailId).FirstOrDefault();
                        if (m != null)
                        {
                            m.TransNum = m.TransNum +item.PickActualNum;
                            toEdit.Add(m);
                        }


                    }

                    if (toEdit != null && toEdit.Count() > 0)
                    {
                        _db.Instance.Updateable<TMMPickApplyDetailDbModel>(toEdit).ExecuteCommand();
                    }

                    if (!_db.Instance.Queryable<TMMPickApplyDetailDbModel>().Any(p => p.TransNum > 0 && p.MainId == mainEntity.SourceId))
                    {
                        _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == false).ExecuteCommand();
                    }
                    else
                    {
                        _db.Instance.Updateable<TMMPickApplyMainDbModel>().Where(p => p.ID == mainEntity.SourceId).SetColumns(p => p.TransferFlag == true).ExecuteCommand();
                    }

                    //单条删除
                    mainFlag = await _db.Instance.Updateable<TWMProductionMainDbModel>()
                        .SetColumns(p => p.DeleteFlag == true)
                        .Where(p => p.ID == requestObject.PostData.ID)
                        .ExecuteCommandAsync() > 0;
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
    }
}
