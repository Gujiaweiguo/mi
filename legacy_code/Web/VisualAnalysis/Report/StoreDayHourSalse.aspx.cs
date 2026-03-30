using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseInfo.Dept;
using System.Data;
using Base.XML;
using BaseInfo.User;
using System.IO;
using Base.Biz;
public partial class VisualAnalysis_Report_StoreDayHourSalse : System.Web.UI.Page
{
    protected string deptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region 
    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        PreLoad();
    }
    #endregion
    private void PreLoad()
    {
        deptName = "项目" + DateTime.Now.Date.ToString("yyyy年MM月dd日") +"销售统计图表";
        if (Request.QueryString["MallID"] != null)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "DeptID=" + Request.QueryString["MallID"].ToString();
            DataSet dt = baseBo.QueryDataSet(new Dept());
            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["DeptName"].ToString() + deptName;
            }

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            Column3D line = new Column3D();
            string strSql = @"select RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'时',3) 'Hour',sum(PaidAmt) 'Sales'" +
                            " from TransSku where storeid=" + Request.QueryString["MallID"].ToString() +
                            " and bizdate='" + DateTime.Now.Date.ToString("yyyy-MM-dd") +"'" +
                            " group by RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'时',3)";
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreDayHourSalse1.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);
                line.xAxisName = "";
                line.yAxisName = "";
                line.Caption = "项目时段销售统计";
                line.GetXml(strFile, dt.Tables[0]);
                this.xmlFile.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreDayHourSalse1.xml";
            }

            strSql = "select trade2name,sum(PaidAmt) 'Sales'from transsku where storeid="  + Request.QueryString["MallID"].ToString() +
                    " and bizdate='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "' group by trade2name";
            dt=baseBo.QueryDataSet(strSql);
            if(dt.Tables[0].Rows.Count>0)
            {
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreDayHourSalse2.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);
                Pie2D pie2d = new Pie2D();
                pie2d.Caption = "项目业态销售统计";
                pie2d.ShowNames = true;
                pie2d.GetXml(strFile, dt.Tables[0]);
                this.xmlFile2.InnerText="../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreDayHourSalse2.xml";
            }

            strSql = "select floorname,sum(PaidAmt) 'Sales'from transsku where storeid=" + Request.QueryString["MallID"].ToString() +
                     " and bizdate='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "' group by floorname";
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreDayHourSalse3.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);
                Pie2D pie2d = new Pie2D();
                pie2d.Caption = "项目楼层销售统计";
                pie2d.ShowNames = true;
                pie2d.GetXml(strFile, dt.Tables[0]);
                this.xmlFile3.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreDayHourSalse3.xml";
            }
        }
    }
}
