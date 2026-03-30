using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Base.Page;

public partial class ReportM_ShowReport : BasePage
{
    public String RepWebSer = "CrystalReport.aspx?";
    //public String RepWebSer = "http://localhost:6164/Web/ReportM/CrystalReport.aspx?rptPathName=";
    //public String RepWebSer = "http://192.168.0.20/ReportM/CrystalReport.aspx?rptPathName=";

    protected void Page_Load(object sender, EventArgs e)
    {


        String RptName = Request["ReportName"];

        RptName = GetResx(RptName);
        //String RptName1 = this.Session["tempxhq"].ToString();
        //RptName = "/Mi/Base/Customer.rpt";//"Request["ReportName"];
        this.rpturl.Value = RepWebSer + "rptPathName=" + RptName;
    }

    /* 中文转换为unicode码

     * 
     * 
     */ 
    private string bytesToUnicode(string strChinese)
    {
        if (strChinese == null || strChinese.Length == 0)
        {
            return "";
        }

        StringBuilder strUnicode = new StringBuilder();
        string s1;
        string s2;

        for (int index = 0; index < strChinese.Length; index++)
        {
            byte[] bt = System.Text.Encoding.Unicode.GetBytes(strChinese.Substring(index, 1));
            if (bt.Length > 1) //判断是否汉字
            {
                s1 = Convert.ToString((short)(bt[1] - '\0'), 16); //转化为16进制字符串

                s2 = Convert.ToString((short)(bt[0] - '\0'), 16); //转化为16进制字符串

                s1 = (s1.Length == 1 ? "0" : "") + s1; //不足位补0
                s2 = (s2.Length == 1 ? "0" : "") + s2; //不足位补0
                strUnicode.Append("%u" + s1 + s2);
            }
        }
        strUnicode.Append("%u0020");

        return strUnicode.ToString();
    }

    /* 获取资源数据
     * 
     * 
     */
    private String GetResx(String s)
    {
        char[] delimit = new char[] { '#' };
        String resx = "";
        String sR = "";
        int i = 0;
        foreach (string substr in s.Split(delimit))
        {
            if (i == 0)
            {
                resx += substr;
            } else
            {
                sR = (String) GetGlobalResourceObject("BaseInfo", substr);
                sR = bytesToUnicode(sR);
                if (i == 1)
                    resx += "&title=" + sR;
                else
                    resx += "&f" + (i - 1) + "=" + sR;
            }
            i++;
        }
        return resx;
    }


}
