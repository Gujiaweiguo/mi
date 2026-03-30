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

using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.Store;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptStoreTradeSalesAnalysis : BasePage
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
        this.Response.Redirect("~/ReportM/RptSale/RptStoreTradeSalesAnalysis.aspx");
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
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreTradeSalesAnalysis");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select dept.orderid,dept.deptname,transshopmth.month,traderelation.tradename,sum(conshop.rentarea) rentarea,sum(transshopmth.paidamt) paidamt,aa.ypaidamt
                            from transshopmth
                            inner join dept on dept.deptid=transshopmth.storeid
                            inner join traderelation on traderelation.tradeid=transshopmth.tradeid
                            inner join conshop on conshop.shopid=transshopmth.shopid
                            left join (
	                            select storeid,tradeid,sum(paidamt) ypaidamt
	                            from transshopmth
	                            where year(month)='" + txtSaleMonth.Text.Substring(0, 4) + "' and month(month)<='" + DateTime.Parse(txtSaleMonth.Text).Month + @"' 
	                            group by storeid,tradeid) aa on aa.storeid=transshopmth.storeid and aa.tradeid=transshopmth.tradeid
                            where 1=1";
                            
        
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " and transshopmth.storeid='" + ddlBizproject.SelectedValue + "'";
        }
        if (ddlTrade.Text != "")
        {
            str_sql = str_sql + " and transshopmth.tradeid='" + ddlTrade.SelectedValue + "' ";
        }
        if (txtSaleMonth.Text != "")
        {
            str_sql = str_sql + " and transshopmth.month='" + txtSaleMonth.Text + "' ";
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

        str_sql = str_sql + strAuth + " group by dept.orderid,dept.deptname,transshopmth.month,traderelation.tradename,aa.ypaidamt order by dept.orderid";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreTradeSalesAnalysis.rpt";

    }
}
