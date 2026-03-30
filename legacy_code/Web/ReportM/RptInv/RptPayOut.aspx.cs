using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.Page;
using Lease.ConShop;

public partial class ReportM_RptInv_RptPayOut : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_PayOutList");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            txtShopName.Attributes.Add("onclick", "ShowTree()");
        }
    }

    private void BindData(string whereStr,string whereStr1)
    {
        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[21];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[21];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXDate";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_Date");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXPayInID";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_PayOutID");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXPayInAmt";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_PayInAmt");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXBankChgAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_BankChgAmt");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXAmtSum";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_AmtSum");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptPayOut_Amt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXShopCode";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXPayInDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_PayInDate");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXContractCode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXAgoArrear";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AgoArrear");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXInterestAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvInterest");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXMustTotal";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_RMBTotalMoney");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXAmount";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_JVMustTotal");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXChargeItem";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeItem");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXPayInAmts";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PayInAmt");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXKeepAmt";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_KeepAmt");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXCardAmt";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CardAmt");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXBrand";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Brand");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        paraField[19] = new ParameterField();
        paraField[19].Name = "REXSubTitle";
        discreteValue[19] = new ParameterDiscreteValue();
        discreteValue[19].Value = "(结算月" + txtInvStartDate.Text + "-" + txtInvEndDate.Text +")";
        paraField[19].CurrentValues.Add(discreteValue[19]);

        paraField[20] = new ParameterField();
        paraField[20].Name = "REXShopPayInAmt";
        discreteValue[20] = new ParameterDiscreteValue();
        discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptShopPayInAmt");
        paraField[20].CurrentValues.Add(discreteValue[20]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        whereStr += " 1=1";
        whereStr1 += " 1=1";
        string whereStr2 = "";
        string whereStr4 = " 1=1";

        if (txtShopCode.Text.Trim() != "")
        {
            whereStr2 = "WHERE ConShop.ShopID = '" + txtShopCode.Text.Trim() + "'";
        }

        if (txtInvStartDate.Text.Trim() != "")
        {
            whereStr4 += " AND PayOut.PayOutDate >= '" + Convert.ToDateTime(txtInvStartDate.Text).ToString("yyyy-MM-dd 00:00:00.000") + "'";
        }
        if (txtInvEndDate.Text.Trim() != "")
        {
            whereStr4 += " AND PayOut.PayOutDate <= '" + Convert.ToDateTime(txtInvEndDate.Text).ToString("yyyy-MM-dd 23:59:59.998") + "'";
        }

        string str_sql = "SELECT 1 as ID, '银行内卡' as BankCard," +
                               " Customer.CustName,min(PayIN.PayInID) as PayInID," +
                            " Customer.PostCode,Customer.PostAddr,min(CustContact.ContactMan) as ContactMan,"+
                            " sum(PayIn.PaidAmt) as payinamt,min(PayIn.ShopID) as ShopID," +
                             " min(ConShop.ShopCode) as ShopCode,Contract.ContractID,Contract.ContractCode,"+
                             " sum(ISNULL(PayIn.CommRate,0)) as BankChgAmt," +
                            " 0 as ActPayAmt,0 as iAmt,0 as ShopPayInAmt" +
                             " FROM PayIn"+
                             " INNER JOIN ConShop ON (ConShop.ShopID = PayIn.ShopID)"+
                             " INNER JOIN Contract ON (Contract.contractID = ConShop.contractID)"+
                             " INNER JOIN Customer ON (Contract.CustID = Customer.CustID) "+
                             " INNER JOIN CustContact ON (CustContact.CustID = Customer.CustID)"+
                             " WHERE" + whereStr +
	                            " AND PayInType = 1"+
                             " GROUP BY"+
                            "  PayIn.ShopID,"+
                            " Customer.CustName,"+
                            " Customer.PostCode,Customer.PostAddr,"+
                           "  Contract.ContractID,Contract.ContractCode"+
                            " union all"+
                            " SELECT 2 as ID,'银行外卡' as BankCard,"+
                               "  Customer.CustName,min(PayIN.PayInID) as PayInID," +
                            " Customer.PostCode,Customer.PostAddr,min(CustContact.ContactMan) as ContactMan,"+
                            " sum(PayIn.PaidAmt) as payinamt,min(PayIn.ShopID) as ShopID," +
                            "  min(ConShop.ShopCode) as ShopCode,Contract.ContractID,Contract.ContractCode,"+
                             " sum(ISNULL(PayIn.CommRate,0)) as BankChgAmt," +
                            " 0 as ActPayAmt,0 as iAmt,0 as ShopPayInAmt" +
                             " FROM PayIn"+
                            "  INNER JOIN ConShop ON (ConShop.ShopID = PayIn.ShopID)"+
                             " INNER JOIN Contract ON (Contract.contractID = ConShop.contractID)"+
                             " INNER JOIN Customer ON (Contract.CustID = Customer.CustID) "+
                             " INNER JOIN CustContact ON (CustContact.CustID = Customer.CustID)"+
                             " WHERE " + whereStr +
	                            " AND PayInType = 2"+
                             " GROUP BY"+
                            "  PayIn.ShopID,"+
                            " Customer.CustName,"+
                           "  Customer.PostCode,Customer.PostAddr,"+
                            " Contract.ContractID,Contract.ContractCode"+
                            " union all"+
                            " SELECT 3 as ID, '往期欠款' as BankCard,"+
                                " Customer.CustName,min(PayIN.PayInID) as PayInID," +
                            " Customer.PostCode,Customer.PostAddr,min(CustContact.ContactMan) as ContactMan," +
                            " 0 as  payinamt,min(PayIn.ShopID) as ShopID,"+
                             " min(ConShop.ShopCode) as ShopCode,Contract.ContractID as ContractID,Contract.ContractCode,"+
                             " 0 as BankChgAmt,"+
                            " ISNULL(Inv.ActPayAmt,0) + SUM(ISNULL(POut.PayOutAmt,0)) as ActPayAmt,0 as iAmt,0 as ShopPayInAmt" +
                             " FROM PayIn"+
                            " INNER JOIN ConShop ON (ConShop.ShopID = PayIn.ShopID)"+
                             " INNER JOIN Contract ON (Contract.contractID = ConShop.contractID)"+
                             " INNER JOIN Customer ON (Contract.CustID = Customer.CustID) " +
                             " INNER JOIN CustContact ON (CustContact.CustID = Customer.CustID)" +
                             " LEFT JOIN "+
	                         " ("+
		                            " SELECT SUM(PayOut.PayOutAmt) AS PayOutAmt, PayOut.PayInID"+
		                            " FROM PayOut"+
		                            " WHERE " +// whereStr4 +
                                    //" AND
                                    " PayOutType = 2" +
		                            " GROUP BY PayOut.PayInID"+
	                           " ) AS POut ON (PayIn.PayInID = POut.PayInID)"+
                            " INNER JOIN"+
                            " ("+
	                            " SELECT SUM(InvoiceDetail.invActPayAmt - InvoiceDetail.invPaidAmt) as ActPayAmt,"+
                                        " InvoiceHeader.ContractID,ConShop.ShopID" +
                                " FROM InvoiceDetail,InvoiceHeader,ConShop,Contract" +
	                            " WHERE InvoiceDetail.InvID = InvoiceHeader.InvID"+
                                " AND ConShop.ContractID = Contract.ContractID"+
	                            " AND InvoiceHeader.ContractID = Contract.ContractID"+
                                " AND NOT EXISTS"+
                                " ("+
                                 " SELECT ChargeTypeID "+
                                " From ChargeType"+
                                " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID"+
                                 " ) " +
                                " GROUP BY InvoiceHeader.ContractID,ConShop.ShopID" +
                            " ) AS Inv ON (Inv.ContractID = Contract.ContractID AND Inv.ShopID = PayIn.ShopID)" + whereStr2 +
                            " Group by PayIn.ShopID,Inv.ActPayAmt,Contract.ContractID,Customer.CustName,Customer.PostCode,Customer.PostAddr,Contract.ContractCode " +
                            " union all" +
                            " SELECT 4 as ID ,'滞纳金' as BankCard,"+
                                " Customer.CustName,min(PayIN.PayInID) as PayInID," +
                           "  Customer.PostCode,Customer.PostAddr,min(CustContact.ContactMan) as ContactMan,"+
                            " 0 as  payinamt,min(PayIn.ShopID) as ShopID,"+
                            "  min(ConShop.ShopCode) as ShopCode,min(Contract.ContractID) as ContractID,Contract.ContractCode,"+
                            "  0 as BankChgAmt,0 as ActPayAmt,"+
                            " ISNULL(iactpay.iAmt,0) as iAmt,0 as ShopPayInAmt" +
                            "  FROM PayIn"+
                            " INNER JOIN ConShop ON (ConShop.ShopID = PayIn.ShopID)"+
                             " INNER JOIN Contract ON (Contract.contractID = ConShop.contractID)"+
                            " INNER JOIN Customer ON (Contract.CustID = Customer.CustID) "+
                             " INNER JOIN CustContact ON (CustContact.CustID = Customer.CustID)"+
                            " INNER JOIN"+
                            " ("+
	                            "  SELECT invH.contractID,ConShop.ShopID,"+
	                             " SUM(invI.interestamt) AS iAmt "+
	                             " FROM InvoiceHeader AS invH INNER JOIN"+
	                             " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN"+
	                            " Contract  ON (invH.ContractID = Contract.ContractID) INNER JOIN"+
	                             " ConShop ON ( ConShop.ContractID = Contract.ContractID)INNER JOIN	 "+
	                             " invoiceInterest AS invI ON (invD.invDetailID = invI.lateinvdetailid)"+
	                             " WHERE EXISTS ( "+
	                             " SELECT 1 "+
	                             " FROM invoiceDetail AS a"+
	                            "  WHERE a.invID = invI.invCode"+
	                              " AND (invDetStatus = 1 OR invDetStatus = 2)"+
	                           "  )"+
	                             " GROUP BY invH.contractID,ConShop.ShopID"+
                            " ) AS iactpay ON (iactpay.ContractID = Contract.ContractID AND iactpay.ShopID = PayIn.ShopID)"+ whereStr2 +
                            " Group by PayIn.ShopID,Customer.CustName,iactpay.iAmt,"+
                            " Customer.PostCode,Customer.PostAddr,Contract.ContractCode"+
                            " union all" +
                            " select 5 as ID,'商户缴款' as BankCard," +
                            " Customer.CustName,min(PayIN.PayInID) as PayInID," +
                            "  Customer.PostCode,Customer.PostAddr,min(CustContact.ContactMan) as ContactMan," +
                            " 0 as  payinamt,min(PayIn.ShopID) as ShopID," +
                            "  min(ConShop.ShopCode) as ShopCode,min(Contract.ContractID) as ContractID,Contract.ContractCode," +
                            "  0 BankChgAmt,0 as ActPayAmt," +
                            " 0 as iAmt,ISNULL(SUM(PayIn.PaidAmt),0) as ShopPayInAmt" +
                            " FROM PayIn" +
                            " INNER JOIN ConShop ON (ConShop.ShopID = PayIn.ShopID)" +
                            " INNER JOIN Contract ON (Contract.contractID = ConShop.contractID)" +
                            " INNER JOIN Customer ON (Contract.CustID = Customer.CustID) " +
                            " INNER JOIN CustContact ON (CustContact.CustID = Customer.CustID)" +
                            " WHERE " + whereStr +
                            " AND PayIn.PayInType = 3" +
                            " Group by PayIn.ShopID,Customer.CustName," +
                            " Customer.PostCode,Customer.PostAddr,Contract.ContractCode" +
                            " order by ID";

        

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\PayOut.rpt";

        string str_Subsql = " SELECT ContractId,ShopCode,ShopName FROM Conshop WHERE ConShop.ShopStatus= " + ConShop.CONSHOP_TYPE_INGEAR;
        Session["subReportSql"] = str_Subsql;
        Session["subRpt"] = "ShopInfo";

        this.Response.Redirect("..\\ReportShow.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string str = "";
        string str1 = "";
        if (txtShopCode.Text.Trim() != "")
        {
            str += " ConShop.ShopID = '" + txtShopCode.Text.Trim() + "' AND ";
            str1 += " ConShop.ShopID =  '" + txtShopCode.Text.Trim() + "' AND ";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value =  '商铺名称不能为空。'", true);
        }
        if (txtInvStartDate.Text.Trim() != "")
        {
            str += " PayIn.PayInDate >= '" + Convert.ToDateTime(txtInvStartDate.Text).ToString("yyyy-MM-dd 00:00:00.000") + "' AND ";
            str1 += " TransSkuMedia.BizDate >= '" + Convert.ToDateTime(txtInvStartDate.Text).ToString("yyyy-MM-dd 00:00:00.000") + "' AND ";
        }
        if (txtInvEndDate.Text.Trim() != "")
        {
            str += " PayIn.PayInDate <= '" + Convert.ToDateTime(txtInvEndDate.Text).ToString("yyyy-MM-dd 23:59:59.998") + "' AND ";
            str1 += " TransSkuMedia.BizDate <= '" + Convert.ToDateTime(txtInvEndDate.Text).ToString("yyyy-MM-dd 23:59:59.998") + "' AND ";
        }
        if (str != "")
        {
            BindData(str,str1);
        }
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptPayOut.aspx");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.txtShopCode.Text = ds.Tables[0].Rows[0]["ShopID"].ToString();
            this.txtShopName.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
        }
    }
}
