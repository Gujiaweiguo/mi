using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
namespace Base.XML
{
    public class ChartLine2D
    {
        private string caption = "";//主标题
        private string xaxisName = "";//x轴标题
        private string yaxisName = "";//y轴标题
        private string subcaption = "";//子标题
        private bool showvalues = false;
        private bool shownames=false ;
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
        /// 子标题
        /// </summary>
        public string SubCaption
        {
            set { this.subcaption = value; }
        }
        /// <summary>
        /// 是否显示数值
        /// </summary>
        public bool ShowValues
        {
            set { this.showvalues = value; }
        }

        public bool ShowNames
        {
            set {this.shownames=value;}
        }
        /// <summary>
        /// 生成符合2DLine数据格式要求的xml文件
        /// </summary>
        /// <param name="strFilePath">文件，包括路径</param>
        /// <param name="dt">数据表</param>
        public void GetXml(string strFilePath, DataTable dt)
        {
            StringBuilder xmlData = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0 || dt.Columns.Count!=3)
            {
                xmlData.Append("<Chart></Chart>");
            }
            else
            {
                StringBuilder[] strDs = new StringBuilder[dt.Rows.Count ];
                xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlData.Append("<graph xAxisName='" + xaxisName + "' yAxisName='" + yaxisName + "' subCaption='" + subcaption + "' baseFontSize='12' caption='" + caption + "' decimalPrecision='0' showValues='" + Convert.ToInt16(this.showvalues).ToString() + "' ");
                xmlData.Append("formatNumberScale='0' showNames='" + Convert.ToInt16(this.shownames).ToString() + "' divLineColor='ff5904' divLineAlpha='20' alternateHGridAlpha='5' ");
                xmlData.Append("showAlternateHGridColor='1' AlternateHGridColor='ff5904' ");
                xmlData.Append("aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");

                for (int i = 0; i <= dt.Rows.Count-1; i++)
                {
                    strDs[i] = new StringBuilder();
                    strDs[i].AppendFormat("<set name='{0}' value='{1}' hoverText='{2}' />",dt.Rows[i][0].ToString(),dt.Rows[i][1].ToString(),dt.Rows[i][2].ToString());
                }
                for (int i = 0; i <= strDs.Length-1; i++)
                {
                    xmlData.Append(strDs[i].ToString());
                }
                xmlData.Append("</graph>");
            }

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
