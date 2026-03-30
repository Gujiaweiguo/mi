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
using Lease.ChangeLease;
using Lease.ConOvertimeBill;
using Lease.ConOvertimeBillStop;
using Base.Page;
public partial class Lease_ConOvertimeBillStop_ConTerminateBillStop : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ListBind();
        }
    }
    protected void btnQueryContract_Click(object sender, EventArgs e)
    {
        GetContractInfo(txtCustCode.Text.Trim());
    }
    #region 获取合同信息
    protected void GetContractInfo(string contractCode)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractCode = '" + contractCode + "'";
        DataSet contractDs = baseBo.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0)
        {
            int flag = Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"]);
            if (flag == Contract.CONTRACTSTATUS_TYPE_FIRST)
            {
                ViewState["myFlag"] = "Inserted";
            }
            else if (flag == Contract.CONTRACTSTATUS_TYPE_TEMP)
            {
                ViewState["myFlag"] = "Updated";
            }
            int custId = Convert.ToInt32(contractDs.Tables[0].Rows[0]["CustID"]);
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustID = " + custId;
            DataSet custDS = baseBo.QueryDataSet(new Customer());
            if (custDS.Tables[0].Rows.Count > 0)
            {
                txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
                txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustShortName"].ToString();
            }
            cmbContractStatus.Text = Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"]));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

            cmbTradeID.SelectedValue = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtNewConEndDate.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();
            txtBargain.Text = contractDs.Tables[0].Rows[0]["EConURL"].ToString();
            listBoxRemark.Text = contractDs.Tables[0].Rows[0]["Note"].ToString();
            StringBuilder sb = new StringBuilder();


            ViewState["contractID"] = Convert.ToInt32(Request["VoucherID"]);
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
            ViewState["ConStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            ViewState["chargeStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            ViewState["ConEndDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "';", true);
        }
    }
    #endregion

    protected void ListBind()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        /*绑定二级经营类别*/
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        rs = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
        {
            cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        }
        baseBo.WhereClause = "";

        /*提前终止处罚*/
        int[] statusPenalty = Contract.GetPenaltyTypeStatus();
        int sPenalty = statusPenalty.Length;
        for (int i = 0; i < sPenalty; i++)
        {
            DDownListPenalty.Items.Add(new ListItem(Contract.GetPenaltyTypeStatusDesc(statusPenalty[i]), statusPenalty[i].ToString()));
        }

        /*绑定终约通知期限*/
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTerm.Items.Add(new ListItem(Contract.GetNoticeDesc(status[i]), status[i].ToString()));
        }
    }
    protected void btnOverTime_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        ConTerminateBill conTerminateBill = new ConTerminateBill();
        conTerminateBill.ConTermD = BaseApp.GetConTermD();
        conTerminateBill.ContractID = Convert.ToInt32(ViewState["ContractID"]);
        conTerminateBill.CreateTime =  DateTime.Now;
        conTerminateBill.CreateUserID = sessionUser.UserID;
        conTerminateBill.ModifyTime = DateTime.Now;
        conTerminateBill.ModifyUserID =sessionUser.UserID;
        conTerminateBill.Operator = sessionUser.UserID;
        conTerminateBill.OprDeptID = sessionUser.DeptID;
        conTerminateBill.OprRoleID = sessionUser.RoleID;
        conTerminateBill.RefID = Convert.ToInt32(txtRefID.Text);
        conTerminateBill.TermDate = Convert.ToDateTime(txtNewConStartDate.Text);

        if (baseBO.Insert(conTerminateBill) < 1)
        {
            
        }
        int voucherID = 0;
        voucherID = Convert.ToInt32(conTerminateBill.ConTermD);
        String voucherHints = txtContractCode.Text;
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID,sessionUser.UserID );
        WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);
    }
}
