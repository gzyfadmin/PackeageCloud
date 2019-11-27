///////////////////////////////////////////////////////////////////////////////////////
// File: TBMSupplierFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_BM_SupplierFile Model
    /// </summary>
    [SugarTable("T_BM_SupplierFile")]
    public class TBMSupplierFileDbModel:ModelContext
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 供应商编号
        /// </summary>
        [SugarColumn(ColumnName = "SupplierCode")]
        [Required]
        [StringLength(maximumLength:20)]
        public string SupplierCode { get; set; }
        
        /// <summary>
        /// 供应商名称
        /// </summary>
        [SugarColumn(ColumnName = "SupplierName")]
        [Required]
        [StringLength(maximumLength:50)]
        public string SupplierName { get; set; }
        
        /// <summary>
        /// 供应商简称
        /// </summary>
        [SugarColumn(ColumnName = "ShortName")]
        [StringLength(maximumLength:10)]
        public string ShortName { get; set; }
        
        /// <summary>
        /// 供应商类型ID
        /// </summary>
        [SugarColumn(ColumnName = "SupplierTypeId")]
        [Required]
        public int SupplierTypeId { get; set; }
        
        /// <summary>
        /// 法人
        /// </summary>
        [SugarColumn(ColumnName = "LegalPerson")]
        [StringLength(maximumLength:10)]
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
        [StringLength(maximumLength:20)]
        public string City { get; set; }
        
        /// <summary>
        /// 客户地址
        /// </summary>
        [SugarColumn(ColumnName = "Address")]
        [StringLength(maximumLength:100)]
        public string Address { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        [Required]
        public bool DeleteFlag { get; set; } = false;
        

        [SugarColumn(IsIgnore = true)]
        public List<TBMSupplierContactDbModel> Child
        {
            get
            {
                return base.CreateMapping<TBMSupplierContactDbModel>().Where(it => it.SupplierId == this.ID).ToList();
            }
        }
    }
}
