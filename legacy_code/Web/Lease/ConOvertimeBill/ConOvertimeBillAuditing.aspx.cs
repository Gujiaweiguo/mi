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

public partial class Lease_ConOvertimeBill_ConOvertimeBillAuditing : BasePage
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
        //Page.Response.Buffer = false;
        //Page.Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            if (Request.QueryString["VoucherID"] != null)
            {

                ViewState["ConOverTimeID"] = Convert.ToInt32(Request["VoucherID"]);
                SelContractInfo(Convert.ToInt32(ViewState["ConOverTimeID"]));

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConOverTimeID", Request.QueryString["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                cookies.Values.Add("conID", ViewState["ContractID"].ToString());
                Response.AppendCookie(cookies);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

            }
            else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "")
            {
                ViewState["ConOverTimeID"] = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
                SelContractInfo(Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
            }

            inserStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            titleName = (String)GetGlobalResourceObject("BaseInfo", "ConTerminateBill_ConTerminateBill");
            changeLease = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_NotUpdate");
            changeLeaseNow = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_Update");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError");
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
    //#region 获取合同信息
    //protected void GetContractInfo(string contractCode)
    //{
    //    BaseBO baseBO = new BaseBO();
    //    baseBO.WhereClause = "ContractCode = '" + contractCode + "'";
    //    DataSet contractDs = baseBO.QueryDataSet(new Contract());
    //    if (contractDs.Tables[0].Rows.Count > 0)
    //    {
    //        int flag = Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"]);
    //        if (flag == Contract.CONTRACTSTATUS_TYPE_FIRST)
    //        {
    //            ViewState["myFlag"] = "Inserted";
    //        }
    //        else if (flag == Contract.CONTRACTSTATUS_TYPE_TEMP)
    //        {
    //            ViewState["myFlag"] = "Updated";
    //        }
    //        int custId = Convert.ToInt32(contractDs.Tables[0].Rows[0]["CustID"]);
    //        baseBO.WhereClause = "";
    //        baseBO.WhereClause = "CustID = " + custId;
    //        DataSet custDS = baseBO.QueryDataSet(new Customer());
    //        if (custDS.Tables[0].Rows.Count > 0)
    //        {
    //            txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
    //            txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustShortName"].ToString();
    //        }
    //        cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"])));
    //        txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
    //        txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
    //        listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

    //        cmbTradeID.SelectedValue = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
    //        txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
    //        txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
    //        DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
    //        DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
    //        txtNewConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).AddDays(1).ToString("yyyy-MM-dd");
    //        this.ddlContractType.SelectedValue = (contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString());
    //        this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());
            
    //        StringBuilder sb = new StringBuilder();

    //        ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "';", true);
    //    }
    //}
    //#endregion


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
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_IN)
        {
            InsertConOverTime();
        }
        else
        {
            UpdateConOverTime();
        }
    }
    protected void btnOverTime_Click(object sender, EventArgs e)
    {
        try
        {
            int voucherID = 0;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["NodeID"]);

            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            ClearCookies();
            ClearText();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
    }

    private void SelContractInfo(int conOverTimeID)
    {
        ConOvertimeBill conOvertimeBill = new ConOvertimeBill();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = "a.ContractID=b.ContractID and b.CustID=c.CustID and ConOverTimeID=" + conOverTimeID;
        rs = baseBO.Query(conOvertimeBill);

        if (rs.Count > 0)
        {
            conOvertimeBill = rs.Dequeue() as ConOvertimeBill;
            ViewState["ContractID"] = conOvertimeBill.ContractID;
            txtCustName.Text = conOvertimeBill.CustName;
            txtCustShortName.Text = conOvertimeBill.CustShortName;
            cmbTradeID.SelectedValue = conOvertimeBill.TradeID.ToString();
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(conOvertimeBill.ContractStatus));
            txtContractCode.Text = conOvertimeBill.ContractCode;
            txtRefID.Text = conOvertimeBill.RefID.ToString();
            txtConStartDate.Text = conOvertimeBill.ConStartDate.ToString("yyyy-MM-dd");
            txtConEndDate.Text = conOvertimeBill.ConEndDate.ToString("yyyy-MM-dd");
            DDownListPenalty.SelectedValue = conOvertimeBill.Penalty.ToString();
            DDownListTerm.SelectedValue = conOvertimeBill.Notice.ToString();
            txtNewConStartDate.Text = conOvertimeBill.NewConStartDate.ToString("yyyy-MM-dd");
            txtNewConEndDate.Text = conOvertimeBill.NewConEndDate.ToString("yyyy-MM-dd");
            txtBargain.Text = conOvertimeBill.EConURL;
            listBoxAddItem.Text = conOvertimeBill.AdditionalItem;
            listBoxRemark.Text = conOvertimeBill.Note;
            setDdl(conOvertimeBill.ContractID);
        }
    }

    private void InsertConOverTime()
    {
        BaseTrans baseTrans = new BaseTrans();
        ConOvertimeBill conOvertimeBill = new ConOvertimeBill();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";

        try
        {
            conOvertimeBill.ConOverTimeID = BaseApp.GetConOverTimeID();

            voucherID = conOvertimeBill.ConOverTimeID;
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
            int mywrkFlwID = 0;
            int mynodeID = 0;

            baseTrans.BeginTrans();
            conOvertimeBill.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            conOvertimeBill.CreateUserID = sessionUser.UserID;
            conOvertimeBill.CreateTime = DateTime.Now;
            conOvertimeBill.ModifyUserID = sessionUser.UserID;
            conOvertimeBill.ModifyTime = DateTime.Now;
            conOvertimeBill.OprRoleID = sessionUser.RoleID;
            conOvertimeBill.OprDeptID = sessionUser.DeptID;
            conOvertimeBill.PrvEndDate = Convert.ToDateTime(txtConEndDate.Text);
            conOvertimeBill.NewConEndDate = Convert.ToDateTime(txtNewConEndDate.Text);
            conOvertimeBill.NewConStartDate = Convert.ToDateTime(txtNewConStartDate.Text);
            conOvertimeBill.AdditionalItem = listBoxAddItem.Text.Trim();
            conOvertimeBill.EConURL = txtBargain.Text;
            conOvertimeBill.Operator = sessionUser.UserID;
            conOvertimeBill.RefID = txtRefID.Text.Trim();
            conOvertimeBill.Note = listBoxRemark.Text.Trim();
            if (baseTrans.Insert(conOvertimeBill) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
                baseTrans.Rollback();
                return;
            }

            if (Request.QueryString["WrkFlwID"] != null)
            {
                mywrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            }
            else
            {
                mywrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            }

            if (Request.QueryString["NodeID"] != null)
            {
                mynodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            }
            else
            {
                mynodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
            }


            WrkFlwApp.CommitVoucherDraft(mywrkFlwID, mynodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);

            if (Request["WrkFlwID"] != null)
            {
                HttpCookie cookies = new HttpCookie("Info");

                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", ViewState["ContractID"].ToString());
                cookies.Values.Add("wrkFlwID", mywrkFlwID.ToString());
                cookies.Values.Add("nodeID", mynodeID.ToString());
                cookies.Values.Add("ConOverTimeID", conOvertimeBill.ConOverTimeID.ToString());
                cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());

                Response.AppendCookie(cookies);

            }

            HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            baseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
            baseTrans.Rollback();
        }
    }

    private void UpdateConOverTime()
    {
        BaseBO baseBO = new BaseBO();
        ConOvertimeBill conOvertimeBill = new ConOvertimeBill();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        baseBO.WhereClause = "ConOverTimeID=" + ViewState["ConOverTimeID"];

        conOvertimeBill.CreateUserID = sessionUser.UserID;
        conOvertimeBill.CreateTime = DateTime.Now;
        conOvertimeBill.ModifyUserID = sessionUser.UserID;
        conOvertimeBill.ModifyTime = DateTime.Now;
        conOvertimeBill.OprRoleID = sessionUser.RoleID;
        conOvertimeBill.OprDeptID = sessionUser.DeptID;
        conOvertimeBill.PrvEndDate = Convert.ToDateTime(txtConEndDate.Text);
        conOvertimeBill.NewConEndDate = Convert.ToDateTime(txtNewConEndDate.Text);
        conOvertimeBill.NewConStartDate = Convert.ToDateTime(txtNewConStartDate.Text);
        conOvertimeBill.AdditionalItem = listBoxAddItem.Text.Trim();
        conOvertimeBill.EConURL = txtBargain.Text;
        conOvertimeBill.Operator = sessionUser.UserID;
        conOvertimeBill.RefID = txtRefID.Text.Trim();
        conOvertimeBill.Note = listBoxRemark.Text.Trim();

        if (baseBO.Update(conOvertimeBill) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
            return;
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

            VoucherInfo vInfo = new VoucherInfo(Convert.ToInt32(ViewState["ConOverTimeID"]), txtCustShortName.Text, txtVoucherMemo.Text, deptID, operatorID);
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo,baseTrans);

            baseTrans.Commit();

            butOk.Enabled = false;
            butBlankOut.Enabled = false;

            ClearCookies();

            ClearText();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("审批租金公式错误:", ex);
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
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = txtVoucherMemo.Text.Trim();

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

    private void ClearText()
    {
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        cmbTradeID.SelectedIndex = 0;
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        DDownListPenalty.SelectedIndex = 0;
        DDownListTerm.SelectedIndex = 0;
        txtNewConStartDate.Text = "";
        txtNewConEndDate.Text = "";
        txtBargain.Text = "";
        listBoxAddItem.Text = "";
        listBoxRemark.Text = "";
        txtVoucherMemo.Text = "";
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
