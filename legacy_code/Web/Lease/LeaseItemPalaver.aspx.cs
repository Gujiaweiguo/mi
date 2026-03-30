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

public partial class Lease_LeaseItemPalaver : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    private ConLease conLease;

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

            if (Request.Cookies["Info"].Values["conID"] != "")
            {
                int contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }
            GetLeaseItemInfo();
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
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }
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
}
