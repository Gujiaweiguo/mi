using System;
using System.Web.UI.WebControls;

using CrystalDecisions.Shared;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using BaseInfo.Dept;

public partial class Rpt_StoreInvDetail : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e) 
    {
        if (!IsPostBack)
        {
            BindBizItem();
            this.RadioButton4.Checked = true;
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreInvDetail");
            btnQuery.Attributes.Add("onclick", "return FormulaValidator()");
        }
    }

    //绑定商业项目
    private void BindBizItem()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
        Resultset rs = baseBo.Query(new Dept());
        txtBizItem.Items.Add(new ListItem("", ""));
        foreach (Dept dept in rs)
        {
            txtBizItem.Items.Add(new ListItem(dept.DeptName, dept.DeptID.ToString()));
        }
    }


    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field=new ParameterField[2];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreInvDetail");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXMallTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = Session["MallTitle"].ToString();
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        
        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string storesql = "";
        string str_sql = "";

        if (txtBizItem.SelectedValue != "")
        {
            storesql = " AND c.storeid=" + int.Parse(txtBizItem.SelectedValue.Trim()) + "";
        }
        
        if (this.RadioButton4.Checked)   //按权责发生制，invoicedetail
        {
            str_sql = "select c.storeid,(select deptname from dept where deptid=c.storeid) storename,a.period,budget.monthamt,yearbudget.yearbudget YearBudget,yearbudgettotal.yearbudgettotal YearBudgetTotal,sum(a.InvActPayAmtl) InvPayAmt," +
                        "aa.YearInvPayAmt,chargetype.chargetypename " +
                        "from invoicedetail a " +
                        "inner join invoiceheader b on (a.invid=b.invid) " +
                        "inner join conshop c on (b.contractid=c.contractid) " +
                        "left join (select storeid,chargetypeid,sum(monthamt) monthamt from budget where budget.budgetyear='" + txtPeriod.Text.Trim().Substring(0, 4) + "' group by storeid,chargetypeid,budgetyear) budget " +
                            "on (budget.storeid=c.storeid and budget.chargetypeid=a.chargetypeid) " +
                        "left join (select storeid,chargetypeid,sum(monthamt)*month('" + txtPeriod.Text.Trim() + "') yearbudget from budget group by storeid,chargetypeid) yearbudget " +
                            "on (yearbudget.storeid=c.storeid and yearbudget.chargetypeid=a.chargetypeid) " +
                        "left join (select storeid,chargetypeid,sum(yearamt) yearbudgettotal from budget group by storeid,chargetypeid) yearbudgettotal " +
                            "on (yearbudgettotal.storeid=c.storeid and yearbudgettotal.chargetypeid=a.chargetypeid) " +
                        "left join (select conshop.storeid,sum(invoicedetail.InvActPayAmtl) YearInvPayAmt,invoicedetail.chargetypeid  " +
                            "from invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid) " +
                            "inner join conshop on (invoiceheader.contractid=conshop.contractid) " +
                            "where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) and invoicedetail.period  between cast(year('" + txtPeriod.Text.Trim() + "') as char(4)) + '-1-1' and '" + txtPeriod.Text.Trim() + "' " +
                            "group by conshop.storeid,invoicedetail.chargetypeid) aa " +
                            "on (aa.storeid=c.storeid and aa.chargetypeid=a.chargetypeid) " +
                        "left join chargetype on chargetype.chargetypeid=a.chargetypeid " +
                        "where a.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) and a.period='" + txtPeriod.Text.Trim() + "' " + storesql +
                        "group by budget.monthamt,a.period,c.storeid,aa.YearInvPayAmt,yearbudget.yearbudget,yearbudgettotal.yearbudgettotal,chargetype.chargetypename " +
                        "order by c.storeid,a.period ";

        }
        else   //按收付实现制
        {
            str_sql = "select c.storeid,(select deptname from dept where deptid=c.storeid) storename,b.invperiod period,budget.monthamt,yearbudget.yearbudget YearBudget,yearbudgettotal.yearbudgettotal YearBudgetTotal,sum(b.InvActPayAmtl) InvPayAmt," +
                        "aa.YearInvPayAmt,chargetype.chargetypename " +
                        "from invoiceheader b " +
                        "inner join invoicedetail a on (a.invid=b.invid) " +
                        "inner join conshop c on (b.contractid=c.contractid) " +
                        "left join (select storeid,chargetypeid,sum(monthamt) monthamt from budget where budget.budgetyear='" + txtPeriod.Text.Trim().Substring(0, 4) + "' group by storeid,chargetypeid,budgetyear) budget " +
                            "on (budget.storeid=c.storeid and budget.chargetypeid=a.chargetypeid) " +
                        "left join (select storeid,chargetypeid,sum(monthamt)*month('" + txtPeriod.Text.Trim() + "') yearbudget from budget group by storeid,chargetypeid) yearbudget " +
                            "on (yearbudget.storeid=c.storeid and yearbudget.chargetypeid=a.chargetypeid) " +
                        "left join (select storeid,chargetypeid,sum(yearamt) yearbudgettotal from budget group by storeid,chargetypeid) yearbudgettotal " +
                            "on (yearbudgettotal.storeid=c.storeid and yearbudgettotal.chargetypeid=a.chargetypeid) " +
                        "left join (select conshop.storeid,sum(invoiceheader.InvActPayAmtl) YearInvPayAmt,invoicedetail.chargetypeid " +
                            "from invoiceheader  " +
                            "inner join invoicedetail on (invoiceheader.invid=invoicedetail.invid) " +
                            "inner join conshop on (invoiceheader.contractid=conshop.contractid) " +
                            "where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) and invoiceheader.invperiod  between cast(year('" + txtPeriod.Text.Trim() + "') as char(4)) + '-1-1' and '" + txtPeriod.Text.Trim() + "' " +
                            "group by conshop.storeid,invoicedetail.chargetypeid) aa " +
                            "on (aa.storeid=c.storeid and aa.chargetypeid=a.chargetypeid) " +
                        "left join chargetype on chargetype.chargetypeid=a.chargetypeid " +
                        "where a.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) and b.invperiod='" + txtPeriod.Text.Trim() + "' " + storesql +
                        "group by budget.monthamt,b.invperiod,c.storeid,aa.YearInvPayAmt,yearbudget.yearbudget,yearbudgettotal.yearbudgettotal,chargetype.chargetypename " +
                        "order by c.storeid,b.invperiod ";

        }
                

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptStoreInvDetail.rpt";

    }

    private void ClearPage()
    {
        txtBizItem.SelectedIndex = 0;
        txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
        this.RadioButton4.Checked = true;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        //if (txtCountMonth.Text != "")
        //{
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        //}
        //else {
        //    return;
        //}
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
