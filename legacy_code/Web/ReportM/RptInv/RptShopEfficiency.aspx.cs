using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using BaseInfo.authUser;
using BaseInfo.User;


public partial class RptBaseMenu_RptShopEfficiency : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblShopEfficiency");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }


    /* 判断数据空值,返回默认值

     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 初始化下拉列表

     * 
     * 
     */
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
    private void InitDDL()
    {
        int intMonth = 12;
        ddlMonth.Items.Clear();
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        this.ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    /* 取得表头资源
     * appjkleai
     * EAIServer
     */
    private String GetRptResx()
    {
        String s = "%23Rpt_lblShopEfficiency";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "RentableArea_lblAreaName";
        s += "%23" + "Rpt_InvPeriod";
        s += "%23" + "Rpt_ChargeAmt";
        s += "%23" + "Rpt_SalesAmt";
        s += "%23" + "Rpt_Efficiency";
        return s;
    }



    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXShopCode";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "RexSDate";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = this.ddlYear.SelectedValue.ToString() + "年" + this.ddlMonth.SelectedValue.ToString() + "月";
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[3].CurrentValues.Add(discreteValue[3]);
        //业态名称
        paraField[4] = new ParameterField();
        paraField[4].Name = "RexTradeName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID");
        paraField[4].CurrentValues.Add(discreteValue[4]);
        //签约面积
        paraField[5] = new ParameterField();
        paraField[5].Name = "RexRentArea";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvPayAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FixedRental");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXCostAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CostAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXPayPaidAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvSalesRate");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblShopEfficiency");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);


        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "RexSeach";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SeachMonth");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
            //for (int i = 0; i < 5; i++)
            //{
            //    //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
            //    strAuth = strAuth.Replace("ConShop", "transshopday");
            //}
        }

        string str_sql = "select TransShopMth.TradeName,TransShopMth.storeid,TransShopMth.storename storeshortname,Customer.CustCode,Customer.CustName,TransShopMth.ShopCode,TransShopMth.ShopName,TransShopMth.BuildingName,TransShopMth.FloorName," +
                        " TransShopMth.ShopTypeName,InvoiceHeader.InvPeriod,InvoiceHeader.InvActPayAmtl InvAmt,TransShopMth.PaidAmt salesAmt,InvoiceHeader.InvActPayAmtl/TransShopMth.PaidAmt PayPaidAmt,conshop.rentarea" +
                        " from TransShopMth inner join conshop on (transshopmth.shopid=conshop.shopid)" +
                        " inner join contract on (contract.contractid=conshop.contractid)" +
                        " inner join customer on (contract.custid=customer.custid)" +
                        " inner join invoiceheader on (invoiceheader.contractid=contract.contractid)" +
                        " where invoiceheader.invperiod=TransShopMth.Month" + strAuth;

        if (this.ddlYear.SelectedValue.ToString() != "" || this.ddlMonth.SelectedValue.ToString() !="")
        {
            str_sql = str_sql + " and TransShopMth.month='" + this.ddlYear.SelectedValue.ToString() + "-" + this.ddlMonth.SelectedValue.ToString() + "-01'";
        }
        else
        {
            str_sql = str_sql + " and TransShopMth.month='" + DateTime.Now.ToString("yyyy-MM-01") + "'";
        }

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND TransShopMth.storeid=" + int.Parse(ddlBizproject.SelectedValue);
        }      


        str_sql = str_sql + "  ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\ShopEfficiency.rpt";

    }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptShopEfficiency.aspx");
    }
}
