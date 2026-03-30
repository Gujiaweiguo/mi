using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class RptBaseMenu_RptCustPaidDetail : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
        this.RadioButton4.Checked = true;

        ////绑定楼


        //BaseBO baseBo = new BaseBO();
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs = baseBo.Query(new Building());
        //ddlBuildingName.Items.Add(new ListItem("", ""));
        //foreach (Building bd in rs)
        //    ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingCode));

        ////绑定楼层
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs1 = baseBo.Query(new Floors());
        //ddlFloorName.Items.Add(new ListItem("", ""));
        //foreach (Floors bd in rs1)
        //    ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorCode));

        ////绑定合同状态



        //int[] contractStutas = Contract.GetContractTypeStatus();
        //int k = contractStutas.Length + 1;
        //ddlContractStatus.Items.Add(new ListItem("", ""));
        //for (int j = 1; j < k; j++)
        //{
        //    ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(contractStutas[j - 1])), contractStutas[j - 1].ToString()));
        //}


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
        String s = "%23Rpt_lblCustPaidDetail";
        s += "%23" + "LeaseholdContract_labContractCode";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "InvoiceHeader_lblInvID";
        s += "%23" + "Rpt_InvPeriod";
        s += "%23" + "Rpt_AdvAmt";
        s += "%23" + "Rpt_InPeriodPaidAmt";
        s += "%23" + "InvoiceHeader_lblInvPaidAmt";
        s += "%23" + "InvoiceHeader_lblInvPayDate"; 
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

        sCon += "%26" + "CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        sCon += "%26" + "ContractStatus=" + GetStrNull(this.ddlContractStatus.Text);
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
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXContractCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustCode";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXInvCode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvCode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustPaidDetail_Title");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXTotalAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvPayAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSInvPaidAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_SInvPaidAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXInvPeriod";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPeriod");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXCustName";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXInvPayDate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPayDate");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXBizProject";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXMallTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = Session["MallTitle"].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);


        paraField[13] = new ParameterField();
        paraField[13].Name = "RexChargeTypeName";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_lblChargeType");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
                

        string whereStr1 = "";
        string whereStr2 = "";
        string whereStr3 = "";

        if (this.txtInvStartDate.Text != "" && this.txtInvEndDate.Text != "")
        {
            whereStr1 = " And invoiceDetail.InvStartDate >='" + this.txtInvStartDate.Text + "' And InvoiceDetail.InvEndDate <='" + this.txtInvEndDate.Text + "'";
        }

        if (txtContractID.Text != "")
        {
            whereStr2 = " and Contract.ContractCode = '" + txtContractID.Text + "'";
        }


        if (ddlBizproject.Text != "")
        {
            whereStr3 = " AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string authWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        string str_sql = "";
        if (this.RadioButton5.Checked)  //收付实现制：采用invoiceHeader.InvPeriod作为记账月
        {
            str_sql = "select store.storeid,store.storeshortname,Contract.ContractCode,inv.CustCode,inv.CustName," +
                            " inv.InvCode,inv.InvPeriod,pay.InvPayDate,inv.chargetypename," +
                            " Sum(inv.InvActPayAmt) as InvPayAmt," +
                            " sum(pay.InvPaidAmt) as SInvPaidAmt" +
                            " FROM Contract INNER JOIN " +
                                "  (" +
                                 " SELECT invoiceHeader.ContractID,invoiceHeader.invID,invoiceHeader.custID,invoiceHeader.custName,invoiceHeader.invCode," +
                                       "  invoiceHeader.InvPeriod,(SELECT custCode FROM customer WHERE customer.CustID = invoiceHeader.CustID) AS custCode,chargetype.chargetypename," +
                                        " SUM(invoiceDetail.InvActPayAmt) AS InvActPayAmt" +
                                   " FROM invoiceHeader INNER JOIN" +
                                       "  invoiceDetail ON (invoiceHeader.invID = invoiceDetail.invID) " +
                                       " inner join chargetype on (invoiceDetail.chargetypeid=chargetype.chargetypeid) where 1=1 " + whereStr1 +
                               "  GROUP BY invoiceHeader.ContractID,invoiceHeader.invID,invoiceHeader.custID," +
                                         " invoiceHeader.custName,invoiceHeader.invCode,invoiceHeader.InvPeriod,chargetype.chargetypename" +
                                 " ) AS inv ON (Contract.ContractID = inv.ContractID) INNER JOIN" +
                                 " (" +
                                "  SELECT invoicePayDetail.invID,InvoicePay.InvPayDate,SUM(InvoicePayDetail.InvPaidAmt) AS InvPaidAmt" +
                                   " FROM invoicePay INNER JOIN " +
                                        " invoicePayDetail ON (invoicePay.invPayID = invoicePayDetail.invPayID)" +
                                  " GROUP BY invoicePayDetail.invID,InvoicePay.InvPayDate" +
                                 " ) AS pay ON (inv.invID = pay.invID) " +
                                " INNER JOIN ConShop ON (ConShop.ContractID = Contract.ContractID) INNER JOIN store ON (conshop.storeid=store.storeid) where 1=1 " + whereStr2 + authWhere +
                           " GROUP BY store.storeid,store.storeshortname, Contract.ContractCode,inv.CustCode,inv.CustName," +
                                      " inv.InvCode,inv.InvPeriod,pay.InvPayDate,inv.chargetypename";
        }
        else   //权责发生制:采用invoiceDetail.Period为记账月
        {
            str_sql = "select store.storeid,store.storeshortname,Contract.ContractCode,inv.CustCode,inv.CustName," +
                " inv.InvCode,inv.InvPeriod,pay.InvPayDate,inv.chargetypename," +
                " Sum(inv.InvActPayAmt) as InvPayAmt," +
                " sum(pay.InvPaidAmt) as SInvPaidAmt" +
                " FROM Contract INNER JOIN " +
                    "  (" +
                     " SELECT invoiceHeader.ContractID,invoiceHeader.invID,invoiceHeader.custID,invoiceHeader.custName,invoiceHeader.invCode," +
                           "  invoiceDetail.Period InvPeriod,(SELECT custCode FROM customer WHERE customer.CustID = invoiceHeader.CustID) AS custCode,chargetype.chargetypename," +
                            " SUM(invoiceDetail.InvActPayAmt) AS InvActPayAmt" +
                       " FROM invoiceHeader INNER JOIN" +
                           "  invoiceDetail ON (invoiceHeader.invID = invoiceDetail.invID) " +
                           " inner join chargetype on (invoiceDetail.chargetypeid=chargetype.chargetypeid) where 1=1 " + whereStr1 +
                   "  GROUP BY invoiceHeader.ContractID,invoiceHeader.invID,invoiceHeader.custID," +
                             " invoiceHeader.custName,invoiceHeader.invCode,invoiceDetail.Period,chargetype.chargetypename" +
                     " ) AS inv ON (Contract.ContractID = inv.ContractID) INNER JOIN" +
                     " (" +
                    "  SELECT invoicePayDetail.invID,InvoicePay.InvPayDate,SUM(InvoicePayDetail.InvPaidAmt) AS InvPaidAmt" +
                       " FROM invoicePay INNER JOIN " +
                            " invoicePayDetail ON (invoicePay.invPayID = invoicePayDetail.invPayID)" +
                      " GROUP BY invoicePayDetail.invID,InvoicePay.InvPayDate" +
                     " ) AS pay ON (inv.invID = pay.invID) " +
                    " INNER JOIN ConShop ON (ConShop.ContractID = Contract.ContractID) INNER JOIN store ON (conshop.storeid=store.storeid) where 1=1 " + whereStr2 + authWhere +
               " GROUP BY store.storeid,store.storeshortname, Contract.ContractCode,inv.CustCode,inv.CustName," +
                          " inv.InvCode,inv.InvPeriod,pay.InvPayDate,inv.chargetypename";
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\CustPaidDetail.rpt";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        
        this.Response.Redirect("~/ReportM/RptInv/RptCustPaidDetail.aspx");
    }
}
