using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;

public partial class ReportM_RptTraf_RptTrafficDirection : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficDirection");
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    protected void BindStore()
    {
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
        this.Response.Redirect("RptTrafficDirection.aspx");
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
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select counterid,name from trafficcounter");
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTrafficDirection";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficDirection");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].ParameterFieldName = "REXBusinessItem";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].ParameterFieldName = "REXMonth";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].ParameterFieldName = "REXTotal";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "RptSalesTraffic_Traffic");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].ParameterFieldName = "REXName1";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = ds.Tables[0].Rows[0][1].ToString();
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].ParameterFieldName = "REXName2";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = ds.Tables[0].Rows[1][1].ToString();
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].ParameterFieldName = "REXName3";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = ds.Tables[0].Rows[2][1].ToString();
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].ParameterFieldName = "REXName4";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = ds.Tables[0].Rows[3][1].ToString();
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXYear";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = ddlYear.SelectedValue.ToString() + "年";
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXLYear";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (Int32.Parse(ddlYear.SelectedValue) - 1).ToString() + "年";
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].ParameterFieldName = "REXName5";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = ds.Tables[0].Rows[4][1].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].ParameterFieldName = "REXName6";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = ds.Tables[0].Rows[5][1].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);


        string str_sql = "select a.storeid,store.StoreShortName,convert(varchar(10),year)+'年' year,month,sum(innum) innum,sum(innum1) innum1,sum(innum2) innum2,sum(innum3) innum3,sum(innum4) innum4,sum(innum5) innum5 " +
                            "from ( " +
                            "select storeid,year(bizdate) year,month(bizdate) month,sum(innum) innum,'' innum1,'' innum2,'' innum3,'' innum4,'' innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[0][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate) " +
                            "union all " +
                            "select storeid,year(bizdate) year,month(bizdate) month,'' innum,sum(innum) innum1,'' innum2,'' innum3,'' innum4,'' innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[1][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate) " +
                            "union all " +
                            "select storeid,year(bizdate) year,month(bizdate) month,'' innum,'' innum1,sum(innum) innum2,'' innum3,'' innum4,'' innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[2][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate) " +
                            "union all " +
                            "select storeid,year(bizdate) year,month(bizdate) month,'' innum,'' innum1,'' innum2,sum(innum) innum3,'' innum4,'' innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[3][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate) "+
                            "union all " +
                            "select storeid,year(bizdate) year,month(bizdate) month,'' innum,'' innum1,'' innum2,'' innum3,sum(innum) innum4,'' innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[4][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate) "+
                            "union all " +
                            "select storeid,year(bizdate) year,month(bizdate) month,'' innum,'' innum1,'' innum2,'' innum3,'' innum4,sum(innum) innum5 " +
                                        "from trafficdata " +
                                        "where year(bizdate) in ('" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "','" + ddlYear.SelectedValue + "') and counterid='" + ds.Tables[0].Rows[5][0].ToString() + "' " +
                                        "group by storeid,year(bizdate),month(bizdate)) a " +
                            "left join store on a.storeid=store.storeid " +
                            "where a.storeid='" + this.ddlStoreName.SelectedValue.ToString() + "'  " +
                            "group by a.storeid,year,month,store.StoreShortName ";

        string str_sql1 = "select d.storeid,Month,d.counterid,a.name,sum(Tyinnum) as Tyinnum,sum(Lyinnum) as Lyinnum " +
                            "from (select storeid,month(bizdate) as  Month,counterid,'' as Tyinnum,sum(innum) Lyinnum " +
                                        "from trafficdata where year(bizdate)='" + (Int32.Parse(ddlYear.SelectedValue) - 1) + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "' " +
                                        "group by storeid,month(bizdate),counterid " +
                                "union all " +
                                "select storeid,month(bizdate) Month,counterid,sum(innum) Tyinnum,'' as  Lyinnum " +
                                        "from trafficdata where year(bizdate)='" + ddlYear.SelectedValue + "' and storeid='" + this.ddlStoreName.SelectedValue.ToString() + "' " +
                                        "group by storeid,month(bizdate),counterid) d " +
                            "left join trafficcounter a on d.counterid=a.counterid "+
                            "group by d.storeid,Month,d.counterid,a.name ";


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficDirection.rpt";

        Session["subParaFil"] = paraFields;
        Session["subReportSql"] = str_sql1;
        Session["subRpt"] = "RptTrafficD";  
    }
}