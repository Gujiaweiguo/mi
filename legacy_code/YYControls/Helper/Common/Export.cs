using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Data;
using System.IO;

namespace YYControls.Helper
{
    /// <summary>
    /// 常用Helper
    /// </summary>
    public partial class Common
    {
        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();

            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            string[] headers = new string[dtExport.Columns.Count];
            string[] fields = new string[dtExport.Columns.Count];

            for (int i = 0; i < dtExport.Columns.Count; i++)
            {
                headers[i] = dtExport.Columns[i].ColumnName;
                fields[i] = ReplaceSpecialChars(dtExport.Columns[i].ColumnName);
            }

            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }

        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="columnIndexList">列索引集合</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, int[] columnIndexList, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();

            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            string[] headers = new string[columnIndexList.Length];
            string[] fields = new string[columnIndexList.Length];

            for (int i = 0; i < columnIndexList.Length; i++)
            {
                headers[i] = dtExport.Columns[columnIndexList[i]].ColumnName;
                fields[i] = ReplaceSpecialChars(dtExport.Columns[columnIndexList[i]].ColumnName);
            }

            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }

        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="columnIndexList">列索引集合</param>
        /// <param name="headers">字段数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, int[] columnIndexList, string[] headers, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();

            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            string[] fields = new string[columnIndexList.Length];

            for (int i = 0; i < columnIndexList.Length; i++)
            {
                fields[i] = ReplaceSpecialChars(dtExport.Columns[columnIndexList[i]].ColumnName);
            }

            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }

        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="headers">表头数组</param>
        /// <param name="fields">字段数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        private static void Export(DataSet ds, string[] headers, string[] fields, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = String.Format("text/{0}", exportFormat.ToString().ToLower());
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}.{1}", fileName, exportFormat.ToString().ToLower()));
            HttpContext.Current.Response.ContentEncoding = encoding;

            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, encoding);

            CreateStylesheet(writer, headers, fields, exportFormat);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            XmlDataDocument xmlDoc = new XmlDataDocument(ds);
            XslCompiledTransform xslTran = new XslCompiledTransform();
            xslTran.Load(new XmlTextReader(stream));

            System.IO.StringWriter sw = new System.IO.StringWriter();
            xslTran.Transform(xmlDoc, null, sw);

            HttpContext.Current.Response.Write(sw.ToString());
            sw.Close();
            writer.Close();
            stream.Close();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 动态生成XSL，并写入XML流
        /// </summary>
        /// <param name="writer">XML流</param>
        /// <param name="headers">表头数组</param>
        /// <param name="fields">字段数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        private static void CreateStylesheet(XmlTextWriter writer, string[] headers, string[] fields, ExportFormat exportFormat)
        {
            string ns = "http://www.w3.org/1999/XSL/Transform";
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("xsl", "stylesheet", ns);
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("xsl:output");
            writer.WriteAttributeString("method", "text");
            writer.WriteAttributeString("version", "4.0");
            writer.WriteEndElement();

            // xsl-template
            writer.WriteStartElement("xsl:template");
            writer.WriteAttributeString("match", "/");

            // xsl:value-of for headers
            for (int i = 0; i < headers.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", "'" + headers[i] + "'");
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != fields.Length - 1) writer.WriteString((exportFormat == ExportFormat.CSV) ? "," : "	");
            }

            // xsl:for-each
            writer.WriteStartElement("xsl:for-each");
            writer.WriteAttributeString("select", "Export/Values");
            writer.WriteString("\r\n");

            // xsl:value-of for data fields
            for (int i = 0; i < fields.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", fields[i]);
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != fields.Length - 1) writer.WriteString((exportFormat == ExportFormat.CSV) ? "," : "	");
            }

            writer.WriteEndElement(); // xsl:for-each
            writer.WriteEndElement(); // xsl-template
            writer.WriteEndElement(); // xsl:stylesheet
        }
    }
}
