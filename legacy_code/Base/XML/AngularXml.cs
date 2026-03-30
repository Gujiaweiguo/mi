using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Base.XML
{
    /// <summary>
    /// 用了生成FusionCharts中AngularChart格式的数据、样式xml
    /// </summary>
    public class AngularXml
    {
        private int  lowerLimit = 0;
        private int upperLimit = 0;
        //当前完成数量
        private int totalValue = 0;
        //说明文字
        private string totalTxt = "";
        //年度完成警戒指标
        private int lowValue = 0;
        //年度完成警戒说明文字
        private string lowValueTxt = "年度最低预算完成指标";
        //标题
        private string bigtital = "";
        private string smltital = "";
        //分段值

        private int maxvalue1 = 0;
        private int maxvalue2 = 0;

        public int MaxValue1
        {
          //  get { return this.maxvalue1; }
            set { this.maxvalue1 = value; }
        }
        public int MaxValue2
        {
           // get { return this.maxvalue2; }
            set { this.maxvalue2 = value; }
        }
        /// <summary>
        /// 生成XML文件
        /// </summary>
        /// <param name="strFilePath">xml绝对路径</param>
        public void GetXml(string strFilePath)
        {
            bool xmlFiles = File.Exists(strFilePath);
            if (xmlFiles)
            {
                File.Delete(strFilePath);
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<chart  bgColor='FFFFFF' lowerLimit='" + lowerLimit.ToString() + "' upperLimit='" + upperLimit.ToString() + "' tickValueDistance='20' showValue='0' showBorder='0' paletteThemeColor='669933' chartLeftMargin='25' chartRightMargin='25' decimals='3' aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>" +
            "<colorRange>" +
            "<color minValue='0' maxValue='" + maxvalue1.ToString() + "' code='FF654F'/>" +
            "<color minValue='" + maxvalue1.ToString() + "' maxValue='" + maxvalue2.ToString() + "' code='8efdfe'/>" +
            "<color minValue='" + maxvalue2.ToString() + "' maxValue='" + upperLimit.ToString() + "' code='F6BD0F'/>" +
            "</colorRange>" +
            "<dials>" +
            "<dial value='" + totalValue.ToString() + "' Tooltext='" + totalTxt + "' rearExtension='10' tooltext='" + totalValue.ToString() + "'/>" +
            "</dials>" +
            "<trendpoints>" +
            "<point startValue='" + lowValue.ToString() + "' valueInside='0' displayValue='' color='666666' useMarker='1' markerColor='F1f1f1' markerBorderColor='666666' markerRadius='5' markerTooltext='" + lowValue.ToString() + "' />" +
            "</trendpoints>" +
            "<annotations>" +
            "<annotationGroup id='Grp1' >" +
            "<annotation type='text' x='85' y='30' label='" + bigtital.ToString() + "' font='宋体' fontcolor='333333' fontSize='12' />" +
            "<annotation type='text' x='87' y='46' label='" + smltital.ToString() + "' font='宋体'  fontcolor='333333' fontSize='11' />" +
            "</annotationGroup>" +
            "</annotations>" +
            "</chart>");
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
        /// <returns></returns>
        public string GetXml()
        {
            string strxml;
            strxml = "<chart  bgColor='FFFFFF' lowerLimit='" + lowerLimit.ToString() + "' upperLimit='" + upperLimit.ToString() + "' tickValueDistance='20' showValue='0' showBorder='0' paletteThemeColor='669933' chartLeftMargin='25' chartRightMargin='25' decimals='3' aboutMenuItemLabel='About Wincor Nixdorf' aboutMenuItemLink='n-http://www.wincor-nixdorf.com'>" +
                    "<colorRange>" +
                    "<color minValue='0' maxValue='" + maxvalue1.ToString() + "' code='FF654F'/>" +
                    "<color minValue='" + maxvalue1.ToString() + "' maxValue='" + maxvalue2.ToString() + "' code='8efdfe'/>" +
                    "<color minValue='" + maxvalue2.ToString() + "' maxValue='" + upperLimit.ToString() + "' code='F6BD0F'/>" +
                    "</colorRange>" +
                    "<dials>" +
                    "<dial value='" + totalValue.ToString() + "' link='javascript:updateChart(,101);' Tooltext='" + totalTxt + "' rearExtension='10' tooltext='" + totalValue.ToString() + "'/>" +
                    "</dials>" +
                    "<trendpoints>" +
                    "<point startValue='" + lowValue.ToString() + "' valueInside='0' displayValue='' color='666666' useMarker='1' markerColor='F1f1f1' markerBorderColor='666666' markerRadius='5' markerTooltext='" + lowValue.ToString() + "' />" +
                    "</trendpoints>" +
                    "<annotations>" +
                    "<annotationGroup id='Grp1' >" +
                    "<annotation type='text' x='85' y='30' label='" + bigtital.ToString() + "' font='宋体' fontcolor='333333' fontSize='12' />" +
                    "<annotation type='text' x='87' y='46' label='" + smltital.ToString() + "' font='宋体'  fontcolor='333333' fontSize='11' />" +
                    "</annotationGroup>" +
                    "</annotations>" +
                    "</chart>";
            return strxml;
 
        }
        /// <summary>
        /// 最低指标
        /// </summary>
        public int LowerLimit 
        {
        //    get { return this.lowerLimit; }
            set { this.lowerLimit = value; }
        }
        /// <summary>
        /// 最高指标
        /// </summary>
        public int UpperLimit
        {
          //  get { return this.upperLimit; }
            set { this.upperLimit = value; }
        }
        /// <summary>
        /// 当前完成指标
        /// </summary>
        public int TotalValue
        {
          //  get { return this.totalValue; }
            set { this.totalValue = value; }
        }
        /// <summary>
        /// 当前完成指标说明文字
        /// </summary>
        public string TotalTxt
        {
          //  get { return this.totalTxt; }
            set { this.totalTxt = value; }
        }
        /// <summary>
        /// 年度指标警戒
        /// </summary>
        public int LowValue
        {
          // get { return this.lowValue; }
            set { this.lowValue = value; }
        }
        /// <summary>
        /// 年度指标警戒说明文字
        /// </summary>
        public string LowValueTxt
        {
          //  get { return this.lowValueTxt; }
            set { this.lowValueTxt = value; }
        }
        /// <summary>
        /// 大标题
        /// </summary>
        public string BigTital
        {
        //    get { return this.bigtital; }
            set { this.bigtital = value; }
        }
        /// <summary>
        /// 小标题
        /// </summary>
        public string SmlTital
        {
          //  get { return this.smltital; }
            set { this.smltital = value; }
        }

    }
}
