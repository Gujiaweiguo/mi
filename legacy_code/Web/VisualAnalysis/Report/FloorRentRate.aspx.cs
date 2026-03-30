using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Base.Biz;
using BaseInfo.Dept;
using RentableArea;
public partial class VisualAnalysis_Report_FloorRentRate : System.Web.UI.Page
{
    public string baseInfo;
    public string strPath;
    public string deptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string locaID = "";
        deptName = "测试购物中心一层";
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        if (Request.QueryString["FloorID"] != null)
        {
            locaID = Request.QueryString["FloorID"].ToString();
            strPath = "../Disktop.aspx?FloorID=" + locaID;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "FloorID=" + locaID.ToString();
            DataSet dt = baseBo.QueryDataSet(new Floors());

            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["FloorName"].ToString().Trim();
            }
        }
        else
        {
            strPath = "../Disktop.aspx";
        }


        //XML文件路径
        this.xmlFile.InnerText = "../../FusionCharts/XML/zzColumn2D.xml";
    }
    public int GetXml()
    {
        this.xmlFile.InnerText = "../../FusionCharts/XML/zzColumn2D.xml";
        return 0;
    }
}