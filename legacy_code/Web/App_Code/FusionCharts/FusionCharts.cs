using System;
using System.Data;
using System.Text;
using System.Data.Odbc;

namespace EssWebControls
{
	/// <summary>
	/// FusionCharts 
	/// </summary>
	public class FusionCharts
	{
		public static string EncodeDataURL (string dataURL, bool noCacheStr)
		{
			string result = dataURL;
			if (noCacheStr)
			{
				result += (dataURL.IndexOf("?") != -1) ? "&" : "?";
				result += "FCCurrTime=" + DateTime.Now.ToString().Replace(":", "_");
			}

			return System.Web.HttpUtility.UrlEncode(result);
		}

		/// <summary>
		/// 生成FusionCharts的HTML,需要在页面中引入FusionCharts的JS文件
		/// </summary>
        /// <param name="chartSWF">FusionCharts的类型SWF文件</param>
        /// <param name="strURL">FusionCharts的xml链接地址，可空</param>
        /// <param name="strXML">FusionCharts的xml内容,储存在页面</param>
        /// <param name="chartId">用了渲染FusionCharts的DIV ID</param>
		/// <param name="chartWidth">Chart Width</param>
		/// <param name="chartHeight">Chart Height</param>
        /// <param name="debugMode">是否启动调试模式</param>
		/// <param name="registerWithJS"></param>
		/// <returns>返还带HTML和相应JS的字符串</returns>
		public static string RenderChart(string chartSWF, string strURL, string strXML, string chartId, 
			string chartWidth, string chartHeight, bool debugMode, bool registerWithJS)
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendFormat("<!-- START Script Block for Chart {0} -->" + Environment.NewLine, chartId);
            builder.AppendFormat("<div id='{0}Div' align='center'>" + Environment.NewLine, chartId);
			builder.Append("Chart." + Environment.NewLine);
			builder.Append("</div>" + Environment.NewLine);
			builder.Append("<script type=\"text/javascript\">" + Environment.NewLine);
			builder.AppendFormat("var chart_{0} = new FusionCharts(\"{1}\", \"{0}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\");" + Environment.NewLine, chartId, chartSWF, chartWidth, chartHeight, boolToNum(debugMode), boolToNum(registerWithJS));
			if (strXML.Length == 0)
			{
				builder.AppendFormat("chart_{0}.setDataURL(\"{1}\");" + Environment.NewLine, chartId, strURL);
			}
			else
			{
				builder.AppendFormat("chart_{0}.setDataXML(\"{1}\");" + Environment.NewLine, chartId, strXML);
			}
			
			
			builder.AppendFormat("chart_{0}.render(\"{1}Div\");" + Environment.NewLine, chartId, chartId);
			builder.Append("</script>" + Environment.NewLine);
			builder.AppendFormat("<!-- END Script Block for Chart {0} -->" + Environment.NewLine, chartId);
			return builder.ToString();
		}

		/// <summary>
        /// 生成FusionCharts的HTML,需要在页面中引入FusionCharts的JS文件
        /// 用于没有安装Flash插件的浏览器
		/// </summary>
        /// <param name="chartSWF">FusionCharts的类型SWF文件</param>
        /// <param name="strURL">FusionCharts的xml链接地址，可空</param>
        /// <param name="strXML">FusionCharts的xml内容,储存在页面</param>
        /// <param name="chartId">用了渲染FusionCharts的DIV ID</param>
		/// <param name="chartWidth"></param>
		/// <param name="chartHeight"></param>
		/// <param name="debugMode"></param>
		/// <returns></returns>
		public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, 
			string chartWidth, string chartHeight, bool debugMode)
		{
			StringBuilder strFlashVars = new StringBuilder();
			string flashVariables = String.Empty;
			if (strXML.Length == 0)
			{
				//DataURL Mode
				flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&dataURL={3}", chartWidth, chartHeight, boolToNum(debugMode),strURL);
			}
			else
				//DataXML Mode
			{
				flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&dataXML={3}", chartWidth,chartHeight,boolToNum(debugMode),strXML);
			}
			
			strFlashVars.AppendFormat("<!-- START Code Block for Chart {0} -->" + Environment.NewLine, chartId);
			strFlashVars.AppendFormat("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\" width=\"{0}\" height=\"{1}\" name=\"{2}\">" + Environment.NewLine, chartWidth,chartHeight, chartId);
			strFlashVars.Append("<param name=\"allowScriptAccess\" value=\"always\" />" + Environment.NewLine);
			strFlashVars.AppendFormat("<param name=\"movie\" value=\"{0}\"/>" + Environment.NewLine, chartSWF);
			strFlashVars.AppendFormat("<param name=\"FlashVars\" value=\"{0}\" />" + Environment.NewLine, flashVariables);
			strFlashVars.Append("<param name=\"quality\" value=\"high\" />" + Environment.NewLine);
			strFlashVars.AppendFormat("<embed src=\"{0}\" FlashVars=\"{1}\" quality=\"high\" width=\"{2}\" height=\"{3}\" name=\"{4}\"  allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />" + Environment.NewLine, chartSWF,flashVariables,chartWidth,chartHeight,chartId);
			strFlashVars.Append("</object>" + Environment.NewLine);
			strFlashVars.AppendFormat("<!-- END Code Block for Chart {0} -->" + Environment.NewLine, chartId);
											
			return strFlashVars.ToString();
		}

		/// <summary>
		/// 根据bool的值返还相应数字
		/// </summary>
		/// <param name="value">True Or False</param>
		/// <returns>返还0或1，True返还1</returns>
		private static int boolToNum(bool value)
		{
			return value ? 1 : 0;			
		}

	}
}
