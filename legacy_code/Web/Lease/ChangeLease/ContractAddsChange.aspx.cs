using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Lease;
using Lease.Contract;
using Lease.Customer;
using Base.Page;
public partial class Lease_ChangeLease_ContractAddsChange : BasePage
{
    #region 定义

    private BaseBO baseBo = new BaseBO();
    private Resultset rs = new Resultset();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDownList();

            int contractId = Convert.ToInt32(Request["VoucherID"]);
            GetContractInfo(contractId);
            GetLeaseInfo(contractId);
        }
    }

    /** 根据合同ID,获取合同信息
     * 
     * 
     */
    private void GetContractInfo(int contractID)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = " Contractid = " + contractID;
        DataSet contractDs = baseBo.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0) 
        {
            //招商员



            baseBo.WhereClause = "";
            baseBo.WhereClause = " UserID = " + contractDs.Tables[0].Rows[0]["commOper"].ToString();
            Resultset rs = baseBo.Query(new Users());
            if (rs.Count > 0 )
            {
                Users user = rs.Dequeue() as Users;
                txtAttr.Text = user.UserName;
            }
            //商户信息
            int custId = Convert.ToInt32(contractDs.Tables[0].Rows[0]["CustID"]);
            baseBo.WhereClause = "";
            baseBo.WhereClause = " CustID = " + custId;
            DataSet custDs = baseBo.QueryDataSet(new Customer());
            if (custDs.Tables[0].Rows.Count > 0)
            {
                txtCustomCode.Text = custDs.Tables[0].Rows[0]["CustCode"].ToString();
                txtCustomName.Text = custDs.Tables[0].Rows[0]["CustName"].ToString();
                txtCustomShortName.Text = custDs.Tables[0].Rows[0]["CustShortName"].ToString();
            }

            //合同信息
            String refId = contractDs.Tables[0].Rows[0]["RefID"].ToString();
            txtRefID.Text = refId == null ? "" : refId;
            txtConStartDate.Text = Convert.ToDateTime( contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            txtChargeStartD.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");

            String sNote = "　［" + contractDs.Tables[0].Rows[0]["Note"].ToString() + "］"; 
            StringBuilder sb = new StringBuilder();
            if (sNote != null)
            {
                sb.Append(sNote);
            }
            lbNote.Text = (sNote == null ? "" : sb.ToString());

        }

    }
    /* 根据合同ID 获取租赁信息
     * 
     * 
     */
    private void GetLeaseInfo(int contractId)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = " ContractID = " + contractId;
        rs = baseBo.Query(new ConLease());
        if (rs.Count == 1)
        {
            ConLease conLease = rs.Dequeue() as ConLease;
            ddltBillCycle.SelectedValue = conLease.BillCycle.ToString();
            txtMonthSettleDays.Text = conLease.MonthSettleDays.ToString();
            ddltYesNoMin.SelectedValue = conLease.IfPrepay.ToString();
            txtFirstAccountMonth.Text = conLease.BalanceMonth.ToString();
            txtLatePayInt.Text = conLease.LatePayInt.ToString();
            txtIntDay.Text = conLease.IntDay.ToString();
        }
    }
    /* 初始化下拉列表



     * 
     * 
     */ 
    private void InitDownList()
    {
        BandBillCycle();
        BandIfPrepay();
    }
    /* 绑定：结算周期



     * 
     * 
     */
    private void BandBillCycle()
    {
        int[] status = ConLease.GetFirstSetAcountMonStatus();
        ddltBillCycle.Items.Add(new ListItem("--请选择--"));
        for (int i=0;i< status.Length ;i++)
        {
            ddltBillCycle.Items.Add(
                new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
        }

    }
    /* 绑定：是否预收保底



     * 
     * 
     */
    private void BandIfPrepay()
    {
        int[] status = ConLease.GetIfPrepayStatus();
        ddltYesNoMin.Items.Add(new ListItem("--请选择--"));
        for (int i=0;i < status.Length ;i++)
        {
            ddltYesNoMin.Items.Add(new ListItem(ConLease.GetIfPrepayStatusDesc(status[i]), status[i].ToString()));
        }
    }
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        UpdateDate();
    }
    /*
     * 
     * 
     */
    private void UpdateDate()
    {
        
    }
}
