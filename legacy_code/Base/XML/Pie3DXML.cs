using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;

namespace Base.XML
{
    public class Pie3DXML
    {
        private string caption = "";
        /// <summary>
        /// 获得Pie3D使用的xml
        /// </summary>
        /// <param name="strFilePath">返还xml的路径</param>
        /// <param name="dt">第一列是名称、第二列是数据</param>
        public void GetXml(string strFilePath,DataTable dt)
        {
            bool xmlFiles = File.Exists(strFilePath);
            if (xmlFiles)
            {
                File.Delete(strFilePath);
            }

            XmlDocument doc = new XmlDocument();
            string strset = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strset = strset + "<set label='" + dt.Rows[i][0].ToString() + "' value='" + dt.Rows[i][1].ToString() + "' />";
                }
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<chart bgColor='ffffff' enableRotation ='1' baseFontSize='12' palette='4' decimals='0' showvalues='0' bleSmartLabels='1' bgAlpha='40,100' bgRatio='0,100' bgAngle='360' showBorder='0' startingAngle='170' numberScaleValue='W' formatNumber='0' caption='" + Caption + "' defaultAnimation='0' aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>" +
                            strset + "</chart>");
            }
            else 
            {
                doc.LoadXml("<Chart></Chart>");
            }

            try
            {
                doc.Save(strFilePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public string GetXml(DataTable dt)
        {
            string strxml;
            if (dt.Rows.Count > 0)
            {
                string strset = "";
                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    strset = strset + "<set label='" + dt.Rows[i][0].ToString() + "' value='" + dt.Rows[i][1].ToString() + "' />";
                }
                strxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<chart bgColor='ffffff' enableRotation ='1' baseFontSize='12' palette='4' decimals='0' showvalues='0' bleSmartLabels='1' bgAlpha='40,100' bgRatio='0,100' bgAngle='360' showBorder='0' startingAngle='170' numberScaleValue='W' formatNumber='0' caption='" + Caption + "' defaultAnimation='0' aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>" +
                            strset + "</chart>";
            }
            else
            {
                strxml = "<Chart></Chart>";
            }
            return strxml;
        }

        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }
    }
}
