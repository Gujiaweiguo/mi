using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using Lease.Subs;
using RentableArea;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptBase_RptADContractInfo : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptADContractInfo");
        strFresh = (String)GetGlobalResourceObject("ReportInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            InitDDL();

        }
    }
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
        ddlSubsID.Items.Add(new ListItem("", ""));
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs = baseBo.Query(new Subs());
        foreach (Subs sub in rs)
            ddlSubsID.Items.Add(new ListItem(sub.SubsShortName, sub.SubsID.ToString()));

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
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        //paraField[0] = new ParameterField();
        //paraField[0].ParameterFieldName = "REXContractID";
        //discreteValue[0] = new ParameterDiscreteValue();
        //discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        //paraField[0].CurrentValues.Add(discreteValue[0]);

        //paraField[1] = new ParameterField();
        //paraField[1].Name = "REXCustCode";
        //discreteValue[1] = new ParameterDiscreteValue();
        //discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        //paraField[1].CurrentValues.Add(discreteValue[1]);

        //paraField[2] = new ParameterField();
        //paraField[2].Name = "REXCustName";
        //discreteValue[2] = new ParameterDiscreteValue();
        //discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName");
        //paraField[2].CurrentValues.Add(discreteValue[2]);

        //paraField[3] = new ParameterField();
        //paraField[3].Name = "REXBizMode";
        //discreteValue[3] = new ParameterDiscreteValue();
        //discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_BizMode");
        //paraField[3].CurrentValues.Add(discreteValue[3]);

        //paraField[4] = new ParameterField();
        //paraField[4].Name = "REXContractStatus";
        //discreteValue[4] = new ParameterDiscreteValue();
        //discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        //paraField[4].CurrentValues.Add(discreteValue[4]);

        //paraField[5] = new ParameterField();
        //paraField[5].Name = "REXFStartDate";
        //discreteValue[5] = new ParameterDiscreteValue();
        //discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FStartDate");
        //paraField[5].CurrentValues.Add(discreteValue[5]);

        //paraField[6] = new ParameterField();
        //paraField[6].Name = "REXFEndDate";
        //discreteValue[6] = new ParameterDiscreteValue();
        //discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FEndDate");
        //paraField[6].CurrentValues.Add(discreteValue[6]);


        //paraField[7] = new ParameterField();
        //paraField[7].Name = "REXTotalArea";
        //discreteValue[7] = new ParameterDiscreteValue();
        //discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_TotalArea");
        //paraField[7].CurrentValues.Add(discreteValue[7]);


        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptADContractInfo");
        paraField[0].CurrentValues.Add(discreteValue[0]);


        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);


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




    string str_sql =@" select contract.contractcode,customer.custcode,customer.custname,
                        Case Contract.ContractStatus when 3 then '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_ADBrand") + "' End as BizMode ,Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + @"' End as ContractStatus,
                        contract.constartdate,contract.conenddate,sum(conadboard.rentarea) retarea,subsidiary.subsshortname
                        from contract 
                        inner join customer on contract.custid=customer.custid
                        inner join conadboard on contract.contractid=conadboard.contractid
                        inner join subsidiary on contract.subsid=subsidiary.subsid
                        where contract.bizmode=3 ";

        

        if (txtContractID.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode like '%" + txtContractID.Text + "%' ";
        }
        if (txtCustCode.Text != "")
        { 
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%' ";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%' ";
        }
        if (ddlContractStatus.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractStatus = '" + ddlContractStatus.SelectedValue + "' ";
        }
        if (ddlSubsID.Text != "")
        {
            str_sql = str_sql + " AND Contract.subsid = '" + ddlSubsID.Text + "' ";
        }
        if (txtConStartDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConStartDate >= '" + txtConStartDate.Text + "' ";
        }
        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate <= '" + txtEndDate.Text + "' ";
        }

        str_sql = str_sql + " group by contract.contractcode,customer.custcode,customer.custname,ContractStatus,contract.constartdate,contract.conenddate,subsidiary.subsshortname ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\ADContractInfo.rpt";

    }

    private void ClearPage()
    {
        txtContractID.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        ddlSubsID.SelectedIndex = 0;
        txtConStartDate.Text = "";
        txtEndDate.Text = "";
        ddlContractStatus.SelectedIndex = 0;

    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }

}
