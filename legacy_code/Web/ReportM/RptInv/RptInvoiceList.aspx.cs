using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Page;



public partial class ReportM_RptInv_RRptInvoiceList : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = "单据和付款明细表";//String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");

        }

    }
    private void BindDDL()
    {
        for (int i = 1; i <= 12; i++)
        {
            DDL1.Items.Add(new ListItem(i.ToString().Trim()));
            DDL3.Items.Add(new ListItem(i.ToString().Trim()));
            
        }

        for (int i = 2007; i <= DateTime.Now.Year + 1; i++)
        {
            DDL2.Items.Add(new ListItem(i.ToString().Trim()));
            DDL4.Items.Add(new ListItem(i.ToString().Trim()));
        }

        DDL1.Text = DateTime.Now.Month.ToString();
        DDL3.Text = DateTime.Now.Month.ToString();
        DDL2.Text = DateTime.Now.Year.ToString();
        DDL4.Text = DateTime.Now.Year.ToString();    
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";

            BindData();

        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        //结算单号
        paraField[0] = new ParameterField();
        paraField[0].Name = "REXInvCode";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvCode");
        paraField[0].CurrentValues.Add(discreteValue[0]);
        //客户
        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCust";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = "商户";
        paraField[1].CurrentValues.Add(discreteValue[1]);
        //生成日期
        paraField[2] = new ParameterField();
        paraField[2].Name = "REXInvDate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvDate");
        paraField[2].CurrentValues.Add(discreteValue[2]);
        //合同号
        paraField[3] = new ParameterField();
        paraField[3].Name = "REXContractCode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[3].CurrentValues.Add(discreteValue[3]);
        //商铺号
        paraField[4] = new ParameterField();
        paraField[4].Name = "REXShopID";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        paraField[4].CurrentValues.Add(discreteValue[4]);
        //商铺名称
        paraField[5] = new ParameterField();
        paraField[5].Name = "REXShopCode";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[5].CurrentValues.Add(discreteValue[5]);
        //费用周期
        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvPeriod";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = "费用周期";
        paraField[6].CurrentValues.Add(discreteValue[6]);
        //时间段
        paraField[7] = new ParameterField();
        paraField[7].Name = "REXInvTime";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Date");
        paraField[7].CurrentValues.Add(discreteValue[7]);
        //调整金额
        paraField[8] = new ParameterField();
        paraField[8].Name = "REXInvAdjAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAdjAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);
        //已付金额
        paraField[9] = new ParameterField();
        paraField[9].Name = "REXInvPaidAmt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoiceHeader_InvPaidAmt");
        paraField[9].CurrentValues.Add(discreteValue[9]);
        //欠款金额
        paraField[10] = new ParameterField();
        paraField[10].Name = "REXInvOweAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_OweAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);
        //总计
        paraField[11] = new ParameterField();
        paraField[11].Name = "REXTotalAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[11].CurrentValues.Add(discreteValue[11]);
        //标题
        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = "单据和付款明细表";
        paraField[12].CurrentValues.Add(discreteValue[12]);
        //单据金额
        paraField[13] = new ParameterField();
        paraField[13].Name = "REXInvPayAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = "单据金额";
        paraField[13].CurrentValues.Add(discreteValue[13]);
        //合计
        paraField[14] = new ParameterField();
        paraField[14].Name = "REXInvAll";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = "合计";
        paraField[14].CurrentValues.Add(discreteValue[14]);
        //大标题
        paraField[15] = new ParameterField();
        paraField[15].Name = "REXMallTitle";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = Session["MallTitle"].ToString();
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string whereStr1 = "";
        string whereStr2 = "";
        string whereStr3 = "";
        //string whereStr4 = "";
        //string whereStr5 = "";
        //string whereStr6 = "";

        string str_sql = "";
        whereStr2 ="AND invoiceHeader.invPeriod BETWEEN '"+ DDL2.SelectedValue.ToString() + "-" + DDL1.SelectedValue.ToString()+"-1' AND '"+DDL4.SelectedValue.ToString() + "-" + DDL3.SelectedValue.ToString()+"-1'";



        if (RadioButton1.Checked)
        {
            whereStr3 = "AND invoiceHeader.invStatus in (1)";
        }
        if (RadioButton2.Checked)
        {
            whereStr3 = "AND invoiceHeader.invStatus in (2)";
        }
        if (RadioButton3.Checked)
        {
            whereStr3 = "AND invoiceHeader.invStatus in (3)";
        }

        //if (TextBox5.Text.Trim() != "")
        //{
        //    whereStr1 = " AND contract.ContractCode='" + TextBox5.Text.Trim() + "'";
        //}
        if (TextBox6.Text.Trim() != "")
        {
            whereStr1 = whereStr1 + " AND customer.CustCode='" + TextBox6.Text.Trim() + "'";
        }
        if (TextBox7.Text.Trim() != "")
        {
            whereStr1 = whereStr1 + " AND invoiceHeader.CustName like '%" + TextBox7.Text.Trim() + "%'";
        }
        if (TextBox4.Text.Trim() != "")
        {
            whereStr1 = whereStr1 + " AND contractID IN (SELECT contractID FROM conShop WHERE conShop.conShopCode = '" + TextBox4.Text.Trim() + "') ";
        }
        if (TextBox1.Text.Trim() != "")
        {
            whereStr1 = whereStr1 + " AND invoiceHeader.InvDate >= '" + TextBox1.Text.Trim() + " '";
        }
        if (TextBox3.Text.Trim() != "")
        {
            whereStr1 = whereStr1 + " AND invoiceHeader.InvDate <= '" + TextBox3.Text.Trim() + " '";
        }


        str_sql = "SELECT " +
                    "isnull(store.storename,'') storename,invoiceHeader.InvCode" +
                   ",customer.CustCode AS CustID" +
                   ",invoiceHeader.CustName" +
                   ",invoiceHeader.ContractID" +
                   ",contract.ContractCode" +
                   ",'BizMode' =  case contract.bizMode  " +
                   "when '1' then '租赁' when '2' then '联营'" +
                   " end  " +
                   " ,conShopInfo_view.ShopID" +
                   ",conShopInfo_view.ShopCode" +
                   ",conShopInfo_view.ShopName" +
                   ",invoiceHeader.InvDate" +
                   ",invoiceHeader.InvPeriod" +
                   ",invoiceDetail.ChargeTypeID" +
                   ",(SELECT ChargeTypeName FROM ChargeType WHERE ChargeType.ChargeTypeID = invoiceDetail.ChargeTypeID) AS ChargeTypeName " +
                   ",invoiceDetail.InvStartDate" +
                   ",invoiceDetail.InvEndDate " +
                   ",invoiceDetail.InvPayAmtL " +
                   ",invoiceDetail.InvAdjAmtL " +
                   ",invoiceDetail.InvPaidAmtL " +
                   ",(invoiceDetail.invActPayAmtL - invoiceDetail.invPaidAmtL) AS InvBalanceAmt " +
                   ",invoiceDetail.RentType " +
                "FROM customer INNER JOIN invoiceHeader ON(customer.custid=invoiceHeader.custid)  " +
                   "INNER JOIN invoiceDetail ON (invoiceHeader.invID = invoiceDetail.invID) " +
                   "INNER JOIN contract ON (invoiceHeader.contractID = contract.contractID ) " +
                   "left join conshop on (conshop.contractid=contract.contractid) " +
                   "left join store on (conshop.storeid=store.storeid) "+
                   "INNER JOIN conShopInfo_view ON (invoiceHeader.contractID = conShopInfo_view.contractID) " +
                "WHERE invoiceDetail.invActPayAmtL <> 0 " +
                "AND contract.bizMode = 1 " + whereStr1 + whereStr2 + whereStr3 +
                "AND invoiceHeader.invStatus < 4 ";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptInvoiceList.rpt";


    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        TextBox1.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        //TextBox5.Text = "";
        TextBox6.Text = "";
        TextBox7.Text = "";

    }
}
