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

public partial class Lease_LeaseUnionItemModify_LeaseUnionItemModifyAutiding : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*绑定下拉列表*/
        BindListBox();

        GetLeaseItemInfo();
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
    protected void GetLeaseItemInfo()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            baseBO.WhereClause = "ConModID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }

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
}
