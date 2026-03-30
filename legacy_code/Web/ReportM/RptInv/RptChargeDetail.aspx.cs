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
using Base.Page;
using Lease.PotBargain;
using Base;
using Lease;
using Invoice;
using BaseInfo.User;
using BaseInfo.authUser;
using Lease.ConShop;

/// <summary>
/// Author: hesijian
/// Date: 2009-11-04
/// Content: Create
/// </summary>
public partial class ReportM_RptInv_RptChargeDetail : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindChargeType();
            GetChargeTypeName();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            txtShopCode.Attributes.Add("OnClick", "ShowTree(LinkButton1)");
        }
    }


    //费用类型
    private void BindChargeType()
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = baseBo.QueryDataSet(new ChargeType());
        ChargeList.DataSource = ds;
        ChargeList.DataBind();

    }

    //获得费用名称
    private void GetChargeTypeName()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new ChargeType());
        int counter = ChargeList.Items.Count;
        for (int i = 0; i < counter; i++)
        {
            foreach (ChargeType charge in rs)
            {
                if (Convert.ToInt32(ChargeList.Items[i].Value) == charge.ChargeTypeID)
                {
                    ChargeList.Items[i].Selected = true;
                }
            }

        }
    }

    //选择"全选"时,ChargeList所有属性均选中，否则均未选中
    protected void All_CheckedChanged(object sender, EventArgs e)
    {
        int counter = ChargeList.Items.Count;
        if (All.Checked)
        {
            for (int i = 0; i < counter; i++)
            {
                ChargeList.Items[i].Selected = true;
            }
        }
        else
        {
            for (int i = 0; i < counter; i++)
            {
                ChargeList.Items[i].Selected = false;
            }
        }
    }


    //数据绑定
    private void BindData(string strwhere)
    {
        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ChargeDetail");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXChargeTypeName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_ChargeTypeName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXChargeStartDate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FStartDate");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXChargeEndDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FEndDate");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMallTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = Session["MallTitle"].ToString();
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXContractID";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvoiceID";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoiceHeader_lblInvID");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXAmount";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXShopName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXShopCode";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXMonAccount";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = GetAccountMonth();
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXPrintDate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXChargeMonth";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_lblAccountMon");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXTotal";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_btnTotalMoney");
        paraField[13].CurrentValues.Add(discreteValue[13]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string strAnd = "";

        string str_sql = "";

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAnd += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql += @"SELECT ConShop.ShopCode,ConShop.ShopName,ConShop.ShopID,
	                       Contract.ContractCode,Contract.ContractID,
	                       CASE WHEN InvoiceDetail.RentType IN (" + InvoiceDetail.RENTTYPE_FIXED_P + "," + InvoiceDetail.RENTTYPE_MUNCH_P + "," + InvoiceDetail.RENTTYPE_SINGLE_P + @") THEN '" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_RentAmount") + "(" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_rdoFormula") + @")'
	                       ELSE ChargeType.ChargeTypeName END as ChargeTypeName,
                           InvoiceDetail.ChargeTypeID,InvoiceHeader.InvID,
	                       InvoiceDetail.InvActPayAmt,InvoiceDetail.InvActPayAmtL,
	                       InvoiceDetail.InvStartDate,InvoiceDetail.InvEndDate
                    FROM InvoiceDetail,InvoiceHeader,ConShop,Contract,ChargeType
                    WHERE InvoiceHeader.InvID = InvoiceDetail.InvID
                      AND InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID
                      AND InvoiceHeader.ContractID = Contract.ContractID
                      AND ConShop.ContractID = Contract.ContractID
                      AND InvoiceDetail.InvActPayAmt != 0
" + strwhere + strAnd + @" ORDER BY ConShop.ShopCode,Contract.ContractCode,InvoiceHeader.InvID";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptChargeDetail.rpt";

        this.Response.Redirect("../ReportShow.aspx");

    }

    //验证输入有效性

    private bool CheckInput()
    {
        if (txtAccountMon.Text == "" && txtContractID.Text == "" && txtShopCode.Text == "" && ChargeList.SelectedIndex == -1)
        {
            return false;
        }
        return true;
    }

    //记账月截取

    private string GetAccountMonth()
    {
        string str = txtAccountMon.Text;
        string endstr = "";
        if (str != "")
        {
            endstr = str.Substring(0, 7);
            return endstr;
        }
        return endstr;
    }

    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string str = "";

        if (!CheckInput())
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputQuery") + "')", true);
            return;
        }

        if (txtContractID.Text.Trim() != "")
        {
            str += "  AND Contract.ContractCode = '" + txtContractID.Text.Trim() + "'";
        }
        if (txtShopCode.Text.Trim() != "")
        {
            str += " AND ConShop.ShopCode = '" + ViewState["shopcode"].ToString() + "' ";
        }
        if (txtAccountMon.Text.Trim() != "")
        {
            str += " AND InvoiceHeader.InvPeriod = '" + txtAccountMon.Text.Trim() + "'";
        }

        int counter = ChargeList.Items.Count;

        string s1 = "";
        for (int i = 0; i < counter; i++)
        {
            if (ChargeList.Items[i].Selected == true)
            {
                s1 += ChargeList.Items[i].Value + ",";
            }
        }

        if (s1.Length > 0)
        {
            str += " AND ChargeType.ChargeTypeID in (" + s1.TrimEnd(',') + ")";

        }

        BindData(str);
    }
    
    //撤销操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("RptChargeDetail.aspx");
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopcode"] = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
        }
    }
}
