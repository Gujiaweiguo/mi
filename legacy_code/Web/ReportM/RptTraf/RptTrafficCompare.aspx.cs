using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
public partial class ReportM_RptTraf_RptTrafficCompare : BasePage
{
    public string strBaseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            BindStore();
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficCompare");
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
        this.Response.Redirect("RptTrafficCompare.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtPeriod.Text != "")
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
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXBusinessItem";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");//商业项目
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMonthSellCount";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = txtPeriod.Text.Substring(0, 7);
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXoldMPaidAmt";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = DateTime.Parse(txtPeriod.Text).AddMonths(-1).ToString("yyyy-MM-dd").Substring(0, 7);
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXoldMRate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMRate");//环比差异
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMallTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = Session["MallTitle"].ToString();
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXyearMPaidAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = DateTime.Parse(txtPeriod.Text).AddYears(-1).ToString("yyyy-MM-dd").Substring(0, 7);
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXyearRate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");//同比差异
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXTrafficCompare";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_TrafficCompare");//客流量比较分析报表
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXStrDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SelectMonth");//查询月
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXDate";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = txtPeriod.Text;
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXUntilMonthTotalLY";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UntilMonthTotalLY");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXUntilMonthTotal";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_UntilMonthTotal");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXRate";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Rate");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        string str_sql = @"SELECT trafficdata.storeid,store.StoreShortName,AA.innum,BB.lminnum,CC.lyinnum,DD.total,EE.lytotal " +
                            "FROM trafficdata " +
                            "left JOIN " +
                            "(select storeid,sum(innum) innum from trafficdata where convert(varchar(7),bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + "' group by storeid) AS AA ON (AA.storeid = trafficdata.storeid) " +
                            "left JOIN " +
                            "(select storeid,sum(innum) lminnum from trafficdata where convert(varchar(7),bizdate,120)='" + DateTime.Parse(txtPeriod.Text).AddMonths(-1).ToString("yyyy-MM-dd").Substring(0, 7) + "' group by storeid) AS BB ON (BB.storeid = trafficdata.storeid) " +
                            "left JOIN " +
                            "(select storeid,sum(innum) lyinnum from trafficdata where convert(varchar(7),bizdate,120)='" + DateTime.Parse(txtPeriod.Text).AddYears(-1).ToString("yyyy-MM-dd").Substring(0, 7) + "' group by storeid) AS CC ON (CC.storeid = trafficdata.storeid) " +
                            "left JOIN " +
                            "(select storeid,sum(innum) total from trafficdata where convert(varchar(4),bizdate,120)='" + txtPeriod.Text.Substring(0, 4) + "' and month(bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' group by storeid) AS DD ON (DD.storeid = trafficdata.storeid) " +
                            "left JOIN " +
                            "(select storeid,sum(innum) lytotal from trafficdata where convert(varchar(4),bizdate,120)='" + (Int32.Parse(txtPeriod.Text.Substring(0, 4)) - 1).ToString() + "' and month(bizdate)<='" + DateTime.Parse(txtPeriod.Text).Month + "' group by storeid) AS EE ON (EE.storeid = trafficdata.storeid) " +
                            "left join store on trafficdata.storeid=store.storeid " +
                            "where 1=1 ";

        if (this.ddlStoreName.SelectedItem.Text.Trim() != "")
        {
            str_sql = str_sql + " and trafficdata.StoreID = " + this.ddlStoreName.SelectedValue.ToString();
        }


        //if (this.txtPeriod.Text.Trim() != "")
        //{
        //    str_sql = str_sql + " and transshopmth.month = '" + this.txtPeriod.Text.Trim() + "'";
        //}
        //else
        //{
        //    str_sql = str_sql + " and transshopmth.month='" + DateTime.Now.ToString("yyyy-MM-01") + "'";
        //}

        //SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        //if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        //{
        //    string strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        //    for (int i = 0; i < 5; i++)
        //    {
        //        //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
        //        strAuth = strAuth.Replace("ConShop", "transshopmth");
        //    }
        //    str_sql = str_sql + strAuth;
        //}
        str_sql = str_sql + " group by trafficdata.storeid,store.StoreShortName,AA.innum,BB.lminnum,CC.lyinnum,DD.total,EE.lytotal";

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Traf\\RptTrafficCompare.rpt";
    }
}