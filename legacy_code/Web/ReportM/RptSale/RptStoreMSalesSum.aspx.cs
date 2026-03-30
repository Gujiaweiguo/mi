using System;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using CrystalDecisions.Shared;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptSale_RptStoreMSalesSum : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindStore();
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
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
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.Text = "";
        txtPeriod.Text = DateTime.Now.ToShortDateString();
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
        ParameterField[] paraField = new ParameterField[10];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[10];
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
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMPaidAmt");//环比销售额
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
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearMPaidAmt");//同比销售额
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXyearRate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");//同比差异
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXStoreMSalesSum";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreMSalesSum");//项目月销售额汇总表
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "RexStrDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SelectMonth");//查询月
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "RexDate";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = this.txtPeriod.Text;
        paraField[9].CurrentValues.Add(discreteValue[9]);

   

        string str_sql = "select storeid,storename storeshortname,sum(paidamt) mpaidamt,sum(pppaidamt) oldmpaidamt,sum(lypaidamt) yearmpaidamt, " +
                        "(sum(paidamt)-sum(pppaidamt))/sum(pppaidamt) oldMRate,(sum(paidamt)-sum(lypaidamt))/sum(lypaidamt) yearRate" +
                        " from transshopmth where 1=1 ";

        if (this.ddlStoreName.SelectedItem.Text.Trim() != "")
        {
            str_sql = str_sql + " and transshopmth.StoreID = " + this.ddlStoreName.SelectedValue.ToString();
        }


        if (this.txtPeriod.Text.Trim() != "")
        {
            str_sql = str_sql + " and transshopmth.month = '" + this.txtPeriod.Text.Trim() + "'";
        }
        else
        {
            str_sql = str_sql + " and transshopmth.month='" + DateTime.Now.ToString("yyyy-MM-01") + "'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            string strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            for (int i = 0; i < 5; i++)
            {
                //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
                strAuth = strAuth.Replace("ConShop", "transshopmth");
            }
            str_sql = str_sql + strAuth;
        }
        str_sql = str_sql + " group by storeid,storename";

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql ;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreMSalesSum.rpt";
    }
}
