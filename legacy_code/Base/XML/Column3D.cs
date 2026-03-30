using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
namespace Base.XML
{
    public class Column3D
    {
        private string caption = "";//主标题
        private string xaxisName = "";//x轴标题
        private string yaxisName = "";//y轴标题

        /// <summary>
        /// 主标题
        /// </summary>
        public string Caption
        {
            set { this.caption = value; }
        }
        /// <summary>
        /// X轴标题
        /// </summary>
        public string xAxisName
        {
            set { this.xaxisName = value; }
        }
        /// <summary>
        /// Y轴标题
        /// </summary>
        public string yAxisName
        {
            set { this.yaxisName = value; }
        }

        /// <summary>
        /// 生成符合Column3D数据格式要求的xml文件
        /// </summary>
        /// <param name="strFilePath">xml文件，包括路径</param>
        /// <param name="dt">数据表,包括Name和value两列</param>
        public void GetXml(string strFilePath, DataTable dt)
        {
             StringBuilder xmlData = new StringBuilder();
             if (dt == null || dt.Rows.Count == 0)
             {
                 xmlData.Append("<Chart></Chart>");
             }
             else
             {
                 StringBuilder[] strDs = new StringBuilder[dt.Rows.Count ];
                 xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                 xmlData.Append("<graph xAxisName='" + xaxisName + "' yAxisName='" + yaxisName + "' baseFontSize='12' caption='" + caption + "' decimalPrecision='0' formatNumberScale='0' ");
                 xmlData.Append("aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");

                string[] strColor=new string[12];
                strColor[0] = "AFD8F8";
                strColor[1] = "F6BD0F";
                strColor[2] = "8BBA00";
                strColor[3] = "FF8E46";
                strColor[4] = "008E8E";
                strColor[5] = "D64646";
                strColor[6] = "8E468E";
                strColor[7] = "588526";
                strColor[8] = "B3AA00";
                strColor[9] = "008ED6";
                strColor[10] = "9D080D";
                strColor[11] = "A186BE";
                for (int i = 0; i <= dt.Rows.Count-1; i++)
                {
                    strDs[i] = new StringBuilder();
                    strDs[i].AppendFormat("<set name='{0}' value='{1}' color='{2}' />", dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), i < 12 ? strColor[i] : strColor[0]);
                }
                for (int i = 0; i <= strDs.Length-1; i++)
                {
                    xmlData.Append(strDs[i].ToString());
                }
                xmlData.Append("</graph>");
                 if (File.Exists(strFilePath))
                {
                    File.Delete(strFilePath);
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData.ToString());
                doc.Save(strFilePath);
             }
        }
    }
}
