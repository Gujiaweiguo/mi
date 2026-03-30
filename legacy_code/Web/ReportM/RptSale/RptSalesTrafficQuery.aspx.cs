using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class RptBaseMenu_RptSalesTrafficQuery : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptSalesTrafficQuery_Title");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }


    /* 判断数据空值,返回默认值
     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 判断日期空值,返回默认值
     * 
     * 
     */
    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
    }
    /* 初始化下拉列表
     * 
     * 
     */
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
       // ddlBizproject.Items.Add(new ListItem("", ""));
        this.ddlBizproject.Items.Clear();
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

    }
    private void InitDDL()
    {
        txtsDate.Text  = DateTime.Now.ToString("yyyy-MM-dd");
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
        ParameterField[] paraField = new ParameterField[8];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesTrafficQuery_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTotalSales";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvSalesAmt");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXSdate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXQDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = Convert.ToDateTime(txtsDate.Text).ToLongDateString().ToString();
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMallTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = Session["MallTitle"].ToString();
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXBizProject";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[5].CurrentValues.Add(discreteValue[5]);


        paraField[6] = new ParameterField();
        paraField[6].Name = "RexTraffic";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "RptSalesTraffic_Traffic");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "RexReciptTraffic";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "RptSalesTraffic_ReciptTraffic");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }       
        
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        string strWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";

            for (int i = 0; i < 5; i++)
            {
                strAuth = strAuth.Replace("ConShop", "TransSku");
            }
        }

        if (txtsDate.Text != "")
        {
            strWhere = " AND TransSku.Bizdate = '" + txtsDate.Text + "'";
        }
        if (RB1.Checked)
        {
            strWhere += "";
        }
        if (RB2.Checked)
        {
            strWhere += " AND TransSku.datasource=1";
        }
        if (RB3.Checked)
        {
            strWhere += " AND TransSku.datasource=2";
        }
        if (RB4.Checked)
        {
            strWhere += " AND TransSku.datasource=3";
        }
        if (ddlBizproject.Text != "")
        {
            strWhere += " AND store.storeid=" + ddlBizproject.SelectedValue ;
        }


        //string str_sql = "select TransSku.storeid,Store.storeName, TransSku.Trade2Name,FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3) transTime, " +
        //        "sum(PaidAmt) PAmt from  TransSku " +
        //        "Inner join Store on (store.storeid=TransSku.storeid) " +
        //        "where 1=1" + strWhere + strAuth + strWhere1 +
        //        " group by TransSku.Trade2Name, TransSku.storeid,Store.storeName,FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3)";

        string str_sql = "select TransSku.storeid,Store.storeName, RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3) transTime," +
                         "sum(PaidAmt) PAmt,isnull(aa.InNum,0) InNum,isnull(aa.OutNum,0) OutNum,isnull(aa.AtNum,0) AtNum,bb.totalrecipt from TransSku " +
                         "left join (select storeid,bizdate,hour,Sum(InNum) InNum,sum(OutNum) OutNum,sum(InNum) AtNum from trafficdata group by storeid,bizdate,hour) as aa " +
                                "on (transsku.storeid=aa.storeid and aa.bizdate=transsku.bizdate and datepart(hh,transsku.transtime)=aa.hour) " +
                         "inner join (select storeid,bizdate, datepart(hh,transtime) hour,count(distinct receiptid) totalrecipt from transsku group by storeid,bizdate,datepart(hh,transtime)) as bb " +
                                "on (bb.storeid=transsku.storeid and bb.bizdate=transsku.bizdate and datepart(hh,transsku.transtime)=bb.hour) " +
                         "Inner join Store on (store.storeid=TransSku.storeid) " +
                         "where 1=1 " + strWhere + strAuth +
                         " group by TransSku.storeid,Store.storeName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3),aa.InNum,aa.OutNum,aa.AtNum,bb.totalrecipt";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptSalesTrafficQuery.rpt";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptSalesTrafficQuery.aspx");
    }
}
