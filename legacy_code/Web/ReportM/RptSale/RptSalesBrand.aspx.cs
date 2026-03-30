using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using Lease.ConShop;
using BaseInfo.User;
using BaseInfo.authUser;
/// <summary>
/// ADD by TJM at 20090324
/// </summary>
public partial class ReportM_RptSale_RptSalesBrand : BasePage
{

    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pageclear();
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_SalesBrand");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }

    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.OrderBy = "OrderID";
        baseBo.WhereClause = "DeptType=" +BaseInfo.Dept.Dept.DEPT_TYPE_MALL ;
        //绑定商业项目
        Resultset rs4 = baseBo.Query(new BaseInfo.Dept.Dept());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (BaseInfo.Dept.Dept  bd in rs4)
            ddlStoreName.Items.Add(new ListItem(bd.DeptName, bd.DeptID.ToString()));
  
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
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_SalesBrand");
        paraField[0].CurrentValues.Add(discreteValue[0]);
        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        //paraField[1] = new ParameterField();
        //paraField[1].Name = "REXBrandName"; //品牌名称
        //discreteValue[1] = new ParameterDiscreteValue();
        //discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandName");
        //paraField[1].CurrentValues.Add(discreteValue[1]);

        //paraField[2] = new ParameterField();
        //paraField[2].Name = "REXMonthSellCount"; //当月销售额
        //discreteValue[2] = new ParameterDiscreteValue();
        //discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthSellCount");
        //paraField[2].CurrentValues.Add(discreteValue[2]);

        //paraField[3] = new ParameterField();
        //paraField[3].Name = "REXyearMPaidAmt"; //同比销售额
        //discreteValue[3] = new ParameterDiscreteValue();
        //discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMPaidAmt");
        //paraField[3].CurrentValues.Add(discreteValue[3]);

        //paraField[4] = new ParameterField();
        //paraField[4].Name = "REXoldMPaidAmt";//环比销售额
        //discreteValue[4] = new ParameterDiscreteValue();
        //discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearMPaidAmt");
        //paraField[4].CurrentValues.Add(discreteValue[4]);

        //paraField[5] = new ParameterField();
        //paraField[5].Name = "REXYearLYRate";//环比增长率
        //discreteValue[5] = new ParameterDiscreteValue();
        //discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_YearLYRate");
        //paraField[5].CurrentValues.Add(discreteValue[5]);

        //paraField[6] = new ParameterField();
        //paraField[6].Name = "REXOldPPRate";//同比增长率
        //discreteValue[6] = new ParameterDiscreteValue();
        //discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_OldPPRate");
        //paraField[6].CurrentValues.Add(discreteValue[6]);

        //paraField[7] = new ParameterField();
        //paraField[7].Name = "REXMallTitle";
        //discreteValue[7] = new ParameterDiscreteValue();
        //discreteValue[7].Value = Session["MallTitle"].ToString();
        //paraField[7].CurrentValues.Add(discreteValue[7]);

        //paraField[8] = new ParameterField();
        //paraField[8].Name = "REXKeepAccountsMth";//月份
        //discreteValue[8] = new ParameterDiscreteValue();
        //discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");
        //paraField[8].CurrentValues.Add(discreteValue[8]);

        //paraField[9] = new ParameterField();
        //paraField[9].Name = "REXTotal";//总计
        //discreteValue[9] = new ParameterDiscreteValue();
        //discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");
        //paraField[9].CurrentValues.Add(discreteValue[9]);

        //paraField[10] = new ParameterField();
        //paraField[10].Name = "REXStoreDesc";//项目名称
        //discreteValue[10] = new ParameterDiscreteValue();
        //discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
        //paraField[10].CurrentValues.Add(discreteValue[10]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string strWhere = "";
        if (ddlStoreName.Text != "")
        {
            strWhere = strWhere + " AND transshopmth.storeID = '" + ddlStoreName.SelectedValue + "'";
        }
        if (ddlYear.Text != "")
        {
            strWhere += " AND (year(transshopmth.month)='" + ddlYear.Text + "' or year(dateadd(year,1,transshopmth.month))='" + ddlYear.Text + "') ";
        }


        string str_sql = "select dept.deptname storename,aa.brandname,cast(year(aa.month) as char(4)) +'年' year,sum(aa.saleamt) totalamt," +
                        "sum(case when month(aa.month)=1 then aa.saleamt else 0 end) one," +
                        "sum(case when month(aa.month)=2 then aa.saleamt else 0 end) two," +
                        "sum(case when month(aa.month)=3 then aa.saleamt else 0 end) three," +
                        "sum(case when month(aa.month)=4 then aa.saleamt else 0 end) four," +
                        "sum(case when month(aa.month)=5 then aa.saleamt else 0 end) five," +
                        "sum(case when month(aa.month)=6 then aa.saleamt else 0 end) six," +
                        "sum(case when month(aa.month)=7 then aa.saleamt else 0 end) seven," +
                        "sum(case when month(aa.month)=8 then aa.saleamt else 0 end) eight," +
                        "sum(case when month(aa.month)=9 then aa.saleamt else 0 end) nine," +
                        "sum(case when month(aa.month)=10 then aa.saleamt else 0 end) ten," +
                        "sum(case when month(aa.month)=11 then aa.saleamt else 0 end) ele," +
                        "sum(case when month(aa.month)=12 then aa.saleamt else 0 end) twn" +
                        " from (" +
                            "select transshopmth.storeid,transshopmth.brandname,transshopmth.month,sum(transshopmth.paidamt) SaleAmt" +
                            " from transshopmth inner join deptbrand on (deptbrand.deptid=transshopmth.storeid and transshopmth.brandid=deptbrand.brandid)" +
                            " where 1=1" + strWhere +
                            " group by transshopmth.storeid,transshopmth.brandname,transshopmth.month" +
                        ") aa inner join dept on (aa.storeid=dept.deptid)" +
                        " group by dept.deptid,dept.deptname,aa.brandname,year(aa.month)" +
                        " order by dept.deptid,brandname,year";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\SalesBrandNew.rpt";

    }

    protected void pageclear()
    {
        ddlStoreName.Text = "";
        RB1.Checked = true;
        RB2.Checked = false;

        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        pageclear();

    }

}
