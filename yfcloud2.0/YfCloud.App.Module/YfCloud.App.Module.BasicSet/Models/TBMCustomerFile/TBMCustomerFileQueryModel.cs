///////////////////////////////////////////////////////////////////////////////////////
// File: TBMCustomerFile.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.BasicSet.Models.TBMCustomerContact;

namespace YfCloud.App.Module.BasicSet.Models.TBMCustomerFile
{
    /// <summary>
    /// TBMCustomerFile Query Model
    /// </summary>
    public class TBMCustomerFileQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerCode { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// 客户简称
        /// </summary>
        public string ShortName { get; set; }
        
        /// <summary>
        /// 客户类型ID
        /// </summary>
        public int CustomerTypeId { get; set; }

        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string CustomerTypeName { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        public string LegalPerson { get; set; }
        
        /// <summary>
        /// 行业ID
        /// </summary>
        public int? IndustryId { get; set; }
        
        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        
        /// <summary>
        /// 客户地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TBMCustomerContactQueryModel> ChildList { get; set; }


    }
}
