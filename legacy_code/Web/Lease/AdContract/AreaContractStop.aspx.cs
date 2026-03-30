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

public partial class Lease_AdContract_AreaContractStop : BasePage
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
    public string billOfDocumentDelete;

    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息
    public string espression;  //结算公式

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Response.Buffer = false;
        //Page.Response.Cache.SetNoStore();
        if (!IsPostBack)
        {

            btnTempSave.Attributes.Add("onclick", "return InputValidator(this)");
            btnMessage.Attributes.Add("onclick", "ShowMessage()");
            if (Request.QueryString["VoucherID"] != null)
            {
                ViewState["ConTerID"] = Convert.ToInt32(Request["VoucherID"]);
                SelContractInfo("ConTerID = " + Convert.ToInt32(ViewState["ConTerID"]));

                /*把工作流ID和节点ID存入Cookies*/

                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                cookiesWorkFlow.Values.Add("SequenceID", Request.QueryString["Sequence"]);
                Response.AppendCookie(cookiesWorkFlow);

                ////////
                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConOverTimeID", Request.QueryString["VoucherID"].ToString());
                cookies.Values.Add("conID", ViewState["ContractID"].ToString());
                //cookies.Expires = System.DateTime.Now.AddHours(1);
                cookies.Values.Add("modify", "1");
                Response.AppendCookie(cookies);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

                btnQueryContract.Enabled = false;
                btnOverTime.Enabled = true;
                btnTempSave.Enabled = true;
                this.btnBalnkOut.Enabled = true;
                this.btnMessage.Enabled = true;

                //HttpCookie cookiesCustumer = new HttpCookie("Info");

                //cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                //cookiesCustumer.Values.Add("modify", "1");
                //Response.AppendCookie(cookiesCustumer);

                if (Request.QueryString["WrkFlwID"] != null)
                {
                    HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
                }
                else
                {
                    HidenWrkID.Value = Request.Cookies["WorkFlow"].Values["WorkFlowID"].ToString();
                }
                if (Request.QueryString["VoucherID"] != null)
                {
                    HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
                }
                else
                {
                    HidenVouchID.Value = Request.Cookies["Info"].Values["ConOverTimeID"].ToString();
                }
            }
            else if (Request.QueryString["Type"] == "New")
            {
                /*删除Cookies续约ID*/
                HttpCookie cookiesCustumer = new HttpCookie("Info");

                cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                cookiesCustumer.Values.Add("ConOverTimeID", "");
                cookiesCustumer.Values.Add("conID", "");
                cookiesCustumer.Values.Add("modify", "1");
                Response.AppendCookie(cookiesCustumer);
                /*把工作流ID和节点ID存入Cookies*/
                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                cookiesWorkFlow.Values.Add("SequenceID", "");
                cookiesWorkFlow.Values.Add("ConOverTimeID", "");
                Response.AppendCookie(cookiesWorkFlow);
                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");
                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                Response.AppendCookie(cookiesDisprove);
            }
            else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0)
            {
                ViewState["ConTerID"] = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
                SelContractInfo("ConTerID=" + Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
                //cmbContractStatus.Text = Request.Cookies["Infuo"].Values["conStatus"];
                btnQueryContract.Enabled = false;
                btnOverTime.Enabled = true;
                btnTempSave.Enabled = true;
                btnQueryContract.Enabled = false;
                this.btnMessage.Enabled = true;
                this.btnBalnkOut.Enabled = true;

                if (Request.QueryString["WrkFlwID"] != null)
                {
                    HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
                }
                else
                {
                    HidenWrkID.Value = Request.Cookies["WorkFlow"].Values["WorkFlowID"].ToString();
                }
                if (Request.QueryString["VoucherID"] != null)
                {
                    HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
                }
                else
                {
                    HidenVouchID.Value = Request.Cookies["Info"].Values["ConOverTimeID"].ToString();
                }
            }
            else if (Request.Cookies["Info"].Values["conID"].ToString() != "")
            {
                GetContractInfo("ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]) + " And ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR);
                btnOverTime.Enabled = true;
                btnTempSave.Enabled = true;
                this.btnMessage.Enabled = true;
            }

            inserStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            titleName = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo");
            changeLease = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_NotUpdate");
            changeLeaseNow = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_Update");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblAreaInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance");

            btnBalnkOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
            ListBind();
            BindSubCompany();
        }
    }

    protected void butAuditing_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        voucherID = Convert.ToInt32(ViewState["ConTerID"]);
        String voucherHints = ViewState["ContractID"].ToString();
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "Close();", true);
    }

    protected void btnQueryContract_Click(object sender, EventArgs e)
    {
        GetContractInfo("ContractCode = '" + txtCustCode.Text.Trim() + "' And ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR);
    }
    private void BindSubCompany()
    {
        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new Subs(), "SubsID", "SubsName", this.ddlSubs);
    }
    #region 获取合同信息
    protected void GetContractInfo(string sqlStr)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = sqlStr;
        DataSet contractDs = baseBO.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0)
        {

            if (Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"]) != Contract.CONTRACTSTATUS_TYPE_INGEAR)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotContract") + "';", true);
                return;
            }

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
            baseBO.WhereClause = "";
            baseBO.WhereClause = "CustID = " + custId;
            DataSet custDS = baseBO.QueryDataSet(new Customer());
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"])));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

            //cmbTradeID.SelectedValue = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            //txtNewConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).AddDays(1).ToString("yyyy-MM-dd");
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());

            StringBuilder sb = new StringBuilder();

            ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();

            btnTempSave.Enabled = true;

            /*把合同ID放入到Cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", ViewState["ContractID"].ToString());
            Response.AppendCookie(cookies);

        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "';", true);
        }
    }
    #endregion
    protected void ListBind()
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        /*绑定二级经营类别*/
        //baseBO.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //rs = baseBO.Query(new TradeRelation());
        //foreach (TradeRelation tradeDef in rs)
        //{
        //    cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        //}
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
            String voucherHints = txtContractCode.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0 && Request.Cookies["Info"].Values["ReturnSequence"] != "")
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["WorkFlow"].Values["SequenceID"]) != 0 && Request.Cookies["WorkFlow"].Values["SequenceID"] != "")
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["WorkFlow"].Values["SequenceID"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ClearCookies();
            ClearText();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
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
            //cmbTradeID.SelectedValue = conTerminateBill.TradeID.ToString();
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
            txtStopDate.Text = (conTerminateBill.TerDate.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "" : conTerminateBill.TerDate.ToString("yyyy-MM-dd"));
            txtTerReason.Text = conTerminateBill.TerReason;
        }
    }

    private void InsertConOverTime()
    {
        BaseTrans baseTrans = new BaseTrans();
        ConTerminateBill conTerminateBill = new ConTerminateBill();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        String voucherHints = txtContractCode.Text.Trim();
        String voucherMemo = "";

        try
        {
            baseTrans.BeginTrans();

            conTerminateBill.ConTerID = BaseApp.GetConTerID();

            voucherID = conTerminateBill.ConTerID;
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
            int mywrkFlwID = 0;
            int mynodeID = 0;

            conTerminateBill.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            conTerminateBill.CreateUserID = sessionUser.UserID;
            conTerminateBill.CreateTime = DateTime.Now;
            conTerminateBill.ModifyUserID = sessionUser.UserID;
            conTerminateBill.ModifyTime = DateTime.Now;
            conTerminateBill.OprRoleID = sessionUser.RoleID;
            conTerminateBill.OprDeptID = sessionUser.DeptID;
            conTerminateBill.TerDate = Convert.ToDateTime(txtStopDate.Text);
            conTerminateBill.TerReason = txtTerReason.Text.Trim();
            conTerminateBill.Note = txtNode.Text.Trim();

            if (baseTrans.Insert(conTerminateBill) < 1)
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
                mywrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            }

            if (Request.QueryString["NodeID"] != null)
            {
                mynodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            }
            else
            {
                mynodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
            }


            WrkFlwApp.CommitVoucherDraft(mywrkFlwID, mynodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);


            /*将合同号，终止单号，工作流节点号保存到Cookies*/
            HttpCookie cookies = new HttpCookie("Info");

            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", ViewState["ContractID"].ToString());
            cookies.Values.Add("ConOverTimeID", conTerminateBill.ConTerID.ToString());
            cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());

            Response.AppendCookie(cookies);


            HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            btnOverTime.Enabled = true;
            btnQueryContract.Enabled = false;
            baseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加租赁合同终约信息错误:", ex);
            baseTrans.Rollback();
        }
    }

    private void UpdateConOverTime()
    {
        try
        {
            BaseBO baseBO = new BaseBO();
            ConTerminateBill conTerminateBill = new ConTerminateBill();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            baseBO.WhereClause = "ConTerID=" + ViewState["ConTerID"];

            conTerminateBill.ModifyUserID = sessionUser.UserID;
            conTerminateBill.ModifyTime = DateTime.Now;
            conTerminateBill.OprRoleID = sessionUser.RoleID;
            conTerminateBill.OprDeptID = sessionUser.DeptID;
            conTerminateBill.TerDate = Convert.ToDateTime(txtStopDate.Text);
            conTerminateBill.TerReason = txtTerReason.Text.Trim();
            conTerminateBill.Note = txtNode.Text.Trim();

            if (baseBO.Update(conTerminateBill) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                btnQueryContract.Enabled = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("更新租赁合同终约信息错误:", ex);
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

        HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

        cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
        cookiesWorkFlow.Values.Add("WorkFlowID", "");
        cookiesWorkFlow.Values.Add("NodeID", "");
        cookiesWorkFlow.Values.Add("SequenceID", "");
        Response.AppendCookie(cookiesWorkFlow);
    }

    private void ClearText()
    {
        txtCustCode.Text = "";
        //cmbTradeID.SelectedIndex = 0;
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
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        try
        {
            int voucherID = 0;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            String voucherHints = txtCustCode.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["NodeID"]);

            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ClearCookies();
            ClearText();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
    }
}
