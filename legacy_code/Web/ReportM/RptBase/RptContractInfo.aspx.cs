using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using RentableArea;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;


public partial class RptBaseMenu_RptContractInfo : BasePage
{
    public string baseInfo;
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_LeaseContractInformationList");
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
        String s = "%23Rpt_Title_lblContractInfo";
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
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_Title");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        string func_sql = "";
        if (AuthBase.GetAuthUser(objSessionUser.UserID) > 0)
        {
            func_sql = " And  Contract.ContractID in (" + AuthBase.AUTH_SQL_CONTRACID + objSessionUser.UserID + ")";
        }


        string str_sql = "select traderelation.tradename,conshopbrand.brandname,chargetype.chargetypename,store.storeshortname,store.orderid,floors.floorid, rtrim(floors.floorname) + shoptype.shoptypename as shoptype, Contract.ContractCode,Customer.CustCode,Customer.CustName, " +
                         " Case Contract.BizMode when 1 then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_LEASE") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_UNIT") + "' End as BizMode , " +
                         " Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus, " +
                         "ConFormulaH.FStartDate,ConFormulaH.FEndDate, Case ConFormulaH.FormulaType when 'F' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labFastnessHire") + "' when 'V' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labDeductAndKeep") + "' when 'O' then '一次性收取' end as FormulaType," +
                         "ConFormulaH.TotalArea,ConFormulaH.UnitPrice,FixedRental,ConFormulaP.Pcent,ConFormulaP.SalesTo as PSalesTo,ConFormulaM.MinSum,ConFormulaM.SalesTo as MSalesTo " +
                         "from Contract Left join ConFormulaH on (Contract.ContractId=ConFormulaH.ContractId) " +
                         "left join ConFormulaP on (ConFormulaP.FormulaID=ConFormulaH.FormulaID) " +
                         "left join ConFormulaM on (ConFormulaM.FormulaID=ConFormulaH.FormulaID) " +
                         " inner join conshop on (conshop.contractid=contract.contractid)" +
                         "inner join store on (conshop.storeid=store.storeid)"+
                         " inner join shoptype on (conshop.shoptypeid=shoptype.shoptypeid) " +
                         " inner join traderelation on (contract.tradeid=traderelation.tradeid)" +
                         " left join conshopbrand on (conshopbrand.brandid=conshop.brandid)" +
                         " inner join chargetype on (ConFormulaH.chargetypeid=chargetype.chargetypeid)" +
                         " inner join floors on (floors.floorid=conshop.floorid) " + "left join Customer On (Customer.CustId=Contract.CustId) " +
                         "where Contract.Contractstatus=2 and store.storestatus=1 " + func_sql;
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND store.storeid='" + ddlBizproject.SelectedValue + "'";
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
        if(RBRent.Checked)
        {
            str_sql = str_sql + " AND  ConFormulaH.FormulaType = 'F'";
        }
        else if (RBJoin.Checked)
        {
            str_sql = str_sql + " AND  ConFormulaH.FormulaType = 'V'";      
        };
        str_sql = str_sql + " Order by Contract.ContractID,Customer.CustCode,Contract.BizMode,ConFormulaH.chargetypeid,ConFormulaH.FStartDate ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\ContractInfo.rpt";

    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptContractInfo.aspx");
    }
}
