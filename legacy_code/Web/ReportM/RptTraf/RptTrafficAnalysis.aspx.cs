using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;

public partial class ReportM_RptTraf_RptTrafficAnalysis : BasePage
{
    public string strBaseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            BindStore();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficAnalysis");
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    protected void BindStore()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "DeptType=" + BaseInfo.Dept.Dept.DEPT_TYPE_MALL;
        objBaseBo.OrderBy = "OrderID";
        Resultset rs = objBaseBo.Query(new BaseInfo.Dept.Dept());
        this.ddlStoreName.Items.Clear();
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (BaseInfo.Dept.Dept  bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.DeptName, bd.DeptID.ToString()));


        //绑定年份
        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("RptTrafficAnalysis.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (ddlYear.SelectedValue.ToString() != "")
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
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficAnalysis");//客流量分析报告
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string strWhere = "";
        if (this.ddlStoreName.Text != "")
        {
            strWhere += " And trafficdata.StoreId=" + ddlStoreName.SelectedValue.ToString();
        }
        if (this.ddlYear.Text.Trim() != "")
        {
            strWhere += " And (year(trafficdata.bizdate)=" + Convert.ToInt32(this.ddlYear.Text.Trim()).ToString() + " or year(dateadd(year,1,trafficdata.bizdate))=" + Convert.ToInt32(this.ddlYear.Text.Trim()).ToString() + ")";
        }

        string str_sql = "select dept.deptname storename,cast(aa.year as char(4)) + '年' year,sum(aa.TotalTra) TotalAmt," +
                       "sum(case when aa.month=1 then aa.TotalTra else 0 end) one," +
                       "sum(case when aa.month=2 then aa.TotalTra else 0 end) two," +
                       "sum(case when aa.month=3 then aa.TotalTra else 0 end) three," +
                       "sum(case when aa.month=4 then aa.TotalTra else 0 end) four," +
                       "sum(case when aa.month=5 then aa.TotalTra else 0 end) five," +
                       "sum(case when aa.month=6 then aa.TotalTra else 0 end) six," +
                       "sum(case when aa.month=7 then aa.TotalTra else 0 end) seven," +
                       "sum(case when aa.month=8 then aa.TotalTra else 0 end) eight," +
                       "sum(case when aa.month=9 then aa.TotalTra else 0 end) nine," +
                       "sum(case when aa.month=10 then aa.TotalTra else 0 end) ten," +
                       "sum(case when aa.month=11 then aa.TotalTra else 0 end) ele," +
                       "sum(case when aa.month=12 then aa.TotalTra else 0 end) twn" +
                       " from (" +
                           "select trafficdata.storeid,year(trafficdata.bizdate) year,month(trafficdata.bizdate) month,sum(trafficdata.InNum) TotalTra" +
                           " from trafficdata where 1=1 " + strWhere +
                           " group by trafficdata.storeid,year(trafficdata.bizdate),month(trafficdata.bizdate)" +
                       ") aa inner join dept on (aa.storeid=dept.deptid)" +
                       " group by aa.storeid,dept.deptname,cast(aa.year as char(4)) + '年'" +
                       "order by storeid,year";


        //string Weekend = WeekendStr(Int32.Parse(ddlYear.SelectedValue) - 1);         

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;

        if (rbtDetail.Checked == true)
        {
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficAnalysis2.rpt";
        }
        else
        {
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficAnalysisSum.rpt";
        }
        //Session["subParaFil"] = paraFields;
        //Session["subReportSql"] = str_sql1;
        //Session["subRpt"] = "RptTrafficA";  
    }

    private string WeekendStr(int year)
    {
        int DayNum;
        DateTime dt = DateTime.Parse(year.ToString() + "-01" + "-01");
        string WeekEndStr = "";
        if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
        {
            DayNum = 366 + 365;
        }
        else
        {
            if ((year + 1) % 4 == 0 && (year + 1) % 100 != 0 || (year + 1) % 400 == 0)
            {
                DayNum = 365 + 366;
            }
            else
            {
                DayNum = 365 + 365;
            }
        }
        for (int i = 1; i <= DayNum; i++)
        {
            switch (dt.DayOfWeek)
            {
                case System.DayOfWeek.Saturday:
                case System.DayOfWeek.Sunday:
                    WeekEndStr += "'" + dt.ToString("yyyy-MM-dd") + "',";
                    break;
            }
            dt = dt.AddDays(1);
        }
        WeekEndStr = WeekEndStr.TrimEnd(',');
        return WeekEndStr;
    }
}