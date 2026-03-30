using System;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using CrystalDecisions.Shared;
using Shop.ShopType;

public partial class ReportM_RptSale_RptStoreMSalesSum : BasePage
{
    public string strBaseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindStore();
            BindShopType();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UnitTypeSalesSum");
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    protected void BindStore()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = objBaseBo.Query(new Store());
        this.ddlStoreName.Items.Clear();
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));

        int intMonth = 12;
        ddlMonth.Items.Clear();
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    /// <summary>
    /// 绑定商铺类型
    /// </summary>
    private void BindShopType()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = objBaseBo.Query(new ShopType());
        this.ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType bd in rs)
            this.ddlShopType.Items.Add(new ListItem(bd.ShopTypeName, bd.ShopTypeID.ToString()));
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.Text = "";
        this.ddlShopType.Text = "";
        this.ddlYear.Text = DateTime.Now.Year.ToString();
        this.ddlMonth.Text = "";
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (this.ddlYear.Text != "")
        {
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        }
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXBusinessItem";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");//商业项目
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMonthSellCount";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthSellCount");//当月销售额
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXoldMPaidAmt";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMPaidAmt");//同比销售额
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXoldMRate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMRate");//同比差异
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMallTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = Session["MallTitle"].ToString();
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXyearMPaidAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearMPaidAmt");//环比销售额
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXyearRate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");//环比差异
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXUnitTypeSalesSum";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UnitTypeSalesSum");//项目月销售额汇总表
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXShopTypeName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");//商铺类型
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "RexQDate";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = "销售月份";
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "RexSdate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = this.ddlYear.Text + "年" + this.ddlMonth.Text + "月";
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "RexYearAmtRate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_YearAmtRate");//年度累计差异
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "RexLyYearPaidAmt";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblLyYearPaidAmt");//截至该月本年累计情况
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "RexYearPaidAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblYearPaidAmt");//截至该月去年累计情况
        paraField[13].CurrentValues.Add(discreteValue[13]);

        string str_sql = "select storeid,storename storeshortname,shoptypename,sum(paidamt) mpaidamt," +
                        "(select isnull(sum(paidamt),0) from transshopmth aa where aa.storeid=transshopmth.storeid and aa.shoptypename=transshopmth.shoptypename and aa.month=dateadd(month,-1,transshopmth.month)) oldmpaidamt," +
                        "(select isnull(sum(paidamt),0) from transshopmth aa where aa.storeid=transshopmth.storeid and aa.shoptypename=transshopmth.shoptypename and aa.month=dateadd(year,-1,transshopmth.month)) yearmpaidamt, " +
                        "(select isnull(sum(a.paidamt),0) from transshopmth a where a.storeid=transshopmth.storeid and a.shoptypename=transshopmth.shoptypename" +
                            " and a.month between cast(year(transshopmth.month) as char(4)) and transshopmth.month) as yearpaidamt," +   //年度累计
                        "(select isnull(sum(b.paidamt),0) from transshopmth b where b.storeid=transshopmth.storeid and b.shoptypename=transshopmth.shoptypename" +
                            " and b.month between cast(year(dateadd(year,-1,transshopmth.month)) as char(4)) and  dateadd(year,-1,transshopmth.month)) as lyyearpaidamt" +  //同比年度累计
                        " from transshopmth where 1=1 ";

        if (this.ddlStoreName.SelectedItem.Text.Trim() != "")
        {
            str_sql += " and transshopmth.storeid=" + this.ddlStoreName.SelectedValue.ToString();
        }

        if (this.ddlShopType.SelectedItem.Text.Trim() != "")
        {
            str_sql += " and transshopmth.shoptypeid=" + this.ddlShopType.SelectedValue.ToString() ;
        }

        if (this.ddlYear.Text.Trim() !="" && this.ddlMonth.Text.Trim() !="")
        {
            str_sql += " and transshopmth.month = '" + this.ddlYear.Text.Trim() + "-" + this.ddlMonth.Text.Trim() + "-01'";
        }
 
        str_sql += " group by storeid,storename,shoptypename,month";

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] =str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptUnitTypeSalesSum.rpt";
    }
}
