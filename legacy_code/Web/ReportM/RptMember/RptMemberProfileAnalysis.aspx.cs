using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
public partial class ReportM_RptMember_RptMemberInformation : BasePage 
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_MemberProfileAnalysis");
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        string sql = "";
        if (rdoNation.Checked = true) sql = SqlResult("Nationality");
        if (rdoRace.Checked = true) sql = SqlResult("Race");
        if (rdoJob.Checked = true) sql = SqlResult("Position");
        if (rdoSex.Checked = true) sql = SqlResult("Sex");
        if (rdoCompany.Checked = true) sql = SqlResult("Distance");
        if (rdoMar.Checked = true) sql = SqlResult("MarStatus");
        if (rdoEdu.Checked = true) sql = SqlResult("EduLevel");
        if (rdoInCome.Checked = true) sql = SqlResult("Salary");

        BaseBO basebo = new BaseBO();
        DataTable Dt = new DataTable();
        Dt = basebo.QueryDataSet(sql).Tables[0];

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[4];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[4];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        for (int i = 1; i <=Dt.Columns.Count; i++)
        {
            paraField[i] = new ParameterField();
            paraField[i].ParameterFieldName = Dt.Columns[i - 1].Caption;
            discreteValue[i] = new ParameterDiscreteValue();
            discreteValue[i].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_" + Dt.Columns[i-1].Caption);
            paraField[i].CurrentValues.Add(discreteValue[i]);
        }

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "f0";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_MemberProfileAnalysis");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[3] = new ParameterField();
        paraField[3].ParameterFieldName = "f20";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberProfileAnalysis.rpt";
    }

    private string SqlResult(string strcolumn)
    {
        string sql = "";
        sql = "Select " + strcolumn + " as Name,Count(" + strcolumn + ") as Count from Member Group by " + strcolumn + " ";
        return sql;
    }
}
