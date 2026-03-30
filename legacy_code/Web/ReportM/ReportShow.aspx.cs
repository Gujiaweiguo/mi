using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using Base.Biz;
using Base.DB;
using Base.Util;
using BaseInfo.Role;
using BaseInfo.User;

public partial class ReportM_ReportShow : System.Web.UI.Page
{
    private ReportDocument reportDocument1;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        //if (!IsPostBack)
        //{
            if (Session["paraFil"] != null)
            {
                this.CrystalReportViewer1.ParameterFieldInfo = (ParameterFields)Session["paraFil"];                
            }            

            //以下为cr2008特殊设置
            this.CrystalReportViewer1.DisplayToolbar = true;
            this.CrystalReportViewer1.DisplayPage = true;
            this.CrystalReportViewer1.Enabled = true;
 

            this.CrystalReportViewer1.HasSearchButton = false;
            this.CrystalReportViewer1.SeparatePages = true;  //分页显示


            // this.CrystalReportViewer1.DisplayGroupTree = false;

            this.CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;
            this.CrystalReportViewer1.HasCrystalLogo = false;
            this.CrystalReportViewer1.HasZoomFactorList = false; // 缩放  
            BindData();
        //}
        //if (IsPostBack)
        //{
        //    reportDocument1 = (ReportDocument)(Session["cr"]);
        //}
     
        //SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //BaseBO baseBO = new BaseBO();
        //baseBO.WhereClause = "FuncID = " + Convert.ToInt32(Session["funcID"]) + " AND RoleID = " + objSessionUser.RoleID;
        //Resultset rs = baseBO.Query(new RoleAuth());
        //if (rs.Count == 1)
        //{
        //    RoleAuth roleAuth = rs.Dequeue() as RoleAuth;
        //    if (roleAuth.IsPrint == 1)
        //    {
        //        this.CrystalReportViewer1.HasPrintButton = true;
        //    }
        //    else
        //    {
        //        this.CrystalReportViewer1.HasPrintButton = false;
        //    }
        //    if (roleAuth.IsExport == 1)
        //    {
        //        this.CrystalReportViewer1.HasExportButton = true;
        //    }
        //    else
        //    {
        //        this.CrystalReportViewer1.HasExportButton = false;
        //    }
        //}
        //else
        //{
        //    this.CrystalReportViewer1.HasPrintButton = false;
        //    this.CrystalReportViewer1.HasExportButton = false;
        //}
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (reportDocument1 != null)
        //{
        //    reportDocument1.Dispose();
        //}
    }

    private void BindData()
    {
        string str_sql = Session["sql"].ToString();
        string crUrl = Session["rpUrl"].ToString();
        string son_str_sql = "";
        string sizepar = "";

        DataSet ds = new DataSet();
        BaseBO baseBo = new BaseBO();
        ds = baseBo.QueryDataSet(str_sql);

        reportDocument1 = new ReportDocument();
        string reportFilePath = Server.MapPath(crUrl);

        this.reportDocument1.Load(reportFilePath);
        this.reportDocument1.SetDataSource(ds.Tables[0]);
        this.reportDocument1.Refresh();


        DataSet sonDS = new DataSet();
        if (Session["subReportSql"] != null && !Session["subReportSql"].Equals(""))
        {
            son_str_sql = Session["subReportSql"].ToString();
            sizepar = Session["subRpt"].ToString();
            sonDS = baseBo.QueryDataSet(son_str_sql);
            ReportDocument rdStuff = reportDocument1.Subreports[sizepar];
            rdStuff.SetDataSource(sonDS.Tables[0]);
        }
        if (Session["subReportSql1"] != null && !Session["subReportSql1"].Equals(""))
        {
            son_str_sql = Session["subReportSql1"].ToString();
            sizepar = Session["subRpt1"].ToString();
            sonDS = baseBo.QueryDataSet(son_str_sql);
            ReportDocument rdStuff = reportDocument1.Subreports[sizepar];
            rdStuff.SetDataSource(sonDS.Tables[0]);
        }

        this.CrystalReportViewer1.ReportSource = reportDocument1;
        this.CrystalReportViewer1.DataBind();
        Session.Add("cr", reportDocument1);
        
        int a = reportDocument1.PrintOptions.PageContentHeight;
        int b = reportDocument1.PrintOptions.PageContentWidth;

    }

    private void SetDBLogonForReport(ReportDocument reportDocument)
    {
        ConnectionInfo connectionInfo = new ConnectionInfo();
        DBParam dbp = ParamManager.GetDBParam("mi_net");

        connectionInfo.ServerName = dbp.DataSource;
        connectionInfo.DatabaseName = dbp.ID;
        connectionInfo.UserID = dbp.UserID;
        connectionInfo.Password = dbp.Password;

        //设置数据源




        //connectionInfo.LogonProperties = DataSourceManager 

        Tables tables = reportDocument.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
        {
            TableLogOnInfo tableLogonInfo = table.LogOnInfo;
            tableLogonInfo.ConnectionInfo = connectionInfo;

            table.ApplyLogOnInfo(tableLogonInfo);
        }

    }
}
