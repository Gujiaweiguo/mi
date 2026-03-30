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
using Lease.SMSPara;
using Lease.Subs;

public partial class Lease_AdContract_AreaContractPalaver : BasePage
{
    #region 定义
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    BaseTrans baseTrans = new BaseTrans();
    #endregion

    #region
    public string baseInfo;  //基本信息
    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息
    public string espression;  //结算公式
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //初始化DropDownList
            BindPenalty();
            BindNotice();
            BindSubCompany();
            /*获取合同信息
           存入cookies*/
            int contractID = 0;
            if (Request["VoucherID"] != null)
            {
                contractID = Convert.ToInt32(Request["VoucherID"]);
                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", Request["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                Response.AppendCookie(cookies);
            }
            else if (Request.Cookies["Info"].Values["conID"] != "")
            {
                contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }
            GetContractInfo(contractID);

            if (Request.QueryString["WrkFlwID"] != null)
            {
                HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
            }
            else
            {
                HidenWrkID.Value = Request.Cookies["Info"].Values["wrkFlwID"].ToString();
            }
            if (Request.QueryString["VoucherID"] != null)
            {
                HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
            }
            else
            {
                HidenVouchID.Value = Request.Cookies["Info"].Values["conID"].ToString();
            }
            btnMessage.Attributes.Add("onclick", "ShowMessage()");

            ViewState["contractID"] = contractID;
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblAreaInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance");
        }
    }

    #region 初始化DropDownList

    protected void BindPenalty()
    {
        int[] status = Contract.GetPenaltyTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetPenaltyTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    private void BindSubCompany()
    {
        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new Subs(), "SubsID", "SubsName", this.ddlSubs);
    }

    //绑定终约通知期限
    protected void BindNotice()
    {
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTerm.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetNoticeDesc(status[i])), status[i].ToString()));
        }
    }
    #endregion

    #region 获取合同信息
    protected void GetContractInfo(int contractID)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + contractID;
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
                txtCustCode.Text = custDS.Tables[0].Rows[0]["CustCode"].ToString();
                txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
                txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustShortName"].ToString();
            }
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"])));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            txtChargeStart.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            //listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();


            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            txtNode.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());

            txtNorentDays.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();
            //获取招商员姓名




            baseBo.WhereClause = "";
            baseBo.WhereClause = "UserID = " + contractDs.Tables[0].Rows[0]["CommOper"];
            Resultset rs = baseBo.Query(new Users());
            if (rs.Count > 0)
            {
                Users user = rs.Dequeue() as Users;
                txtCommOper.Text = user.UserName;
            }

            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
        }
    }
    #endregion
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        int wrkFlwID = 0;
        int nodeID = 0;
        int sequence = 0;
        if (Request.QueryString["WrkFlwID"] != null)
        {
            wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        }
        else
        {
            wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
        }

        if (Request.QueryString["NodeID"] != null)
        {
            nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        }
        else
        {
            nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
        }

        if (Request.QueryString["Sequence"] != null)
        {
            sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        }
        else
        {
            sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
        }

        int deptID = objSessionUser.DeptID;
        //int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        //int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        //int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);

        String voucherHints = txtCustName.Text.Trim();
        String voucherMemo = listBoxRemark.Text.Trim();
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        int operatorID = objSessionUser.UserID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.SmtToMgr(wrkFlwID, nodeID, sequence, vInfo);
        Response.Write("<script language=javascript>alert('操作成功!!');</script>");
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";
        BaseTrans basetrans = new BaseTrans();

        int wrkFlwID = 0;
        int nodeID = 0;
        int sequence = 0;
        if (Request.QueryString["WrkFlwID"] != null)
        {
            wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        }
        else
        {
            wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
        }

        if (Request.QueryString["NodeID"] != null)
        {
            nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        }
        else
        {
            nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
        }

        if (Request.QueryString["Sequence"] != null)
        {
            sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        }
        else
        {
            sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
        }


        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

        basetrans.BeginTrans();

        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, basetrans);

        basetrans.Commit();
        ClearPage();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PalaverYes") + "'", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
    }
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        try
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = listBoxRemark.Text;

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);
            ClearPage();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回审批信息错误:", ex);
        }
    }

    private void ClearPage()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        txtChargeStart.Text = "";
        txtNorentDays.Text = "";
        txtBargain.Text = "";
        DDownListTerm.SelectedIndex = 0;
        DDownListPenalty.SelectedIndex = 0;
        txtCommOper.Text = "";
        listBoxRemark.Text = "";
    }
}
