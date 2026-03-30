using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using BaseInfo.Dept;

public partial class RptBaseMenu_RptFloatSaleQuery : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Title");
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
       // BaseBO baseBo = new BaseBO();
       // baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
       // baseBo.OrderBy = "orderid";
       // Resultset rs = baseBo.Query(new Dept());
       //// ddlBizproject.Items.Add(new ListItem("", ""));
       // this.ddlBizproject.Items.Clear();
       // foreach (Dept store in rs)
       // {
       //     ddlBizproject.Items.Add(new ListItem(store.DeptName, store.DeptID.ToString()));
       // }
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "StoreStatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Clear();
        foreach (Store objStore in rs)
        {
            ddlBizproject.Items.Add(new ListItem(objStore.StoreName, objStore.StoreId.ToString()));
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

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_lblFloatSaleQuery";
        s += "%23" + "Rpt_Salevalue";
        s += "%23" + "Rpt_lblFloorSaleRate";
        s += "%23" + "Rpt_lblTradeSaleRate";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26sDate=" + GetdateNull(this.txtsDate.Text);
        return sCon;
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[6];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[6];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Title");
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

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string strWhere = "";
        string strWhere1 = "";

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
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
            strWhere1 = "";
        }
        if (RB2.Checked)
        {
            strWhere1 = "AND TransSku.datasource=1";
        }
        if (RB3.Checked)
        {
            strWhere1 = "AND TransSku.datasource=2";
        }
        if (RB4.Checked)
        {
            strWhere1 = "AND TransSku.datasource=3";
        }
        if (ddlBizproject.Text != "")
        {
            strWhere1 += "AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }


        string str_sql = "select TransSku.storeid,Store.storeshortname storeName, TransSku.Trade2Name,FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3) transTime, " +
                "sum(PaidAmt) PAmt from  TransSku " +
                "Inner join Store on (store.storeid=TransSku.storeid) " +
                "where 1=1" + strWhere + strAuth + strWhere1 +
                " group by TransSku.Trade2Name, TransSku.storeid,Store.storeshortname,FloorName,RIGHT('0' + rtrim(convert(char(2),datepart(hh,transtime)))+'h',3)";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\FloatSaleQuery.rpt";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptFloatSaleQuery.aspx");
    }
}
