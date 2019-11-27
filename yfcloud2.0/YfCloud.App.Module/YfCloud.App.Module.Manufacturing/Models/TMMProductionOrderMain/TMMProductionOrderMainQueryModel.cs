///////////////////////////////////////////////////////////////////////////////////////
// File: TMMProductionOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/9/5
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderDetail;

namespace YfCloud.App.Module.Manufacturing.Models.TMMProductionOrderMain
{
    /// <summary>
    /// TMMProductionOrderMain Query Model
    /// </summary>
    public class TMMProductionOrderMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 生产类型 0库存生产 1订单生产
        /// </summary>
        public byte ProductionType { get; set; }
        
        /// <summary>
        /// 生产单号
        /// </summary>
        public string ProductionNo { get; set; }
        
        /// <summary>
        /// 源单号ID
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// 源单号
        /// </summary>
        public string SourceNo { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        public int? CustomerId { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        
        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        
        /// <summary>
        /// 计划结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// 审核状态
        /// </summary>
        public byte? AuditStatus { get; set; }
        
        /// <summary>
        /// 审核人ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string AuditName { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 制单人ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单人姓名
        /// </summary>
        public string OperatorName { get; set; }
        
        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime OperatorTime { get; set; }
        
        /// <summary>
        /// MRP运算状态 true已运算 false未运算
        /// </summary>
        public bool? MRPStatus { get; set; }
        
        /// <summary>
        /// MRP运算时间
        /// </summary>
        public DateTime? MRPTime { get; set; }
        
        /// <summary>
        /// 生产状态 0待生产 1生产中 2生产完成
        /// </summary>
        public byte? ProductionStatus { get; set; }
        
        /// <summary>
        /// 转入库单标识 true可转 false不可转
        /// </summary>
        public bool? TransferFlag { get; set; }


        /// <summary>
        /// 转入生产采购单 true可转 false不可转
        /// </summary>
        public bool? IsPurchase { get; set; } = true;

        /// <summary>
        /// 转入领料单 true可转 false不可转
        /// </summary>
        public bool? IsPick { get; set; } = true;

        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识 false未删除 true删除
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// 生产入库单ID
        /// </summary>
        public int TranSourceId { get; set; }

        /// <summary>
        /// 生产入库单单号
        /// </summary>
        public string TranSourceCode { get; set; }

        /// <summary>
        /// 领料单
        /// </summary>
        public List<OrderIDenEntity> PickModel { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public List<OrderIDenEntity> Purchase { get; set; }


        /// <summary>
        /// 入库申请单
        /// </summary>
        public List<OrderIDenEntity> WhApplyMain { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMProductionOrderDetailQueryModel> ChildList { get; set; }
    }

    public class OrderIDenEntity
    {
        public int ID
        {
            get;set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string NO { get; set; }
    }
}
