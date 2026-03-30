using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using Shop.ShopType;

public partial class ReportM_RptSale_RptTotalSales : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBizProject();
            BindShopType();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtSaleMonth.Text = DateTime.Now.ToString("yyyy-MM-01");
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

    private void BindShopType()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new ShopType());
        ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType shoptype in rs)
        {
            ddlShopType.Items.Add(new ListItem(shoptype.ShopTypeName, shoptype.ShopTypeCode));
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptTotalSales.aspx");
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
        ParameterField[] paraField = new ParameterField[10];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[10];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXStoreName";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXSalesMonth";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SalesMonth");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopType";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXRentArea";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblRentArea");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMonthSellCount";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthSellCount");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXMonthAreaSalesRate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTotalSales_MonthAreaSalesRate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXYearSaleSum";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTotalSales_YearSaleSum");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXYearAreaSalesRateSum";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTotalSales_YearAreaSalesRateSum");
        paraField[7].CurrentValues.Add(discreteValue[7]);
        
        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMallTitle";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = Session["MallTitle"].ToString();
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "RptTotalSales_Title");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select aa.storename,CONVERT(varchar(7),aa.month,120) as month,aa.shoptypename,aa.rentarea,aa.paidamt,--当月销售额
	                    (aa.paidamt/aa.rentarea /aa.mthDays) as mthRate,  --当月坪效
	                    aa.yearPaidamt,  --年度累计销售额
	                    (aa.yearPaidamt / aa.rentarea / (aa.yearDays+aa.mthDays)) as yearRate  --年度累计坪效
                        from 
	                    (select transshopmth.storename,transshopmth.month,transshopmth.shoptypename,sum(conshop.rentarea) as rentarea,
		                sum(transshopmth.paidamt) as paidamt,
		                (select datediff(day,transshopmth.month,dateadd(month,1,transshopmth.month))) as mthDays,  --当月天数
		                --当年累计销售
		                (select sum(a.paidamt) from transshopmth a where a.storename=transshopmth.storename and a.shoptypename=transshopmth.shoptypename and a.month between (cast(year(transshopmth.month) as char(4))) and (transshopmth.month)) as yearPaidamt,
		                (select datediff(day,cast(year(transshopmth.month) as char(4)),transshopmth.month)) as yearDays  --截至本月1日当年天数
	                        from transshopmth
	                        inner join conshop on (conshop.shopid=transshopmth.shopid)
	                    where conshop.shopstatus=1";
                        //--查询条件	and transshopmth.storeid=101 and transshopmth.month= '2008-2-1' and transshopmth.shoptypeid=101
                        //group by transshopmth.storename,transshopmth.month,transshopmth.shoptypename) as aa";
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " and transshopmth.storeid='" + ddlBizproject.SelectedValue + "'";
        }
        if (ddlShopType.Text != "")
        {
            str_sql = str_sql + " and transshopmth.shoptypeid='" + ddlShopType.SelectedValue + "' ";
        }
        if (txtSaleMonth.Text != "")
        {
            str_sql = str_sql + " and transshopmth.month='" + txtSaleMonth.Text + "' ";
        }
        //if (RB1.Checked)
        //{

        //    str_sql = str_sql + "";
        //}
        //if (RB2.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=1 ";
        //}
        //if (RB3.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=2 ";
        //}
        //if (RB4.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=3 ";
        //}

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
            for (int i = 0; i < 5; i++)
            {
                //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
                strAuth = strAuth.Replace("ConShop", "transshopmth");
            }
        }

        str_sql = str_sql + strAuth + " group by transshopmth.storename,transshopmth.month,transshopmth.shoptypename) as aa";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\TotalSales.rpt";

    }
}
