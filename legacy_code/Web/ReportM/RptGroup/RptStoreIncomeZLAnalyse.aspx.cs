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

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using Lease.Subs;
using Invoice.InvoiceH;

public partial class ReportM_RptGroup_RptStoreIncomeZLAnalyse : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            BindDdl();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreIncomeZLAnalyse");
        }
    }

    private void BindDdl()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        Subs sub = new Subs();
        rs = baseBo.Query(sub);
        ddlSubs.Items.Clear();
        ddlSubs.Items.Add(new ListItem("", ""));
        foreach (Subs subs in rs)
        {
            ddlSubs.Items.Add(new ListItem(subs.SubsShortName, subs.SubsID.ToString()));
        }

        ChargeType chargeType = new ChargeType();
        Resultset rs1 = new Resultset();
        rs1 = baseBo.Query(chargeType);
        ddlChargeType.Items.Clear();
        ddlChargeType.Items.Add(new ListItem("", ""));
        foreach (ChargeType chType in rs1)
        {
            ddlChargeType.Items.Add(new ListItem(chType.ChargeTypeName, chType.ChargeTypeID.ToString()));
        }
    }
    protected void rbtSum_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtDetail.Checked == true)
        {
            Label5.Visible = true;
            ddlChargeType.Visible = true;
        }
        else if (rbtSum.Checked == true)
        {
            Label5.Visible = false;
            ddlChargeType.Visible = false;
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlSubs.SelectedValue = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtContractCode.Text = "";
        ddlChargeType.SelectedValue = "";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreIncomeZLAnalyse");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "RexMonth";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = txtPeriod.Text.Trim();
        paraField[2].CurrentValues.Add(discreteValue[2]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }


        //SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        //string authWhere = "";
        //if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        //{
        //    authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        //}
        string where = "";

        if (this.ddlSubs.Text != "")
        {
            where += " and contract.subsid='" + ddlSubs.SelectedValue.ToString().Trim() + "' ";
        }

        if (this.txtPeriod.Text.Trim() != "")
        {
            where += " and invoiceheader.invperiod <='" + txtPeriod.Text.Trim() + "' ";
        }

        if (this.txtContractCode.Text.Trim() != "")
        {
            where += " and contract.contractcode= '" + this.txtContractCode.Text.Trim() + "' ";
        }
        if (txtCustCode.Text.Trim() != "")
        {
            where += " and customer.custcode='" + txtCustCode.Text.Trim() + "' ";
        }
        if (txtCustName.Text.Trim() != "")
        {
            where += " and customer.custname like '%" + txtCustName.Text.Trim() + "%' ";
        }
        if (rbtDetail.Checked == true && ddlChargeType.Text.Trim() != "")
        {
            where += " and invoicedetail.chargetypeid='" + ddlChargeType.SelectedValue.ToString().Trim() + "' ";
        }
        if (rbtSum.Checked == true)
        {
            string str_sql = "select dbo.getAllShopCode(aa.contractid) shopcode,aa.custid,aa.custname,aa.tradeid,aa.tradename,aa.subsid,aa.subsname,aa.contractid,bb.deposit," +
                            "sum(case when aa.months between 0 and 1 then aa.ActPayAmt else 0 end) onemonth," +
                            "sum(case when aa.months between 1 and 2 then aa.ActPayAmt else 0 end) twomonth," +
                            "sum(case when aa.months between 2 and 3 then aa.ActPayAmt else 0 end) threemonth," +
                            "sum(case when aa.months between 3 and 6 then aa.ActPayAmt else 0 end) sixmonth," +
                            "sum(case when aa.months between 6 and 9 then aa.ActPayAmt else 0 end) ninemonth," +
                            "sum(case when aa.months between 9 and 12 then aa.ActPayAmt else 0 end) oneyear," +
                            "sum(case when aa.months between 12 and 24 then aa.ActPayAmt else 0 end) twoyear," +
                            "sum(case when aa.months between 24 and 36 then aa.ActPayAmt else 0 end) otheryear," +
                            "sum(case when aa.months >=36 then aa.ActPayAmt else 0 end) threeyear,sum(aa.ActPayAmt) allAmt " +
                        "from " +
                            "(select customer.custid,customer.custname,traderelation.tradeid,traderelation.tradename," +
                                "contract.subsid,subsidiary.subsname,invoiceheader.contractid," +
                                "datediff(month,invoiceheader.invperiod,getdate()) months," +
                                "(sum(invoiceheader.InvActPayAmtL) -sum(invoiceheader.InvPaidAmtL)) ActPayAmt " +
                                "from invoiceheader inner join contract on (contract.contractid=invoiceheader.contractid) " +
                                "inner join subsidiary on (contract.subsid=subsidiary.subsid) " +
                                "inner join customer on (contract.custid=customer.custid) " +
                                "inner join traderelation on (contract.tradeid=traderelation.tradeid) " +
                                "where invstatus in (1,2) " + where +// --未结算和部分结算                                
                                "group by customer.custid,customer.custname,traderelation.tradeid,traderelation.tradename," +
                                "contract.subsid,subsidiary.subsname,invoiceheader.contractid,invoiceheader.invperiod) aa " +//  --取应付押金特殊情况下可能要已付押金
                            "inner join (select invoiceheader.contractid,(sum(invoicedetail.InvActPayAmtL) -sum(invoicedetail.InvPaidAmtL)) as deposit " +
                                "from invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid) " +
                                "inner join chargetype on (chargetype.chargetypeid=invoicedetail.chargetypeid) " +
                                "where chargetype.chargeclass =2 " +
                                "group by invoiceheader.contractid " +
                                ") as bb on (bb.contractid=aa.contractid) ";



            str_sql += " group by aa.custid,aa.custname,aa.tradeid,aa.tradename,aa.subsid,aa.subsname,aa.contractid,bb.deposit order by aa.subsid,aa.custid,aa.contractid";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptStoreIncomeZLSum.rpt";
        }
        else if (rbtDetail.Checked == true)
        {
            string str_sql = "select dbo.getAllShopCode(aa.contractid) shopcode,aa.custid,aa.custname,aa.tradeid,aa.tradename," +
                                "aa.subsid,aa.subsname,aa.contractid,bb.deposit,aa.chargetypename," +
                                "sum(case when aa.months between 0 and 1 then aa.ActPayAmt else 0 end) onemonth," +
                                "sum(case when aa.months between 1 and 2 then aa.ActPayAmt else 0 end) twomonth," +
                                "sum(case when aa.months between 2 and 3 then aa.ActPayAmt else 0 end) threemonth," +
                                "sum(case when aa.months between 3 and 6 then aa.ActPayAmt else 0 end) sixmonth," +
                                "sum(case when aa.months between 6 and 9 then aa.ActPayAmt else 0 end) ninemonth," +
                                "sum(case when aa.months between 9 and 12 then aa.ActPayAmt else 0 end) oneyear," +
                                "sum(case when aa.months between 12 and 24 then aa.ActPayAmt else 0 end) twoyear," +
                                "sum(case when aa.months between 24 and 36 then aa.ActPayAmt else 0 end) otheryear," +
                                "sum(case when aa.months >=36 then aa.ActPayAmt else 0 end) threeyear,sum(aa.ActPayAmt) allAmt " +
                           "from (select customer.custid,customer.custname,traderelation.tradeid,traderelation.tradename," +
                                    "contract.subsid,subsidiary.subsname,invoiceheader.contractid,chargetype.chargetypename," +
                                    "datediff(month,invoiceheader.invperiod,getdate()) months," +
                                    "(sum(invoicedetail.InvActPayAmtL) -sum(invoiceheader.InvPaidAmtL)) ActPayAmt " +
                                    "from invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid) " +
                                    "inner join contract on (contract.contractid=invoiceheader.contractid) " +
                                    "inner join subsidiary on (contract.subsid=subsidiary.subsid) " +
                                    "inner join customer on (contract.custid=customer.custid) " +
                                    "inner join traderelation on (contract.tradeid=traderelation.tradeid) " +
                                    "inner join chargetype on (chargetype.chargetypeid=invoicedetail.chargetypeid) " +
                                    "where invstatus in (1,2) " + where + //--未结算和部分结算
                                    "group by customer.custid,customer.custname,traderelation.tradeid,traderelation.tradename," +
                                    "contract.subsid,subsidiary.subsname,invoiceheader.contractid,invoiceheader.invperiod,chargetype.chargetypename) aa " +
                                "inner join (select invoiceheader.contractid,(sum(invoicedetail.InvActPayAmtL) -sum(invoicedetail.InvPaidAmtL)) as deposit " +
                                    "from invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid) " +
                                    "inner join chargetype on (chargetype.chargetypeid=invoicedetail.chargetypeid) " +
                                    "where chargetype.chargeclass =2 " +
                                    "group by invoiceheader.contractid) as bb on (bb.contractid=aa.contractid) ";



            str_sql += " group by aa.custid,aa.custname,aa.tradeid,aa.tradename,aa.subsid,aa.subsname,aa.contractid,bb.deposit,aa.chargetypename order by aa.subsid,aa.custid,aa.contractid,aa.chargetypename";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptStoreIncomeZLDetail.rpt";
        }
    }
    protected void rbtDetail_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtDetail.Checked == true)
        {
            Label5.Visible = true;
            ddlChargeType.Visible = true;
        }
        else if (rbtSum.Checked == true)
        {
            Label5.Visible = false;
            ddlChargeType.Visible = false;
        }
    }
}
