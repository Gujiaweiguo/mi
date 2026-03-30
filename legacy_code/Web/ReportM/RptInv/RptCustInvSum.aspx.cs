using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class RptBaseMenu_RptCustInvSum : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblCustInvSum");
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
    /* 初始化下拉列表

     * 
     * 
     */
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

    }
    private void InitDDL()
    {
        this.txtInvEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.txtInvStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.RadioButton4.Checked = true;  //默认权责
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
        String s = "%23Rpt_lblCustInvSum";
        s += "%23" + "LeaseholdContract_labContractCode";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "ConLease_labFastnessHire";
        s += "%23" + "Rpt_ConDeposit";
        s += "%23" + "Rpt_OtherCharge";
        s += "%23" + "Rpt_InvAdjAmt";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26ContractCode=" + GetStrNull(this.txtContractID.Text);
        sCon += "%26CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26" + "InvStartDate=" + GetdateNull(this.txtInvStartDate.Text);
        sCon += "%26" + "InvEndDate=" + GetdateNull(this.txtInvEndDate.Text);

        return sCon;
    }

    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractCode";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXInvPeriod";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXShopName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXAdvAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_AdvAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXPreAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PreAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXWaterInv";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_WaterInv");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXInvAdjAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAdjAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustInvSum_Title");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXFanstAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_FanstAmt");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXDepositAmt";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_DepositAmt");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXBizProject";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXMallTitle";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = Session["MallTitle"].ToString();
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "RexYearAmt";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_YearAmt");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "";
        if (this.RadioButton5.Checked)
        {
            str_sql = " select store.storeid,store.storeshortname,Contract.ContractCode,Customer.CustCode,Customer.CustName," +
                      " InvoiceHeader.InvPeriod,InvoiceHeader.InvAdjAmt,InvoiceHeader.InvID," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //租金类
                              " where InvoiceDetail.InvID=InvoiceHeader.InvID " +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND (ChargeType.ChargeClass = '1')" +
                              " ) as AdvAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //押金类
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID" +
                              " AND ChargeType.ChargeClass = '2'" +
                              " ) as PreAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //每月其他
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '3'" +
                              " ) as FanstAmt," +
                          //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                          //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                          //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                          //   " AND ChargeType.ChargeClass = '4'" +
                          //    " ) as ApportionAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +   //能源类
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '5'" +
                              " ) as waterInv," +
                          //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                          //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                          //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                          //    " AND ChargeType.ChargeClass = '6'" +
                          //    " ) as PredictAnv," +
                          //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                          //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                          //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                          //    " AND ChargeType.ChargeClass = '7'" +
                          //    " ) as MaintainAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +   //年抽成
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '8'" +
                              " ) as YearInv," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '9'" +
                              " ) as OtherAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType" +
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID" +
                              " AND ChargeType.ChargeClass = '11'" +
                              " ) as InterestAmt" +
                      " from InvoiceHeader" +
                      " INNER JOIN Customer on (InvoiceHeader.CustID = Customer.CustID)" +
                      " INNER JOIN InvoiceDetail ON (InvoiceHeader.InvID = InvoiceDetail.InvID)" +
                      " INNER JOIN Contract on (Contract.ContractID = InvoiceHeader.ContractID)" +
                      " INNER JOIN ConShop ON (Contract.ContractID = ConShop.ContractID) inner join store on(conshop.storeid=store.storeid) WHERE 1 =1 ";

        }
        else
        {
            str_sql = " select store.storeid,store.storeshortname,Contract.ContractCode,Customer.CustCode,Customer.CustName," +
                      " InvoiceDetail.period InvPeriod,InvoiceHeader.InvAdjAmt,InvoiceHeader.InvID," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //租金类
                              " where InvoiceDetail.InvID=InvoiceHeader.InvID " +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND (ChargeType.ChargeClass = '1')" +
                              " ) as AdvAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //押金类
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID" +
                              " AND ChargeType.ChargeClass = '2'" +
                              " ) as PreAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +    //每月其他
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '3'" +
                              " ) as FanstAmt," +
                            //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                            //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                            //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                            //   " AND ChargeType.ChargeClass = '4'" +
                            //    " ) as ApportionAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +   //能源类
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '5'" +
                              " ) as waterInv," +
                            //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                            //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                            //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                            //    " AND ChargeType.ChargeClass = '6'" +
                            //    " ) as PredictAnv," +
                            //" (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                            //    " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                            //    " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                            //    " AND ChargeType.ChargeClass = '7'" +
                            //    " ) as MaintainAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +   //年抽成
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '8'" +
                              " ) as YearInv," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType " +
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID " +
                              " AND ChargeType.ChargeClass = '9'" +
                              " ) as OtherAmt," +
                          " (select SUM(ISNULL(InvoiceDetail.InvPaidAmt,0)) from InvoiceDetail,ChargeType" +
                              " where InvoiceHeader.InvID = InvoiceDetail.InvID" +
                              " AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID" +
                              " AND ChargeType.ChargeClass = '11'" +
                              " ) as InterestAmt" +
                      " from InvoiceHeader" +
                      " INNER JOIN Customer on (InvoiceHeader.CustID = Customer.CustID)" +
                      " INNER JOIN InvoiceDetail ON (InvoiceHeader.InvID = InvoiceDetail.InvID)" +
                      " INNER JOIN Contract on (Contract.ContractID = InvoiceHeader.ContractID)" +
                      " INNER JOIN ConShop ON (Contract.ContractID = ConShop.ContractID) inner join store on(conshop.storeid=store.storeid) WHERE 1 =1 ";

        }
  
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }
        if (txtContractID.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode ='" + txtContractID.Text + "'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }

        if (txtInvStartDate.Text != "")
        {
            str_sql = str_sql + " AND InvoiceDetail.InvStartDate >='" + txtInvStartDate.Text + "'";
        }
        if (txtInvEndDate.Text != "")
        {
            str_sql = str_sql + " AND InvoiceDetail.InvEndDate <='" + txtInvEndDate.Text + "'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }

        if (this.RadioButton5.Checked)
        {
            str_sql = str_sql + " GROUP BY store.storeid,store.storeshortname, Contract.ContractCode,Customer.CustCode,Customer.CustName," +
                                " InvoiceHeader.InvPeriod,InvoiceHeader.InvAdjAmt,InvoiceHeader.InvID";
        }
        else
        {
            str_sql +=  " GROUP BY store.storeid,store.storeshortname, Contract.ContractCode,Customer.CustCode,Customer.CustName," +
                    " InvoiceDetail.Period,InvoiceHeader.InvAdjAmt,InvoiceHeader.InvID";
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\CustInvSum.rpt";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.InitDDL();
        this.Response.Redirect("~/ReportM/RptInv/RptCustInvSum.aspx");

    }
}
