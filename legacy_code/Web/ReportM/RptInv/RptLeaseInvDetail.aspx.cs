using System;
using System.Web.UI;
using CrystalDecisions.Shared;

using Base.Page;
using Lease.Contract;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class RptBaseMenu_RptLeaseInvDetail : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "RptLeaseInvDetail_Title");
        }

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        if (txtPeriod.Text != "")
        {
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '记账月不能为空。'", true);
            return;
        }
        
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
     

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitleDetail";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptLeaseInvDetail_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXSdate";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMallTitle";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = Session["MallTitle"].ToString();
        paraField[2].CurrentValues.Add(discreteValue[2]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string whereStr1 = "";
        string whereStr2 = "";
        string whereStr3 = "";
        string whereStr4 = "";
        string whereStr5 = "";
        string whereStr6 = "";

        string sql = "(SELECT MAX(InvPeriod) FROM InvoiceHeader,Contract WHERE InvoiceHeader.ContractID = Contract.ContractID AND Contract.ContractCode = '" + txtContractID.Text + "')";

        if (txtContractID.Text != "" && txtPeriod.Text == "")
        {
            whereStr1 = " AND invoiceHeader.InvPeriod = " + sql +
                                " AND Contract.ContractCode = '" + txtContractID.Text + "'";
            whereStr2 = " AND InvoiceDetail.Period = " + sql;
            whereStr3 = " AND invD.Period < " + sql;
            whereStr4 = " AND ( InvoiceDetail.Period = " + sql;
            whereStr5 = " AND Contract.ContractCode = '" + txtContractID.Text + "'";
            whereStr6 = sql;
        }

        if (txtPeriod.Text != "" && txtContractID.Text == "")
        {
            whereStr1 = " AND invoiceHeader.InvPeriod = '" + txtPeriod.Text + "'";
            whereStr2 = " AND InvoiceDetail.Period = '" + txtPeriod.Text + "'";
            whereStr3 = " AND invD.Period < '" + txtPeriod.Text + "'";
            whereStr4 = " AND ( InvoiceDetail.Period = '" + txtPeriod.Text + "'";
            whereStr6 = txtPeriod.Text.Trim();
        }

        if(txtContractID.Text != "" && txtPeriod.Text != "")
        {
            whereStr1 = " AND Contract.ContractCode = '" + txtContractID.Text +
                       "' AND invoiceHeader.InvPeriod = '" + txtPeriod.Text + "'";
            whereStr2 = " AND InvoiceDetail.Period = '" + txtPeriod.Text + "'";
            whereStr3 = " AND invD.Period < '" + txtPeriod.Text + "'";
            whereStr4 = " AND ( InvoiceDetail.Period = '" + txtPeriod.Text + "'";
            whereStr5 = " AND Contract.ContractCode = '" + txtContractID.Text + "'";
            whereStr6 = txtPeriod.Text.Trim();
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string authWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        string str_sql = " SELECT " +
                           " Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                           "  Customer.PostCode,Customer.PostAddr,CustContact.ContactMan,ConShop.ShopCode,ConShop.ShopName," +
                           "  CurrencyType.CurTypeName,invoiceHeader.InvID,sPay.InvPeriod as InvPeriod,sPay.ChargeTypeName," +
                           "  ISNULL(sPay.InvActPayAmt,0) as sAmt," +
                           "  ISNULL(sActPay.actpayAmt,0) as sactpayAmt," +
                           "  ISNULL(sIpay.iAmt,0) as siAmt" +
                           "  FROM invoiceHeader " +
                           "  INNER JOIN" +
                           "  (" +
                                " SELECT SUM(InvoiceDetail.InvActPayAmt) AS InvActPayAmt," +
                                   "  SUM(InvoiceDetail.InvPaidAmt) as InvPaidAmt,invhe.InvPeriod," +
                                   "  ContractID,InvoiceDetail.ChargeTypeID,ct.ChargeTypeName,invhe.InvID" +
                                " FROM invoiceHeader as invhe " +
                                    "  INNER JOIN InvoiceDetail ON (InvoiceDetail.InvID = invhe.InvID)" +
                                     " INNER JOIN ChargeType as ct  ON (InvoiceDetail.ChargeTypeID = ct.ChargeTypeID)" +
                                " WHERE " +
                                     " InvoiceDetail.ChargeTypeID NOT IN " +
                                     " (" +
                                     " SELECT ChargeTypeID " +
                                     " FROM ChargeType" +
                                     " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                     " ) AND invhe.InvPeriod = '" + txtPeriod.Text +
                                     "' AND InvoiceDetail.RentType not in (3,4,5)" +
                                " group by  ContractID,InvoiceDetail.ChargeTypeID,ct.ChargeTypeName,invhe.InvID,invhe.InvPeriod" +
                            " ) as sPay ON (invoiceHeader.InvID = sPay.InvID)" +
                            " LEFT JOIN" +
                            " (" +
                                " SELECT SUM(invD.invActPayAmt) AS actpayAmt,ContractID," +
                                        " invD.ChargeTypeID,ChargeType.ChargeTypeName" +
                                " FROM InvoiceHeader AS invH INNER JOIN" +
                                " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN" +
                                " ChargeType ON (ChargeType.ChargeTypeID = invD.ChargeTypeID) " +
                                " WHERE invD.ChargeTypeID NOT IN " +
                                         " (" +
                                         " SELECT ChargeTypeID " +
                                         " FROM ChargeType" +
                                          " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                        " ) AND invD.Period = '" + Convert.ToDateTime(txtPeriod.Text).AddMonths(-1) +
                                         "' AND invD.RentType in (3,4,5)" +
                                 " group by  ContractID ,invD.ChargeTypeID,ChargeType.ChargeTypeName" +
                             " ) as sActPay ON (sActPay.ContractID = sPay.ContractID " +
                                             " AND sActPay.ChargeTypeID = sPay.ChargeTypeID)" +
                              " LEFT JOIN" +
                             " (" +
                                 " SELECT invH.contractID,invD.ChargeTypeID," +
                                 " SUM(invI.interestamt) AS iAmt " +
                                 " FROM InvoiceHeader AS invH INNER JOIN" +
                                 " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN"+
                                 " invoiceInterest AS invI ON (invD.invDetailID = invI.lateinvdetailid)"+
                             " GROUP BY invH.contractID,invD.ChargeTypeID" +
                             " ) AS sIpay ON (invoiceHeader.contractID = sIpay.contractID and" +
                                            "  sIpay.ChargeTypeID = spay.ChargeTypeID" +
                            " )" +
                             " INNER JOIN" +
                              " Contract ON (invoiceHeader.contractID = Contract.ContractID ) INNER JOIN" +
                              " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                              " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                              " CustContact ON (CustContact.CustID = Customer.CustID) INNER JOIN" +
                            "  ConShop ON (Contract.contractID = ConShop.contractID)" +
                            " WHERE Contract.BizMode = " + Contract.BIZ_MODE_LEASE + whereStr5 + authWhere;

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\InvoiceDetail.rpt";
        //Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\LeaseInvDetail.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtContractID.Text = "";
        txtPeriod.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
}
