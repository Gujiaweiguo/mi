using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
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
using BaseInfo.User;

public partial class RptInvDetail : BasePage
{
    private String InvCode = "";
    private String PrtName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
        }

        InvCode = Request["InvCode"];
        PrtName = Request["PrtName"];

        //String sCon = GetRptCond(InvCode, PrtName);
        //String sldl = GetRptResx();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        PrtName = Convert.ToString(objSessionUser.UserName);
        //this.Response.Redirect("ShowReport.aspx?ReportName=/Mi/Base/InvoiceDetail2.rpt" + sCon + sldl);
        BindData();
        this.Response.Redirect("ReportShow.aspx");
    }


    /* 判断数据空值,返回默认值

     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return (s==null || s.Trim() == "") ? "-32760" : s;
    }
    /* 初始化下拉列表

     * 
     * 
     */
    private void InitDDL()
    {


    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        String sCon = GetRptCond(InvCode, PrtName);
        String sldl = GetRptResx();

        //this.Session["tempxhq"] = "/Mi/Base/ContractInfo.rpt" + sCon + sldl;

        //this.Response.Redirect("ShowReport.aspx?ReportName=/Mi/Base/InvoiceDetail.rpt" + sCon + sldl);
    }

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_InvComTitile";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labContractCode";
        s += "%23" + "CustType_lblNote";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "InvoiceHeader_lblInvPayCurID";
        s += "%23" + "InvoiceHeader_lblInvID";
        s += "%23" + "Account_lblChargeType";
        s += "%23" + "Lease_lblChargeDate";
        s += "%23" + "Account_lblChargeMoney";
        s += "%23" + "User_lblNote";
        s += "%23" + "Account_lblTotalMoney";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "PotCustomer_lblBankName";
        s += "%23" + "PotCustomer_lblBankAcct";
        s += "%23" + "PotCustomer_Contact";
        s += "%23" + "Dept_lblTel";
        s += "%23" + "PotCustomer_lblOfficeAddr";
        s += "%23" + "Rpt_PrintMan";
        s += "%23" + "Rpt_InvDate";
        s += "%23" + "Rpt_PrintDate";
        s += "%23" + "Rpt_ReprintFlag";
        s += "%23" + "Rpt_InvAddsTitile";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond(String InvCode, String PrtName)
    {
        String sCon = "%26sPara=''";
        sCon += "%26invcode1=" + GetStrNull(InvCode) + "%26invcode2=" + GetStrNull(InvCode);
        sCon += "%26prtName=" + GetStrNull(PrtName);
        return sCon;
    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[19];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[19];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInvoiceDetail2_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXContractCode";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXChargeTypeName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeTypeName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXPeriod";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvActPayAmtL";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXNote";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Note");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXCurTypeName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CurTypeName");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXInvcode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Invcode");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXLegalCode";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_LegalCode");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXBankName";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankName");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXBankAcct";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_BankAcct");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXOfficeAddr";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_OfficeAddr");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXPrtName";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtName");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXCreateTime";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvDate");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXPrtDate";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXRePrint";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_RePrint");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        str_sql = " SELECT invoiceheader.ContractID," +
                  "        Customer.CustID," +
                  "        Customer.CustName as CustName ," +
                  "        Contract.ContractCode," +
                  "        InvoiceHeader.invcode," +
                  "        CurTypeName," +
                  "        invoiceHeader.InvExRate," +
                  "        LegalCode as LegalCode," +
                  "        BankName as BankName," +
                  "        BankAcct as BankAcct," +
                  "        Customer.OfficeAddr as OfficeAddr," +
                  "        InvoiceHeader.CreateTime," +
                  "        InvoiceHeader.IsFirst," +
                  "        '" + PrtName + "' as prtName," +
                  "        PrintFlag ," +
                  "        ChargeType.ChargeTypeName," +
                  "        invoiceDetail.InvStartDate as InvStartDate," +
                  "        invoiceDetail.InvEndDate as InvEndDate," +
                  "        invoiceDetail.InvActPayAmt," +
                  "        invoiceheader.Note " +
                  "   FROM invoiceheader INNER JOIN " +
                  "        invoicedetail ON (invoiceheader.invid = invoiceDetail.invid) INNER JOIN " +
                  "        ChargeType ON (invoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID) INNER JOIN " +
                  "        Contract ON (InvoiceHeader.ContractId = Contract.ContractId) INNER JOIN " +
                  "        Customer ON (Contract.CustId=Customer.CustId) INNER JOIN " +
                  "        CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) " +
                  " WHERE  InvoiceDetail.InvActPayAmt>0 AND InvoiceHeader.invcode =  " + InvCode;

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\InvoiceDetail2.rpt";


        string str_Subsql = "";
        str_Subsql = " SELECT ContractId,ShopCode,ShopName FROM Conshop WHERE ConShop.ShopStatus=0 ";
        Session["subReportSql"] = str_Subsql;
        Session["subRpt"] = "InvoiceShopInfo";

    }
}
