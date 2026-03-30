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
using Base.Page;
using Base.Util;
using Lease.Subs;

public partial class Lease_ChangeLease_ContractChangeAuditing :BasePage
{
    DataSet ds = new DataSet();
    DataSet dsMod = new DataSet();
    DataTable dt = new DataTable();
    DataTable dtMod = new DataTable();
    DataSet DeductMoneyDS = new DataSet();
    DataTable DeductMoneyDT = new DataTable();
    DataSet KeepMinDS = new DataSet();
    DataTable KeepMinDT = new DataTable();
    public string inserStr = "";
    public string titleName = "";
    public string changeLease = "";
    public string changeLeaseNow = "";
    public string dateError = "";
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VoucherID"] != null)
            {
                ViewState["ConFormulaModID"] = Convert.ToInt32(Request.QueryString["VoucherID"]);
                SelContractInfo("ConFormulaModID = " + Convert.ToInt32(ViewState["ConFormulaModID"]));

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConFormulaModID", Request.QueryString["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                cookies.Values.Add("conID", ViewState["ContractID"].ToString());
                cookies.Values.Add("CustShortName", txtCustShortName.Text.ToString());
                Response.AppendCookie(cookies);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);
            }
            else if (Request.Cookies["Info"].Values["ConFormulaModID"] != "")
            {
                SelContractInfo("ConFormulaModID = " + Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]));
            }

            inserStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            titleName = (String)GetGlobalResourceObject("BaseInfo", "Lease_ExpressionMod");
            changeLease = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_NotUpdate");
            changeLeaseNow = (String)GetGlobalResourceObject("BaseInfo", "Lease_AddrentalFormulaMod");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            ListBind();
        }
    }

    protected void butAuditing_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        voucherID = Convert.ToInt32(ViewState["ConOverTimeID"]);
        String voucherHints = ViewState["ContractID"].ToString();
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "Close();", true);
    }

    protected void ListBind()
    {
        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        ///*绑定二级经营类别*/
        //baseBO.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //rs = baseBO.Query(new TradeRelation());
        //foreach (TradeRelation tradeDef in rs)
        //{
        //    cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        //}
        //baseBO.WhereClause = "";

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
    private void SelContractInfo(string whereStr)
    {
        CustContract custContract = new CustContract();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();

        baseBO.WhereClause = whereStr;
        ds = baseBO.QueryDataSet(custContract);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            txtCustShortName.Text = ds.Tables[0].Rows[0]["CustShortName"].ToString();
            SelectTradeID(Convert.ToInt32(ds.Tables[0].Rows[0]["TradeID"]));
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(ds.Tables[0].Rows[0]["ContractStatus"])));
            txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
            txtRefID.Text = ds.Tables[0].Rows[0]["RefID"].ToString();
            txtConStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            DDownListPenalty.SelectedValue = ds.Tables[0].Rows[0]["Penalty"].ToString();
            DDownListTerm.SelectedValue = ds.Tables[0].Rows[0]["Notice"].ToString();
            txtModReason.Text = ds.Tables[0].Rows[0]["ModReason"].ToString();
            ViewState["ContractID"] = ds.Tables[0].Rows[0]["ContractID"];
            this.ddlContractType.SelectedValue = (ds.Tables[0].Rows[0]["ContractTypeID"].ToString() == null ? "" : ds.Tables[0].Rows[0]["ContractTypeID"].ToString());
            this.ddlSubs.SelectedValue = (ds.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : ds.Tables[0].Rows[0]["SubsID"].ToString());
        }
    }
    protected void butConsent_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int conFormulaModID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]));
        int wrkFlwID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]));
        int nodeID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]));
        int sequence = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]));
        String voucherHints = txtCustShortName.Text;
        String voucherMemo = txtCustShortName.Text;
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;

        try
        {
            baseTrans.BeginTrans();

            VoucherInfo vInfo = new VoucherInfo(conFormulaModID, voucherHints, voucherMemo, deptID, operatorID);
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo,baseTrans);

            baseTrans.Commit();

            butConsent.Enabled = false;

            ClearCookies();

            ClearText();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加租金公式变更错误:", ex);
        }
    }
    private void ClearText()
    {
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        txtTradeID.Text = "";
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        DDownListPenalty.SelectedIndex = 0;
        DDownListTerm.SelectedIndex = 0;
        txtModReason.Text = "";
        txtOverrule.Text = "";
    }

    private void ClearCookies()
    {
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddDays(1);
        cookies.Values.Add("conID", "");
        cookies.Values.Add("wrkFlwID", "");
        cookies.Values.Add("sequence", "");
        cookies.Values.Add("nodeID", "");
        cookies.Values.Add("Disprove", "");
        cookies.Values.Add("ConOverTimeID", "");
        cookies.Values.Add("ReturnSequence", "");
        cookies.Values.Add("ConFormulaModID", "");
        cookies.Values.Add("CustShortName", "");
        Response.AppendCookie(cookies);
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
            int voucherID = Convert.ToInt32(Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]));
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = txtOverrule.Text.Trim();

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);

            ClearCookies();

            ClearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回租金公式错误:", ex);
        }
    }

    private void SelectTradeID(int tradeID)
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "TradeID = " + tradeID;
        rs = baseBo.Query(new TradeRelation());
        if (rs.Count == 1)
        {
            TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
            txtTradeID.Text = tradeRelation.TradeName;
        }
        else
        {
            txtTradeID.Text = "";
        }
    }
}

