using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Invoice;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class ReportM_RptInv_RptCharge : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtInvEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtInvStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
        }
    }
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

    private void BindData(string whereStr)
    {

        BaseBO baseBO = new BaseBO();
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[17];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[17];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptWAndE_Title");
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
        paraField[4].Name = "REXLastRead";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ReadLastData");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXCurQty";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ReadCurrentData");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXCostQty";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Usage");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXFreeQty";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_lblFreeQty");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXPrice";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Price");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTimes";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Account_lblTimes");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalMoney";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXContractID";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXShopName";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXShopCode";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXCustName";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXBizProject";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXMallTitle";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = Session["MallTitle"].ToString();
        paraField[16].CurrentValues.Add(discreteValue[16]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string str_sql = "SELECT store.storeid,store.storeshortname, ChargeDetail.ChgName,ChargeDetail.StartDate,ChargeDetail.EndDate,ChargeDetail.LastQty,ChargeDetail.CurQty,ChargeDetail.CostQty,ChargeDetail.FreeQty,ChargeDetail.Price,ChargeDetail.Times,ChargeDetail.ChgAmt," +
                        " Contract.ContractCode,ConShop.ShopName,ConShop.ShopCode,Customer.CustName" +
                        " FROM ChargeDetail,Charge,Contract,ConShop,Customer,store" +
                        " WHERE ChargeDetail.ChgID = Charge.ChgID AND ConShop.ShopID = Charge.ShopID and conshop.storeid=store.storeid AND ConShop.ContractID = Contract.ContractID" +
                        " AND Customer.CustID = Contract.CustID " +
                        " AND Charge.ChgStatus = " + Charge.CHGSTATUS_TYPE_ATTREM + whereStr;
                     

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\WaterAndElect.rpt";

        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

        if (txtInvStartDate.Text.Trim() != "" && txtInvEndDate.Text.Trim() != "")
        {
            string str = "";
            if (ddlBizproject.Text != "")
            {
                str += "AND store.storeid='" + ddlBizproject.SelectedValue + "'";
            }
            if (txtContractID.Text.Trim() != "")
            {
                str += "AND Contract.ContractCode = '" + txtContractID.Text.Trim() + "' ";
            }
            if (txtShopCode.Text.Trim() != "")
            {
                str += "AND ConShop.ShopCode = '" + txtShopCode.Text.Trim() + "' ";
            }
            str += "AND ChargeDetail.StartDate >= '" + txtInvStartDate.Text.Trim() + "' AND ChargeDetail.EndDate <= '" + txtInvEndDate.Text.Trim() + "' ";

            BindData(str);
        }
        else 
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Msg_InputDate") + "'", true);
        }
        
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtInvEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtInvStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtContractID.Text = "";
        txtShopCode.Text = "";
        ddlBizproject.SelectedValue = "";
    }
}
