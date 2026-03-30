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

using Base.Page;
using Base.DB;
using BaseInfo.authUser;
using BaseInfo.User;
using Base;
using Base.Biz;


/// <summary>
/// 编写人:hesijian
/// 编写日期：2009年7月3日

/// </summary>

public partial class ReportM_RptInv_RptInvoiceDetailList : BasePage
{
    public string baseInfo; 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    //水晶报表数据绑定
    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[14];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXMallTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvoiceDatailList");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        //费用发生时间 
        Field[2] = new ParameterField();
        Field[2].Name = "REXInvDate";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvoiceDate");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        //合同号

        Field[3] = new ParameterField();
        Field[3].Name = "REXContractCode";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labContractCode");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        //客户编号
        Field[4] = new ParameterField();
        Field[4].Name = "REXCustCode";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseAreaType_CustCode");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        //客户名称
        Field[5] = new ParameterField();
        Field[5].Name = "REXCustName";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        //实收
        Field[6] = new ParameterField();
        Field[6].Name = "REXInvPaidAmt";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidAmt2");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        //应收
        Field[7] = new ParameterField();
        Field[7].Name = "REXInvAmt";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAmt2");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        //代收款

        Field[8] = new ParameterField();
        Field[8].Name = "REXPayInAmt";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PayInAmt2");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        //代收款返还

        Field[9] = new ParameterField();
        Field[9].Name = "REXPayOutAmt";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PayOutAmt2");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        //代收款抵扣

        Field[10] = new ParameterField();
        Field[10].Name = "REXInvPaidOutAmt";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidOutAmt2");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        //查询日期
        Field[11] = new ParameterField();
        Field[11].Name = "REXCheckDate";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        //日期值

        Field[12] = new ParameterField();
        Field[12].Name = "REXDateNm";
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = txtInvDate.Text.Trim();
        Field[12].CurrentValues.Add(DiscreteValue[12]);

        //合计
        Field[13] = new ParameterField();
        Field[13].Name = "REXTotal";
        DiscreteValue[13] = new ParameterDiscreteValue();
        DiscreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        Field[13].CurrentValues.Add(DiscreteValue[13]);


        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";
        string strWhere = "";
        string strAuth = "";

        //权限设置
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }


        //条件查询
        if (txtContractID.Text.Trim() != "")
        {
            strWhere += " AND ContractCode = '" + txtContractID.Text + "'";
        }

        if (txtCustCode.Text.Trim() != "")
        {
            strWhere += " AND Customer.CustCode = '" + txtCustCode.Text + "'"; 
        }

        if (txtShopCode.Text.Trim() != "")
        {
            strWhere += " AND ConShop.ShopCode = '" + txtShopCode.Text + "'";
        }

        if (txtInvDate.Text.Trim() != "")
        {
            strWhere += " AND InvDate >= '" + txtInvDate.Text + " 00:00:00" + "' AND InvDate <= '" + txtInvDate.Text + " 23:59:59" + "'";
        
        }

        str_sql = @"Select 
                        InvDate,
                        ContractCode, 
                        Customer.CustCode, 
                        CustName, 
                        Sum(InvAmt) as InvAmt, 
                        Sum(InvPaidAmt) as InvPaidAmt, 
                        Sum(PayInAmt) as PayInAmt, 
                        Sum(PayOutAmt) as PayOutAmt, 
                        Sum(InvPaidOutAmt) as InvPaidOutAmt 
                        From (
                        Select InvDate,InvoiceHeader.ContractID,Sum(InvPayAmt) as InvAmt, 0 as InvPaidAmt,0 as PayInAmt,0 as PayOutAmt,0 as InvPaidOutAmt
                        From InvoiceHeader
                        Group By InvDate,InvoiceHeader.ContractID
                        Union All

                        Select InvPayTime,InvoicePay.ContractID,0 as InvAmt,Sum(InvPaidAmt) as InvPaidAmt,0 as PayInAmt,0 as PayOutAmt,0 as InvPaidOutAmt
                        From InvoicePay 
                        Group By InvPayTime,InvoicePay.ContractID
                        Union All

                        Select PayInDate,ConShop.ContractID,0 as InvAmt,0 as InvPaidAmt,Sum(PayInAmt) as PayInAmt,0 as PayOutAmt,0 as InvPaidOutAmt 
                        From PayIn Left Join ConShop On PayIn.ShopID = ConShop.ShopID
                        Group By PayInDate,ConShop.ContractID
                        Union All

                        Select PayOutDate,ConShop.ContractID,0 as InvAmt,0 as InvPaidAmt,0 as PayInAmt,Sum(PayOutAmt) as PayOutAmt,0 as InvPaidOutAmt
                        From PayOut Inner Join PayIn On PayOut.PayInID = PayIn.PayInID 
                        Left Join ConShop On PayIn.ShopID = ConShop.ShopID 
                        Group By PayOutDate,ConShop.ContractID
                        Union All

                        Select InvPayTime,InvoicePay.ContractID,0 as InvAmt,0 as InvPaidAmt,0 as PayInAmt,0 as PayOutAmt,Sum(InvPaidAmt) as InvPaidOutAmt
                        From InvoicePay Where InvPayType = 5
                        Group By InvPayTime,InvoicePay.ContractID
                        ) as a 
                     Left Join Contract on a.ContractID = Contract.ContractID 
                     Left Join Customer On Contract.CustID = Customer.CustID
                     Left Join ConShop On Contract.ContractID = ConShop.ContractID 
                     Where 1=1 " + strWhere + strAuth + @" 
                     Group By InvDate,ContractCode,Customer.CustCode,CustName Order By InvDate";

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptInvoiceDetailList.rpt";

    }

    //清除页面
    private void ClearPage()
    {
        txtContractID.Text = "";
        txtCustCode.Text = "";
        txtInvDate.Text = "";
        txtShopCode.Text = "";
    }

    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    //撤销操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
