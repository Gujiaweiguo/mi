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
public partial class Lease_ConOvertimeBillStop_ConTerminateBillAuditing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            ListBind();
            if (Request["VoucherID"] == "" || Request["VoucherID"] == null)
            {
                ViewState["ConTermD"] = 0;
            }
            else
            {
                ViewState["ConTermD"] = Request["VoucherID"];
            }

            ConTerminateBill conTerminateBill = new ConTerminateBill();
            baseBO.WhereClause = "ConTermD='" + ViewState["ConTermD"] + "'";
            rs = baseBO.Query(conTerminateBill);
            if (rs.Count > 0)
            {
                conTerminateBill = rs.Dequeue() as ConTerminateBill;
                ViewState["ContractID"] = conTerminateBill.ContractID;
                txtNewConStartDate.Text = conTerminateBill.TermDate.ToString("yyyy-MM-dd");
                txtCustCode.Text = ViewState["ContractID"].ToString();
                GetContractInfo(Convert.ToInt32(ViewState["ContractID"]));
            }
        }
    }
    #region 获取合同信息
    protected void GetContractInfo(int contractCode)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = '" + contractCode + "'";
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
        
            //ViewState["contractID"] = Convert.ToInt32(Request["VoucherID"]);
            //ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
            //ViewState["ConStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            //ViewState["chargeStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            //ViewState["ConEndDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            //ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();
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

    protected void butConsent_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
        int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
        int sequence = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["Sequence"]);
        int voucherID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["VoucherID"]);
        String voucherHints = ViewState["ConTermD"].ToString();
        String voucherMemo = ViewState["ConTermD"].ToString();
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;

        ContractTerminateBill contractTerminateBill = new ContractTerminateBill();
        contractTerminateBill.ConEndDate = Convert.ToDateTime(txtNewConStartDate.Text);
        baseBO.WhereClause ="ContractID=" + ViewState["ContractID"];
        if (baseBO.Update(contractTerminateBill)<1)
        {
 
        }

        VoucherInfo vInfo = new VoucherInfo(Convert.ToInt32(ViewState["ContractID"]), voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo);
    }
}
