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

using BaseInfo.User;
using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease.PotBargain;
using Invoice.InvoiceH;
using Lease.InvoicePara;
using Lease.ConShop;

public partial class ReportM_RptLeaseInvJV : System.Web.UI.Page
{
    private String InvCode = "";
    private String PrtName = "";
    private int flag;  //是否重打标志（0：非重打；1：重打）
    private int invoiceParaID;
    private string bacthID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            flag = Convert.ToInt32(Request["flag"]);
            if (Request["InvCode"] != null)
            {
                InvCode = Request["InvCode"];
            }
            if (Request["paraID"] != null)
            {
                invoiceParaID = Convert.ToInt32(Request["paraID"]);
            }
            if (Request["bacthID"] != null)
            {
                bacthID = Request["bacthID"];
            }
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            PrtName = Convert.ToString(objSessionUser.UserName);
            BindData();
        }
    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        //报表标题及备注
        DataSet ds = new DataSet();
        if (flag == 0)
        {
            ds = InvoiceJVParaPO.GetInvoiceJVParaDefault();
        }
        else
        {
            ds = InvoiceJVParaPO.GetInvoiceJVParaByID(invoiceParaID);
        }
        int count = ds.Tables[0].Rows.Count;
        if (count == 1)
        {
            int colCount = ds.Tables[0].Columns.Count;

            ParameterFields paraFields = new ParameterFields();
            ParameterField[] paraField = new ParameterField[36 + colCount];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[36 + colCount];
            ParameterRangeValue rangeValue = new ParameterRangeValue();

            for (int i = 0; i < colCount; i++)
            {
                paraField[i] = new ParameterField();
                paraField[i].Name = ds.Tables[0].Columns[i].ColumnName.ToString();
                discreteValue[i] = new ParameterDiscreteValue();
                discreteValue[i].Value = ds.Tables[0].Rows[0][ds.Tables[0].Columns[i].ColumnName.ToString()];
                paraField[i].CurrentValues.Add(discreteValue[i]);
            }

            paraField[colCount] = new ParameterField();
            paraField[colCount].ParameterFieldName = "REXTitle";
            discreteValue[colCount] = new ParameterDiscreteValue();
            discreteValue[colCount].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInvInterest_Title");
            paraField[colCount].CurrentValues.Add(discreteValue[colCount + 0]);

            paraField[colCount + 1] = new ParameterField();
            paraField[colCount + 1].Name = "REXDeptName";
            discreteValue[colCount + 1] = new ParameterDiscreteValue();
            discreteValue[colCount + 1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvComTitile");
            paraField[colCount + 1].CurrentValues.Add(discreteValue[colCount + 1]);

            paraField[colCount + 2] = new ParameterField();
            paraField[colCount + 2].Name = "REXBankName";
            discreteValue[colCount + 2] = new ParameterDiscreteValue();
            discreteValue[colCount + 2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_lblBankName");
            paraField[colCount + 2].CurrentValues.Add(discreteValue[colCount + 2]);

            paraField[colCount + 3] = new ParameterField();
            paraField[colCount + 3].Name = "REXBankAcct";
            discreteValue[colCount + 3] = new ParameterDiscreteValue();
            discreteValue[colCount + 3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_lblBankAcct");
            paraField[colCount + 3].CurrentValues.Add(discreteValue[colCount + 3]);

            paraField[colCount + 4] = new ParameterField();
            paraField[colCount + 4].Name = "REXShopCode";
            discreteValue[colCount + 4] = new ParameterDiscreteValue();
            discreteValue[colCount + 4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
            paraField[colCount + 4].CurrentValues.Add(discreteValue[colCount + 4]);

            paraField[colCount + 5] = new ParameterField();
            paraField[colCount + 5].Name = "REXBrand";
            discreteValue[colCount + 5] = new ParameterDiscreteValue();
            discreteValue[colCount + 5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Brand");
            paraField[colCount + 5].CurrentValues.Add(discreteValue[colCount + 5]);

            paraField[colCount + 6] = new ParameterField();
            paraField[colCount + 6].Name = "REXContractCode";
            discreteValue[colCount + 6] = new ParameterDiscreteValue();
            discreteValue[colCount + 6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
            paraField[colCount + 6].CurrentValues.Add(discreteValue[colCount + 6]);

            paraField[colCount + 7] = new ParameterField();
            paraField[colCount + 7].Name = "REXInvCode";
            discreteValue[colCount + 7] = new ParameterDiscreteValue();
            discreteValue[colCount + 7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvCode");
            paraField[colCount + 7].CurrentValues.Add(discreteValue[colCount + 7]);

            paraField[colCount + 8] = new ParameterField();
            paraField[colCount + 8].Name = "REXInvThisCost";
            discreteValue[colCount + 8] = new ParameterDiscreteValue();
            discreteValue[colCount + 8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvThisCost");
            paraField[colCount + 8].CurrentValues.Add(discreteValue[colCount + 8]);

            paraField[colCount + 9] = new ParameterField();
            paraField[colCount + 9].Name = "REXAgoArrear";
            discreteValue[colCount + 9] = new ParameterDiscreteValue();
            discreteValue[colCount + 9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AgoArrear");
            paraField[colCount + 9].CurrentValues.Add(discreteValue[colCount + 9]);

            paraField[colCount + 10] = new ParameterField();
            paraField[colCount + 10].Name = "REXInterestAmt";
            discreteValue[colCount + 10] = new ParameterDiscreteValue();
            discreteValue[colCount + 10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvInterest");
            paraField[colCount + 10].CurrentValues.Add(discreteValue[colCount + 10]);

            paraField[colCount + 11] = new ParameterField();
            paraField[colCount + 11].Name = "REXPayAmt";
            discreteValue[colCount + 11] = new ParameterDiscreteValue();
            discreteValue[colCount + 11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PayAmt");
            paraField[colCount + 11].CurrentValues.Add(discreteValue[colCount + 11]);

            paraField[colCount + 12] = new ParameterField();
            paraField[colCount + 12].Name = "REXInvDetail";
            discreteValue[colCount + 12] = new ParameterDiscreteValue();
            discreteValue[colCount + 12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvDetail");
            paraField[colCount + 12].CurrentValues.Add(discreteValue[colCount + 12]);

            paraField[colCount + 13] = new ParameterField();
            paraField[colCount + 13].Name = "REXChargeTypeName";
            discreteValue[colCount + 13] = new ParameterDiscreteValue();
            discreteValue[colCount + 13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ItemType");
            paraField[colCount + 13].CurrentValues.Add(discreteValue[colCount + 13]);

            paraField[colCount + 14] = new ParameterField();
            paraField[colCount + 14].Name = "REXAmount";
            discreteValue[colCount + 14] = new ParameterDiscreteValue();
            discreteValue[colCount + 14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_RMBTotalMoney");
            paraField[colCount + 14].CurrentValues.Add(discreteValue[colCount + 14]);

            paraField[colCount + 15] = new ParameterField();
            paraField[colCount + 15].Name = "REXWaterAndElectro";
            discreteValue[colCount + 15] = new ParameterDiscreteValue();
            discreteValue[colCount + 15].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_WaterAndElectro");
            paraField[colCount + 15].CurrentValues.Add(discreteValue[colCount + 15]);

            paraField[colCount + 16] = new ParameterField();
            paraField[colCount + 16].Name = "REXMachineName";
            discreteValue[colCount + 16] = new ParameterDiscreteValue();
            discreteValue[colCount + 16].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_MachineName");
            paraField[colCount + 16].CurrentValues.Add(discreteValue[colCount + 16]);

            paraField[colCount + 17] = new ParameterField();
            paraField[colCount + 17].Name = "REXDate";
            discreteValue[colCount + 17] = new ParameterDiscreteValue();
            discreteValue[colCount + 17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Date");
            paraField[colCount + 17].CurrentValues.Add(discreteValue[colCount + 17]);

            paraField[colCount + 18] = new ParameterField();
            paraField[colCount + 18].Name = "REXReadData";
            discreteValue[colCount + 18] = new ParameterDiscreteValue();
            discreteValue[colCount + 18].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ReadLastData");
            paraField[colCount + 18].CurrentValues.Add(discreteValue[colCount + 18]);

            paraField[colCount + 19] = new ParameterField();
            paraField[colCount + 19].Name = "REXCurrentData";
            discreteValue[colCount + 19] = new ParameterDiscreteValue();
            discreteValue[colCount + 19].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ReadCurrentData");
            paraField[colCount + 19].CurrentValues.Add(discreteValue[colCount + 19]);

            paraField[colCount + 20] = new ParameterField();
            paraField[colCount + 20].Name = "REXUsage";
            discreteValue[colCount + 20] = new ParameterDiscreteValue();
            discreteValue[colCount + 20].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Usage");
            paraField[colCount + 20].CurrentValues.Add(discreteValue[colCount + 20]);

            paraField[colCount + 21] = new ParameterField();
            paraField[colCount + 21].Name = "REXPrice";
            discreteValue[colCount + 21] = new ParameterDiscreteValue();
            discreteValue[colCount + 21].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Price");
            paraField[colCount + 21].CurrentValues.Add(discreteValue[colCount + 21]);

            paraField[colCount + 22] = new ParameterField();
            paraField[colCount + 22].Name = "REXOfficeAddr";
            discreteValue[colCount + 22] = new ParameterDiscreteValue();
            discreteValue[colCount + 22].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_OfficeAddr");
            paraField[colCount + 22].CurrentValues.Add(discreteValue[colCount + 22]);

            paraField[colCount + 23] = new ParameterField();
            paraField[colCount + 23].Name = "REXTel";
            discreteValue[colCount + 23] = new ParameterDiscreteValue();
            discreteValue[colCount + 23].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_Tel");
            paraField[colCount + 23].CurrentValues.Add(discreteValue[colCount + 23]);

            paraField[colCount + 24] = new ParameterField();
            paraField[colCount + 24].Name = "REXCurreyName";
            discreteValue[colCount + 24] = new ParameterDiscreteValue();
            discreteValue[colCount + 24].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CurTypeName");
            paraField[colCount + 24].CurrentValues.Add(discreteValue[colCount + 24]);

            paraField[colCount + 25] = new ParameterField();
            paraField[colCount + 25].Name = "REXInvPayAmt";
            discreteValue[colCount + 25] = new ParameterDiscreteValue();
            discreteValue[colCount + 25].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAmt");
            paraField[colCount + 25].CurrentValues.Add(discreteValue[colCount + 25]);

            paraField[colCount + 26] = new ParameterField();
            paraField[colCount + 26].Name = "REXInvoiceDetail";
            discreteValue[colCount + 26] = new ParameterDiscreteValue();
            discreteValue[colCount + 26].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvoiceDetail");
            paraField[colCount + 26].CurrentValues.Add(discreteValue[colCount + 26]);

            paraField[colCount + 27] = new ParameterField();
            paraField[colCount + 27].Name = "REXWaterAndElecDetail";
            discreteValue[colCount + 27] = new ParameterDiscreteValue();
            discreteValue[colCount + 27].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_WaterElecDetail");
            paraField[colCount + 27].CurrentValues.Add(discreteValue[colCount + 27]);

            paraField[colCount + 28] = new ParameterField();
            paraField[colCount + 28].Name = "REXMustTotal";
            discreteValue[colCount + 28] = new ParameterDiscreteValue();
            discreteValue[colCount + 28].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_JVMustTotal");
            paraField[colCount + 28].CurrentValues.Add(discreteValue[colCount + 28]);

            paraField[colCount + 29] = new ParameterField();
            paraField[colCount + 29].Name = "REXChargeItem";
            discreteValue[colCount + 29] = new ParameterDiscreteValue();
            discreteValue[colCount + 29].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeItem");
            paraField[colCount + 29].CurrentValues.Add(discreteValue[colCount + 29]);

            paraField[colCount + 30] = new ParameterField();
            paraField[colCount + 30].Name = "REXStandardPrice";
            discreteValue[colCount + 30] = new ParameterDiscreteValue();
            discreteValue[colCount + 30].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_StandardPrice");
            paraField[colCount + 30].CurrentValues.Add(discreteValue[colCount + 30]);

            paraField[colCount + 31] = new ParameterField();
            paraField[colCount + 31].Name = "REXJVKeapTotal";
            discreteValue[colCount + 31] = new ParameterDiscreteValue();
            discreteValue[colCount + 31].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_JVKeapTotal");
            paraField[colCount + 31].CurrentValues.Add(discreteValue[colCount + 31]);

            paraField[colCount + 32] = new ParameterField();
            paraField[colCount + 32].Name = "REXJVSalesAmt";
            discreteValue[colCount + 32] = new ParameterDiscreteValue();
            discreteValue[colCount + 32].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_JVSalesAmt");
            paraField[colCount + 32].CurrentValues.Add(discreteValue[colCount + 32]);

            paraField[colCount + 33] = new ParameterField();
            paraField[colCount + 33].Name = "REXInvTaxRate";
            discreteValue[colCount + 33] = new ParameterDiscreteValue();
            discreteValue[colCount + 33].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvTaxRate");
            paraField[colCount + 33].CurrentValues.Add(discreteValue[colCount + 33]);

            paraField[colCount + 34] = new ParameterField();
            paraField[colCount + 34].Name = "REXRateTicketAmt";
            discreteValue[colCount + 34] = new ParameterDiscreteValue();
            discreteValue[colCount + 34].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_RateTicketAmt");
            paraField[colCount + 34].CurrentValues.Add(discreteValue[colCount + 34]);

            paraField[colCount + 35] = new ParameterField();
            paraField[colCount + 35].Name = "REXTime";
            discreteValue[colCount + 35] = new ParameterDiscreteValue();
            discreteValue[colCount + 35].Value = (String)GetGlobalResourceObject("ReportInfo", "RptLease_Times");
            paraField[colCount + 35].CurrentValues.Add(discreteValue[colCount + 35]);


            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }

            if (bacthID == "")
            {
                QuerySQLByInvCode(paraFields);
            }
            else
            {
                QuerySQLByBacthID(paraFields);
            }

            string str_Subsql1 = " SELECT ContractId,ShopCode,ShopName FROM Conshop WHERE ConShop.ShopStatus= " + ConShop.CONSHOP_TYPE_INGEAR;
            Session["subReportSql1"] = str_Subsql1;
            Session["subRpt1"] = "ShopInfo";

            this.Response.Redirect("ReportShow.aspx");
        }

    }


    private void QuerySQLByInvCode(ParameterFields paraFields)
    {
        string str1 = "invJVD.invActPayAmt";
        string str2 = "";
        string str3 = " 1=1";
        string str4 = "invD.InvActPayAmt";
        string str5 = "";
        if (Convert.ToInt32(Request["billFlag"]) != 1)
        {
            str1 = "invJVD.invActPayAmt - invJVD.invPaidAmt";
            str2 = " AND (invJVD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invJVD.invDetStatus =" + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")";
            str3 = " invD.InvActPayAmt <> invD.InvPaidAmt";
            str4 = "invD.InvActPayAmt - invD.InvPaidAmt";
            str5 = "  AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")";
        }

        string str_sql = "SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                        " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                        " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                        " CurrencyType.CurTypeName," +
                        " invoiceJVDetail.InTaxRate AS InvTaxRate, " +
                        " SUM(InvoiceJVDetail.invActPayAmt) amt," +
                        " SUM(InvoiceJVDetail.InvSalesAmt) as salesAmt," +
                        " SUM(InvoiceJVDetail.InvSalesAmt)-SUM(InvoiceJVDetail.invActPayAmt) as ticketAmt," +
                        " ISNULL(Pay.actPayAmt,0) AS actPayAmt," +
                        " 0 AS iActAmt" +
                        " FROM invoiceHeader LEFT JOIN" +
                        " ( " +
			                   " SELECT invH.contractID,invJVD.chargeTypeID AS ctype," +
                                   " SUM(" + str1 + ") AS actpayAmt" +
                                " FROM InvoiceHeader AS invH INNER JOIN" +
                                   " InvoiceJVDetail AS invJVD ON (invH.invID = invJVD.invID)" +
                                " WHERE " + //invJVD.invActPayAmt <> invJVD.invPaidAmt AND " +
                               " invH.invCode < '" + InvCode + "'" + str2 +
                               //" AND (invJVD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invJVD.invDetStatus =" + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                            " GROUP BY invH.contractID,invJVD.chargeTypeID" +
                           " ) AS pay ON (invoiceHeader.contractID = pay.contractID)" +
                        " INNER JOIN " +
                        " InvoiceJVDetail ON (InvoiceHeader.InvID = InvoiceJVDetail.InvID)" +
                        " INNER JOIN" +
                        " ChargeType ON (InvoiceJVDetail.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
                        " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
                        " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                        " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                        " CustContact ON (CustContact.CustID = Customer.CustID)" +
                        " WHERE invCode = '" + InvCode + "'" +
                        " AND ( InvoiceJVDetail.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR InvoiceJVDetail.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                        " AND InvoiceJVDetail.ChargeTypeID <> " +
                                " (" +
                                    " SELECT ChargeTypeID " +
                                    " From ChargeType" +
                                    " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                " )" +
                        " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                            " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                            " InvoiceHeader.invcode,pay.actpayAmt," +
                            " CurrencyType.CurTypeName,ChargeType.chargeTypeName,invoiceJVDetail.InTaxRate";

        str_sql += " UNION ALL ";

        str_sql += " SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                    " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                    " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                    " CurrencyType.CurTypeName," +
                    " 0 as InvTaxRate," +
                   " SUM(invDetail.invActPayAmt) amt," +
                    " 0 as salesAmt," +
                    " 0 as ticketAmt," +
                   " ISNULL(SUM(pay.actpayAmt),0) AS actPayAmt," +
                   " ISNULL(SUM(ipay.iamt),0) AS iActAmt" +
              " FROM invoiceHeader INNER JOIN" +
                    " (SELECT invH.invID,invH.ContractID, invD.ChargeTypeID AS ChargeTypeID, SUM(" + str4 + ") AS InvActPayAmt " +
                        " FROM InvoiceHeader AS invH INNER JOIN " +
                        " InvoiceDetail AS invD " +
                        " ON invH.InvID = invD.InvID " +
                        " WHERE (" + str3 + ") AND (invH.invID = '" + InvCode + "')" + str5 +
                        //" AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                         " GROUP BY invH.invID,invH.ContractID, invD.ChargeTypeID) AS invDetail " +
                    " on (invoiceHeader.invID = invDetail.invID) FULL JOIN" +
                   " (" +
                    " SELECT invH.contractID,invD.chargeTypeID AS ctype," +
                           " SUM(invD.InvActPayAmt - invD.InvPaidAmt) AS actpayAmt" +
                      " FROM InvoiceHeader AS invH INNER JOIN" +
                           " InvoiceDetail AS invD ON (invH.invID = invD.invID)" +
                     " WHERE invD.invActPayAmt <> invD.invPaidAmt " +
                       " AND invH.invCode < '" + InvCode + "'" +
                       " AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                    " GROUP BY invH.contractID,invD.chargeTypeID" +
                   " ) AS pay ON (invoiceHeader.contractID = pay.contractID AND invDetail.chargeTypeID = pay.ctype) FULL JOIN" +
                   " (" +
                    " SELECT invH.contractID,invD.chargeTypeID AS itype," +
                          " SUM(invI.interestamt) AS iAmt" +
                      " FROM InvoiceHeader AS invH INNER JOIN" +
                           " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN" +
                           " invoiceInterest AS invI ON (invD.invDetailID = invI.lateinvdetailid)" +
                     " WHERE EXISTS (" +
                                   " SELECT 1 " +
                                     " FROM invoiceDetail AS a" +
                                    " WHERE a.invID = invI.invCode" +
                                      " AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                                  " )" +
                    " GROUP BY invH.contractID,invD.chargeTypeID" +
                    " ) AS ipay ON (invoiceHeader.contractID = ipay.contractID AND invDetail.chargeTypeID = ipay.itype) INNER JOIN" +
             " ChargeType ON (invDetail.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
            " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
            " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
           "  CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
            " CustContact ON (CustContact.CustID = Customer.CustID)" +
             " WHERE invCode = '" + InvCode + "'" +
               " AND invDetail.ChargeTypeID <> " +
                        " (" +
                            " SELECT ChargeTypeID " +
                            " From ChargeType" +
                            " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                        " )" +
            " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                    " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                    " InvoiceHeader.invcode," +
                    " CurrencyType.CurTypeName,ChargeType.chargeTypeName";

        str_sql += " UNION ALL ";

        str_sql += " SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                            " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                            " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                            " CurrencyType.CurTypeName," +
                            " 0 as InvTaxRate," +
                           " 0 as amt," +
                            " 0 as salesAmt," +
                            " 0 as ticketAmt," +
                           " Pay.actPayAmt AS actPayAmt," +
                           " 0 AS iActAmt" +
                    " FROM invoiceHeader INNER JOIN" +
                        " (" +
                            " SELECT " + InvCode + " as ID,InvD.ChargeTypeID,InvH.ContractID, SUM(InvD.InvActPayAmt-InvD.InvPaidAmt) as actPayAmt" +
                            " FROM InvoiceHeader AS InvH INNER JOIN" +
                                 " InvoiceDetail AS InvD ON (InvH.InvID = InvD.InvID)" +
                            " WHERE InvH.InvCode < '" + InvCode +
		                    "' AND InvD.ChargeTypeID NOT IN" +
		                    " (" +
			                    " SELECT ChargeTypeID FROM InvoiceDetail WHERE InvID = '" + InvCode + 
			                    "' AND InvoiceDetail.InvActPayAmt <> InvoiceDetail.InvPaidAmt" +
		                    " )" +
		                    " AND InvH.ContractID IN" +
		                    " (" +
			                    " SELECT ContractID FROM InvoiceHeader WHERE InvID = '" + InvCode +
		                    "' )" +
		                    " AND InvD.InvActPayAmt != InvD.InvPaidAmt " +
		                    " GROUP BY InvD.ChargeTypeID,InvH.ContractID" +
	                    " ) as Pay ON (InvoiceHeader.InvID = ID)" +
	                    " INNER JOIN" +
                     " ChargeType ON (Pay.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
                    " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
                    " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                    " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                    " CustContact ON (CustContact.CustID = Customer.CustID)" +
                    " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
		                    " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
		                    " InvoiceHeader.invcode,Pay.actPayAmt," +
		                    " CurrencyType.CurTypeName,ChargeType.chargeTypeName";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\YTLeaseInvJVDetail.rpt";


        string str_Subsql = " SELECT ShopHdw.HdwName,ChargeDetail.ChgName,ChargeDetail.StartDate," +
                    " ChargeDetail.EndDate,ChargeDetail.LastQty,ChargeDetail.CurQty," +
                    " ChargeDetail.CostQty,ChargeDetail.Price,ChargeDetail.ChgAmt,ChargeDetail.Times,Charge.InvCode" +
                    " FROM ShopHdw INNER JOIN" +
                    " ChargeDetail ON (ShopHdw.HdwID = ChargeDetail.HdwID) INNER JOIN" +
                    " Charge ON (Charge.ChgID = ChargeDetail.ChgID)" +
                    " WHERE Charge.InvCode = '" + InvCode + "'";
        Session["subParaFil"] = paraFields;
        Session["subReportSql"] = str_Subsql;
        Session["subRpt"] = "WaterAndElec";
    }

    private void QuerySQLByBacthID(ParameterFields paraFields)
    {
        string str_sql = "SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                        " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                        " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                        " CurrencyType.CurTypeName," +
                        " invoiceJVDetail.InTaxRate AS InvTaxRate, " +
                        " SUM(InvoiceJVDetail.invActPayAmt) amt," +
                        " SUM(InvoiceJVDetail.InvSalesAmt) as salesAmt," +
                        " SUM(InvoiceJVDetail.InvSalesAmt)-SUM(InvoiceJVDetail.invActPayAmt) as ticketAmt," +
                        " ISNULL(Pay.actPayAmt,0) AS actPayAmt," +
                        " 0 AS iActAmt" +
                        " FROM invoiceHeader LEFT JOIN" +
                        " ( " +
                               " SELECT invH.contractID,invJVD.chargeTypeID AS ctype," +
                                   " SUM(invJVD.invActPayAmt - invJVD.invPaidAmt) AS actpayAmt" +
                                " FROM InvoiceHeader AS invH INNER JOIN" +
                                   " InvoiceJVDetail AS invJVD ON (invH.invID = invJVD.invID)" +
                                " WHERE invJVD.invActPayAmt <> invJVD.invPaidAmt" +
                               " AND invH.BancthID < '" + bacthID + "'" +
                               " AND (invJVD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invJVD.invDetStatus =" + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                            " GROUP BY invH.contractID,invJVD.chargeTypeID" +
                           " ) AS pay ON (invoiceHeader.contractID = pay.contractID)" +
                        " INNER JOIN " +
                        " InvoiceJVDetail ON (InvoiceHeader.InvID = InvoiceJVDetail.InvID)" +
                        " INNER JOIN" +
                        " ChargeType ON (InvoiceJVDetail.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
                        " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
                        " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                        " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                        " CustContact ON (CustContact.CustID = Customer.CustID)" +
                        " WHERE invoiceHeader.BancthID = '" + bacthID + "'" +
                        " AND ( InvoiceJVDetail.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR InvoiceJVDetail.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                        " AND InvoiceJVDetail.ChargeTypeID <> " +
                                " (" +
                                    " SELECT ChargeTypeID " +
                                    " From ChargeType" +
                                    " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                                " )" +
                        " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                            " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                            " InvoiceHeader.invcode,pay.actpayAmt," +
                            " CurrencyType.CurTypeName,ChargeType.chargeTypeName,invoiceJVDetail.InTaxRate";

        str_sql += " UNION ALL ";

        str_sql += " SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                    " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                    " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                    " CurrencyType.CurTypeName," +
                    " 0 as InvTaxRate," +
                   " SUM(invDetail.invActPayAmt) amt," +
                    " 0 as salesAmt," +
                    " 0 as ticketAmt," +
                   " ISNULL(SUM(pay.actpayAmt),0) AS actPayAmt," +
                   " ISNULL(SUM(ipay.iamt),0) AS iActAmt" +
              " FROM invoiceHeader INNER JOIN" +
                    " (SELECT invH.invID,invH.ContractID, invD.ChargeTypeID AS ChargeTypeID, SUM(invD.InvActPayAmt - invD.InvPaidAmt) AS InvActPayAmt " +
                        " FROM InvoiceHeader AS invH INNER JOIN " +
                        " InvoiceDetail AS invD " +
                        " ON invH.InvID = invD.InvID " +
                        " WHERE (invD.InvActPayAmt <> invD.InvPaidAmt) AND (invH.BancthID = '" + bacthID + "')" +
                        " AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                         " GROUP BY invH.invID,invH.ContractID, invD.ChargeTypeID) AS invDetail " +
                    " on (invoiceHeader.invID = invDetail.invID) FULL JOIN" +
                   " (" +
                    " SELECT invH.contractID,invD.chargeTypeID AS ctype," +
                           " SUM(invD.invActPayAmt - invD.invPaidAmt) AS actpayAmt" +
                      " FROM InvoiceHeader AS invH INNER JOIN" +
                           " InvoiceDetail AS invD ON (invH.invID = invD.invID)" +
                     " WHERE invD.invActPayAmt <> invD.invPaidAmt" +
                       " AND invH.BancthID < '" + bacthID + "'" +
                       " AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                    " GROUP BY invH.contractID,invD.chargeTypeID" +
                   " ) AS pay ON (invoiceHeader.contractID = pay.contractID AND invDetail.chargeTypeID = pay.ctype) FULL JOIN" +
                   " (" +
                    " SELECT invH.contractID,invD.chargeTypeID AS itype," +
                          " SUM(invI.interestamt) AS iAmt" +
                      " FROM InvoiceHeader AS invH INNER JOIN" +
                           " InvoiceDetail AS invD ON (invH.invID = invD.invID) INNER JOIN" +
                           " invoiceInterest AS invI ON (invD.invDetailID = invI.lateinvdetailid)" +
                     " WHERE EXISTS (" +
                                   " SELECT 1 " +
                                     " FROM invoiceDetail AS a" +
                                    " WHERE a.invID = invI.invCode" +
                                      " AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")" +
                                  " )" +
                    " GROUP BY invH.contractID,invD.chargeTypeID" +
                    " ) AS ipay ON (invoiceHeader.contractID = ipay.contractID AND invDetail.chargeTypeID = ipay.itype) INNER JOIN" +
             " ChargeType ON (invDetail.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
            " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
            " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
           "  CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
            " CustContact ON (CustContact.CustID = Customer.CustID)" +
             " WHERE invoiceHeader.BancthID = '" + bacthID + "'" +
               " AND invDetail.ChargeTypeID <> " +
                        " (" +
                            " SELECT ChargeTypeID " +
                            " From ChargeType" +
                            " WHERE ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST +
                        " )" +
            " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                    " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                    " InvoiceHeader.invcode," +
                    " CurrencyType.CurTypeName,ChargeType.chargeTypeName";

        str_sql += " UNION ALL ";

        str_sql += " SELECT ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                            " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                            " InvoiceHeader.invcode,ChargeType.chargeTypeName," +
                            " CurrencyType.CurTypeName," +
                            " 0 as InvTaxRate," +
                           " 0 as amt," +
                            " 0 as salesAmt," +
                            " 0 as ticketAmt," +
                           " Pay.actPayAmt AS actPayAmt," +
                           " 0 AS iActAmt" +
                    " FROM invoiceHeader INNER JOIN" +
                        " (" +
                            " SELECT " + bacthID + " as ID,InvD.ChargeTypeID,InvH.ContractID, SUM(InvD.InvActPayAmt-InvD.InvPaidAmt) as actPayAmt" +
                            " FROM InvoiceHeader AS InvH INNER JOIN" +
                                 " InvoiceDetail AS InvD ON (InvH.InvID = InvD.InvID)" +
                            " WHERE InvH.BancthID < '" + bacthID +
                            "' AND InvD.ChargeTypeID NOT IN" +
                            " (" +
                                " SELECT InvoiceDetail.ChargeTypeID FROM InvoiceDetail,InvoiceHeader WHERE InvoiceHeader.InvID = InvoiceDetail.InvID" +
                                " AND InvoiceHeader.BancthID = '" + bacthID + "'" +
                                " AND InvoiceHeader.ContractID = InvH.ContractID " +
                            " )" +
                            " AND InvH.ContractID IN" +
                            " (" +
                                " SELECT ContractID FROM InvoiceHeader WHERE BancthID = '" + bacthID + 
                            "' )" +
                            " AND InvD.InvActPayAmt != InvD.InvPaidAmt " +
                            " GROUP BY InvD.ChargeTypeID,InvH.ContractID" +
                        " ) as Pay ON (InvoiceHeader.InvID = ID)" +
                        " INNER JOIN" +
                     " ChargeType ON (Pay.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN" +
                    " Contract ON (Contract.ContractID = invoiceHeader.ContractID) INNER JOIN" +
                    " Customer ON (Contract.CustID = Customer.CustID) INNER JOIN" +
                    " CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN" +
                    " CustContact ON (CustContact.CustID = Customer.CustID)" +
                    " GROUP BY ChargeType.chargeTypeID,Contract.ContractCode,Customer.CustName,Contract.ContractID," +
                            " Customer.PostCode,Customer.PostAddr,CustContact.ContactMan," +
                            " InvoiceHeader.invcode,Pay.actPayAmt," +
                            " CurrencyType.CurTypeName,ChargeType.chargeTypeName";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\YTLeaseInvJVDetail.rpt";


        string str_Subsql = " SELECT ShopHdw.HdwName,ChargeDetail.ChgName,ChargeDetail.StartDate," +
                    " ChargeDetail.EndDate,ChargeDetail.LastQty,ChargeDetail.CurQty," +
                    " ChargeDetail.CostQty,ChargeDetail.Price,ChargeDetail.ChgAmt,ChargeDetail.Times,Charge.InvCode" +
                    " FROM ShopHdw INNER JOIN" +
                    " ChargeDetail ON (ShopHdw.HdwID = ChargeDetail.HdwID) INNER JOIN" +
                    " Charge ON (Charge.ChgID = ChargeDetail.ChgID) INNER JOIN" +
                    " InvoiceHeader ON (InvoiceHeader.InvCode = Charge.InvCode)" +
                    " WHERE InvoiceHeader.BancthID = '" + bacthID + "'";
        Session["subParaFil"] = paraFields;
        Session["subReportSql"] = str_Subsql;
        Session["subRpt"] = "WaterAndElec";
    }
}
