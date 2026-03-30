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

public partial class ReportM_RptMember_RptExChangeGift : BasePage 
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ExChangeGift");
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
        string sql = "SELECT ExDate,MembCardId,ReceiptNum,TransAmt,GiftId,GiftQty as Count" +
                        " FROM ExTrans" +
                        " WHERE 1=1" ;
        if (txtStartDate.Text!="")
        {
            sql = sql + " AND ExDate>='" + txtStartDate.Text + "'";
        }

        if (txtEndDate.Text != "")
        {
            sql = sql + " AND ExDate<='" + txtEndDate.Text + "'";
        }

        if (ddlGiftId.Text != "")
        {
            sql = sql + " AND GiftID='" + ddlGiftId.SelectedItem.Text.Substring(0,ddlGiftId.SelectedItem.Text.IndexOf(" ")) + "'";
        }

        BaseBO basebo = new BaseBO();
        DataTable Dt = new DataTable();
        Dt = basebo.QueryDataSet(sql).Tables[0];

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[8];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[8];
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
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ExChangeGift");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[7] = new ParameterField();
        paraField[7].ParameterFieldName = "f20";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptExChangeGift.rpt";
    }
}
