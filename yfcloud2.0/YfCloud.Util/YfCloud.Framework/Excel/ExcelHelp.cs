using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using YfCloud.Models;

namespace YfCloud.Framework.Excel
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelp
    {

        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="tableName"></param>
        /// <param name="lstColumn"></param>
        /// <param name="lstTitle"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys, string tableName, List<string> lstColumn, List<string> lstTitle)
        {
            if (lstColumn.Count != lstTitle.Count)
            {
                throw new Exception("列的编码和名称集合数量不一致");
            }
            DataTable dt = new DataTable();
            if (entitys.Count == 0) return dt;

            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            var pDic = entityProperties.ToDictionary(x => x.Name.ToUpperInvariant(), x => x);
            Dictionary<string, string> titleDic = new Dictionary<string, string>();

            string header;
            for (int i = 0; i < lstColumn.Count; i++)
            {
                var col = lstColumn[i];
                var title = lstTitle[i];
                header = col.ToUpperInvariant();
                if (pDic.ContainsKey(header))
                {
                    Type columnType = pDic[header].PropertyType;

                    if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        columnType = columnType.GetGenericArguments()[0];
                    }

                    dt.Columns.Add(header, columnType);

                    titleDic.Add(header, title);
                }
            }

            if (entitys != null)
            {
                foreach (var entity in entitys)
                {
                    if (entity.GetType() != entityType)
                    {
                        throw new Exception("要转换的集合元素类型不一致");
                    }

                    DataRow dr = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        var property = pDic[col.ColumnName];
                        dr[col] = property.GetValue(entity) ?? DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            foreach (DataColumn col in dt.Columns)
            {
                col.ColumnName = titleDic[col.ColumnName];
            }
            dt.TableName = tableName;
            return dt;
        }

        public static MemoryStream Export<T>(ExportParaVM<T> model)
        {
            DataTable dt = ListToDataTable<T>(model.Entitys, model.TableName, model.Columns, model.Titles);

            return ExportExcel2007(dt, model.HeaderText);
        }

        private static MemoryStream ExportExcel2007(DataTable dtSource, string strHeaderText)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = workbook.CreateSheet() as XSSFSheet;
            //格式日期
            XSSFCellStyle dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFDataFormat format = workbook.CreateDataFormat() as XSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
            //格式数字
            XSSFCellStyle decimelStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFDataFormat decimelformat = workbook.CreateDataFormat() as XSSFDataFormat;
            decimelStyle.DataFormat = decimelformat.GetFormat("0.00####");
            // 取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }

            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                if (rowIndex == 1048576 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as XSSFSheet;
                    }

                    #region 表头及样式

                    {
                        XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);
                        XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        XSSFFont font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    }

                    #endregion 表头及样式

                    #region 列头及样式

                    {
                        XSSFRow headerRow = sheet.CreateRow(1) as XSSFRow;
                        XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        XSSFFont font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.IsLocked = true;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] > 255 ? 254 : arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //sheet.CreateFreezePane(0, 2, 0, dtSource.Columns.Count - 1);
                    }

                    rowIndex = 2;

                    #endregion 列头及样式

                    rowIndex = 2;
                }

                #region 填充内容

                XSSFRow dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    XSSFCell newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime"://日期类型
                            if (drValue.Length > 0)
                            {
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);

                                newCell.CellStyle = dateStyle;//格式化显示
                            }
                            break;

                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;

                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;

                        case "System.Decimal"://浮点型
                        case "System.Double":
                            Double doubV = 0;
                            Double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            newCell.CellStyle = decimelStyle;
                            break;

                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;

                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion 填充内容

                rowIndex++;
            }

            NpoiMemoryStream ms = new NpoiMemoryStream();
            ms.AllowClose = false;
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }

        /// <summary>
        /// 根据模板导出数据
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="exportTemplateFilePath"></param>
        /// <param name="fillRow"></param>
        /// <param name="replaceCells"></param>
        /// <returns></returns>
        private static MemoryStream ExportExcelByTemplate(DataTable dtSource, string exportTemplateFilePath, int fillRow)
        {
            try
            {
                //打开Excle模板文件 
                IWorkbook workbook = null;
                using (FileStream fileOne = new FileStream(exportTemplateFilePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fileOne); //获取第一个工作表  

                }
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);//获取第一个sheet 
                //格式日期
                XSSFCellStyle dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                XSSFDataFormat format = workbook.CreateDataFormat() as XSSFDataFormat;
                dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                //格式数字
                XSSFCellStyle decimelStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                XSSFDataFormat decimelformat = workbook.CreateDataFormat() as XSSFDataFormat;
                decimelStyle.DataFormat = decimelformat.GetFormat("0.00####");
                //单元格样式
                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;

                int rowIndex = fillRow;

                foreach (DataRow row in dtSource.Rows)
                {

                    #region 填充内容
                    //sheet.ShiftRows(rowIndex, sheet.LastRowNum, 1, true, false);
                    XSSFRow dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        XSSFCell newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;
                        string drValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                break;

                            case "System.DateTime"://日期类型
                                if (drValue.Length > 0)
                                {
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);

                                    newCell.CellStyle = dateStyle;//格式化显示
                                }
                                break;

                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;

                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;

                            case "System.Decimal"://浮点型
                            case "System.Double":
                                Double doubV = 0;
                                Double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                newCell.CellStyle = decimelStyle;
                                break;

                            case "System.DBNull"://空值处理
                                newCell.SetCellValue("");
                                break;

                            default:
                                newCell.SetCellValue("");
                                break;
                        }


                        newCell.CellStyle = style;
                    }

                    #endregion 填充内容

                    rowIndex++;
                }

                NpoiMemoryStream ms = new NpoiMemoryStream();
                ms.AllowClose = false;
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                ms.AllowClose = true;
                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        /// <summary>
        /// 导出期初模板
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="exportTemplateFilePath"></param>
        /// <param name="fillRow"></param>
        /// <param name="replaceCells"></param>
        /// <returns></returns>
        private static MemoryStream ExportOpeningTemplate(DataTable dtSource, string[] dropDowndtSource, string exportTemplateFilePath, int fillRow, int dropDownFillStartCell, int dropDownFillEndCell)
        {
            try
            {
                //打开Excle模板文件 
                IWorkbook workbook = null;
                using (FileStream fileOne = new FileStream(exportTemplateFilePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fileOne); //获取第一个工作表  

                }
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);//获取第一个sheet 
                XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper(sheet);
                XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper
                        .CreateExplicitListConstraint(dropDowndtSource);
                CellRangeAddressList addressList = new CellRangeAddressList(1, dtSource.Rows.Count, dropDownFillStartCell, dropDownFillEndCell);
                XSSFDataValidation validation = (XSSFDataValidation)dvHelper.CreateValidation(dvConstraint, addressList);
                sheet.AddValidationData(validation);



                //格式日期
                XSSFCellStyle dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                XSSFDataFormat format = workbook.CreateDataFormat() as XSSFDataFormat;
                dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                //格式数字
                XSSFCellStyle decimelStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                XSSFDataFormat decimelformat = workbook.CreateDataFormat() as XSSFDataFormat;
                decimelStyle.DataFormat = decimelformat.GetFormat("0.00####");
                //单元格样式
                ICellStyle style = workbook.CreateCellStyle();
                //style.BorderBottom = BorderStyle.Thin;
                //style.BorderLeft = BorderStyle.Thin;
                //style.BorderRight = BorderStyle.Thin;
                //style.BorderTop = BorderStyle.Thin;

                int rowIndex = fillRow;

                foreach (DataRow row in dtSource.Rows)
                {

                    #region 填充内容
                    //sheet.ShiftRows(rowIndex, sheet.LastRowNum, 1, true, false);
                    XSSFRow dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        XSSFCell newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;
                        string drValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                break;

                            case "System.DateTime"://日期类型
                                if (drValue.Length > 0)
                                {
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);

                                    newCell.CellStyle = dateStyle;//格式化显示
                                }
                                break;

                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;

                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;

                            case "System.Decimal"://浮点型
                            case "System.Double":
                                Double doubV = 0;
                                Double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                newCell.CellStyle = decimelStyle;
                                break;

                            case "System.DBNull"://空值处理
                                newCell.SetCellValue("");
                                break;

                            default:
                                newCell.SetCellValue("");
                                break;
                        }


                        newCell.CellStyle = style;
                    }

                    #endregion 填充内容

                    rowIndex++;
                }

                NpoiMemoryStream ms = new NpoiMemoryStream();
                ms.AllowClose = false;
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                ms.AllowClose = true;
                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MemoryStream ExportOpeningTemplate<T>(ExportOpeningTemplateVM<T> model)
        {
            DataTable dt = ListToDataTable<T>(model.Entitys, model.TableName, model.Columns, model.Titles);

            return ExportOpeningTemplate(dt, model.DropDownEntitys, model.Path, model.FillRow, model.DropDownFillStartCell, model.DropDownFillEndCell);
        }



        public static MemoryStream ExportExcelByTemplate<T>(ExportTemplateVM<T> model)
        {
            DataTable dt = ListToDataTable<T>(model.Entitys, model.TableName, model.Columns, model.Titles);

            return ExportExcelByTemplate(dt, model.Path, model.FillRow);
        }
    }

    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
