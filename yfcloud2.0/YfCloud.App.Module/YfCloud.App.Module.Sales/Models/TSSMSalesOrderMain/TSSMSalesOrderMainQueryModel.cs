///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetail;

namespace YfCloud.App.Module.Sales.Models.TSSMSalesOrderMain
{
    /// <summary>
    /// TSSMSalesOrderMain Query Model
    /// </summary>
    public class TSSMSalesOrderMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        public int SalesmanId { get; set; }

        /// <summary>
        /// 业务员姓名
        /// </summary>
        public string SalesmanName { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        
        /// <summary>
        /// 订单类型ID
        /// </summary>
        public int OrderTypeId { get; set; }

        /// <summary>
        /// 订单类型名称
        /// </summary>
        public string OrderTypeName { get; set; }
        
        /// <summary>
        /// 结算方式ID
        /// </summary>
        public int? SettlementTypeId { get; set; }
        
        /// <summary>
        /// 结算方式名称
        /// </summary>
        public string SettementTypeName { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; } = 0;
        
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
        /// 制单员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNumber { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; } = false;

        /// <summary>
        /// 转单状态 true可转 false不可转
        /// </summary>
        public bool? TransferStatus { get; set; } = true;
        
        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// 销售数量总计
        /// </summary>
        public decimal? SalesNum { get; set; }

        /// <summary>
        /// 销售金额总计
        /// </summary>
        public decimal? SalesAmount { get; set; }

        /// <summary>
        /// 销售订单转生产单标志，True可转，false不可转
        /// </summary>
        public bool? TransProdStatus { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TSSMSalesOrderDetailQueryModel> ChildList { get; set; }
        
    }
}
