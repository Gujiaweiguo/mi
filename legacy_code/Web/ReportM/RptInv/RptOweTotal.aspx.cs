using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class ReportM_RptInv_RptOweTotal : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "RptInv_OweTitle");
        }
    }
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
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
        ddlBizproject.SelectedValue = "";
        txtContractID.Text = "";
        txtPeriod.Text = "";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[15];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[15];
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
        paraField[2].Name = "RexPeriod";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXPayAmt";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PayAmt");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXSInvPaidAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidAmt");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXOweAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_OweAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTitle";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_OweTitle");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSdate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[7].CurrentValues.Add(discreteValue[7]);
        
        paraField[8] = new ParameterField();
        paraField[8].Name = "REXShopName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXAmount";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXBizProject";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "RexTrade";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "RexBrand";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblMainBrand");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "RexRentArea";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string authWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";

            for (int i = 0; i < 5; i++)
            {
                //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
                authWhere = authWhere.Replace("ConShop", "c");
            }
        }

        string str_sql = "select a.period invperiod,d.storeid,d.storename storeshortname,f.contractcode,e.custname,f.contractid," +
                       " sum(a.InvActPayAmt) amt,sum(a.invpaidamtl) paidamt," +  //已付金额
                       " 0 iactamt,sum(a.InvActPayAmtl) actpayamt," +  //应付金额
                       " traderelation.tradename,conshopbrand.brandname," +
                       " (select sum(rentarea) from conshop where conshop.contractid=f.contractid) as rentarea," +   //签约面积
                       " (select top 1 conshop.shopname from conshop where conshop.contractid=f.contractid) as shopname" +
                       " from invoicedetail a inner join invoiceheader b on (a.invid=b.invid)" +
                       " right join conshop c on (b.contractid=c.contractid) inner join store d on (c.storeid=d.storeid)" + //考虑一个合同多个商铺 
                       " inner join customer e on (e.custid=b.custid) inner join contract f on (b.contractid=f.contractid)" +
                       " inner join conshopbrand on (c.brandid=conshopbrand.brandid) inner join traderelation on (f.tradeid=traderelation.tradeid)" +
                       " where a.invactpayamtl <>0 and (a.invactpayamtl-a.invpaidamtl)>0 " + authWhere ;   

        if (ddlBizproject.Text != "")
        {
            str_sql += " AND c.storeid='" + ddlBizproject.SelectedValue + "'";
        }

        if (this.txtPeriod.Text.Trim() != "")
        {
            str_sql += " And a.period <='" + this.txtPeriod.Text.Trim() + "'";
        }

        if (this.txtContractID.Text.Trim() != "")
        {
            str_sql += " And f.contractCode= '" + this.txtContractID.Text.Trim() + "'";
        }

        str_sql += " group by a.period,d.storeid,d.storename,f.contractcode,e.custname,f.contractid,traderelation.tradename,conshopbrand.brandname";
        str_sql += " order by d.storeid,e.custname,a.period";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\OweTotal.rpt";

    }
}
