using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Page;

public partial class ReportM_RptInv_RptCompanyBudgetIncomeSum : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_CompanyBudgetIncomeSum");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void InitDDL()
    {
        int intMonth = 12;
        ddlMonth.Items.Clear();
        ddlMonth.Items.Add(new ListItem("", ""));
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        int year = Convert.ToInt16(DateTime.Now.Year);
        //  ddlYear.Items.Add(new ListItem("",""));
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
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
        InitDDL();
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[8];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_CompanyBudgetIncomeSum");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXHaveDone"; //执行情况
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UntilMonthTotal").ToString().Substring(6);
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXBudget"; //预算情况
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthCount").ToString().Substring(4);
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXBudgetCountVar"; //与预算差异
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_BudgetCountVar");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXYearHaveDone";//截至该月本年执行情况
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UntilMonthTotal");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXYearBudget";//截至本月年度预算情况
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_EndMonthCount");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        //paraField[6] = new ParameterField();
        //paraField[6].Name = "REXOldPPRate";//同比增长率
        //discreteValue[6] = new ParameterDiscreteValue();
        //discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_OldPPRate");
        //paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXMallTitle";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = Session["MallTitle"].ToString();
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXKeepAccountsMth";//月份
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        //paraField[9] = new ParameterField();
        //paraField[9].Name = "REXTotal";//总计
        //discreteValue[9] = new ParameterDiscreteValue();
        //discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");
        //paraField[9].CurrentValues.Add(discreteValue[9]);

        //paraField[10] = new ParameterField();
        //paraField[10].Name = "RexStoreName";//项目名称
        //discreteValue[10] = new ParameterDiscreteValue();
        //discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
        //paraField[10].CurrentValues.Add(discreteValue[10]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        string wheresql = "";

        if (ddlYear.Text != "" && ddlMonth.Text != "")
        {
            wheresql = " AND invoicedetail.period ='" + ddlYear.Text + "-" + ddlMonth.Text + "-1' ";
        }
        else if (ddlYear.Text != "")
        {
            wheresql = "AND year(invoicedetail.period)=" + Convert.ToInt32(ddlYear.Text);
        }

        str_sql = "select invoicedetail.period, sum(invoicedetail.invPayAmtL) invPayAmt," +
                    "(select sum(invPayAmtL) from invoicedetail where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) " +
                    "and year(invoicedetail.period)=" + Convert.ToInt32(ddlYear.Text) + ") yearinvPayAmt," +
                    "(select sum(monthamt) from budget where budgetyear=" + Convert.ToInt32(ddlYear.Text) + ") monthbudgetamt," +
                    "(select sum(monthamt) * month(invoicedetail.period) from budget where budgetyear=" + Convert.ToInt32(ddlYear.Text) + ") yearbudgetamt " +
                    "from invoicedetail " +
                    "where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass in (1,3)) " + wheresql +
                    " group by invoicedetail.period  order by invoicedetail.period";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptCompanyBudgetIncomeSum.rpt";

    }
}
