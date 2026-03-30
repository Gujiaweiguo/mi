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
using Lease.ConTerminateBill;
using Base.Page;
using Base.Util;
using Lease.Subs;

public partial class Lease_LeaseContractStop_LeaseContractStopAuditing : BasePage
{
    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息
    public string espression;  //结算公式
    public string titleName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ListBind();

            if (Request.QueryString["VoucherID"] != null)
            {

                ViewState["ConTerID"] = Convert.ToInt32(Request["VoucherID"]);
                SelContractInfo("ConTerID = " + Convert.ToInt32(ViewState["ConTerID"]));

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConOverTimeID", Request.QueryString["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                cookies.Values.Add("conID", ViewState["ContractID"].ToString());
                Response.AppendCookie(cookies);

            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0)
            {
                ViewState["ConTerID"] = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
                SelContractInfo("ConTerID=" + Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
            }
            titleName = (String)GetGlobalResourceObject("BaseInfo", "Menu_LeaseContractStop");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance"); 
        }
    }

    private void SelContractInfo(string strSql)
    {
        ConTerminateBill conTerminateBill = new ConTerminateBill();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = strSql;
        rs = baseBO.Query(conTerminateBill);

        if (rs.Count > 0)
        {
            conTerminateBill = rs.Dequeue() as ConTerminateBill;

            ViewState["ContractID"] = conTerminateBill.ContractID;
            cmbTradeID.SelectedValue = conTerminateBill.TradeID.ToString();
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(conTerminateBill.ContractStatus));
            txtContractCode.Text = conTerminateBill.ContractCode;
            txtRefID.Text = conTerminateBill.RefID.ToString();
            txtConStartDate.Text = conTerminateBill.ConStartDate.ToString("yyyy-MM-dd");
            txtConEndDate.Text = conTerminateBill.ConEndDate.ToString("yyyy-MM-dd");
            DDownListPenalty.SelectedValue = conTerminateBill.Penalty.ToString();
            DDownListTerm.SelectedValue = conTerminateBill.Notice.ToString();
            txtBargain.Text = conTerminateBill.EConURL;
            listBoxAddItem.Text = conTerminateBill.AdditionalItem;
            txtNode.Text = conTerminateBill.Note;
            txtStopDate.Text = conTerminateBill.TerDate.ToString("yyyy-MM-dd");
            txtTerReason.Text = conTerminateBill.TerReason;
            setDdl(conTerminateBill.ContractID);
        }
    }
    protected void butConsent_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseTrans baseTrans = new BaseTrans();

        int operatorID = sessionUser.UserID;
        int deptID = sessionUser.DeptID;
        int wrkFlwID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]));
        int nodeID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]));
        int sequence = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]));

        try
        {
            baseTrans.BeginTrans();

            VoucherInfo vInfo = new VoucherInfo(Convert.ToInt32(ViewState["ConTerID"]), txtContractCode.Text, txtVoucherMemo.Text.Trim(), deptID, operatorID);
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

            baseTrans.Commit();

            butConsent.Enabled = false;
            butOverrule.Enabled = false;

            ClearCookies();

            ClearText();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("审批租赁合同终约错误:", ex);
            baseTrans.Rollback();
        }
    }
    protected void butOverrule_Click(object sender, EventArgs e)
    {
        try
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;
            int wrkFlwID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]));
            int nodeID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]));
            int sequence = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]));
            int voucherID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
            String voucherHints = txtContractCode.Text.Trim();
            String voucherMemo = txtVoucherMemo.Text.Trim();

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);

            ClearCookies();

            ClearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回租金公式错误:", ex);
        }
    }

    private void ClearCookies()
    {
        /*清除合同Cookies 合同ID,工作流ID,节点ID,单据ID*/
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddDays(1);
        cookies.Values.Add("conID", "");
        cookies.Values.Add("wrkFlwID", "");
        cookies.Values.Add("sequence", "");
        cookies.Values.Add("nodeID", "");
        cookies.Values.Add("Disprove", "");
        cookies.Values.Add("ConOverTimeID", "");
        cookies.Values.Add("ReturnSequence", "");
        Response.AppendCookie(cookies);
    }

    private void ClearText()
    {
        cmbTradeID.SelectedIndex = 0;
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        DDownListPenalty.SelectedIndex = 0;
        DDownListTerm.SelectedIndex = 0;
        txtStopDate.Text = "";
        txtBargain.Text = "";
        listBoxAddItem.Text = "";
        txtNode.Text = "";
        txtTerReason.Text = "";
    }

    protected void ListBind()
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        /*绑定二级经营类别*/
        baseBO.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        rs = baseBO.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
        {
            cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        }
        baseBO.WhereClause = "";

        /*提前终止处罚*/
        int[] statusPenalty = Contract.GetPenaltyTypeStatus();
        int sPenalty = statusPenalty.Length;
        for (int i = 0; i < sPenalty; i++)
        {
            DDownListPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetPenaltyTypeStatusDesc(statusPenalty[i])), statusPenalty[i].ToString()));
        }

        /*绑定终约通知期限*/
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTerm.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetNoticeDesc(status[i])), status[i].ToString()));
        }

        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new Subs(), "SubsID", "SubsName", this.ddlSubs);
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new ContractType(), "ContractTypeID", "ContractTypeName", this.ddlContractType);
    }

    private void setDdl(int contractID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ContractID =" + contractID;
        DataSet contractDs = baseBO.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0)
        {
            this.ddlContractType.SelectedValue = (contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString());
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());
        }
    }
}
