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
using Lease.ContractMod;

public partial class Lease_LeaseUnionItemModify_LeaseUnionItemModify : BasePage
{
    private static int DISPROVE_IN = 1;
    private static int DISPROVE_UP = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*绑定下拉列表*/
            BindListBox();


            if (Request.Cookies["Info"].Values["conID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["conID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 2)
            {
                /*获取联营相关信息*/
                GetLeaseItemInfo(Convert.ToInt32(Request.Cookies["Info"].Values["conID"]));
            }
            else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0 && Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == 1)
            {
                GetLeaseItemModInfo(Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
            }

            txtRentInc.Attributes.Add("onkeydown", "textleave()");
            txtTaxRate.Attributes.Add("onkeydown", "textleave()");
            txtInTaxRate.Attributes.Add("onkeydown", "textleave()");
            txtOutTaxRate.Attributes.Add("onkeydown", "textleave()");
        }
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
        DDownListBillCycle.Items.Add(new ListItem(selected));
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
    protected void GetLeaseItemInfo(int contract)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = "ContractID = " + contract;
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
        }
    }


    /*获联营修改相关条款信息*/
    protected void GetLeaseItemModInfo(int conModID)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = "ConModID = " +conModID;
        rs = baseBO.Query(new ConUnionMod());
        if (rs.Count == 1)
        {
            ConUnionMod conUnionMod = rs.Dequeue() as ConUnionMod;
            DDownListBillCycle.SelectedValue = conUnionMod.BillCycle.ToString();
            DDownListCurrencyType.SelectedValue = conUnionMod.CurTypeID.ToString();
            DDownListAccountCycle.SelectedValue = conUnionMod.AccountCycle.ToString();
            txtRentInc.Text = conUnionMod.RentInc;
            txtTaxRate.Text = ((conUnionMod.TaxRate) * 100).ToString();
            DDownListTaxType.SelectedValue = conUnionMod.TaxType.ToString();
            txtInTaxRate.Text = ((conUnionMod.InTaxRate) * 100).ToString();
            txtOutTaxRate.Text = ((conUnionMod.OutTaxRate) * 100).ToString();
        }
    }

    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        ConUnionMod conUnionMod = new ConUnionMod();

        try
        {
            conUnionMod.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
            conUnionMod.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);
            conUnionMod.AccountCycle = Convert.ToInt32(DDownListAccountCycle.SelectedValue);
            conUnionMod.RentInc = txtRentInc.Text;
            conUnionMod.TaxRate = Convert.ToDecimal(txtTaxRate.Text) / 100;
            conUnionMod.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);
            conUnionMod.InTaxRate = Convert.ToDecimal(txtInTaxRate.Text) / 100;
            conUnionMod.OutTaxRate = Convert.ToDecimal(txtOutTaxRate.Text) / 100;

            baseBO.WhereClause = "ConModID = " + Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            if (baseBO.Update(conUnionMod) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加联营合同修改条款信息错误:", ex);
        }
    }
}
