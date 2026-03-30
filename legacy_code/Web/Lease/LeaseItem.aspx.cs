using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Lease;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Contract;
using Base.Page;
using Base.Util;

public partial class Lease_LeaseItem : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    private ConLease conLease;

    public string conLeaseBillCycle;  //请选择结算周期!
    public string conLeasePayTypeID;  //请选择押金方式!
    public string conLeaseSettleMode; //请选择结算处理方式!
    public string conLeaseMonthSettleDays; //请输入月结天数设定!
    public string conLeaseIfPrepay;        //请选择是否预收保底!
    public string conLeaseBalanceMonth;    //请输入结算月份!


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBillCycle();
            BindSettleMode();
            BindCurrencyTypeType();
            BindIfPrepay();
            BindPayTypeId();
            BindTaxType();
            int contractID = 0;
            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);

            }
    
            GetLeaseItemInfo();

            conLeaseBillCycle = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeaseBillCycle");
            conLeasePayTypeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeasePayTypeID");
            conLeaseSettleMode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeaseSettleMode");
            conLeaseMonthSettleDays = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeaseMonthSettleDays");
            conLeaseIfPrepay = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeaseIfPrepay");
            conLeaseBalanceMonth = (String)GetGlobalResourceObject("BaseInfo", "Prompt_conLeaseBalanceMonth");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            txtMonthSettleDays.Attributes.Add("onkeydown", "textleave()");
            txtTaxRate.Attributes.Add("onkeydown", "textleave()");
            txtLatePayInt.Attributes.Add("onkeydown", "textleave()");
            txtIntDay.Attributes.Add("onkeydown", "textleave()");
            //如果是终止合同，则不能保存 add by lcp
            BaseBO objBaseBo = new BaseBO();
            objBaseBo.WhereClause = "ContractID = " + contractID;
            Resultset rs = objBaseBo.Query(new Contract());
            if (rs.Count == 1)
            {
                Contract objContract = rs.Dequeue() as Contract;
                if (objContract.ContractStatus == 4)
                {
                    this.btnSave.Visible = false;
                }
            }
            //
        }
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
       InsertOrUpdateLeaseItemInfo();
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
            DDownListSettleMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",ConLease.GetSettleModeTypeStatusDesc(status[i])), status[i].ToString()));
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
        //    DDownListPayTypeId.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",ConLease.GetPayTypeIdTypeStatusDesc(status[i])), status[i].ToString()));

        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        PayType payType = new PayType();
        baseBo.WhereClause = " PayTypeStatus='" + PayType.ISPAYTYPESTATUS_YES + "' ";
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
            DDownListTaxType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",ConLease.GetTaxTypeStatusDesc(status[i])), status[i].ToString()));
    }
    #endregion

    #region 获取赁相关条款信息


    protected void GetLeaseItemInfo()
    {
        baseBo.WhereClause = "";
        int contractID = 0;
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }
        baseBo.WhereClause = "ContractID = " + contractID;
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

    #region 判断是添加还是修改租赁相关信息


    protected void InsertOrUpdateLeaseItemInfo()
    {
        if (ViewState["myFlag"].ToString() == "Inserted")
        {
            FillLeaseItemInfo();
            if (baseBo.Insert(conLease) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseFormulaH") + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ViewState["myFlag"] = "Updated";
        }
        else if (ViewState["myFlag"].ToString() == "Updated")
        {
            FillLeaseItemInfo();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            if (baseBo.Update(conLease) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseFormulaH") + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
    }
    #endregion

    #region 填充租赁相关条款信息
    protected void FillLeaseItemInfo()
    {
        try
        {
            conLease = new ConLease();
            conLease.ContractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            conLease.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
            conLease.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);
            conLease.SettleMode = Convert.ToInt32(DDownListSettleMode.SelectedValue);
            conLease.MonthSettleDays = float.Parse(txtMonthSettleDays.Text.Trim());
            conLease.IfPrepay = Convert.ToInt32(DDownListIfPrepay.SelectedValue);
            conLease.BalanceMonth = Convert.ToDateTime(txtBalanceMonth.Text);
            conLease.PayTypeID = Convert.ToInt32(DDownListPayTypeId.SelectedValue);
            conLease.LatePayInt = (txtLatePayInt.Text == "" ? 0m : Convert.ToDecimal(txtLatePayInt.Text) / 1000);
            conLease.IntDay = txtIntDay.Text == "" ? 0 : Convert.ToInt32(txtIntDay.Text);
            conLease.TaxRate = txtTaxRate.Text == "" ? 0m : Convert.ToDecimal(txtTaxRate.Text) / 100;
            conLease.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);
            conLease.RentInc = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同条款信息错误:", ex);
        }
    }
    #endregion
}
