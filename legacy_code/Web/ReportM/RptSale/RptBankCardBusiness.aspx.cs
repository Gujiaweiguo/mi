using System;
using System.Data;
using System.Web.UI.WebControls;

using CrystalDecisions.Shared;
using Base.Biz;
using Lease.Contract;
using Lease.ConShop;
using Invoice.BankCard;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptBankCardBusiness : System.Web.UI.Page
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
        }

    }

    /* 初始化下拉列表*/
    private void InitDDL()
    {
        BaseBO baseBO = new BaseBO();
        /*绑定经营类型*/
        int[] bizModes = Contract.GetBizModes();
        cmbBizMode.Items.Add(new ListItem("", ""));
        for (int i = 0; i < bizModes.Length; i++)
        {
            cmbBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        if (rdoRptPub_Sum.Checked)
        {
            BindDataSum();
        }
        else
        {
            BindDataParticular();
        }
        this.Response.Redirect("../ReportShow.aspx");
    }


    private void BindDataParticular()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBankCrdSumRpt");
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
        paraField[3].Name = "REXBankAmt";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_TransAmt");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXBankChgAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXBankNetAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSdate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = txtStartBizTime.Text;
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXQEDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = txtEndBizTime.Text;
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXContractCode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXShopCode";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopName";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXAmount";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMallTitle";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = Session["MallTitle"].ToString();
        paraField[13].CurrentValues.Add(discreteValue[13]);



        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "SELECT CustCode,CustName,BankAmt,BankChgAmt," +
                            "BankNetAmt,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName,BankEFTID,BankTransTime,BankCardID FROM BankTransDet LEFT jOIN ConShop ON BankTransDet.ShopID=ConShop.ShopID LEFT JOIN Contract ON " +
                            "ConShop.ContractID=Contract.ContractID LEFT JOIN Customer ON Contract.CustID = Customer.CustID WHERE ReconcType=" + BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES;

        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }
        if (txtContractCode.Text != "")
        {
            str_sql = str_sql + " AND Contract.CONTRACTCODE='" + txtContractCode.Text + "'";
        }
        if (txtShopCode.Text != "")
        {
            str_sql = str_sql + " AND ConShop.ShopId='" + ViewState["shopID"].ToString() + " ' ";
        }

        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND BankTransDet.BankTransTime >='" + txtStartBizTime.Text + " 00:00:00'";
        }

        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND BankTransDet.BankTransTime  <='" + txtEndBizTime.Text + " 23:59:59'";
        }
        if (RB1.Checked)
        {

            str_sql = str_sql + " ";
        }
        if (RB2.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=1 ";
        }
        if (RB3.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=2 ";
        }
        if (RB4.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=3 ";
        }


        if (Convert.ToInt32(cmbBizMode.SelectedIndex) != 0)
        {
            str_sql = str_sql + " AND Contract.BizMode =" + cmbBizMode.SelectedValue;
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + " GROUP BY CustCode,CustName,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName,BankTransDet.BankAmt,BankTransDet.BankChgAmt,BankTransDet.BankNetAmt,BankTransDet.BankEFTID,BankTransDet.BankTransTime,BankTransDet.BankCardID";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptBankCardBusiness.rpt";
    }

    private void BindDataSum()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBankCrdSumRpt");
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
        paraField[3].Name = "REXBankAmt";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_TransAmt");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXBankChgAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXBankNetAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSdate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = txtStartBizTime.Text;
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXQEDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = txtEndBizTime.Text;
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXContractCode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXShopCode";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopName";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXAmount";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMallTitle";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = Session["MallTitle"].ToString();
        paraField[13].CurrentValues.Add(discreteValue[13]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "SELECT CustCode,CustName,SUM(BankAmt) AS BankAmt,SUM(BankChgAmt) AS BankChgAmt," +
                            "SUM(BankNetAmt) AS BankNetAmt,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName FROM BankTransDet LEFT jOIN ConShop ON BankTransDet.ShopID=ConShop.ShopID LEFT JOIN Contract ON " +
                            "ConShop.ContractID=Contract.ContractID LEFT JOIN Customer ON Contract.CustID = Customer.CustID WHERE ReconcType=" + BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES;

        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }
        if (txtContractCode.Text != "")
        {
            str_sql = str_sql + " AND Contract.CONTRACTCODE='" + txtContractCode.Text + "'";
        }
        if (txtShopCode.Text != "")
        {
            str_sql = str_sql + " AND ConShop.ShopId='" + ViewState["shopID"].ToString() + " ' ";
        }

        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND BankTransDet.BankTransTime >='" + txtStartBizTime.Text + " 00:00:00'";
        }

        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND BankTransDet.BankTransTime  <='" + txtEndBizTime.Text + " 23:59:59'";
        }

        if (Convert.ToInt32(cmbBizMode.SelectedIndex) != 0)
        {
            str_sql = str_sql + " AND Contract.BizMode =" + cmbBizMode.SelectedValue;
        }
        if (RB1.Checked)
        {

            str_sql = str_sql + " ";
        }
        if (RB2.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=1 ";
        }
        if (RB3.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=2 ";
        }
        if (RB4.Checked)
        {

            str_sql = str_sql + " AND BankTransDet.datasource=3 ";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + " GROUP BY CustCode,CustName,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptBankCardBusiness.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        cmbBizMode.Text = "";
        txtContractCode.Text = "";
        txtCustCode.Text = "";
        txtEndBizTime.Text = "";
        txtShopCode.Text = "";
        txtStartBizTime.Text = "";
        RB1.Checked = true;
        RB2.Checked = false;
        RB3.Checked = false;
        RB4.Checked = false;
        rdoRptPub_Sum.Checked = true;
        rdoRptPub_Particular.Checked = false;

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";

        }
    }
}
