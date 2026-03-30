using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
namespace Base.XML
{
    public class StCol3D
    {
        private string caption = "";//主标题
        private string xaxisName = "";//x轴标题
        private string yaxisName = "";//y轴标题
        private string subcaption = "";//子标题
        private bool showvalues = false;
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

        /// <summary>
        /// 生成符合StCol3D数据格式要求的xml文件
        /// </summary>
        /// <param name="strFilePath">文件，包括路径</param>
        /// <param name="dt">数据表</param>
        public void GetXml(string strFilePath, DataTable dt)
        {
            StringBuilder xmlData = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                xmlData.Append("<Chart></Chart>");
            }
            else
            {
                StringBuilder strCat = new StringBuilder();
                StringBuilder[] strDs = new StringBuilder[dt.Columns.Count - 1];

                strCat.Append("<categories>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strCat.AppendFormat("<category name='{0}' />", dt.Rows[i][0].ToString());
                }
                strCat.Append("</categories>");
                string[] strColor = new string[3];
                strColor[0] = "AFD8F8";
                strColor[1] = "F6BD0F";
                strColor[2] = "8BBA00";
                
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i < strDs.Length)
                    {
                        strDs[i] = new StringBuilder();
                        strDs[i].AppendFormat("<dataset seriesName='{0}' color='{1}' showValues='{2}'>", dt.Columns[i + 1].ColumnName,i>2 ? strColor[3].ToString():strColor[i].ToString(), Convert.ToInt16(showvalues).ToString());

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            strDs[i].AppendFormat("<set value='{0}' />", dt.Rows[j][i + 1].ToString());
                        }
                        strDs[i].Append("</dataset>");
                    }
                }
                xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlData.Append("<graph xAxisName='" + xaxisName + "' yAxisName='" + yaxisName + "' subCaption='" + subcaption + "' baseFontSize='12' caption='" + caption + "' decimalPrecision='0' rotateNames='1' numDivLines='3' showValues='0' formatNumberScale='0' ");
                xmlData.Append("aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");
                xmlData.Append(strCat);
                for (int i = 0; i < strDs.Length; i++)
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
