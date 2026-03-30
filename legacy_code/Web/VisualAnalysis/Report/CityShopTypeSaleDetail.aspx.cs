using System;
using System.Data;
using Base.Biz;
using BaseInfo.Dept;
public partial class VisualAnalysis_Report_CityShopTypeSaleDetail : System.Web.UI.Page
{
    public string baseInfo;
    public string strPath;
    public string deptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string locaID = "";
        deptName = "测试大区";
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        if (Request.QueryString["CityID"] != null)
        {
            locaID = Request.QueryString["CityID"].ToString();
            strPath = "../Disktop.aspx?CityID=" + locaID;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "DeptID=" + locaID.ToString();
            DataSet dt = baseBo.QueryDataSet(new Dept());

            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["DeptName"].ToString().Trim();
            }
        }
        else
        {
            strPath = "../Disktop.aspx";
        }  

    }
}
