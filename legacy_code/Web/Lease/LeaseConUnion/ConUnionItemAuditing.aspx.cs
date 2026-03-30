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

public partial class Lease_LeaseConUnion_ConUnionItemAuditing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*绑定下拉列表*/
            BindListBox();

            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                ViewState["ContractID"] = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);

                /*获取联营相关信息*/
                GetLeaseItemInfo();
            }

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
            txtOutTaxRate.Text =  ((union.OutTaxRate) * 100).ToString();
            txtMonthSettleDays.Text = union.MonthSettleDays.ToString();
            txtLatePayInt.Text = Convert.ToString(Convert.ToDecimal(union.LatePayInt) * 1000);
            txtIntDay.Text = union.IntDay.ToString();
        }
    }
}
