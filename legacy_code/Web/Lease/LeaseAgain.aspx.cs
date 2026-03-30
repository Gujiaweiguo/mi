using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Base;
using Lease;
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

public partial class Lease_LeaseAgain : BasePage
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
            contractCode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_contractCode");
            contractBeginDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractBeginDate");
            contractEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractEndDate");
            conLeaseTradeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            beginChargeDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_DateTime");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            btnBlankOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
            //初始化DropDownList
            BindPenalty();
            BindNotice();
            this.BindContractType();//绑定合同类型
            this.BindSubCompany();//绑定管理公司
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
                btnCancel.Attributes.Add("onclick", "return InputValidator(form1)");
            }

            if (Request.QueryString["Type"] == "New")
            {
                //设置合同状态

                cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_FIRST)));
                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
              //cookies.Values.Add("conStatus", Contract.CONTRACTSTATUS_TYPE_FIRST.ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                Response.AppendCookie(cookies);


                /*保存Cookies驳回和草稿状态*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                Response.AppendCookie(cookiesDisprove);
      
            }
            else if (Request.QueryString["VoucherID"] != null)
            {
                contractID = Convert.ToInt32(Request["VoucherID"]);
                ViewState["contractID"] = contractID;

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", Request["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
              
                Response.AppendCookie(cookies);

                GetContractInfo(contractID);

                /*保存Cookies驳回和草稿状态*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

                btnPutIn.Enabled = true;
                btnBlankOut.Enabled = true;
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

                btnPutIn.Enabled = true;
                btnBlankOut.Enabled = true;
            }
            contractCode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_contractCode");
            contractBeginDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractBeginDate");
            contractEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractEndDate");
            conLeaseTradeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
            //txtRefID.Attributes.Add("onkeydown", "textleave()");
            txtTradeID.Attributes.Add("onclick", "ShowTree()");
 
            //baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasic");
            //leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseItem");
            //shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopInfo");
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

    /// <summary>
    /// 绑定管理公司 add by lcp at 2009-3-25
    /// </summary>
    private void BindSubCompany()
    {
        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new Subs(), "SubsID", "SubsName", this.ddlSubs);
    }
    /// <summary>
    /// 绑定合同类型 add by lcp at 2009-3-25
    /// </summary>
    private void BindContractType()
    {
        BaseBO objBaseBo = new BaseBO();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new ContractType(), "ContractTypeID", "ContractTypeName", this.ddlContractType);
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
            
            hidTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            SelectTradeID();

            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            // add by lcp at 2009-3-25
            this.ddlContractType.SelectedValue = (contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString());
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());
            // add end
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
            

            StringBuilder sb = new StringBuilder();
            if (contractDs.Tables[0].Rows[0]["Note"].ToString() != "")
            {
                sb.Append(contractDs.Tables[0].Rows[0]["Note"].ToString());
            }
            //如果有驳回意见，则取驳回意见
            int mywrkFlwID = 0;
            int mynodeID = 0;
            int mysequence = 0;
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

            if (Request.QueryString["Sequence"] != null)
            {
                mysequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            }
            else
            {
                mysequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
            }
            int wrkFlwID = Convert.ToInt32(mywrkFlwID);
            int sequence = Convert.ToInt32(mysequence);
            int nodeID = Convert.ToInt32(mynodeID);

            //WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);
            //string ss = objWrkFlwEntity.VoucherMemo;
            //if (ss != "")
            //{
            //    sb.Append("［");
            //    sb.Append(ss);
            //    sb.Append("］");
            //}
            listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : sb.ToString());

            ViewState["contractID"] = contractID;
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
            ViewState["ConStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            ViewState["chargeStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            ViewState["ConEndDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
        }
    }
    #endregion

    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_IN)
        {
            SaveBaseBargain();
        }
        else if (Convert.ToInt32(Request.Cookies["Disprove"].Values["DisproveID"]) == DISPROVE_UP)
        {
            UpdateBaseBargain();
        }
        btnPutIn.Enabled = true;
        btnBlankOut.Enabled = true;
    }
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        try
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            int dateDay = 0;
            int clearDay = 0;
            TimeSpan timeSpan;

            baseBO.WhereClause = "ChargeClass <> " + ChargeType.CHARGECLASS_WATERORDLECT + " and ChargeClass <>" + ChargeType.CHARGECLASS_MAINTAIN + " and ChargeClass <> " + ChargeType.CHARGECLASS_OTHER;
            rs = baseBO.Query(new ChargeType());
            foreach (ChargeType chargeType in rs)
            {
                string sqlStr = "select ConStartDate,ConEndDate,FStartDate,FEndDate,ChargeStartDate from ConFormulaH a inner join Contract b on " +
                    "a.ContractID=b.ContractID where a.ContractID=" + ViewState["contractID"] + " And ChargeTypeID=" + chargeType.ChargeTypeID;
                DataTable dt = baseBO.QueryDataSet(sqlStr).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        timeSpan = Convert.ToDateTime(dt.Rows[i]["FStartDate"]).Date - Convert.ToDateTime(dt.Rows[i]["FEndDate"]).Date;
                        dateDay = dateDay + timeSpan.Days;
                        clearDay++;
                    }
                    timeSpan = Convert.ToDateTime(dt.Rows[0]["ChargeStartDate"]).Date - Convert.ToDateTime(dt.Rows[0]["ConEndDate"]).Date;
                    if (dateDay != timeSpan.Days + (clearDay - 1))
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Contract_ChargeStartDate") + "';", true);
                        return;
                    }
                }
                clearDay = 0;
                dateDay = 0;
            }

            /*判断条款信息*/
            baseBO.WhereClause = "ContractID= " + ViewState["contractID"];
            rs = baseBO.Query(new ConLease());
            if (rs.Count < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseItem") + "'", true);
                return;
            }

            /*判断商铺信息*/
            baseBO.WhereClause = "ContractID= " + ViewState["contractID"];
            rs = baseBO.Query(new ConShop());
            if (rs.Count < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseShop") + "'", true);
                return;
            }
            if (rs.Count > 0)
            {
                baseBO.WhereClause = "shopid in (select shopid from conshop where ContractID= '" + ViewState["contractID"] + "')";
                rs = baseBO.Query(new ConShopUnit());
                if (rs.Count < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseShop") + "'", true);
                    return;
                }
                if (rs.Count > 0)
                {
                    baseBO.WhereClause = "ContractID= " + ViewState["contractID"] + "AND a.ShopTypeID=b.ShopTypeID";
                    ConShop conShop1 = new ConShop();
                    rs = baseBO.Query(conShop1);
                    if (rs.Count == 1)
                    {
                        conShop1 = rs.Dequeue() as ConShop;
                        if (conShop1.BuildingID == null || conShop1.FloorID == null || conShop1.LocationID == null || conShop1.BuildingID == -1 || conShop1.FloorID == -1 || conShop1.LocationID == -1 || conShop1.ShopCode == "" || conShop1.ShopCode == null || conShop1.ShopName == "" || conShop1.ShopName == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseShop") + "'", true);
                            return;
                        }
                    }
                }
            }

            /*判断租金公式信息*/
            baseBO.WhereClause = "ContractID= " + ViewState["contractID"];
            rs = baseBO.Query(new ConFormulaH());
            if (rs.Count < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseFormulaH") + "'", true);
                return;
            }

            baseTrans.BeginTrans();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int voucherID = Convert.ToInt32(ViewState["contractID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

            int wrkFlwID =Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID =Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);

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

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            cookies.Values.Add("wrkFlwID", "");
            cookies.Values.Add("sequence", "");
            cookies.Values.Add("nodeID", "");
            cookies.Values.Add("Disprove", "");
            Response.AppendCookie(cookies);
            ClearPage();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }


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
            baseBo.WhereClause = "";
            baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            ds = baseBo.QueryDataSet(new TradeRelation());

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
            contact.TradeID = Convert.ToInt32(hidTradeID.Text);
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
            contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contact.AdditionalItem = listBoxAddItem.Text;
            contact.EConURL = txtBargain.Text;
            contact.Note = listBoxRemark.Text;
            contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);
            contact.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue);
            contact.SigningMode = Convert.ToChar(Contract.CONTRACTSTATUS_TYPE_N);
            contact.BizMode = Contract.BIZ_MODE_LEASE;
            //add by lcp at 2009-3-25
            try { contact.ContractTypeID = Int32.Parse(this.ddlContractType.SelectedValue); }
            catch { contact.ContractTypeID = 0; }
            try { contact.SubsID = Int32.Parse(this.ddlSubs.SelectedValue); }
            catch { contact.SubsID = 0; }
            //add end
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
        catch(Exception ex)
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
            baseBo.WhereClause = "";
            baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            ds = baseBo.QueryDataSet(new TradeRelation());

            contact.CustID = Convert.ToInt32(ViewState["custId"]);
            contact.ContractCode = txtContractCode.Text;
            contact.RefID = txtRefID.Text.Trim();
            contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
            contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
            contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
            contact.TradeID = Convert.ToInt32(hidTradeID.Text);
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
            contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
            contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contact.AdditionalItem = listBoxAddItem.Text;
            contact.EConURL = txtBargain.Text;
            contact.Note = listBoxRemark.Text;
            contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);
            contact.CommOper = Convert.ToInt32(cmbCommOper.SelectedValue);

            //add by lcp at 2009-3-25
            try { contact.ContractTypeID = Int32.Parse(this.ddlContractType.SelectedValue); }
            catch { contact.ContractTypeID = 0; }
            try { contact.SubsID = Int32.Parse(this.ddlSubs.SelectedValue); }
            catch { contact.SubsID = 0; }
            //add end

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

    protected void imgCustCodeQ_Click(object sender, EventArgs e)
    {
        if (txtCustCode.Text != "")
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustCode= '" + txtCustCode.Text.Trim() + "'";
            Resultset rs = baseBo.Query(new Customer());
            if (rs.Count == 1)
            {
                Customer customer = rs.Dequeue() as Customer;
                txtCustName.Text = customer.CustName;
                txtCustShortName.Text = customer.CustShortName;
                ViewState["custId"] = customer.CustID;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_NoCustCode") + "'", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
    }
    protected void imgCustNameQ_Click(object sender, EventArgs e)
    {
        if (txtCustShortName.Text != "")
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustShortName like '%" + txtCustShortName.Text.Trim() + "%'";
            Resultset rs = baseBo.Query(new Customer());
            if (rs.Count == 1)
            {
                Customer customer = rs.Dequeue() as Customer;
                ViewState["custId"] = customer.CustID;
                txtCustCode.Text = customer.CustCode;
                txtCustName.Text = customer.CustName;
                txtCustShortName.Text = customer.CustShortName;
            }
            else if (rs.Count > 1)
            {
                string str = "selectCust()";
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_NoCustShortName") + "'", true);
            }
            else
            {
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
    }


    private void ClearPage()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        hidTradeID.Text = "";
        txtTradeID.Text = "";
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
        this.ddlSubs.SelectedIndex = 0;
        this.ddlContractType.SelectedIndex = 0;
    }
    protected void btnBindDealType_Click(object sender, EventArgs e)
    {
        SelectTradeID();
    }

    private void SelectTradeID()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "TradeID = " + Convert.ToInt32(hidTradeID.Text);
        rs = baseBo.Query(new TradeRelation());
        if (rs.Count == 1)
        {
            TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
            txtTradeID.Text = tradeRelation.TradeName;
        }
        baseBo.WhereClause = "";
    }
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        try
        {

            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = listBoxRemark.Text;
            Resultset rsUnits = new Resultset();
            BaseBO baseBO = new BaseBO();


            baseBo.WhereClause = "a.ShopTypeID=b.ShopTypeID And ContractID=" + voucherID;
            rs = baseBo.Query(new ConShop());

            baseTrans.BeginTrans();

            foreach (ConShop conShop in rs)
            {
                baseBO.WhereClause = "ShopID = " + conShop.ShopId;
                rsUnits = baseBO.Query(new ConShopUnit());

                foreach (ConShopUnit unit in rsUnits)
                {
                    if (baseTrans.ExecuteUpdate("Update Unit Set UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + "Where UnitID = " + unit.UnitID) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                        baseTrans.Rollback();
                        return;
                    }
                }


                baseTrans.WhereClause = "ShopID = " + conShop.ShopId;
                if (baseTrans.Delete(new ConShopUnit()) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                    baseTrans.Rollback();
                    return;
                }
            }

            baseTrans.WhereClause = "ContractID=" + voucherID;

            if (baseTrans.Delete(new ConShop()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                baseTrans.Rollback();
                return;
            }

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);

            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo, baseTrans);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo, baseTrans);
            }

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            Response.AppendCookie(cookies);
            ClearPage();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("再签作废审批信息错误:", ex);
        }
    }
}
