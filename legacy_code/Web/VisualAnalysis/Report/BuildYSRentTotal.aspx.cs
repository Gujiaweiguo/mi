using System;
using System.Data;
using Base.Biz;
using BaseInfo.Dept;
using RentableArea;
public partial class VisualAnalysis_Report_BuildYSRentTotal : System.Web.UI.Page
{
    public string baseInfo;
    public string strPath;
    public string deptName = "";
    protected string strJs;
    protected void Page_Load(object sender, EventArgs e)
    {
        string locaID = "";
        string buildingID = "";
        deptName = "测试购物中心";
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        //生成界面Grid的JS
        string strsql="select deptid,deptcode,deptname from dept";
        strJs =BaseInfo.SigmaGrid.GetGrid(strsql, "gridbox2");

        if (Request.QueryString["MallID"] != null)
        {
            locaID = Request.QueryString["MallID"].ToString();
            strPath = "../Disktop.aspx?MallID=" + locaID;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "DeptID=" + locaID.ToString();
            DataSet dt = baseBo.QueryDataSet(new Dept());

            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["DeptName"].ToString().Trim();
            }
            
        }
        else if (Request.QueryString["BuildingID"] != null)
        {
            buildingID = Request.QueryString["BuildingID"].ToString();
            strPath = "../Disktop.aspx?BuildingID=" + buildingID;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "BuildingID=" + buildingID.ToString();
            DataSet dt = baseBo.QueryDataSet(new Building());
            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["BuildingName"].ToString().Trim();
            }
        }
        else
        {
            strPath = "../Disktop.aspx";
        }
    }
}
