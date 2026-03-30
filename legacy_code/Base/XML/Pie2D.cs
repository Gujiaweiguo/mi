using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
namespace Base.XML
{
    public class Pie2D
    {
        private bool shownames = false;
        private string caption = "";
        public string Caption
        {
            set { this.caption = value; }
        }
        public bool ShowNames
        {
            set { this.shownames = value; }
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
                StringBuilder[] strDs = new StringBuilder[dt.Rows.Count];
                xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlData.Append("<graph baseFontSize='12' caption='" + caption + "' showNames='" + Convert.ToInt16(shownames).ToString() +"' decimalPrecision='0' formatNumberScale='0' ");
                xmlData.Append("aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");
                int intMaxValueRow=0;
                for(int i=0;i<dt.Rows.Count-1;i++)
                {
                    decimal maxVales=0;
                    if(Convert.ToDecimal(dt.Rows[i][1].ToString())>maxVales )
                    {
                        maxVales=Convert.ToDecimal(dt.Rows[i][1].ToString());
                        intMaxValueRow=i;
                    }
                }
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    strDs[i] = new StringBuilder();
                    strDs[i].AppendFormat("<set name='{0}' value='{1}' isSliced='{2}' />", dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(),i==intMaxValueRow?"1":"0");
                }
                for (int i = 0; i <= strDs.Length - 1; i++)
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
