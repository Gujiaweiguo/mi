using System;
using System.Data;
using CrystalDecisions.Shared;

using BaseInfo.User;
using Base.Biz;
using Invoice.InvoiceH;
using Lease.InvoicePara;
using Lease.ConShop;
using BaseInfo.authUser;

public partial class ReportM_RptLeaseInv : System.Web.UI.Page
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
        int intSubsID = 0;

        //根据InvCode获得SubsID，根据Subsid配置结算单格式
        if (InvCode != "")
        {
            DataTable dtSubs=new DataTable();
            string strSubsSql = "Select SubsID from Contract inner join invoiceheader on invoiceheader.contractid=contract.contractid" +
                                " where invoiceheader.invcode='" + InvCode + "'";

            dtSubs = baseBO.QueryDataSet(strSubsSql).Tables[0];
            if (dtSubs.Rows.Count == 1)
            {
                intSubsID = Convert.ToInt32(dtSubs.Rows[0]["SubsID"]);
            }            
        }

        DataSet ds = new DataSet();
        if (flag == 0 )
        {
            //获取子公司的结算单格式
            ds = InvoiceParaPO.GetInvoiceParaDefault(intSubsID);
        }
        else
        {
            ds = InvoiceParaPO.GetInvoiceParaByID(invoiceParaID);
        }

        int count = ds.Tables[0].Rows.Count;
        if (count == 1)
        {
            int colCount = ds.Tables[0].Columns.Count;

            ParameterFields paraFields = new ParameterFields();
            ParameterField[] paraField = new ParameterField[35 + colCount];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[35 + colCount];
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

            paraField[colCount + 10] = new ParameterField();    //已交金额
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
            discreteValue[colCount + 28].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_MustTotal");
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
            paraField[colCount + 31].Name = "REXTimes";
            discreteValue[colCount + 31] = new ParameterDiscreteValue();
            discreteValue[colCount + 31].Value = (String)GetGlobalResourceObject("ReportInfo", "RptLease_Times");
            paraField[colCount + 31].CurrentValues.Add(discreteValue[colCount + 31]);

            paraField[colCount + 32] = new ParameterField();
            paraField[colCount + 32].Name = "REXChargeDate";
            discreteValue[colCount + 32] = new ParameterDiscreteValue();
            discreteValue[colCount + 32].Value = (String)GetGlobalResourceObject("ReportInfo", "RptLease_ChargeDate");
            paraField[colCount + 32].CurrentValues.Add(discreteValue[colCount + 32]);

            paraField[colCount + 33] = new ParameterField();
            paraField[colCount + 33].Name = "REXPeriod";
            discreteValue[colCount + 33] = new ParameterDiscreteValue();
            discreteValue[colCount + 33].Value = "费用月";//(String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
            paraField[colCount + 33].CurrentValues.Add(discreteValue[colCount + 33]);

            paraField[colCount + 34] = new ParameterField();
            paraField[colCount + 34].Name = "RExPrintDate";
            discreteValue[colCount + 34] = new ParameterDiscreteValue();
            discreteValue[colCount + 34].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
            paraField[colCount + 34].CurrentValues.Add(discreteValue[colCount + 34]);
            

            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }

            if(bacthID == "")
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
        string str1 = "0";
        string str2 = "";
        string str3 = "";
        string WhereSQL = "";
        if (Convert.ToInt32(Request["billFlag"]) != 1)
        {
            str1 = "invD.InvPaidAmt";
            str2 = "(invD.InvActPayAmt <> invD.InvPaidAmt) AND ";
            str3 = "AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            WhereSQL = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        string str_sql = "SELECT Contract.ContractCode,Customer.CustName,ChargeType.ChargeTypeName,Contract.ContractID," +
                        " customer.postcode,customer.postaddr,invoiceheader.invcode,invoicedetail.ChargeTypeId," +
                        " currencytype.curtypename,invoicedetail.renttype," +
                        " sum(invoicedetail.invActPayAmt )as amt,isnull(SUM(invoicedetail.InvActPayAmt - invoicedetail.InvPaidAmt) ,0) as actPayAmt,0 as iActAmt" +
                        " ,invoiceheader.InvPeriod as period,invoicedetail.InvStartDate,invoicedetail.InvEndDate from invoiceheader" +
                        " inner join customer on (invoiceheader.custid=customer.custid)" +
                        " inner join contract on (invoiceheader.contractid=contract.contractid)" +
                        " inner join invoicedetail on (invoicedetail.invid=invoiceheader.invid)" +
                        " inner join chargetype on (invoicedetail.chargetypeid=chargetype.chargetypeid)" +
                        " INNER JOIN CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId)" +
                        " INNER JOIN ConShop ON (contract.contractid=ConShop.ContractID)"+
                        " where  invoiceheader.invcode ='" + InvCode + "'"+WhereSQL+"" +
                        " GROUP BY Contract.ContractCode,Customer.CustName,InvoiceHeader.invcode,ChargeType.ChargeTypeName, " +
                        " Customer.PostCode,Customer.PostAddr, CurrencyType.CurTypeName,Contract.ContractID, invoicedetail.chargeTypeID,invoicedetail.RentType,invoiceheader.InvPeriod,invoicedetail.InvStartDate,invoicedetail.InvEndDate";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\YTLeaseInvDetail.rpt";


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
        string str1 = "0";
        string str2 = "";
        string str3 = "";
        string WhereSQL = "";
        if (Convert.ToInt32(Request["billFlag"]) != 1)
        {
            str1 = "invD.InvPaidAmt";
            str2 = "(invD.InvActPayAmt <> invD.InvPaidAmt) AND ";
            str3 = "AND (invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + " OR invD.invDetStatus = " + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + ")";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            WhereSQL = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        string str_sql = "SELECT Contract.ContractCode,Customer.CustName,ChargeType.ChargeTypeName,Contract.ContractID," +
                        " customer.postcode,customer.postaddr,invoiceheader.invcode,invoicedetail.ChargeTypeId," +
                        " currencytype.curtypename,invoicedetail.renttype," +
                        " sum(invoicedetail.invActPayAmt )as amt,isnull(SUM(invoicedetail.InvActPayAmt - invoicedetail.InvPaidAmt) ,0) as actPayAmt,0 as iActAmt" +
                        " ,invoiceheader.InvPeriod as period,invoicedetail.InvStartDate,invoicedetail.InvEndDate from invoiceheader" +
                        " inner join customer on (invoiceheader.custid=customer.custid)" +
                        " inner join contract on (invoiceheader.contractid=contract.contractid)" +
                        " inner join invoicedetail on (invoicedetail.invid=invoiceheader.invid)" +
                        " inner join chargetype on (invoicedetail.chargetypeid=chargetype.chargetypeid)" +
                        " INNER JOIN CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId)" +
                        " INNER JOIN ConShop ON (contract.contractid=ConShop.ContractID)"+
                        " where  invoiceheader.BancthID ='" + bacthID + "'"+WhereSQL+"" +
                        " GROUP BY Contract.ContractCode,Customer.CustName,InvoiceHeader.invcode,ChargeType.ChargeTypeName, " +
                        " Customer.PostCode,Customer.PostAddr, CurrencyType.CurTypeName,Contract.ContractID, invoicedetail.chargeTypeID,invoicedetail.RentType,invoiceheader.InvPeriod,invoicedetail.InvStartDate,invoicedetail.InvEndDate";






        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\YTLeaseInvDetail.rpt";


        string str_Subsql = " SELECT ShopHdw.HdwName,ChargeDetail.ChgName,ChargeDetail.StartDate," +
                    " ChargeDetail.EndDate,ChargeDetail.LastQty,ChargeDetail.CurQty," +
                    " ChargeDetail.CostQty - ChargeDetail.FreeQty AS CostQty,ChargeDetail.Price,ChargeDetail.ChgAmt,ChargeDetail.Times,Charge.InvCode" +
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
