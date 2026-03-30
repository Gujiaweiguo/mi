using System;
using System.Data;
using CrystalDecisions.Shared;

using BaseInfo.User;
using Base.Biz;
using Lease.InvoicePara;

public partial class ReportM_RptInv_RptInvInterest : System.Web.UI.Page
{
    private String ContractCode = "";
    private String PrtName = "";
    private int invoiceParaID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request["paraID"] != null)
            {
                invoiceParaID = Convert.ToInt32(Request["paraID"]);
            }
            if (Request["contractCode"] != null)
            {
                ContractCode = Request["contractCode"];
            }
            //PrtName = Request["PrtName"];

            //String sCon = GetRptCond(InvCode, PrtName);
            //String sldl = GetRptResx();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            PrtName = Convert.ToString(objSessionUser.UserName);
            //this.Response.Redirect("ShowReport.aspx?ReportName=/Mi/Base/InvoiceDetail2.rpt" + sCon + sldl);
            BindData();
            
        }
    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
         //报表标题及备注

        DataSet ds = InvoiceParaPO.GetInvoiceParaByID(invoiceParaID);
        int count = ds.Tables[0].Rows.Count;
        if (count == 1)
        {
            int colCount = ds.Tables[0].Columns.Count;

            ParameterFields paraFields = new ParameterFields();
            ParameterField[] paraField = new ParameterField[28 + colCount];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[28 + colCount];
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
            paraField[colCount].CurrentValues.Add(discreteValue[colCount]);

            paraField[colCount + 1] = new ParameterField();
            paraField[colCount + 1].Name = "REXCustName";
            discreteValue[colCount + 1] = new ParameterDiscreteValue();
            discreteValue[colCount + 1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
            paraField[colCount + 1].CurrentValues.Add(discreteValue[colCount + 1]);

            paraField[colCount + 2] = new ParameterField();
            paraField[colCount + 2].Name = "REXCurName";
            discreteValue[colCount + 2] = new ParameterDiscreteValue();
            discreteValue[colCount + 2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CurTypeName");
            paraField[colCount + 2].CurrentValues.Add(discreteValue[colCount + 2]);

            paraField[colCount + 3] = new ParameterField();
            paraField[colCount + 3].Name = "REXContractCode";
            discreteValue[colCount + 3] = new ParameterDiscreteValue();
            discreteValue[colCount + 3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
            paraField[colCount + 3].CurrentValues.Add(discreteValue[colCount + 3]);

            paraField[colCount + 4] = new ParameterField();
            paraField[colCount + 4].Name = "REXInvCode";
            discreteValue[colCount + 4] = new ParameterDiscreteValue();
            discreteValue[colCount + 4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvCode");
            paraField[colCount + 4].CurrentValues.Add(discreteValue[colCount + 4]);

            paraField[colCount + 5] = new ParameterField();
            paraField[colCount + 5].Name = "REXShopName";
            discreteValue[colCount + 5] = new ParameterDiscreteValue();
            discreteValue[colCount + 5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
            paraField[colCount + 5].CurrentValues.Add(discreteValue[colCount + 5]);

            paraField[colCount + 6] = new ParameterField();
            paraField[colCount + 6].Name = "REXChargeTypeName";
            discreteValue[colCount + 6] = new ParameterDiscreteValue();
            discreteValue[colCount + 6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeTypeName");
            paraField[colCount + 6].CurrentValues.Add(discreteValue[colCount + 6]);

            paraField[colCount + 7] = new ParameterField();
            paraField[colCount + 7].Name = "REXInvDate";
            discreteValue[colCount + 7] = new ParameterDiscreteValue();
            discreteValue[colCount + 7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Date");
            paraField[colCount + 7].CurrentValues.Add(discreteValue[colCount + 7]);

            paraField[colCount + 8] = new ParameterField();
            paraField[colCount + 8].Name = "REXInvActPayAmt";
            discreteValue[colCount + 8] = new ParameterDiscreteValue();
            discreteValue[colCount + 8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayAmt");
            paraField[colCount + 8].CurrentValues.Add(discreteValue[colCount + 8]);

            paraField[colCount + 9] = new ParameterField();
            paraField[colCount + 9].Name = "REXIntStartDate";
            discreteValue[colCount + 9] = new ParameterDiscreteValue();
            discreteValue[colCount + 9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FStartDate");
            paraField[colCount + 9].CurrentValues.Add(discreteValue[colCount + 9]);

            paraField[colCount + 10] = new ParameterField();
            paraField[colCount + 10].Name = "REXIntEndDate";
            discreteValue[colCount + 10] = new ParameterDiscreteValue();
            discreteValue[colCount + 10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FEndDate");
            paraField[colCount + 10].CurrentValues.Add(discreteValue[colCount + 10]);

            paraField[colCount + 11] = new ParameterField();
            paraField[colCount + 11].Name = "REXInterestDay";
            discreteValue[colCount + 11] = new ParameterDiscreteValue();
            discreteValue[colCount + 11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInvInterest_InterestDay");
            paraField[colCount + 11].CurrentValues.Add(discreteValue[colCount + 11]);

            paraField[colCount + 12] = new ParameterField();
            paraField[colCount + 12].Name = "REXBankName";
            discreteValue[colCount + 12] = new ParameterDiscreteValue();
            discreteValue[colCount + 12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankName");
            paraField[colCount + 12].CurrentValues.Add(discreteValue[colCount + 12]);

            paraField[colCount + 13] = new ParameterField();
            paraField[colCount + 13].Name = "REXBankAcct";
            discreteValue[colCount + 13] = new ParameterDiscreteValue();
            discreteValue[colCount + 13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankAcct");
            paraField[colCount + 13].CurrentValues.Add(discreteValue[colCount + 13]);

            paraField[colCount + 14] = new ParameterField();
            paraField[colCount + 14].Name = "REXOfficeAddr";
            discreteValue[colCount + 14] = new ParameterDiscreteValue();
            discreteValue[colCount + 14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_OfficeAddr");
            paraField[colCount + 14].CurrentValues.Add(discreteValue[colCount + 14]);

            paraField[colCount + 15] = new ParameterField();
            paraField[colCount + 15].Name = "REXPrtName";
            discreteValue[colCount + 15] = new ParameterDiscreteValue();
            discreteValue[colCount + 15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtName");
            paraField[colCount + 15].CurrentValues.Add(discreteValue[colCount + 15]);

            paraField[colCount + 16] = new ParameterField();
            paraField[colCount + 16].Name = "REXCreateTime";
            discreteValue[colCount + 16] = new ParameterDiscreteValue();
            discreteValue[colCount + 16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvDate");
            paraField[colCount + 16].CurrentValues.Add(discreteValue[colCount + 16]);

            paraField[colCount + 17] = new ParameterField();
            paraField[colCount + 17].Name = "REXPrtDate";
            discreteValue[colCount + 17] = new ParameterDiscreteValue();
            discreteValue[colCount + 17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
            paraField[colCount + 17].CurrentValues.Add(discreteValue[colCount + 17]);

            paraField[colCount + 18] = new ParameterField();
            paraField[colCount + 18].Name = "REXRePrint";
            discreteValue[colCount + 18] = new ParameterDiscreteValue();
            discreteValue[colCount + 18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_RePrint");
            paraField[colCount + 18].CurrentValues.Add(discreteValue[colCount + 18]);

            paraField[colCount + 19] = new ParameterField();
            paraField[colCount + 19].Name = "REXOfficeTel";
            discreteValue[colCount + 19] = new ParameterDiscreteValue();
            discreteValue[colCount + 19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_Tel");
            paraField[colCount + 19].CurrentValues.Add(discreteValue[colCount + 19]);

            paraField[colCount + 20] = new ParameterField();
            paraField[colCount + 20].Name = "REXInterestAmt";
            discreteValue[colCount + 20] = new ParameterDiscreteValue();
            discreteValue[colCount + 20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInvInterest_InterestAmt");
            paraField[colCount + 20].CurrentValues.Add(discreteValue[colCount + 20]);

            paraField[colCount + 21] = new ParameterField();
            paraField[colCount + 21].Name = "REXTotal";
            discreteValue[colCount + 21] = new ParameterDiscreteValue();
            discreteValue[colCount + 21].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[colCount + 21].CurrentValues.Add(discreteValue[colCount + 21]);

            paraField[colCount + 22] = new ParameterField();
            paraField[colCount + 22].Name = "REXNote";
            discreteValue[colCount + 22] = new ParameterDiscreteValue();
            discreteValue[colCount + 22].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_LegalCode");
            paraField[colCount + 22].CurrentValues.Add(discreteValue[colCount + 22]);

            paraField[colCount + 23] = new ParameterField();
            paraField[colCount + 23].Name = "REXMallTitle";
            discreteValue[colCount + 23] = new ParameterDiscreteValue();
            discreteValue[colCount + 23].Value = Session["MallTitle"].ToString();
            paraField[colCount + 23].CurrentValues.Add(discreteValue[colCount + 23]);

            paraField[colCount + 24] = new ParameterField();
            paraField[colCount + 24].Name = "REXInterestRate";
            discreteValue[colCount + 24] = new ParameterDiscreteValue();
            discreteValue[colCount + 24].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InterestRate");
            paraField[colCount + 24].CurrentValues.Add(discreteValue[colCount + 24]);

            paraField[colCount + 25] = new ParameterField();
            paraField[colCount + 25].Name = "REXInvoiceDate";
            discreteValue[colCount + 25] = new ParameterDiscreteValue();
            discreteValue[colCount + 25].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvDate");
            paraField[colCount + 25].CurrentValues.Add(discreteValue[colCount + 25]);

            paraField[colCount + 26] = new ParameterField();
            paraField[colCount + 26].Name = "REXIinterestDate";
            discreteValue[colCount + 26] = new ParameterDiscreteValue();
            discreteValue[colCount + 26].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_IinterestDate");
            paraField[colCount + 26].CurrentValues.Add(discreteValue[colCount + 26]);

            paraField[colCount + 27] = new ParameterField();
            paraField[colCount + 27].Name = "REXContractMan";
            discreteValue[colCount + 27] = new ParameterDiscreteValue();
            discreteValue[colCount + 27].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_ContractMan");
            paraField[colCount + 27].CurrentValues.Add(discreteValue[colCount + 27]);

            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }
            string str_sql = "";
            str_sql = "select A.ShopCode,A.ShopName,B.ContractCode,C.CurTypeName," +
                      "D.CustCode,D.CustName,D.BankName,D.BankAcct,D.OfficeAddr," +
                      "D.OfficeTel,E.ChargeTypeName,F.InvStartDate,F.InvEndDate," +
                      "G.LatePayAmt as InvActPayAmt,G.IntStartDate,G.IntEndDate,G.InterestDay," +
                      //"G.Note,G.CreateTime,G.InvCode,G.InterestAmt,G.InterestRate,'" + PrtName + "' as PrtName,CustContact.contactMan " +
                      "G.Note,G.CreateTime,G.InvCode,G.InterestAmt,G.InterestRate,'" + PrtName + "' as PrtName"+
                      " from ConShop A,Contract B,CurrencyType C,Customer D,ChargeType E," +
                      //" InvoiceDetail F,InvoiceInterest G,InvoiceHeader H,CustContact" +
                      " InvoiceDetail F,InvoiceInterest G,InvoiceHeader H" +
                      " where A.ContractID = B.ContractID" +
                      " and F.InvID = H.InvID" +
                      " and C.CurTypeID = H.InvCurTypeID" +
                      " and D.CustID = H.CustID" +
                      " and G.ChargeTypeID = E.ChargeTypeID " +
                      " and G.LateInvID = H.InvID " +
                      " and B.ContractID = H.ContractID " +
                   //   " and CustContact.CustID = D.CustID"+
                      " and F.InvDetailID = G.LateInvDetailID";
            //" and G.InvCode = '" + InvCode + "'";

            if (ContractCode != "")
            {
                str_sql += " AND B.ContractCode = '" + ContractCode + "'";
            }

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\InvoiceInterest.rpt";

            this.Response.Redirect("../ReportShow.aspx");
        }
    }
}
