using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 对象导出到excel文件流view modle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportParaVM<T>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public List<T> Entitys { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 列集合
        /// </summary>
        public List<string> Columns { get; set; }
        /// <summary>
        /// 中文列名集合
        /// </summary>
        public List<string> Titles { get; set; }
    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportTemplateVM<T>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public List<T> Entitys { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 列集合
        /// </summary>
        public List<string> Columns { get; set; }
        /// <summary>
        /// 中文列名集合
        /// </summary>
        public List<string> Titles { get; set; }

        /// <summary>
        /// 数据填充开始行
        /// </summary>
        public int FillRow { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string Path { get; set; }
    }


    /// <summary>
    /// 导出期初
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportOpeningTemplateVM<T>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public List<T> Entitys { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 列集合
        /// </summary>
        public List<string> Columns { get; set; }
        /// <summary>
        /// 中文列名集合
        /// </summary>
        public List<string> Titles { get; set; }

        /// <summary>
        /// 数据填充开始行
        /// </summary>
        public int FillRow { get; set; }



        /// <summary>
        /// 下拉数据源
        /// </summary>
        public string[] DropDownEntitys { get; set; }
        /// <summary>
        /// 下拉数据填充开始列
        /// </summary>
        public int DropDownFillStartCell { get; set; }
        /// <summary>
        /// 下拉数据填充结束列
        /// </summary>
        public int DropDownFillEndCell { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string Path { get; set; }
    }
}
