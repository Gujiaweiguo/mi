using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;

public partial class ReportM_RptTraf_RptTrafficAccumAve : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficAccumAve");
        }
    }
    /// <summary>
    /// 绑定ddl
    /// </summary>
    protected void BindStore()
    {
        //绑定项目
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = objBaseBo.Query(new Store());
        //this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));

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
        this.Response.Redirect("RptTrafficMonth.aspx");
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
        ParameterField[] paraField = new ParameterField[8];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTrafficAccumAve";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficAccumAve");//客流量分析报告
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTransDate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SelectMonth");//查询月份
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXYear";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = ddlYear.SelectedValue.ToString() + "年";
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMonth";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");//月份
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXOldPPRate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_OldPPRate");//同比增长率
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXAverage";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Average");//平均
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXLYear";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (Int32.Parse(ddlYear.SelectedValue) - 1).ToString() + "年";
        paraField[7].CurrentValues.Add(discreteValue[7]);


        //string str_sql = @"select a.storeid,store.StoreShortName,month(a.bizdate) as bizmonth,sum(a.innum) lyinnum,AA.tyinnum " +
        //                    "from trafficdata a " +
        //                    "left join store on store.storeid=a.storeid " +
        //                    "left join (select storeid,month(bizdate) month,sum(innum) tyinnum from trafficdata where year(bizdate)='" + ddlYear.Text + "' " +
        //                        "group by storeid,month(bizdate)) as AA on a.storeid=AA.storeid and AA.month=month(a.bizdate) " +
        //                    "where year(a.bizdate)='" + (Int32.Parse(DateTime.Parse(txtPeriod.Text).Year.ToString()) - 1) + "' ";

        string str_sql = "select d.storeid,store.StoreShortName,Month as bizmonth,sum(Lyinnum) as lyinnum ,sum(Tyinnum) as tyinnum " +
                        "from (select storeid,month(bizdate) as  Month,'' as Tyinnum,sum(innum) Lyinnum " +
                                    "from trafficdata where year(bizdate)='" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "' " +
                                    "group by storeid,month(bizdate) " +
                            "union all " +
                            "select storeid,month(bizdate) Month,sum(innum) Tyinnum,'' as  Lyinnum " +
                                    "from trafficdata where year(bizdate)='" + ddlYear.SelectedValue + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "' " +
                                    "group by storeid,month(bizdate)) d " +
                        "left join store on store.storeid=d.storeid " +
                        "group by d.storeid,Month,store.StoreShortName ";


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficAccumAve.rpt";

    }
}