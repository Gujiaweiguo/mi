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

public partial class ReportM_RptMember_RptBookGift : BasePage 
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BookGift");
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void InitDDL()
    {
        string sql = "Select Distinct GiftId,GiftDesc from Gift Order by GiftId";
        BaseBO basebo = new BaseBO();
        DataTable Dt = new DataTable();
        Dt = basebo.QueryDataSet(sql).Tables[0];
        ddlGiftId.Items.Add("");
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            ddlGiftId.Items.Add(new ListItem(Dt.Rows[i]["GiftId"].ToString() + " " + Dt.Rows[i]["GiftDesc"].ToString(), Dt.Rows[i]["GiftId"].ToString()));
        }
    }

    private void BindData()
    {
        string sql = "SELECT BookH.BookId,Gift.GiftID,Gift.GiftDesc ,SUM(BookD.GiftQty) AS Count" +
                        " FROM BookH,BookD,Gift" +
                        " WHERE BookH.BookId = BookD.BookId" +
                        " AND BookD.GiftID = Gift.GiftID" ;
        if (txtStartDate.Text!="")
        {
            sql = sql + " AND IssueDate>='" + txtStartDate.Text + "'";
        }

        if (txtEndDate.Text != "")
        {
            sql = sql + " AND IssueDate<='" + txtEndDate.Text + "'";
        }

        if (ddlGiftId.Text != "")
        {
            sql = sql + " AND Gift.GiftID='" + ddlGiftId.SelectedItem.Text.Substring(0,ddlGiftId.SelectedItem.Text.IndexOf(" ")) + "'";
        }
        sql = sql + " Group By BookH.BookId,Gift.GiftID,Gift.GiftDesc";
        BaseBO basebo = new BaseBO();
        DataTable Dt = new DataTable();
        Dt = basebo.QueryDataSet(sql).Tables[0];

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[6];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[6];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        for (int i = 1; i <= Dt.Columns.Count; i++)
        {
            paraField[i] = new ParameterField();
            paraField[i].ParameterFieldName = Dt.Columns[i - 1].Caption;
            discreteValue[i] = new ParameterDiscreteValue();
            discreteValue[i].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_" + Dt.Columns[i - 1].Caption);
            paraField[i].CurrentValues.Add(discreteValue[i]);
        }

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "f0";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_BookGift");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[5] = new ParameterField();
        paraField[5].ParameterFieldName = "f20";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptBookGift.rpt";
    }
}
