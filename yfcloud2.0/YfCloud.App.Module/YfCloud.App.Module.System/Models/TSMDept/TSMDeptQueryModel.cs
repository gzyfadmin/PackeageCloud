///////////////////////////////////////////////////////////////////////////////////////
// File: TSMDept.cs
// Author: www.cloudyf.com
// Create Time:2019/7/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.System.Models.TSMDept
{
    /// <summary>
    /// TSMDept Query Model
    /// </summary>
    public class TSMDeptQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID为null,则为未分配部门
        /// </summary>
        public int? ActualId { get; set; }

        /// <summary>
        /// 虚拟ID
        /// </summary>
        public int VitualId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        
        /// <summary>
        /// 序号
        /// </summary>
        public int SeqNumber { get; set; }
        
        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        public string PathLogic { get; set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        public string DeptDesc { get; set; }
        
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
    }

    /// <summary>
    /// 部门树
    /// </summary>
    public class TSMDeptQueryTreeModel
    {
        /// <summary>
        /// ID为null,则为未分配部门，用于提交到接口后台
        /// </summary>
        public int? ActualId { get; set; }

        /// <summary>
        /// 虚拟ID 用于前台遍历父子关系
        /// </summary>
        public int VitualId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SeqNumber { get; set; }

        /// <summary>
        /// 逻辑存储路径
        /// </summary>
        public string PathLogic { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string DeptDesc { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public List<TSMDeptQueryTreeModel> Children { get; set; }
    }

    /// <summary>
    /// TSMDept Query Model
    /// </summary>
    public class TSMDeptQueryForDispatchModel
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptID { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class TSMDeptResultForDispatchModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserAccountId { get; set; }


    }
}
