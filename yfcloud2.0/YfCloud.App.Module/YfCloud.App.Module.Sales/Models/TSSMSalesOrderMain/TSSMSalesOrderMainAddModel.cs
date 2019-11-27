///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderMainAddModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.DBModel;
using YfCloud.App.Module.Sales.Models.TSSMSalesOrderDetail;

namespace YfCloud.App.Module.Sales.Models.TSSMSalesOrderMain
{
    /// <summary>
    /// T_SSM_SalesOrderMain Add Model
    /// </summary>
    [UseAutoMapper(typeof(TSSMSalesOrderMainDbModel))]
    public class TSSMSalesOrderMainAddModel : LogModelBase
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public new int ID { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        [Required]
        public int CustomerId { get; set; }
        
        /// <summary>
        /// 业务员ID
        /// </summary>
        [Required]
        public int SalesmanId { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [StringLength(maximumLength:20)]
        public string OrderNo { get; set; }
        
        /// <summary>
        /// 订单类型
        /// </summary>
        [Required]
        public int OrderTypeId { get; set; }
        
        /// <summary>
        /// 结算方式
        /// </summary>
        public int? SettlementTypeId { get; set; }
        
        /// <summary>
        /// 币种
        /// </summary>
        [StringLength(maximumLength:10)]
        public string Currency { get; set; }
        
        /// <summary>
        /// 收货地址
        /// </summary>
        [StringLength(maximumLength:200)]
        public string ReceiptAddress { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }
        
        /// <summary>
        /// 制单员ID
        /// </summary>
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(maximumLength:10)]
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        [StringLength(maximumLength:20)]
        public string ContactNumber { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        [Required]
        public List<TSSMSalesOrderDetailAddModel> ChildList { get; set; }
    }
}
