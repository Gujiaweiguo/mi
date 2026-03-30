using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.PotBargain;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class ReportM_RptInv_RptOweDetail : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            this.BindBizProject();
            this.BindChargetype();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblCustBalanceDetail");
        }
    }
    private void BindChargetype()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new ChargeType());
        this.ddlChargeType.Items.Add(new ListItem("", ""));
        foreach (ChargeType chargetype in rs)
        {
            this.ddlChargeType.Items.Add(new ListItem(chargetype.ChargeTypeName , chargetype.ChargeTypeID.ToString()));
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
        txtContractID.Text = "";
        txtPeriod.Text = "";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitleDetail";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_OweDetailTitle");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTitleTotal";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_OweTitle");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMallTitle";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = Session["MallTitle"].ToString();
        paraField[2].CurrentValues.Add(discreteValue[2]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }


        //string str_sql = "select invoicedetail.period invperiod,store.storeid,store.storeshortname,contract.contractcode," +
        //                " customer.custname,contract.contractid,chargetype.chargetypename," +
        //                " conshop.shopcode,conshop.shopname,invoicedetail.InvActPayAmt amt,invoicedetail.InvActPayAmtL actpayamt," +
        //                " 0 iactamt,invoicedetail.InvPaidAmtL paidamt" +
        //                " from invoicedetail inner join chargetype on (chargetype.chargetypeid=invoicedetail.chargetypeid)" +
        //                " inner join invoiceheader on (invoicedetail.invid=invoiceheader.invid)" +
        //                " inner join contract on (invoiceheader.contractid=contract.contractid)" +
        //                " inner join conshop on (conshop.contractid=contract.contractid)" +
        //                " inner join customer on (customer.custid=contract.custid)" +
        //                " inner join store on (store.storeid=conshop.storeid)" +
        //                " where invoicedetail.invpaidamtl <> invoicedetail.invactpayamtl";

        if (RadioButton2.Checked == true)
        {
            string str_sql = "select a.period invperiod,d.storeid,d.storename storeshortname,f.contractcode,e.custname,f.contractid," +
                            " sum(a.InvActPayAmt) amt,sum(a.invpaidamtl) paidamt," +
                            " 0 iactamt,sum(a.InvActPayAmtl) actpayamt," +
                            " traderelation.tradename,conshopbrand.brandname,chargetype.chargetypename," +
                            " (select sum(rentarea) from conshop where conshop.contractid=f.contractid) as rentarea," +  //签约面积
                            " (select top 1 conshop.shopname from conshop where conshop.contractid=f.contractid) as shopname" +
                            " from invoicedetail a inner join invoiceheader b on (a.invid=b.invid)" +
                            " inner join chargetype on (chargetype.chargetypeid=a.chargetypeid)" +
                            " right join conshop on (b.contractid=conshop.contractid) inner join store d on (conshop.storeid=d.storeid)" +
                            " inner join customer e on (e.custid=b.custid) inner join contract f on (b.contractid=f.contractid)" +
                            " inner join conshopbrand on (conshop.brandid=conshopbrand.brandid) inner join traderelation on (f.tradeid=traderelation.tradeid)" +
                            " where a.invactpayamtl <>0 and (a.invactpayamtl-a.invpaidamtl)>0 ";


            if (ddlBizproject.Text != "")
            {
                str_sql += " AND d.storeid='" + ddlBizproject.SelectedValue + "'";
            }

            if (this.txtPeriod.Text.Trim() != "")
            {
                str_sql += " And a.period <='" + this.txtPeriod.Text.Trim() + "'";
            }

            if (this.txtContractID.Text.Trim() != "")
            {
                str_sql += " And f.contractcode= '" + this.txtContractID.Text.Trim() + "'";
            }
            if (this.ddlChargeType.Text != "")
            {
                str_sql += " And a.chargetypeid=" + this.ddlChargeType.SelectedValue;
            }

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            string authWhere = "";
            if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
            {
                authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            }

            str_sql = str_sql + authWhere;
            str_sql += " group by a.period,d.storeid,d.storename,f.contractcode,e.custname,f.contractid,traderelation.tradename,conshopbrand.brandname," +
                      "chargetype.chargetypename order by d.storeid,e.custname,a.period,chargetype.chargetypename";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\OweDetail.rpt";
        }
        else
        {
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
                       " (select top 1 conshop.shopname from conshop where conshop.contractid=f.contractid) as shopname," +
                       " (select top 1 conshop.shopcode from conshop where conshop.contractid=f.contractid) as shopcode,shoptype.shoptypename" +
                       " from invoicedetail a inner join invoiceheader b on (a.invid=b.invid)" +
                       " right join conshop c on (b.contractid=c.contractid) inner join store d on (c.storeid=d.storeid)" + //考虑一个合同多个商铺 
                       " inner join customer e on (e.custid=b.custid) inner join contract f on (b.contractid=f.contractid)" +
                       " inner join conshopbrand on (c.brandid=conshopbrand.brandid) inner join traderelation on (f.tradeid=traderelation.tradeid)" +
                       " left join shoptype on (shoptype.shoptypeid=c.shoptypeid)"+
                       " where a.invactpayamtl <>0 and (a.invactpayamtl-a.invpaidamtl)>0 " + authWhere;

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

            str_sql += " group by a.period,d.storeid,d.storename,f.contractcode,e.custname,f.contractid,traderelation.tradename,conshopbrand.brandname,shoptype.shoptypename";
            str_sql += " order by d.storeid,e.custname,a.period";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\OweTotal.rpt";
        }
    }
    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        Label14.Visible = false;
        ddlChargeType.Visible = false;
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        Label14.Visible = true;
        ddlChargeType.Visible = true;
    }
}
