using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;

public partial class ReportM_RptTraf_RptTrafficMonth : BasePage
{
    public string strBaseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh= (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            BindStore();
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficMonth");
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    protected void BindStore()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = objBaseBo.Query(new Store());
        //this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("RptTrafficMonth.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtPeriod.Text != "")
        {
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            Session["subReportSql1"] = "";
            Session["subRpt1"] = "";
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        }
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[4];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[4];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTrafficMonth";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficMonth");//客流量分析报告
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTransDate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SelectMonth");//查询月份
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = txtPeriod.Text;
        paraField[3].CurrentValues.Add(discreteValue[3]);

        string str_sql = @"select a.storeid,store.StoreShortName,substring(convert(varchar(10),a.bizdate,120),9,2)+'号' bizdate,sum(a.innum) innum " +
                            "from trafficdata a " +
                            "left join store on store.storeid=a.storeid " +
                            "where convert(varchar(7),a.bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' ";

        string str_sql1 = @"select a.storeid,store.StoreShortName,substring(convert(varchar(10),a.bizdate,120),9,2)+'号' bizdate,a.counterid,b.name,sum(innum) innum,sum(a.outnum) outnum
                            from trafficdata a,trafficcounter b,store
                            where convert(varchar(7),a.bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' and a.counterid=b.counterid and a.storeid=store.storeid ";


        string str_sql2 = "select 1 id,'最繁忙入口' name,* " +
                    "from (" +
                    "select top 1 b.name date,sum(a.innum) innum " +
                    "from trafficdata a " +
                    "left join trafficcounter b on a.counterid=b.counterid " +
                    "where convert(varchar(7),a.bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' and a.storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  " +
                    "group by a.counterid,b.name " +
                    "order by innum desc) a " +
                    "union all " +
                    "select 1 id,'最繁忙日期' name,* " +
                    "from (" +
                    "select top 1 convert(varchar(10),bizdate,120) date,sum(innum) innum " +
                    "from trafficdata " +
                    "where convert(varchar(7),bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  " +
                    "group by bizdate " +
                    "order by innum desc) b " +
                    "union all " +
                    "select 1 id,'最繁忙星期' name,* " +
                    "from ( " +
                    "select top 1 convert(varchar(10),dateadd(day,-datepart(weekday,bizdate)+1,bizdate),120)+'--'+convert(varchar(10),dateadd(day,7-datepart(weekday,bizdate),bizdate),120) date,sum(innum) innum " +
                    "from trafficdata " +
                    "where storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  and convert(varchar(7),bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' " +
                    "group by DATEPART (week,bizdate),dateadd(day,-datepart(weekday,bizdate)+1,bizdate),dateadd(day,7-datepart(weekday,bizdate),bizdate) " +
                    "order by innum desc) c " +
                    "union all " +
                    "select 2 id,'最繁忙入口' name,* " +
                    "from ( " +
                    "select top 1 b.name date,sum(a.innum) innum " +
                    "from trafficdata a " +
                    "left join trafficcounter b on a.counterid=b.counterid " +
                    "where year(a.bizdate)='" + DateTime.Parse(txtPeriod.Text).Year + "' and month(a.bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' and a.storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'   " +
                    "group by a.counterid,b.name " +
                    "order by innum desc) a " +
                    "union all " +
                    "select 2 id,'最繁忙日期' name,* " +
                    "from ( " +
                    "select top 1 convert(varchar(10),bizdate,120) date,sum(innum) innum " +
                    "from trafficdata " +
                    "where year(bizdate)='" + DateTime.Parse(txtPeriod.Text).Year + "' and month(bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  " +
                    "group by bizdate " +
                    "order by innum desc) b " +
                    "union all " +
                    "select 2 id,'最繁忙星期' name,* " +
                    "from ( " +
                    "select top 1 convert(varchar(10),dateadd(day,-datepart(weekday,bizdate)+1,bizdate),120)+'--'+convert(varchar(10),dateadd(day,7-datepart(weekday,bizdate),bizdate),120) date,sum(innum) innum " +
                    "from trafficdata " +
                    "where storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  and year(bizdate)='" + DateTime.Parse(txtPeriod.Text).Year + "' and month(bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' " +
                    "group by DATEPART (week,bizdate),dateadd(day,-datepart(weekday,bizdate)+1,bizdate),dateadd(day,7-datepart(weekday,bizdate),bizdate) " +
                    "order by innum desc) c " +
                    "union all " +
                    "select 2 id,'最繁忙月' name,* " +
                    "from ( " +
                    "select top 1 convert(varchar(7),bizdate,120) date,sum(innum) innum " +
                    "from trafficdata " +
                    "where year(bizdate)='" + DateTime.Parse(txtPeriod.Text).Year + "' and month(bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  " +
                    "group by month(bizdate),convert(varchar(7),bizdate,120) " +
                    "order by innum desc) d ";



        if (this.ddlStoreName.SelectedItem.Text.Trim() != "")
        {
            str_sql = str_sql + " and a.storeid = " + this.ddlStoreName.SelectedValue.ToString();
            str_sql1 = str_sql1 + " and a.storeid = " + this.ddlStoreName.SelectedValue.ToString();
        }

        str_sql = str_sql + " group by a.storeid,a.bizdate,store.StoreShortName "+
	                        " order by a.storeid ";
        str_sql1 = str_sql1 + " group by a.storeid,bizdate,a.counterid,b.name,store.StoreShortName ";

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficMonth.rpt";

        Session["subParaFil"] = paraFields;
        Session["subReportSql"] = str_sql1;
        Session["subRpt"] = "RptTrafficMO";

        Session["subParaFil1"] = paraFields;
        Session["subReportSql1"] = str_sql2;
        Session["subRpt1"] = "RptTrafficMOMax"; 
    }
}