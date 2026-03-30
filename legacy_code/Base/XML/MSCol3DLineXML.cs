using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using Base.Biz;

namespace Base.XML
{
   public class MSCol3DLineXML
    {
       private string caption = "";//标题
       private string pyaxisname = "";//标题
    
        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="stritem">比较的项目</param>
        /// <param name="compare">对比个数</param>
        /// <param name="PYA">对比项目</param>
        /// <returns></returns>
        public string GetXml(string strsql,string[] stritem, string[] strcompare)
        {
            StringBuilder xmlData = new StringBuilder();
            if (strsql == "")
            {
                xmlData.Append("<Chart></Chart>");
            }
            else
            {
                BaseBO baseBo = new BaseBO();
                DataTable dt = baseBo.QueryDataSet(strsql).Tables[0];
                int compare = strcompare.Length;
                if (dt.Rows.Count > 0)
                {
                    StringBuilder strCat = new StringBuilder();
                    StringBuilder[] strDs = new StringBuilder[compare];
                    strCat.Append("<categories>");
                    for (int i = 0; i < stritem.Length; i++)
                    {
                        strCat.Append("<category label='{0}' />" + stritem[i].ToString());
                    }
                    for (int i = 0; i < compare; i++)
                    {
                        if (i == compare) //最后一个用折线
                            strDs[i].Append("<dataset seriesName='{0}' parentYAxis='S'>" + strcompare[i].ToString());
                        else
                            strDs[i].Append("<dataset seriesName='{0}'>" + strcompare[i].ToString());
                    }
                    strCat.Append("</categories>");
                    xmlData.Append("<Chart  baseFontSize='12' caption='" + Caption + "' showBorder='0' showValues='1' ");
                    xmlData.Append("PYAxisName='" + PYAxisName + "' canvasbgColor='ffffff' canvasbgAlpha='40'  aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");
                    xmlData.Append(strCat);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < compare; j++)
                        {
                            if (strcompare[j].ToString() == dt.Rows[i][1].ToString())
                                strDs[j].Append("<set value='" + dt.Rows[i][1].ToString() + "' />");
                            if (i == dt.Rows.Count)
                            {
                                strDs[j].Append("</dataset>");
                                xmlData.Append(strDs[j]);
                            }
                        }
                    }
                    xmlData.Append("</Chart>");
                }
                else
                    xmlData.Append("<Chart></Chart>");
            }
            return xmlData.ToString();
        }


        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="stritem">比较的项目</param>
        /// <param name="compare">对比个数</param>
        /// <param name="PYA">对比项目</param>
        /// <returns></returns>
      //  public string GetXml(string strsql, string[] stritem, string[] strcompare)
        public void GetXml(string strFilePath, DataTable dt)
        {
            StringBuilder xmlData = new StringBuilder();
            if (dt==null || dt.Rows.Count==0)
            {
                xmlData.Append("<Chart></Chart>");
            }
            else
            {
                StringBuilder strCat = new StringBuilder();
                StringBuilder[] strDs = new StringBuilder[dt.Columns.Count-1];
             
                strCat.Append("<categories>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strCat.AppendFormat("<category label='{0}' />" ,dt.Rows[i][0].ToString());
                }
                strCat.Append("</categories>");

                for (int i = 0; i < dt.Columns.Count; i++)
                {                    
                    if(i<strDs.Length)
                    {
                        strDs[i] = new StringBuilder();
                        if (i == 0 && parentyaxis==true)
                        {
                            strDs[i].AppendFormat("<dataset seriesName='{0}' parentYAxis='S'>", dt.Columns[i + 1].ColumnName);
                        }
                        else
                        {
                            strDs[i].AppendFormat("<dataset seriesName='{0}'>", dt.Columns[i + 1].ColumnName);
                        }

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            strDs[i].AppendFormat("<set value='{0}' />" , dt.Rows[j][i + 1].ToString() );
                        }
                        strDs[i].Append("</dataset>");
                    }          
                }
                xmlData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlData.Append("<Chart  baseFontSize='12' caption='" + Caption + "' showBorder='0' showValues='1' ");
                xmlData.Append("PYAxisName='" + PYAxisName + "' canvasbgColor='ffffff' canvasbgAlpha='40'  aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>");
                xmlData.Append(strCat);
                for (int i = 0; i < strDs.Length; i++)
                {
                    xmlData.Append(strDs[i].ToString());
                }
                xmlData.Append("</Chart>");  
            }

            if (File.Exists(strFilePath))
            {
                File.Delete(strFilePath);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.ToString());
            doc.Save(strFilePath);
        }
        private bool parentyaxis=false;
        public bool parentYAxis
        {
            set { this.parentyaxis = value; }
        }
        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }
       public string PYAxisName
       {
           get { return this.pyaxisname; }
           set { this.pyaxisname = value; }
       }

    }
}
