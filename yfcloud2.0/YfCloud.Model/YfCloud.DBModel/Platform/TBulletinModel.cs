///////////////////////////////////////////////////////////////////////////////////////
// File: TBulletin.cs
// Author: www.cloudyf.com
// Create Time:2019/6/20
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_Bulletin Model
    /// </summary>
    [SugarTable("T_PM_Bulletin")]
    public class TBulletinModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID", IsIdentity = true)]
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// 公告主题
        /// </summary>
        [SugarColumn(ColumnName = "BulletinTitle")]
        [Required]
        [StringLength(maximumLength:30)]
        public string BulletinTitle { get; set; }
        
        /// <summary>
        /// 公告详情
        /// </summary>
        [SugarColumn(ColumnName = "BulletinContent")]
        [Required]
        [StringLength(maximumLength:200)]
        public string BulletinContent { get; set; }
        
        /// <summary>
        /// 发布日期
        /// </summary>
        [SugarColumn(ColumnName = "PublishDate")]
        [Required]
        public DateTime PublishDate { get; set; }
        
        /// <summary>
        /// 发布人ID
        /// </summary>
        [SugarColumn(ColumnName = "Publisher")]
        [Required]
        public int Publisher { get; set; }
        
        /// <summary>
        /// 发布人姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TUsersUserName { get; set; }
        
        /// <summary>
        /// 接收租户
        /// </summary>
        [SugarColumn(ColumnName = "Receivers")]
        [Required]
        [StringLength(maximumLength:255)]
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
        [Required]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "CreateId")]
        [Required]
        public int CreateId { get; set; }
        
    }
}
