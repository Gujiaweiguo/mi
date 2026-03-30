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

public partial class ReportM_RptBase_RptAdContractInfoItem : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BillboardContractInformationList");
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
        //绑定合同状态
        int[] contractStutas = Contract.GetContractTypeStatus();
        int k = contractStutas.Length + 1;
        ddlContractStatus.Items.Add(new ListItem("", ""));
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

        ////绑定二级经营类别
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //ddlTrade2ID.Items.Add(new ListItem("", ""));
        //Resultset rs1 = baseBo.Query(new TradeRelation());
        //foreach (TradeRelation tradeDef in rs1)
        //    ddlTrade2ID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
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
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
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
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXBizMode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_BizMode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXContractStatus";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXFStartDate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FStartDate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXFEndDate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FEndDate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXFomulaType";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FormulaType");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXTotalArea";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_TotalArea");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXUnitPrice";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_UnitPrice");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXFixedRental";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FixedRental");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_Title");
        paraField[11].CurrentValues.Add(discreteValue[11]);


        paraField[12] = new ParameterField();
        paraField[12].Name = "REXMallTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = Session["MallTitle"].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string authWhere = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }


        //string str_sql = "select Contract.ContractCode,Customer.CustCode,Customer.CustName, " +
        //                 " Case Contract.BizMode when 1 then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_LEASE") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "BIZ_MODE_UNIT") + "' End as BizMode , " +
        //                 " Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus, " +
        //                 "ConFormulaH.FStartDate,ConFormulaH.FEndDate, Case ConFormulaH.FormulaType when 'F' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labFastnessHire") + "' when 'V' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labDeductAndKeep") + "' when 'O' then '一次性收取' end as FormulaType," +
        //                 "ConFormulaH.TotalArea,ConFormulaH.UnitPrice,FixedRental,ConFormulaP.Pcent,ConFormulaP.SalesTo as PSalesTo,ConFormulaM.MinSum,ConFormulaM.SalesTo as MSalesTo " +
        //                 "from Contract Left join ConFormulaH on (Contract.ContractId=ConFormulaH.ContractId) " +
        //                 "left join ConFormulaP on (ConFormulaP.FormulaID=ConFormulaH.FormulaID) " +
        //                 "left join ConFormulaM on (ConFormulaM.FormulaID=ConFormulaH.FormulaID) " +
        //                 "left join Customer On (Customer.CustId=Contract.CustId) " +
        //                 "where 1=1" + func_sql;

           string str_sql = "  Select Contract.ContractCode,Customer.CustCode,Customer.CustName," +
                            " Case Contract.BizMode when 3 then '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_ADBrand") + "' End as BizMode , " +
                            " Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + "' End as ContractStatus, " +
                            "  ConFormulaH.FStartDate,ConFormulaH.FEndDate, Case ConFormulaH.FormulaType when 'F' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labFastnessHire") + "' when 'V' then '" + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labDeductAndKeep") + "' when 'O' then '" + (String)GetGlobalResourceObject("Parameter", "FORMULATYPE_TYPE_THREE") + "' end as FormulaType," +
                            "  ConFormulaH.TotalArea,ConFormulaH.UnitPrice,FixedRental  " +
                            "  from Contract Left join ConFormulaH on (Contract.ContractId=ConFormulaH.ContractId)" +
                            "  left join ConFormulaP on (ConFormulaP.FormulaID=ConFormulaH.FormulaID) " +
                            "  left join ConFormulaM on (ConFormulaM.FormulaID=ConFormulaH.FormulaID)  " +
                            "  left join Customer On (Customer.CustId=Contract.CustId)" +
                            "  where Contract.BizMode=3 "+authWhere;



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

        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.Text + "'";
        }
        if (ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND Contract.RootTradeID like '%" + ddlTradeID.Text + "%'";
        }
        //if (ddlTrade2ID.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.TradeID like '%" + ddlTrade2ID.Text + "%'";
        //}
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
        str_sql = str_sql + "  Order by Contract.ContractID,Customer.CustCode,Contract.BizMode,ConFormulaH.FStartDate ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\AdContractInfoItem.rpt";

    }

    private void ClearPage()
    {
        txtContractID.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        ddlTradeID.SelectedIndex = 0;
        //ddlTrade2ID.SelectedIndex = 0;
        txtConStartDate.Text = "";
        txtEndDate.Text = "";
        ddlContractStatus.SelectedIndex = 0;
        RBRent.Checked = false;
        RBJoin.Checked = false;
        RBAll.Checked = true;
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
