using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Specialized;

public partial class CrystalReport : System.Web.UI.Page
{
	private ReportDocument reportDoc;
	string sortField  = string.Empty;
    string rptPathName = string.Empty;
    string isSort = string.Empty;
    string title = string.Empty;
    string strStep = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 在此处放置用户代码以初始化页面
        strStep = Request.QueryString["step"];

        if (strStep != null && strStep == "refresh")
        {
            Session.RemoveAll();
            return;
        }
        else
        {
            displayReport();
        }
        
    }

    private void displayReport()
    {
        rptPathName = Request.QueryString["rptPathName"];
        sortField = Request.QueryString["sort"];
        title = Request.QueryString["title"];


        if (sortField == null && ((string)Session["rptPathName"] != rptPathName))
        {
            ParameterFields paramFields = new ParameterFields();
            NameValueCollection coll = Request.QueryString;
            String[] arr1 = coll.AllKeys;
            for (int i = 0; i < arr1.Length; i++)
            {
                string key = arr1[i];
                if (key.IndexOf("sort") > -1)
                {
                    continue;
                }
                else if (key.IndexOf("rptPathName") > -1)
                {
                    continue;
                }
                string theValue = coll.Get(key);
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.ParameterFieldName = key;
                discreteVal.Value = theValue;
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);
            }

            crView.ParameterFieldInfo = paramFields;
            crView.EnableDatabaseLogonPrompt = false;
            string reportPath1 = Server.MapPath("Report");
            string rptName = reportPath1 + rptPathName;
            reportDoc = new ReportDocument();
            Session["reportDOC"] = reportDoc;
            Session["sortField"] = "";
            Session["rptPathName"] = rptPathName;
            Session["title"] = title;
            reportDoc.Load(rptName);
            SetDBLogonForReport(reportDoc);
            crView.DisplayGroupTree = false;
            crView.EnableParameterPrompt = false;
            crView.BestFitPage = true;
            crView.ReportSource = reportDoc;
        }
        else
        {
            if (sortField != (string)Session["sortField"])
            {
                Session["sortField"] = sortField;

                ParameterFields paramFields = new ParameterFields();
                NameValueCollection coll = Request.QueryString;
                String[] arr1 = coll.AllKeys;

                for (int i = 0; i < arr1.Length; i++)
                {
                    string key = arr1[i];
                    if (key.IndexOf("sort") > -1)
                    {
                        continue;
                    }
                    else if (key.IndexOf("rptPathName") > -1)
                    {
                        continue;
                    }
                    string theValue = coll.Get(key);

                    if (key.IndexOf("title") > -1) 
                    {
                        theValue = (String)Session["title"];
                    } 
                    ParameterField paramField = new ParameterField();
                    ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                    paramField.ParameterFieldName = key;
                    discreteVal.Value = theValue;
                    paramField.CurrentValues.Add(discreteVal);
                    paramFields.Add(paramField);
                }

                crView.ParameterFieldInfo = paramFields;
                crView.EnableDatabaseLogonPrompt = false;

                reportDoc = (ReportDocument)Session["reportDOC"];
                crView.DisplayGroupTree = false;
                crView.EnableParameterPrompt = false;
                crView.BestFitPage = true;
                crView.ReportSource = reportDoc;
            }
            else {
                reportDoc = (ReportDocument)Session["reportDOC"];
                setReportSort(reportDoc);
                crView.DisplayGroupTree = false;
                crView.EnableParameterPrompt = false;
                crView.BestFitPage = true;
                crView.ReportSource = reportDoc;
            }
        }
    }

    private void SetDBLogonForReport(ReportDocument reportDocument)
    {
        ConnectionInfo connectionInfo = new ConnectionInfo();
        connectionInfo.ServerName = "JKLEDW";
        connectionInfo.DatabaseName = "jkledw";
        connectionInfo.UserID = "edwuser";
        connectionInfo.Password = "edwuser";

        Tables tables = reportDocument.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
        {
            TableLogOnInfo tableLogonInfo = table.LogOnInfo;
            tableLogonInfo.ConnectionInfo = connectionInfo;

            table.ApplyLogOnInfo(tableLogonInfo);
        }
    }

    private void setReportSort(ReportDocument reportDocument)
    {
        foreach (SortField crSortField in reportDocument.DataDefinition.SortFields)
        {
            if (crSortField.Field.Name == "SortField")
            {
                if (crSortField.SortDirection == CrystalDecisions.Shared.SortDirection.AscendingOrder)
                {
                    crSortField.SortDirection = CrystalDecisions.Shared.SortDirection.DescendingOrder;
                }
                else
                {
                    crSortField.SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder;
                }
            }            
        }

    }

    #region Web 窗体设计器生成的代码
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// 设计器支持所需的方法 - 不要使用代码编辑器修改
    /// 此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion

}
