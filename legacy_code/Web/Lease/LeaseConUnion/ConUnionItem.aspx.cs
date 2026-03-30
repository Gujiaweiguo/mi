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

using Base.Biz;
using Base.DB;
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using RentableArea;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using BaseInfo.User;
using System.Text;
using Base.Page;
using Base.Util;
using Lease.Union;

public partial class Lease_LeaseConUnion_ConUnionItem : BasePage
{

    private static int DISPROVE_IN = 1;
    private static int DISPROVE_UP = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*绑定下拉列表*/
            BindListBox();
            initTXT();

            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                ViewState["ContractID"] = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);

                /*获取联营相关信息*/
                GetLeaseItemInfo();
            }

            txtRentInc.Attributes.Add("onkeydown", "textleave()");
            txtTaxRate.Attributes.Add("onkeydown", "textleave()");
            txtInTaxRate.Attributes.Add("onkeydown", "textleave()");
            txtOutTaxRate.Attributes.Add("onkeydown", "textleave()");
        }
    }
    private void initTXT()
    {
        txtMonthSettleDays.Text = "0";
        txtRentInc.Text = "0";
        txtInTaxRate.Text = "0";
        txtOutTaxRate.Text = "0";
        txtLatePayInt.Text = "0";
        txtIntDay.Text = "0";
        txtTaxRate.Text = "0";
        
    }
    private void BindListBox()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int[] status;
        int s;

        /*结算周期*/
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        status = ConLease.GetFirstSetAcountMonStatus();
       // DDownListBillCycle.Items.Add(new ListItem(selected));
        s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListBillCycle.Items.Add(new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
        }

        /*绑定结算币种*/
        baseBO.WhereClause = "";
        rs = baseBO.Query(new CurrencyType());
        foreach (CurrencyType curType in rs)
        {
            DDownListCurrencyType.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
        }

        /*账期*/
        status = ConUnion.GetAccountCycleStatus();
        s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListAccountCycle.Items.Add(new ListItem(ConUnion.GetAccountCycleStatusDesc(status[i]), status[i].ToString()));
        }

        /*发票类型*/
        status = ConLease.GetTaxTypeStatus();
        s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTaxType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetTaxTypeStatusDesc(status[i])), status[i].ToString()));

        }
    }

    /*获联营相关条款信息*/
    protected void GetLeaseItemInfo()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["ContractID"]);
        rs = baseBO.Query(new ConUnion());
        if (rs.Count == 1)
        {
            ConUnion union = rs.Dequeue() as ConUnion;
            DDownListBillCycle.SelectedValue = union.BillCycle.ToString();
            DDownListCurrencyType.SelectedValue = union.CurTypeID.ToString();
            DDownListAccountCycle.SelectedValue = union.AccountCycle.ToString();
            txtRentInc.Text = union.RentInc;
            txtTaxRate.Text = ((union.TaxRate) * 100).ToString();
            DDownListTaxType.SelectedValue = union.TaxType.ToString();
            txtInTaxRate.Text = ((union.InTaxRate) * 100).ToString();
            txtOutTaxRate.Text = ((union.OutTaxRate) * 100).ToString();
            txtMonthSettleDays.Text = union.MonthSettleDays.ToString();
            txtLatePayInt.Text = Convert.ToString(Convert.ToDecimal(union.LatePayInt) * 1000);
            txtIntDay.Text = union.IntDay.ToString();
            ViewState["MyFlag"] = DISPROVE_UP;
        }
        else
        {
            ViewState["MyFlag"] = DISPROVE_IN;
        }
    }

    /*填充租赁相关条款信息*/
    protected ConUnion FillLeaseItemInfo(ConUnion conUnion)
    {
        try
        {
            conUnion.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            conUnion.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
            conUnion.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);
            conUnion.AccountCycle = Convert.ToInt32(DDownListAccountCycle.SelectedValue);
            conUnion.RentInc = txtRentInc.Text;
            conUnion.TaxRate = Convert.ToDecimal(txtTaxRate.Text) / 100;
            conUnion.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);
            conUnion.InTaxRate = Convert.ToDecimal(txtInTaxRate.Text) / 100;
            conUnion.OutTaxRate = Convert.ToDecimal(txtOutTaxRate.Text) / 100;
            conUnion.IntDay = txtIntDay.Text == "" ? 0 : Convert.ToInt32(txtIntDay.Text);
            conUnion.LatePayInt = (txtLatePayInt.Text == "" ? 0m : Convert.ToDecimal(txtLatePayInt.Text) / 1000);
            conUnion.MonthSettleDays = float.Parse(txtMonthSettleDays.Text.Trim());
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加联营合同条款信息错误:", ex);
        }
        return conUnion;
    }

    /*判断是添加还是修改联营相关信息*/
    protected void InsertOrUpdateLeaseItemInfo()
    {
        BaseBO baseBO = new BaseBO();
        ConUnion conUnion = new ConUnion();

        if (Convert.ToInt32(ViewState["MyFlag"]) == DISPROVE_IN)
        {
            conUnion = FillLeaseItemInfo(conUnion);

            if (baseBO.Insert(conUnion) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
            ViewState["MyFlag"] = DISPROVE_UP;
        }
        else if (Convert.ToInt32(ViewState["MyFlag"]) == DISPROVE_UP)
        {
            conUnion = FillLeaseItemInfo(conUnion);
            baseBO.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["ContractID"]);
            if (baseBO.Update(conUnion) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
    }

    protected void btnTempSave_Click(object sender, EventArgs e)
    {

        if (txtMonthSettleDays.Text == "" || txtRentInc.Text == "" || txtInTaxRate.Text == "" || txtOutTaxRate.Text == "" || txtLatePayInt.Text == "" || txtIntDay.Text=="" || txtTaxRate.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            InsertOrUpdateLeaseItemInfo();

        }
    }
}
