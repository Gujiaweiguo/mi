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
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptInv_RptDeductMoney : BasePage
{
    public string baseInfo;
    public string message;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Message");
            btnOK.Attributes.Add("onclick", "return InputValidator(form1)");
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[17];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[17];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopCode";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXInvThisCost";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvThisCost");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXOpeningBalance";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_OpeningBalance");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXInvInterest";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvInterest");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXThisProductInv";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ThisProductInv");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXLastChouChen";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_LastChouChen");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXLastSlotCard";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_LastSlotCard");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXSlotCardMoney";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SlotCardMoney");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXSdate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopName";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXAmount";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXHandlingCharge";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_HandlingCharge");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXTitle";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TitleDeductMoney");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXShopPayInAmt";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptShopPayInAmt");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXMallTitle";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = Session["MallTitle"].ToString();
        paraField[16].CurrentValues.Add(discreteValue[16]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string whereStr1 = "";

        if (txtContractID.Text != "")
        {
            whereStr1 = " AND Contract.ContractCode = '" + txtContractID.Text + "'"; 
        }

        string str_sql = "SELECT Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                          " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan,ConShop.ShopCode,ConShop.ShopName," +
                          " CurrencyType.CurTypeName," +
                          " ISNULL(StartInv.StartActpayAmt,0) as StartActpayAmt," +
                          " ISNULL(pay.InvActPayAmt,0) as amt," +
                          " ISNULL(sPay.InvActPayAmt,0) as sPayAmt," +
                          " ISNULL(ipay.iamt,0) AS iActAmt," +
                          " ISNULL(CardAmt.BankAmt,0) as BankAmt," +
                          " ISNULL(CardAmt.Rate,0) as Rate,"+
                          " ISNULL(PayIn.PaidAmt,0) as PaidAmt" +
                          " FROM invoiceHeader " +
                          " INNER JOIN" +
                           " Contract ON (invoiceHeader.contractID = Contract.ContractID)" +
                           " full JOIN" +
                           " (" +
                           " SELECT SUM(invD.invActPayAmt)- invpay.InvPaidAmt AS StartActpayAmt," +
                           " ih.ContractID" +
	                            " FROM InvoiceHeader AS ih INNER JOIN" +
	                            " InvoiceDetail AS invD ON (ih.invID = invD.invID)" +
	                            " INNER JOIN " +
				                            " (SELECT MAX('" + txtPeriod.Text + "') as InvPeriod,InvoiceHeader.contractid"+ 
				                                " FROM InvoiceHeader,Contract"+
				                                " WHERE InvoiceHeader.ContractID = Contract.ContractID"+
                                                " group by InvoiceHeader.contractid" +
				                            " ) as InvPer " +
				                            " ON (InvPer.contractid = ih.contractid)" +
	                            " LEFT JOIN" +
	                            " (" +
		                            " SELECT SUM(InvPaidAmt) as InvPaidAmt,InvoicePay.ContractID " +
		                            " FROM InvoicePay" +
		                            " INNER JOIN " +
                                             " (SELECT MAX('" + txtPeriod.Text + "') as InvPeriod,InvoiceHeader.contractid" +
                                                " FROM InvoiceHeader,Contract" +
                                                " WHERE InvoiceHeader.ContractID = Contract.ContractID" +
                                                " group by InvoiceHeader.contractid" +
				                            " ) as vPer " +
				                            " ON (vPer.contractid = InvoicePay.contractid)" +
		                            " WHERE InvoicePay.CreateTime <= vPer.InvPeriod" +
		                            " group by InvoicePay.contractid" +
	                            " ) AS InvPay ON (InvPay.ContractID = ih.contractid)" +
	                            " WHERE "+
                                         //" invD.ChargeTypeID NOT IN " +
                                         //" (" +
                                         //" SELECT ChargeTypeID " +
                                         //" FROM ChargeType" +
                                         //" WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                         //" ) AND" +
	                            " Convert(varchar(10),ih.CreateTime,120) < InvPer.InvPeriod" +
	                            " group by  ih.ContractID ,invpay.InvPaidAmt" +
                            " ) AS StartInv ON (StartInv.ContractID = Contract.ContractID)" +
                            " FULL JOIN" +
                            " (" +
                            " SELECT SUM(InvoiceDetail.InvActPayAmt) AS InvActPayAmt," +
		                            " SUM(InvoiceDetail.InvPaidAmt) as InvPaidAmt," +
		                            " ih.ContractID" +
	                            " FROM InvoiceDetail" +
	                            " INNER JOIN invoiceHeader as ih ON (InvoiceDetail.InvID = ih.InvID)" +
	                            " INNER JOIN " +
                                             " (SELECT MAX('" + txtPeriod.Text + "') as InvPeriod,InvoiceHeader.contractid" +
                                                " FROM InvoiceHeader,Contract" +
                                                " WHERE InvoiceHeader.ContractID = Contract.ContractID" +
                                                " group by InvoiceHeader.contractid" +
				                            " ) as InvPer " +
				                           "  ON (InvPer.contractid = ih.contractid)" +
	                            " WHERE InvoiceDetail.ChargeTypeID NOT IN " +
		                             " (" +
		                             " SELECT ChargeTypeID " +
		                             " FROM ChargeType" +
		                             " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
		                             " )" +
	                            " AND InvPer.InvPeriod = ih.InvPeriod" +
	                            " group by  ih.ContractID" +
                            " ) as Pay ON (Pay.ContractID = contract.ContractID)" +
                            " FULL JOIN" +
                            " (" +
                            " SELECT SUM(InvoiceDetail.InvActPayAmt) AS InvActPayAmt," +
		                            " ih.ContractID" +
	                            " FROM InvoiceDetail" +
	                            " INNER JOIN invoiceHeader as ih ON (InvoiceDetail.InvID = ih.InvID)" +
	                            " INNER JOIN " +
				                            " (SELECT DateAdd(m,-1,MAX('" + txtPeriod.Text + "')) as InvPeriod,InvoiceHeader.contractid " +
				                            " FROM InvoiceHeader,Contract" +
				                             " WHERE InvoiceHeader.ContractID = Contract.ContractID" +
				                            " group by InvoiceHeader.contractid) as InvPer " +
				                            " ON (InvPer.contractid = ih.contractid)" +
	                            " WHERE InvoiceDetail.ChargeTypeID NOT IN " +
		                             " (" +
		                             " SELECT ChargeTypeID " +
		                             " FROM ChargeType" +
		                             " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
		                             " )" +
	                            " AND InvPer.InvPeriod = ih.InvPeriod" +
	                            " AND InvoiceDetail.RentType in (3,4,5)" +
	                            " group by  ih.ContractID" +
                            " ) as sPay ON (sPay.ContractID = contract.ContractID)" +
                            " FULL JOIN" +
                            " (" +
	                            " SELECT SUM(invD.invActPayAmt - invD.invPaidAmt) AS actpayAmt,ih.ContractID" +
	                            " FROM InvoiceHeader AS ih INNER JOIN" +
	                            " InvoiceDetail AS invD ON (ih.invID = invD.invID)" +
	                            " INNER JOIN " +
                                             " (SELECT MAX('" + txtPeriod.Text + "') as InvPeriod,InvoiceHeader.contractid" +
                                                " FROM InvoiceHeader,Contract" +
                                                " WHERE InvoiceHeader.ContractID = Contract.ContractID" +
                                                " group by InvoiceHeader.contractid" +
				                            " ) as InvPer " +
				                            " ON (InvPer.contractid = ih.contractid)" +
	                            " WHERE invD.ChargeTypeID NOT IN " +
			                             " (" +
			                             " SELECT ChargeTypeID " +
			                             " FROM ChargeType" +
			                             " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
			                             " )" +
	                            " AND ih.InvPeriod < InvPer.InvPeriod" +
	                            " group by  ih.ContractID " +
                            " ) AS actPay ON (actPay.ContractID = Contract.ContractID)" +
                            " FULL JOIN" +
                             " (" +
                             " SELECT invH.contractID," +
                             " SUM(invD.invactpayamt - invd.invpaidamt) AS iAmt " +
                             " FROM InvoiceHeader AS invH INNER JOIN" +
                             " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN" +
                             " invoiceInterest AS invI ON (invD.invDetailID = invI.lateinvdetailid)" +
                             " WHERE EXISTS ( " +
                             " SELECT 1 " +
                             " FROM invoiceDetail AS a" +
                             " WHERE a.invID = invI.invCode" +
                            " )" +
                            " AND (invD.invdetstatus = " + Invoice.InvoiceH.InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invdetstatus = " + Invoice.InvoiceH.InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")"+
                             " GROUP BY invH.contractID" +
                             " ) AS ipay ON (invoiceHeader.contractID = ipay.contractID) " +
                            " FULL JOIN" +
                            " (" +
	                            " SELECT TransSkuMedia.ShopID,sum(TransSkuMedia.PaidAmt) as BankAmt," +
			                           "  SUM(TransSkuMedia.CommRate) as Rate," +
			                            " Contract.contractid" +
	                           "  FROM TransSkuMedia" +
	                            " INNER JOIN ConShop ON (ConShop.shopid = TransSkuMedia.ShopID)" +
	                            " INNER JOIN Contract ON (ConShop.ContractID = Contract.ContractID)" +
	                            " INNER JOIN" +
                                     " (SELECT MAX('" + txtPeriod.Text + "') as InvPeriod,InvoiceHeader.contractid" +
                                                " FROM InvoiceHeader,Contract" +
                                                " WHERE InvoiceHeader.ContractID = Contract.ContractID" +
                                                " group by InvoiceHeader.contractid" +
				                            " ) as InvPer " +
				                            " ON (InvPer.contractid = Contract.contractid)" +
		                            " WHERE bizdate >= DateAdd(m,-1,InvPer.InvPeriod)" +
		                             " AND bizdate <= DateAdd(d,-1,InvPer.InvPeriod)" +
		                            " AND TransSkuMedia.MediaMNo in (401,501,601,701,402,502,602,702)" +
		                             " GROUP BY TransSkuMedia.shopid,Contract.contractid" +
	                            " ) AS CardAmt ON (CardAmt.contractid = invoiceHeader.contractID)" +
                            " INNER JOIN" +
                             " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                             " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                             " CustContact ON (CustContact.CustID = Customer.CustID) INNER JOIN" +
                            " ConShop ON (Contract.contractID = ConShop.contractID)" +
                            " LEFT JOIN"+
	                            " (SELECT SUM(PaidAmt) as PaidAmt,payinPeriod,shopid"+
	                              " FROM PayIn"+
		                          " WHERE payinPeriod = '" + Convert.ToDateTime(txtPeriod.Text).AddMonths(-1) +
		                          "' AND payinType = 3"+
		                          " GROUP BY payinPeriod,shopid)"+
                            " AS payin ON ("+
                            " payin.shopid = ConShop.shopID)" +
                            " WHERE Contract.BizMode = " + Lease.Contract.Contract.BIZ_MODE_LEASE +
                            " and(  StartInv.StartActpayAmt!=0" +
                            " OR pay.InvActPayAmt!=0" +
                            " OR sPay.InvActPayAmt!=0" +
                            " OR ipay.iamt!=0" +
                            " OR CardAmt.BankAmt!=0" +
                            " OR CardAmt.Rate!=0" +
                            " OR PayIn.PaidAmt!=0)" +
                            "  GROUP BY Contract.ContractCode,Customer.CustName,CurrencyType.CurTypeName," +
                             " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                             " ConShop.ShopCode,Contract.ContractID," +
                            " StartInv.StartActpayAmt," +
                            " pay.InvActPayAmt,sPay.InvActPayAmt,ipay.iamt,pay.InvPaidAmt," +
                            " CardAmt.BankAmt,CardAmt.Rate,ConShop.ShopName,PayIn.PaidAmt" +
                            " order by ConShop.ShopCode";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\DeductMoney.rpt";
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtContractID.Text = "";
        txtPeriod.Text = "";
    }
}
