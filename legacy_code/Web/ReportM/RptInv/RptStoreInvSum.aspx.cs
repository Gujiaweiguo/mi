using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Page;
using Base.Biz;
using BaseInfo.Store;
using Base.DB;


public partial class ReportM_RptInv_RptStoreInvSum : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DataBind();
            txtPeriod.Text = DateTime.Now.ToString ("yyyy-MM-01");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptStoreInvSum");
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

        //if (txtPeriod.Text != "")
        //{
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        //}
        //else
        //{ 
        ////弹出提示
        //}
    }
    protected void DataBind()
    {
        BaseBO baseBo = new BaseBO();

        //绑定商业项目
        Resultset rs = baseBo.Query(new Store());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));
    
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.Text = "";
        txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

         paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptStoreInvSum");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCount";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[2].CurrentValues.Add(discreteValue[2]);




        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        if (this.txtPeriod.Text.Trim() == "")
        { this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01"); }

        string str_sql = "select c.storeid,(select deptname from dept where deptid=c.storeid) storename,a.period,budget.monthamt,yearbudget.yearbudget YearBudget,sum(a.InvActPayAmtl) InvPayAmt," +
                        "aa.YearInvPayAmt from invoicedetail a inner join invoiceheader b on (a.invid=b.invid)" +
                        " inner join conshop c on (b.contractid=c.contractid) inner join " +
                            //预算月
                              " (select storeid,budgetyear,sum(monthamt) monthamt from budget where budgetyear=year('" + Convert.ToDateTime(this.txtPeriod.Text).ToString() + "') group by storeid,budgetyear) budget " +
                        "on (budget.storeid=c.storeid)" +
                        //截至本月年预算
                        "inner join (select storeid,sum(monthamt)*month('" + Convert.ToDateTime(this.txtPeriod.Text).ToString() + "') yearbudget from budget where budgetyear=year('" + Convert.ToDateTime(this.txtPeriod.Text).ToString() + "')  group by storeid) yearbudget" +
                        " on (yearbudget.storeid=c.storeid)" +
                        //截至本月年执行
                        " inner join (select conshop.storeid,invoicedetail.period,sum(invoicedetail.InvActPayAmtl) YearInvPayAmt from " +
                            " invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid)" +
                            " inner join conshop on (invoiceheader.contractid=conshop.contractid)" +
                            " where invoicedetail.chargetypeid in (101,102) and invoicedetail.period  between cast(year('" + Convert.ToDateTime(this.txtPeriod.Text).ToString() + "' ) as char(4)) + '-1-1' and '" + Convert.ToDateTime(this.txtPeriod.Text).ToString()  +"'" +
                            " group by invoicedetail.period, conshop.storeid) aa" +
                        " on (aa.storeid=c.storeid and aa.period=a.period)" +
                        " where a.chargetypeid in (101,102) ";

        if (ddlStoreName.Text != "" )
        {
            str_sql += "AND c.storeid=" + ddlStoreName.SelectedValue;
        }
        str_sql += "and a.period='" +　Convert.ToDateTime(this.txtPeriod.Text).ToString("yyyy-MM-01") + "'";

        str_sql += " group by budget.monthamt,a.period, c.storeid,aa.YearInvPayAmt,yearbudget.yearbudget order by c.storeid,a.period";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptStoreInvSum.rpt"; 

    }
}
