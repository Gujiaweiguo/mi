using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;


public partial class RptBaseMenu_RptContractSumInfo : BasePage
{
    public string baseInfo;
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ContractInformationSummary");
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
    private void InitDDL()
    {
        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlContractType.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlContractType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }

        //绑定合同状态
        int[] contractStutas = Contract.GetContractTypeStatus();
        int k = contractStutas.Length + 1;
        for (int j = 1; j < k; j++)
        {
            ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(contractStutas[j - 1])), contractStutas[j - 1].ToString()));
        }

        //绑定一级经营类别


        BaseBO baseBo = new BaseBO();
        ddlTradeID.Items.Add(new ListItem("", ""));
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
            ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));

        //绑定二级经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        ddlTrade2ID.Items.Add(new ListItem("", ""));
        Resultset rs1 = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs1)
            ddlTrade2ID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));



    }
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "storestatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

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
        String s = "%23Rpt_Title_lblContractSumInfo";
        s += "%23LeaseholdContract_labContractCode";
        s += "%23PotCustomer_lblCustName";
        s += "%23PotCustomer_lblCustCode";
        s += "%23PotShop_BizMode";
        s += "%23LeaseholdContract_labContractStatus";
        s += "%23LeaseholdContract_labConStartDate";
        s += "%23AdContract_lblEndDate";
        s += "%23" + "Rpt_lblHireType";
        s += "%23" + "RentableArea_lblRentArea";
        s += "%23" + "Rpt_UnitPrice";
        s += "%23" + "ConLease_labFastnessHire";
        return s;
    }
    /* 判断日期空值,返回默认值


     * 
     * 
     */
    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
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
        sCon += "%26CustName=" + GetStrNull(this.txtCustName.Text);
        sCon += "%26BizMode=" + GetStrNull(this.ddlContractType.Text);
        sCon += "%26ContractStatus=" + GetStrNull(this.ddlContractStatus.Text);
        sCon += "%26" + "ConEndDate=" + GetdateNull(this.txtEndDate.Text);
        sCon += "%26" + "ConStartDate=" + GetdateNull(this.txtConStartDate.Text);
        return sCon;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ContractID");
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
        paraField[3].Name = "REXBizMode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_BizMode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXContractStatus";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ContractStatus");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXConStartDate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConStartDate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXConEndDate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConEndDate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXFormulaType";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_FormulaType");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXTotalArea";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_TotalArea");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXMinSumOpt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_MinSumOpt");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTitle";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_Title");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXPcentOpt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_PcentOpt");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXRateType";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_RateType");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXTotalAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXMallTitle";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = Session["MallTitle"].ToString();
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXBizProject";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string func_sql = "";


        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            func_sql = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        string str_sql = " select store.storeshortname,store.orderid,floors.floorid,(rtrim(floors.floorname)+shoptype.shoptypename) as shoptype, max(dbo.Contract.ContractCode) as ContractCode, max(dbo.Customer.CustCode) as CustCode,max(dbo.Customer.CustName) as CustName, " +
                         " Case Contract.BizMode when 1 then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_LEASE") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_UNIT") + "' End as BizMode , " +
                         " Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus, " +
                         " max(ConStartDate) as ConStartDate,max(ConEndDate) as ConEndDate,max(ConFormulaH.TotalArea) as TotalArea" +
                         " FROM dbo.Contract inner JOIN " +
                         " dbo.ConFormulaH ON dbo.Contract.ContractID = dbo.ConFormulaH.ContractID left Join " +
                         " Customer On (Contract.CustId=Customer.CustId) " + 
                         " inner join conshop on (conshop.contractid=contract.contractid)" +
                         " inner join shoptype on (conshop.shoptypeid=shoptype.shoptypeid) " +
                         " inner join floors on (floors.floorid=conshop.floorid) " +
                         "inner join store on (conshop.storeid=store.storeid)"+
                         " where 1=1 and store.storestatus=1 " + func_sql;

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }
        if (txtContractID.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode like '%" + txtContractID.Text + "%'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%'";
        }
        if (ddlContractType.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractTypeID = '" + ddlContractType.Text + "'";
        }
        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.Text + "'";
        }
        if (ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND Contract.TradeID like '%" + ddlTradeID.Text + "%'";
        }
        if (ddlTrade2ID.Text != "")
        {
            str_sql = str_sql + " AND Contract.TradeID like '%" + ddlTrade2ID.Text + "%'";
        }
        if (txtConStartDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConStartDate >= '" + txtConStartDate.Text + "'";
        }
        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate <= '" + txtEndDate.Text + "'";
        }
        
        if (RBRent.Checked)
        {
            str_sql = str_sql + " AND  ConFormulaH.FormulaType = 'F'";
        }
        else if (RBJoin.Checked)
        {
            str_sql = str_sql + " AND  ConFormulaH.FormulaType = 'V'";
        };

        str_sql = str_sql + "Group By store.storeshortname,store.orderid,floors.floorid, ContractCode,CustCode,CustName,Contract.BizMode,ContractStatus,ConStartDate,shoptype.shoptypename,floors.floorname ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\ContractSumInfo.rpt";

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptContractSumInfo.aspx");
    }
}
