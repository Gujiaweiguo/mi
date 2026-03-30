using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base;
using BaseInfo.Dept;
using System.Data;
using Base.XML;
using BaseInfo.User;
using System.IO;
public partial class VisualAnalysis_Report_StoreSalesTrends : System.Web.UI.Page
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
        deptName = "项目销售与客流趋势分析";
        if (Request.QueryString["MallID"] != null)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "DeptID=" + Request.QueryString["MallID"].ToString();
            DataSet dt = baseBo.QueryDataSet(new Dept());
            if (dt.Tables[0].Rows.Count == 1)
            {
                deptName = dt.Tables[0].Rows[0]["DeptName"].ToString() + deptName;
            }

            
            MSCol3DLineXML msxml = new MSCol3DLineXML();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            ChartLine2D line = new ChartLine2D();
            string strSql = @"select top 12 CONVERT(varchar(7), TransShopMth.Month, 120) '月份',isnull(SUM(TransShopMth.paidamt),0)  '本月销售',isnull(SUM(TransShopMth.lypaidamt),0) '环比销售' " +
                "from TransShopMth where StoreId=" + Request.QueryString["MallID"].ToString() +
                " group by TransShopMth.Month order by TransShopMth.Month" ;
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {                
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesTrends.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);

                msxml.parentYAxis = false;
                msxml.Caption = "项目近12个月销售趋势图(单位:元）";
                msxml.GetXml(strFile, dt.Tables[0]);
                this.xmlFile.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesTrends.xml";
            }

            strSql = "select top 12 cast(month(bizdate) as varchar(2)) + '月' as 'Month',sum(inNum) '当月总人数',CONVERT(varchar(7), trafficdata.bizdate, 120) '月份'" +
                " from trafficdata where storeid=" + Request.QueryString["MallID"].ToString() +
                " group by CONVERT(varchar(7), trafficdata.bizdate, 120),month(bizdate)" +
                " order by CONVERT(varchar(7), trafficdata.bizdate, 120)";
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesTrends2.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);
                line.Caption = "项目近12个月客流趋势图(单位:个)";
                line.ShowNames = true;
                line.ShowValues = false;
                line.xAxisName = "";
                line.GetXml(strFile, dt.Tables[0]);
                this.xmlFile2.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesTrends2.xml";
            }

            strSql = "select top 12 cast(month(month) as varchar(2)) + '月' month,sum(totalreceipt) '交易笔数', CONVERT(varchar(7), month, 120) '月份'" +
                    " from TransShopMth where storeid=" + Request.QueryString["MallID"].ToString() +
                    " group by month order by CONVERT(varchar(7), month, 120)";
            dt = baseBo.QueryDataSet(strSql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//StoreSalesTrends3.xml";
                if (File.Exists(strFile))
                    File.Delete(strFile);

                line.Caption = "项目近12个月交易笔数趋势图(单位:笔)";
                line.ShowNames = true;
                line.ShowValues = false  ;
                line.xAxisName = "";
                line.GetXml(strFile, dt.Tables[0]);
                this.xmlFile3.InnerText = "../../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/StoreSalesTrends3.xml";
            }
        }
    }
}
