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

using BaseInfo;
using BaseInfo.authUser;
using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using BaseInfo.User;

public partial class ReportM_RptSale_RptStoreTradeRentalThan : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDdl();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtSaleMonth.Text = DateTime.Now.ToString("yyyy-MM-01");
        }
    }

    private void BindDdl()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = " depttype='" + Dept.DEPT_TYPE_MALL + "' ";
        baseBo.OrderBy = " orderid ";
        Resultset rs = baseBo.Query(new Dept());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Dept dept in rs)
        {
            ddlBizproject.Items.Add(new ListItem(dept.DeptName, dept.DeptID.ToString()));
        }

        BaseBO objBaseBo = new BaseBO();
        Resultset rs1 = objBaseBo.Query(new TradeRelation());
        ddlTrade.Items.Add(new ListItem("", ""));
        foreach (TradeRelation trade in rs1)
        {
            ddlTrade.Items.Add(new ListItem(trade.TradeName, trade.TradeID.ToString()));
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptStoreTradeRentalThan.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreTradeRentalThan");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select dept.orderid,dept.deptname,invoicedetail.period,traderelation.tradename,sum(invoicedetail.invactpayamtl) invpay,aa.paidamt
                            from invoicedetail
                            inner join invoiceheader on invoicedetail.invid=invoiceheader.invid
                            inner join contract on invoiceheader.contractid=contract.contractid
                            inner join traderelation on traderelation.tradeid=contract.tradeid
                            inner join conshop on conshop.contractid=contract.contractid
                            inner join dept on dept.deptid=conshop.storeid
                            left join (
	                            select dept.deptname,transshopmth.month,traderelation.tradename,sum(transshopmth.paidamt) paidamt
	                            from transshopmth
	                            inner join dept on dept.deptid=transshopmth.storeid
	                            left join traderelation on traderelation.tradeid=transshopmth.tradeid
	                            group by dept.deptname,transshopmth.month,traderelation.tradename
	                            ) aa on aa.month=invoicedetail.period and aa.tradename=traderelation.tradename and aa.deptname=dept.deptname
                            where 1=1";


        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " and dept.deptid='" + ddlBizproject.SelectedValue + "'";
        }
        if (ddlTrade.Text != "")
        {
            str_sql = str_sql + " and traderelation.tradeid='" + ddlTrade.SelectedValue + "' ";
        }
        if (txtSaleMonth.Text != "")
        {
            str_sql = str_sql + " and invoicedetail.period='" + txtSaleMonth.Text + "' ";
        }


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + strAuth + " group by dept.orderid,dept.deptname,traderelation.tradename,invoicedetail.period,aa.paidamt order by dept.orderid";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreTradeRentalThan.rpt";

    }
}
