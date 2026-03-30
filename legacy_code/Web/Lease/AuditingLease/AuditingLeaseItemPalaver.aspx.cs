using System;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Lease;
using Lease.Formula;
using Base.Page;
using Lease.PotBargain;

public partial class Lease_AuditingLease_AuditingLeaseItemPalaver : BasePage
{
    public string parameterValue;

    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBillCycle();
            BindSettleMode();
            BindCurrencyTypeType();
            BindSettleMode();
            BindIfPrepay();
            BindPayTypeId();
            BindTaxType();

            int contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            GetLeaseItemInfo();

            parameterValue = Request.Cookies["Info"].Values["conID"] + "," + txtBalanceMonth.Text;
        }
    }

    #region 初始化DropDownList
    //绑定结算周期
    protected void BindBillCycle()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = ConLease.GetFirstSetAcountMonStatus();
        DDownListBillCycle.Items.Add(new ListItem(selected));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListBillCycle.Items.Add(new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定结算币种
    protected void BindCurrencyTypeType()
    {
        //int[] status = ConLease.GetCurrencyTypeTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListCurrencyType.Items.Add(new ListItem(ConLease.GetCurrencyTypeTypeStatusDesc(status[i]), status[i].ToString()));
        baseBo.WhereClause = "";
        rs = baseBo.Query(new CurrencyType());
        foreach (CurrencyType curType in rs)
            DDownListCurrencyType.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
    }

    //结算处理方式
    protected void BindSettleMode()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = ConLease.GetSettleModeTypeStatus();
        DDownListSettleMode.Items.Add(new ListItem(selected));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListSettleMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetSettleModeTypeStatusDesc(status[i])), status[i].ToString()));
    }

    //是否预收保底
    protected void BindIfPrepay()
    {
        int[] status = ConLease.GetIfPrepayStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListIfPrepay.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetIfPrepayStatusDesc(status[i])), status[i].ToString()));
    }

    //押金方式
    protected void BindPayTypeId()
    {
        //int[] status = ConLease.GetPayTypeIdTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListPayTypeId.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetPayTypeIdTypeStatusDesc(status[i])), status[i].ToString()));

        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        PayType payType = new PayType();
        rs = baseBo.Query(payType);
        foreach (PayType pt in rs)
        {
            DDownListPayTypeId.Items.Add(new ListItem(pt.PayTypeName, pt.PayTypeID.ToString()));
        }
    }

    //发票类型
    protected void BindTaxType()
    {
        int[] status = ConLease.GetTaxTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListTaxType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetTaxTypeStatusDesc(status[i])), status[i].ToString()));
    }
    #endregion

    #region 获取赁相关条款信息






    protected void GetLeaseItemInfo()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        rs = baseBo.Query(new ConLease());
        if (rs.Count == 1)
        {
            ConLease lease = rs.Dequeue() as ConLease;
            DDownListBillCycle.SelectedValue = lease.BillCycle.ToString();
            DDownListCurrencyType.SelectedValue = lease.CurTypeID.ToString();
            DDownListSettleMode.SelectedValue = lease.SettleMode.ToString();
            txtMonthSettleDays.Text = lease.MonthSettleDays.ToString();
            DDownListIfPrepay.SelectedValue = lease.IfPrepay.ToString();
            txtBalanceMonth.Text = lease.BalanceMonth.ToString("yyyy-MM-dd");
            DDownListPayTypeId.SelectedValue = lease.PayTypeID.ToString();
            txtLatePayInt.Text = Convert.ToString(Convert.ToDecimal(lease.LatePayInt) * 1000);
            txtIntDay.Text = lease.IntDay.ToString();
            txtTaxRate.Text = Convert.ToString(Convert.ToDecimal(lease.TaxRate) * 100);
            DDownListTaxType.SelectedValue = lease.TaxType.ToString();
            ViewState["myFlag"] = "Updated";
        }
        else
        {
            ViewState["myFlag"] = "Inserted";
        }
    }
    #endregion
    protected void btnFirstCharge_Click(object sender, EventArgs e)
    {
        /*int result = Invoice.ChargeAccount.AccountCharge(Convert.ToInt32(Request.Cookies["Info"].Values["conID"]), Convert.ToDateTime(txtBalanceMonth.Text), 1, chargeType, htb, out invCode);
        if (result == Invoice.ChargeAccount.PROMT_SUCCED) //成功
        {
            Session["rptPathName"] = "";
            Session["reportDOC"] = "";
            if (invCode > 0)
            {
                this.Response.Redirect("../../ReportM/RptInvDetail.aspx?InvCode=" + invCode + "&PrtName=" + objSessionUser.UserID);
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "');</script>");
            }
            //Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "');</script>");
            // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_FIRST_CHARGE_YES)
        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Promt_First_Charge_Yes") + "');</script>");
        }
        if (result == Invoice.ChargeAccount.PROMT_FIRST_CHARGE_NO) //首期费用未生成



        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_First_Charge_No") + "');</script>");
            // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_First_Charge_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_NO) //合同无效
        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "');</script>");
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_INFO_NO) //未有合同信息
        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_Info_No") + "');</script>");
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_Info_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_DATE_NO) //结算时间段完全不在合同时间范围内
        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contact_Date_No") + "');</script>");
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contact_Date_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_BEFORE_CHARGE_NO) //前期费用未生成



        {
            Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No") +"');</script>" );
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No") + "'", true);
        }
        //Response.Redirect("SelectLease.aspx");
        //int voucherID = Convert.ToInt32(ViewState["contractID"]);
        //String voucherHints = txtCustName.Text.Trim();
        //String voucherMemo = "";

        //VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

        //WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PalaverYes") + "'", true);

        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);*/
    }
}
