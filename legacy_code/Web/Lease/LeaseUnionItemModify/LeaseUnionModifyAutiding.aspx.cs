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
using Lease.ContractMod;

public partial class Lease_LeaseUnionItemModify_LeaseUnionModifyAutiding : System.Web.UI.Page
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
            BindDealType();
            BindPenalty();
            BindNotice();

            /*获取合同信息
           存入cookies*/
            int conModID = 0;
            if (Request["VoucherID"] != null)
            {
                conModID = Convert.ToInt32(Request["VoucherID"]);
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
                conModID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            }
            GetContractInfo(conModID);
            ViewState["ConModID"] = conModID;
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasic");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblUnionItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance");
        }
    }

    #region 初始化DropDownList
    //绑定二级经营类别
    protected void BindDealType()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        rs = baseBo.Query(new TradeRelation());
        cmbTradeID.Items.Add(new ListItem(selected));
        foreach (TradeRelation tradeDef in rs)
            cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));

        baseBo.WhereClause = "";

    }

    //绑定提前终约处罚否
    protected void BindPenalty()
    {
        int[] status = Contract.GetPenaltyTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetPenaltyTypeStatusDesc(status[i])), status[i].ToString()));
        }
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
    protected void GetContractInfo(int conModID)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ConModID = " + conModID;
        DataSet contractDs = baseBo.QueryDataSet(new ContractMod());
        if (contractDs.Tables[0].Rows.Count > 0)
        {

            DataTable dt = baseBo.QueryDataSet("Select CustName,CustShortName From Contract a,Customer b Where a.CustID = b.CustID And a.ContractID = " + Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractID"])).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = dt.Rows[0]["CustName"].ToString();
                txtCustShortName.Text = dt.Rows[0]["CustShortName"].ToString();
            }

            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            txtChargeStart.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            cmbTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            txtNode.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
            txtNorentDays.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();
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

        String voucherHints = txtContractCode.Text.Trim();
        String voucherMemo = listBoxRemark.Text.Trim();
        int voucherID = Convert.ToInt32(ViewState["ConModID"]);
        int operatorID = objSessionUser.UserID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.SmtToMgr(wrkFlwID, nodeID, sequence, vInfo);
        //Response.Write("<script language=javascript>alert('操作成功!!');</script>");
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["ConModID"]);
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
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("联营合同相关信息修改驳回审批错误:", ex);
        }
    }

    private void ClearPage()
    {
        cmbTradeID.SelectedIndex = 0;
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        txtChargeStart.Text = "";
        txtNorentDays.Text = "";
        txtBargain.Text = "";
        DDownListTerm.SelectedIndex = 0;
        DDownListPenalty.SelectedIndex = 0;
        listBoxRemark.Text = "";
        listBoxAddItem.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
    }
}
