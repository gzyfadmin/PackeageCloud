///////////////////////////////////////////////////////////////////////////////////////
// File: TPMBulletin.cs
// Author: www.cloudyf.com
// Create Time:2019/8/7
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PM_Bulletin Db Model
    /// </summary>
    [SugarTable("T_PM_Bulletin")]
    public class TPMBulletinDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        public int ID { get; set; }
        
        /// <summary>
        /// 公告主题
        /// </summary>
        [SugarColumn(ColumnName = "BulletinTitle")]
        public string BulletinTitle { get; set; }
        
        /// <summary>
        /// 公告详情
        /// </summary>
        [SugarColumn(ColumnName = "BulletinContent")]
        public string BulletinContent { get; set; }
        
        /// <summary>
        /// 发布日期
        /// </summary>
        [SugarColumn(ColumnName = "PublishDate")]
        public DateTime PublishDate { get; set; }
        
        /// <summary>
        /// 发布人
        /// </summary>
        [SugarColumn(ColumnName = "Publisher")]
        public int Publisher { get; set; }
        
        /// <summary>
        /// 接收租户
        /// </summary>
        [SugarColumn(ColumnName = "Receivers")]
        public string Receivers { get; set; }
        
        /// <summary>
        /// 重新发布
        /// </summary>
        [SugarColumn(ColumnName = "RePublish")]
        public bool? RePublish { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        public int CreateId { get; set; }
        
    }
}
