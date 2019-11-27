///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_BM_CustomerFile Db Model
    /// </summary>
    [SugarTable("T_BM_CustomerFile")]
    public class TBMCustomerFileDbModel: ModelContext
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 客户编号
        /// </summary>
        [SugarColumn(ColumnName = "CustomerCode")]
        public string CustomerCode { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        [SugarColumn(ColumnName = "CustomerName")]
        public string CustomerName { get; set; }
        
        /// <summary>
        /// 客户简称
        /// </summary>
        [SugarColumn(ColumnName = "ShortName")]
        public string ShortName { get; set; }
        
        /// <summary>
        /// 客户类型ID
        /// </summary>
        [SugarColumn(ColumnName = "CustomerTypeId")]
        public int CustomerTypeId { get; set; }
        
        /// <summary>
        /// 法人
        /// </summary>
        [SugarColumn(ColumnName = "LegalPerson")]
        public string LegalPerson { get; set; }
        
        /// <summary>
        /// 行业ID
        /// </summary>
        [SugarColumn(ColumnName = "IndustryId")]
        public int? IndustryId { get; set; }
        
        /// <summary>
        /// 城市
        /// </summary>
        [SugarColumn(ColumnName = "City")]
        public string City { get; set; }
        
        /// <summary>
        /// 客户地址
        /// </summary>
        [SugarColumn(ColumnName = "Address")]
        public string Address { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        public bool DeleteFlag { get; set; } = false;

        [SugarColumn(IsIgnore = true)]
        public List<TBMCustomerContactDbModel> Child
        {
            get
            {
                return base.CreateMapping<TBMCustomerContactDbModel>().Where(it => it.CustomerId==this.ID).ToList();
            }
        }

    }
}
