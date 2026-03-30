using System;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.Page;

public partial class ReportM_RptInv_RptChargeCountLog : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            if (Request["bancthID"] != null)
            {
                BindDate(Request["bancthID"]);
            }
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindDate("NO");
    }

    private void BindDate(string bancthID)
    {
        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[1];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[1];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInvChargeCountLog_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "select A.ChargeCountID, A.ContractID,A.CustID,A.ChargeTypeID,A.ProductStatus,A.InvPayAmt,A.ProductDate,A.ChargePeriod,A.ProductNote,A.BancthID," +
                         "B.ContractCode,C.CustCode,C.CustName,D.ChargeTypeName " +
                         " from ChargeCountLog A ,Contract B , Customer C,ChargeType D " +
                         " where A.ContractID = B.ContractID and A.CustID = C.CustID and A.ChargeTypeID = D.ChargeTypeID ";
        if (bancthID != "NO")
        {
            str_sql += " and A.BancthID = '" + bancthID + "'";
        }
        if (txtContractID.Text != "")
        {
            str_sql += " and A.ContractID = '" + txtContractID.Text + "'";
        }
        if (txtInvStartDate.Text != "")
        {
            str_sql += " and A.ProductDate >= '" + txtInvStartDate.Text + "'";
        }
        if (txtContractID.Text != "")
        {
            str_sql += " and A.ProductDate <= '" + txtInvEndDate.Text + "'";
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\ChargeCountLog.rpt";

        this.Response.Redirect("../ReportShow.aspx");
    }
}
