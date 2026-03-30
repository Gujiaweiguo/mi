using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Page;
using Lease.Contract;

public partial class RptBaseMenu_RptAreaContractInfoItem : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptAreaContractInfoItem_Title");
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
            ddlContractStatus.Items.Add(new ListItem(Contract.GetContractTypeStatusDesc(contractStutas[j - 1]), contractStutas[j - 1].ToString()));
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
        String s = "%23Rpt_Title_lblAreaContractInfoItem";
        s += "%23" + "Rpt_AreaAdContractCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "LeaseholdContract_labContractStatus";
        s += "%23" + "Rpt_SignDate";
        s += "%23" + "LeaseholdContract_labConEndDate";
        s += "%23" + "Rpt_AreaTypeCode";
        s += "%23" + "Rpt_Prepayment";
        s += "%23" + "Rpt_PaymentPerCycle";
        s += "%23" + "AdContract_lblPayingCycle";

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
        sCon += "%26AdContractCode=" + GetStrNull(this.txtAdContractCode.Text);
        sCon += "%26CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26ContractStatus=" + GetStrNull(this.ddlContractStatus.SelectedValue);
        sCon += "%26" + "EndDate=" + GetdateNull(this.txtEndDate.Text);
        sCon += "%26" + "SignDate=" + GetdateNull(this.txtSignDate.Text);
        sCon += "%26" + "StartDate=" + GetdateNull(this.txtStartDate.Text);
        return sCon;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXAdContractCode";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_AdContractCode");
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
        paraField[3].Name = "REXSignDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_SignDate");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXContractStatus";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractStatus");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXEndDate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConEndDate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXAreaTypeCode";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAreaContractInfoItem_AreaTypeCode");
        paraField[6].CurrentValues.Add(discreteValue[6]);


        paraField[7] = new ParameterField();
        paraField[7].Name = "REXPrepayment";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_Prepayment");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXTitle";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAreaContractInfoItem_Title");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXPaymentPerCycle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_PaymentPerCycle");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXPayingCycle";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptAdvisContractInfoItem_PayingCycle");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXTotalAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "sPara";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = Session["MallTitle"].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);

        
        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select contract.contractcode,customer.custname,customer.custcode,
                            Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + @"' End as ContractStatus,
                            contract.constartdate,contract.conenddate,areamanage.areacode,areamanage.areaname,areatype.areatypecode,areatype.areatypedesc,dept.deptname
                            from contract 
                            inner join customer on contract.custid=customer.custid
                            inner join conarea on contract.contractid=conarea.contractid
                            inner join areamanage on conarea.conareacode=areamanage.areaid
                            inner join dept on dept.deptid=areamanage.storeid
                            left join areatype on areatype.areatypeid=areamanage.areatypeid
                            where contract.bizmode=4 ";

        if (txtAdContractCode.Text != "")
        {
            str_sql = str_sql + " AND Contract.ContractCode like '%" + txtAdContractCode.Text + "%'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustCode.Text + "%'";
        }
        if (txtCustName.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text + "%'";
        }
        //if (txtSignDate.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.SignDate = '" + txtSignDate.Text + "'";
        //}
        //if (ddlContractStatus.Text != "")
        //{
        //    str_sql = str_sql + " AND AdContract.AdContractStatus = '" + ddlContractStatus.Text + "'";
        //}
        if (txtStartDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConStartDate >= '" + txtStartDate.Text + "'";
        }
        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate <= '" + txtEndDate.Text + "'";
        }


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\AreaContractInfoItem.rpt";

    }
}
