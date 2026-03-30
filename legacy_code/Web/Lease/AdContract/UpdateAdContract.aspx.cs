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
using Lease.AdContract;
using Lease.Subs;

public partial class Lease_AdContract_UpdateAdContract : BasePage
{
    #region 定义
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    BaseTrans baseTrans = new BaseTrans();
    #endregion

    #region 提示信息
    public string contractCode;        //请输入合同号!
    public string contractBeginDate;  //请输入合同开始时间!
    public string contractEndDate;    //请输入合同结束时间!
    public string conLeaseTradeID;         //请选择经营类别!

    public string beginEndDate;
    public string beginChargeDate;
    public string billOfDocumentDelete;

    #endregion

    #region
    public string baseInfo;  //基本信息
    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息
    public string espression;  //结算公式
    #endregion
    private static int DISPROVE_IN = 1;
    private static int DISPROVE_UP = 2;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            beginChargeDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_DateTime");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            
            //初始化DropDownList

            BindPenalty();
            BindNotice();
            BindSubCompany();
            int contractID;


            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();

            ds = baseBO.QueryDataSet("Select AutoContractCode From SMSPara");
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["AutoContractCode"]) == SMSPara.AUTO_YES)
            {
                txtContractCode.ReadOnly = true;

                ViewState["Lock"] = SMSPara.AUTO_YES;
            }
            else
            {
                
                ViewState["Lock"] = SMSPara.AUTO_NO;
            }

            if (Request.QueryString["VoucherID"] != null)
            {
                contractID = Convert.ToInt32(Request["VoucherID"]);
                ViewState["contractID"] = contractID;

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", Request["VoucherID"].ToString());
                cookies.Values.Add("modify", Request["modify"].ToString());
                Response.AppendCookie(cookies);

                GetContractInfo(contractID);

                /*保存Cookies驳回和草稿状态*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

              
               
            }
            else if (Request.Cookies["Info"].Values["conID"] != "")
            {
                contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
                ViewState["contractID"] = contractID;
                GetContractInfo(contractID);

                /*保存Cookies驳回和草稿状态*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");
                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

            }
            contractCode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_contractCode");
            contractBeginDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractBeginDate");
            contractEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractEndDate");
            conLeaseTradeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
            //txtRefID.Attributes.Add("onkeydown", "textleave()");
            //txtTradeID.Attributes.Add("onclick", "ShowTree()");

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblADInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance");
        }
    }

    #region

    protected void BindPenalty()
    {

        SessionUser sessionUsers = (SessionUser)Session["UserAccessInfo"];
        BaseBO baseBO = new BaseBO();
        //绑定提前终约处罚否


        int[] status = Contract.GetPenaltyTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetPenaltyTypeStatusDesc(status[i])), status[i].ToString()));
        }

        /*招商员选择列表*/
        //baseBO.WhereClause = "UserStatus=" + UserInfo.USER_STATUS_VALID;
        //baseBO.GroupBy = "a.userid,UserName,a.UserCode,WorkNo,OfficeTel,UserStatus";
        //rs = baseBO.Query(new UserInfo());
        //foreach (UserInfo user in rs)
        //{
        //    cmbCommOper.Items.Add(new ListItem(user.UserName, user.UserID.ToString()));
        //}
        //cmbCommOper.SelectedValue = sessionUsers.UserID.ToString();
        //string str_sql = "select A.UserID,A.UserName from Users A,UserRole B where A.UserID = B.UserID and A.UserStatus = " + UserInfo.USER_STATUS_VALID + " and B.DeptID in (" + 109 + "," + 110 + "," + 111 + ")";
        //baseBO.WhereClause = "";
        //DataSet usersDS = baseBO.QueryDataSet(str_sql);
        //int count = usersDS.Tables[0].Rows.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    cmbCommOper.Items.Add(new ListItem(usersDS.Tables[0].Rows[i]["UserName"].ToString(), usersDS.Tables[0].Rows[i]["UserID"].ToString()));
        //}
        string str_sql = "select userID,userName from SignPerson";
        baseBO.WhereClause = "";
        DataSet usersDS = baseBO.QueryDataSet(str_sql);
        int count = usersDS.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            cmbCommOper.Items.Add(new ListItem(usersDS.Tables[0].Rows[i]["UserName"].ToString(), usersDS.Tables[0].Rows[i]["UserID"].ToString()));
        }
        cmbCommOper.SelectedValue = sessionUsers.UserID.ToString();
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
        DDownListTerm.SelectedIndex = 3;
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
            ViewState["shopFlag"] = "modify";

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
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();
            txtNorentDays.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();
            cmbCommOper.SelectedValue = contractDs.Tables[0].Rows[0]["CommOper"].ToString();

            //hidTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            //SelectTradeID();

            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());


            StringBuilder sb = new StringBuilder();
            if (contractDs.Tables[0].Rows[0]["Note"].ToString() != "")
            {
                sb.Append(contractDs.Tables[0].Rows[0]["Note"].ToString());
            }
            
            listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : sb.ToString());

            ViewState["contractID"] = contractID;
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
            ViewState["ConStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            ViewState["chargeStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            ViewState["ConEndDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
        }
    }
    #endregion

    private void BindSubCompany()
    {
        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new Subs(), "SubsID", "SubsName", this.ddlSubs);
    }
   

    #region 基本合同项目保存草稿
    protected void SaveBaseBargain()
    {
        int voucherID = 0;
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        try
        {
            BaseBO baseBO = new BaseBO();
            DataSet dsContract = new DataSet();
            Contract contact = new Contract();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractCode = '" + txtContractCode.Text.Trim() + "'";
            rs = baseBo.Query(contact);

            if (rs.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ContractCode") + "'", true);
                return;
            }

            ds.Clear();
            //baseBo.WhereClause = "";
            //baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            //ds = baseBo.QueryDataSet(new TradeRelation());

            baseTrans.BeginTrans();

            contact.ContractID = BaseApp.GetContractID();
            contact.CustID = Convert.ToInt32(ViewState["custId"]);

            if (Convert.ToInt32(ViewState["Lock"]) == SMSPara.AUTO_YES)
            {
                contact.ContractCode = BaseApp.GetSMSParaNextContractCode().ToString();
            }
            else
            {
                contact.ContractCode = txtContractCode.Text.Trim();
            }

            contact.RefID = txtRefID.Text.Trim();
            contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
            contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
            contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
            //contact.TradeID = Convert.ToInt32(hidTradeID.Text);
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_INGEAR);
            contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contact.AdditionalItem = listBoxAddItem.Text;
            contact.EConURL = txtBargain.Text;
            contact.Note = listBoxRemark.Text;
            //contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);
            contact.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue);
            contact.SigningMode = Convert.ToChar(Contract.CONTRACTSTATUS_TYPE_N);
            contact.BizMode = Contract.BIZ_MODE_AdBoard;
            contact.SubsID = Int32.Parse(this.ddlSubs.SelectedValue);

            if (baseTrans.Insert(contact) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                baseTrans.Rollback();
                return;
            }

            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);

            voucherID = contact.ContractID;

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);

            WrkFlwApp.CommitVoucherDraft(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);

            baseTrans.Commit();

            /*保存草稿提交的节点ID*/
            HttpCookie cookies = new HttpCookie("Info");

            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("wrkFlwID", wrkFlwID.ToString());
            cookies.Values.Add("nodeID", nodeID.ToString());
            cookies.Values.Add("conID", contact.ContractID.ToString());
            cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
            Response.AppendCookie(cookies);

            /*保存Cookies驳回和草稿状态*/
            HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "    " + (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID") + ":" + contact.ContractCode + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
            baseTrans.Rollback();
        }
    }


    protected void UpdateBaseBargain()
    {
        try
        {
            BaseBO baseBO = new BaseBO();
            Contract contact = new Contract();
            baseBO.WhereClause = "";
            baseBO.WhereClause = "ContractCode = '" + txtContractCode.Text.Trim() + "'";
            rs = baseBO.Query(contact);

            if (rs.Count > 0)
            {
                contact = rs.Dequeue() as Contract;

                if (Convert.ToInt32(ViewState["contractID"]) != contact.ContractID)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ContractCode") + "'", true);
                    return;
                }
            }

            ds.Clear();
            //baseBo.WhereClause = "";
            //baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            //ds = baseBo.QueryDataSet(new TradeRelation());

            contact.CustID = Convert.ToInt32(ViewState["custId"]);
            contact.ContractCode = txtContractCode.Text;
            contact.RefID = txtRefID.Text.Trim();
            contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
            contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
            contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
            //contact.TradeID = Convert.ToInt32(hidTradeID.Text);
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_INGEAR);
            contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
            contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contact.AdditionalItem = listBoxAddItem.Text;
            contact.EConURL = txtBargain.Text;
            contact.Note = listBoxRemark.Text;
            //contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);
            contact.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue);
            contact.SubsID = Int32.Parse(this.ddlSubs.SelectedValue);

            baseBO.WhereClause = "ContractID=" + ViewState["contractID"];

            if (baseBO.Update(contact) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("更新合同再签信息错误:", ex);
        }
    }

    #endregion
    private void ClearPage()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //hidTradeID.Text = "";
        //txtTradeID.Text = "";
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
        listBoxRemark.Text = "";
        listBoxAddItem.Text = "";
    }
    protected void btnBindDealType_Click(object sender, EventArgs e)
    {
        //SelectTradeID();
    }

    private void SelectTradeID()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "TradeID = " + Convert.ToInt32(hidTradeID.Text);
        rs = baseBo.Query(new TradeRelation());
        if (rs.Count == 1)
        {
            TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
            //txtTradeID.Text = tradeRelation.TradeName;
        }
        baseBo.WhereClause = "";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_IN)
        {
            SaveBaseBargain();
        }
        else if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_UP)
        {
            UpdateBaseBargain();
        }
      
    }
}
