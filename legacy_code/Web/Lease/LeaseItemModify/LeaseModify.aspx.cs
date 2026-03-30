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
using Lease.ContractMod;

public partial class Lease_LeaseItemModify_LeaseModify : BasePage
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

    #region 提示信息
    public string contractCode;        //请输入合同号!
    public string contractBeginDate;  //请输入合同开始时间!
    public string contractEndDate;    //请输入合同结束时间!
    public string conLeaseTradeID;         //请选择经营类别!
    public string beginEndDate;
    public string beginChargeDate;
    #endregion
    public string baseInfo;  //基本信息
    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Response.Buffer = false;
        //Page.Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            ListBind();

            btnTempSave.Attributes.Add("onclick", "return InputValidator(this)");

            if (Request.QueryString["VoucherID"] != null)
            {

                ViewState["ConModID"] = Convert.ToInt32(Request["VoucherID"]);
                SelContractModInfo(Convert.ToInt32(ViewState["ConModID"]));

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConOverTimeID", Request.QueryString["VoucherID"].ToString());
                cookies.Values.Add("conID", "");
                Response.AppendCookie(cookies);

                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                cookiesWorkFlow.Values.Add("SequenceID", Request.QueryString["Sequence"]);
                Response.AppendCookie(cookiesWorkFlow);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
                Response.AppendCookie(cookiesDisprove);

                btnQueryContract.Enabled = false;
                btnOverTime.Enabled = true;
                btnTempSave.Enabled = true;
            }

            else if (Request.QueryString["Type"] == "New")
            {
                /*删除Cookies续约ID*/
                HttpCookie cookiesCustumer = new HttpCookie("Info");

                cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                cookiesCustumer.Values.Add("ConOverTimeID", "");
                Response.AppendCookie(cookiesCustumer);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                Response.AppendCookie(cookiesDisprove);

                /*把工作流ID和节点ID存入Cookies*/

                if (Request.Cookies["WorkFlow"].Values["WorkFlowID"] == "")
                {
                    HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                    cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                    cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                    cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                    cookiesWorkFlow.Values.Add("SequenceID", "");
                    Response.AppendCookie(cookiesWorkFlow);
                }
            }
            else if (Request.Cookies["Info"].Values["ConOverTimeID"] != "" && Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]) != 0)
            {
                ViewState["ConModID"] = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
                SelContractModInfo(Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]));
                //cmbContractStatus.Text = Request.Cookies["Info"].Values["conStatus"];
                btnQueryContract.Enabled = false;
                btnOverTime.Enabled = true;
                btnTempSave.Enabled = true;
            }
            else if (Request.Cookies["Info"].Values["conID"] != "")
            {
                GetContractInfo("ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]) + " And ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR);
            }

            contractCode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_contractCode");
            contractBeginDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractBeginDate");
            contractEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractEndDate");
            conLeaseTradeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            beginChargeDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_DateTime");

            inserStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            titleName = (String)GetGlobalResourceObject("BaseInfo", "ConTerminateBill_ConTerminateBill");
            changeLease = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_NotUpdate");
            changeLeaseNow = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_Update");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasic");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo");

            txtTradeID.Attributes.Add("onclick", "ShowTree()");
            btnBalnkOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
        }
    }

    protected void butAuditing_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        voucherID = Convert.ToInt32(ViewState["ConModID"]);
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
    #region 获取合同信息
    protected void GetContractInfo(string sqlStr)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = sqlStr;
        DataSet contractDs = baseBO.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0)
        {

            if (Convert.ToInt32(contractDs.Tables[0].Rows[0]["BizMode"]) == Contract.BIZ_MODE_UNIT)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ConUnionMod_Union") + "';", true);
                return;
            }

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
            if (custDS.Tables[0].Rows.Count > 0)
            {
                txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
                txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustShortName"].ToString();
            }
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"])));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

            hidTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            if (hidTradeID.Text != "")
            {
                SelectTradeID();
            }
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtChargeStart.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            txtNorentDays.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();
            StringBuilder sb = new StringBuilder();

            ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();
            ViewState["CustID"] = contractDs.Tables[0].Rows[0]["CustID"].ToString();
            /*将合同号存入Cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", ViewState["ContractID"].ToString());
            Response.AppendCookie(cookies);

            btnTempSave.Enabled = true;
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
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";
            voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);

            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["WorkFlow"].Values["SequenceID"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["WorkFlow"].Values["SequenceID"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);

            ClearCookies();
            ClearText();
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
            txtCustCode.Text = conOvertimeBill.ContractCode;
            txtCustName.Text = conOvertimeBill.CustName;
            txtCustShortName.Text = conOvertimeBill.CustShortName;
            //cmbTradeID.SelectedValue = conOvertimeBill.TradeID.ToString();
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(conOvertimeBill.ContractStatus));
            txtContractCode.Text = conOvertimeBill.ContractCode;
            txtRefID.Text = conOvertimeBill.RefID.ToString();
            txtConStartDate.Text = conOvertimeBill.ConStartDate.ToString("yyyy-MM-dd");
            txtConEndDate.Text = conOvertimeBill.ConEndDate.ToString("yyyy-MM-dd");
            DDownListPenalty.SelectedValue = conOvertimeBill.Penalty.ToString();
            DDownListTerm.SelectedValue = conOvertimeBill.Notice.ToString();
            //txtChargeStart.Text = conOvertimeBill.NewConStartDate.ToString("yyyy-MM-dd");
            //txtNewConEndDate.Text = conOvertimeBill.NewConEndDate.ToString("yyyy-MM-dd");
            txtBargain.Text = conOvertimeBill.EConURL;
            listBoxAddItem.Text = conOvertimeBill.AdditionalItem;
            listBoxRemark.Text = conOvertimeBill.Note;
        }
    }

    private void SelContractModInfo(int conModID)
    {
        ContractMod contractMod = new ContractMod();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        baseBO.WhereClause = "ConModID=" + conModID;
        rs = baseBO.Query(contractMod);

        if (rs.Count > 0)
        {
            contractMod = rs.Dequeue() as ContractMod;

            ViewState["ContractID"] = contractMod.ContractID;

            DataTable dt = baseBO.QueryDataSet("Select CustName,CustShortName From Contract a,Customer b Where a.CustID = b.CustID And a.ContractID = " + contractMod.ContractID).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = dt.Rows[0]["CustName"].ToString();
                txtCustShortName.Text = dt.Rows[0]["CustShortName"].ToString();
            }

            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(contractMod.ContractStatus));
            txtContractCode.Text = contractMod.ContractCode;
            txtRefID.Text = contractMod.RefID.ToString();
            txtConStartDate.Text = contractMod.ConStartDate.ToString("yyyy-MM-dd");
            txtConEndDate.Text = contractMod.ConEndDate.ToString("yyyy-MM-dd");
            txtChargeStart.Text = contractMod.ChargeStartDate.ToString("yyy-MM-dd");
            txtNorentDays.Text = contractMod.NorentDays.ToString();
            DDownListPenalty.SelectedValue = contractMod.Penalty.ToString();
            DDownListTerm.SelectedValue = contractMod.Notice.ToString();
            txtBargain.Text = contractMod.EConURL;
            listBoxAddItem.Text = contractMod.AdditionalItem;
            listBoxRemark.Text = contractMod.Note;

            hidTradeID.Text = contractMod.TradeID.ToString();
            if (hidTradeID.Text != "")
            {
                SelectTradeID();
            }

        }
    }

    private void InsertConOverTime()
    {
        BaseTrans baseTrans = new BaseTrans();
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOShopUnit = new BaseBO();
        ContractMod contractMod = new ContractMod();
        ConShopMod conShopMod = new ConShopMod();
        ConLeaseMod conLeaseMod = new ConLeaseMod();
        ConShop conShop = new ConShop();
        ConLease conLease = new ConLease();
        ConShopUnitMod conShopUnitMod = new ConShopUnitMod();
        Resultset rs = new Resultset();
        Resultset rsShopUnit = new Resultset();


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";

        try
        {
            contractMod.ConModID = BaseApp.GetConModID();

            voucherID = contractMod.ConModID;
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
            int mywrkFlwID = 0;
            int mynodeID = 0;

            baseTrans.BeginTrans();
            ds.Clear();
            baseBO.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            ds = baseBO.QueryDataSet(new TradeRelation());

            contractMod.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            contractMod.ContractCode = txtContractCode.Text.Trim();
            contractMod.RefID = Convert.ToInt32(txtRefID.Text == "" ? "0" : txtRefID.Text);
            contractMod.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
            contractMod.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
            contractMod.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
            contractMod.TradeID = Convert.ToInt32(hidTradeID.Text);
            contractMod.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            contractMod.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contractMod.AdditionalItem = listBoxAddItem.Text;
            contractMod.EConURL = txtBargain.Text;
            contractMod.Note = listBoxRemark.Text;
            contractMod.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contractMod.NorentDays = Convert.ToInt32(txtNorentDays.Text);

            /*查询租赁合同相关信息表的信息存放到修改信息表中*/
            baseBO.WhereClause = "ContractID=" + Convert.ToInt32(ViewState["ContractID"]);
            rs = baseBO.Query(conLease);
            if (rs.Count == 1)
            {
                conLease = rs.Dequeue() as ConLease;
                conLeaseMod.ConModID = voucherID;
                conLeaseMod.ContractID = conLease.ContractID;
                conLeaseMod.AdditionalItem = conLease.AdditionalItem;
                conLeaseMod.BalanceMonth = conLease.BalanceMonth;
                conLeaseMod.BillCycle = conLease.BillCycle;
                conLeaseMod.CurTypeID = conLease.CurTypeID;
                conLeaseMod.EConURL = conLease.EConURL;
                conLeaseMod.IfPrepay = conLease.IfPrepay;
                conLeaseMod.IntDay = conLease.IntDay;
                conLeaseMod.LatePayInt = conLease.LatePayInt;
                conLeaseMod.MonthSettleDays = conLease.MonthSettleDays;
                conLeaseMod.Note = conLease.Note;
                conLeaseMod.PayTypeID = conLease.PayTypeID;
                conLeaseMod.RentInc = conLease.RentInc;
                conLeaseMod.SettleMode = conLease.SettleMode;
                conLeaseMod.TaxRate = conLease.TaxRate;
                conLeaseMod.TaxType = conLease.TaxType;

                if (baseTrans.Insert(conLeaseMod) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
                    baseTrans.Rollback();
                    return;
                }
            }

            /*查询租赁合同商铺信息存放到修改商铺信息表中*/
            baseBO.WhereClause = "a.ShopTypeID = b.ShopTypeID And ContractID=" + Convert.ToInt32(ViewState["ContractID"]);
            rs = baseBO.Query(conShop);

            if (rs.Count > 0)
            {
                foreach (ConShop conShops in rs)
                {
                    conShopMod.AreaId = conShops.AreaId;
                    conShopMod.BrandID = conShops.BrandID;
                    conShopMod.BuildingID = conShops.BuildingID;
                    conShopMod.ConModID = voucherID;
                    conShopMod.ContactorName = conShops.ContactorName.ToString();
                    conShopMod.ContractID = conShops.ContractID;
                    conShopMod.FloorID = conShops.FloorID;
                    conShopMod.LocationID = conShops.LocationID;
                    conShopMod.RefID = conShops.RefID;
                    conShopMod.ShopCode = conShops.ShopCode;
                    conShopMod.ShopEndDate = conShops.ShopEndDate;
                    conShopMod.ShopId = conShops.ShopId;
                    conShopMod.ShopModID = BaseApp.GetConModShopID();
                    conShopMod.ShopName = conShops.ShopName;
                    conShopMod.ShopStartDate = conShops.ShopStartDate;
                    conShopMod.ShopStatus = conShops.ShopStatus;
                    conShopMod.ShopTypeID = conShops.ShopTypeID;
                    conShopMod.Tel = conShops.Tel;
                    conShopMod.RentArea = conShops.RentArea;

                    if (baseTrans.Insert(conShopMod) < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
                        baseTrans.Rollback();
                        return;
                    }

                    /*修改商铺单元存放到修改表中*/
                    baseBOShopUnit.WhereClause="ShopID=" + conShops.ShopId;
                    rsShopUnit = baseBOShopUnit.Query(new ConShopUnit());

                    if (rsShopUnit.Count > 0)
                    {
                        foreach(ConShopUnit conShopUnits in rsShopUnit)
                        {
                            conShopUnitMod.ShopModID = conShopMod.ShopModID;
                            conShopUnitMod.RentArea = conShopUnits.RentArea;
                            conShopUnitMod.RentInfo = conShopUnits.RentInfo;
                            conShopUnitMod.RentLevel = conShopUnits.RentLevel;
                            conShopUnitMod.RentStatus = conShopUnits.RentStatus;
                            conShopUnitMod.ShopID = conShopUnits.ShopID;
                            conShopUnitMod.UnitID = conShopUnits.UnitID;

                            if (baseTrans.Insert(conShopUnitMod) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + inserStr + "'", true);
                                baseTrans.Rollback();
                                return;
                            }
                        }
                    }
                }
            }

            if (baseTrans.Insert(contractMod) < 1)
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

            HttpCookie cookies = new HttpCookie("Info");

            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            cookies.Values.Add("ConOverTimeID", contractMod.ConModID.ToString());
            cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
            Response.AppendCookie(cookies);


            HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            cookiesDisprove.Values.Add("DisproveID", DISPROVE_UP.ToString());
            Response.AppendCookie(cookiesDisprove);

            btnOverTime.Enabled = true;
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
        ContractMod contact = new ContractMod();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        baseBO.WhereClause = "ConModID=" + ViewState["ConModID"];

        ds.Clear();
        baseBO.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
        ds = baseBO.QueryDataSet(new TradeRelation());

        contact.RefID = Convert.ToInt32(txtRefID.Text == "" ? "0" : txtRefID.Text);
        contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
        contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
        contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
        contact.TradeID = Convert.ToInt32(hidTradeID.Text);
        if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
        {
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
        }
        else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
        {
            contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_INGEAR);
        }
        contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);;
        contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
        contact.AdditionalItem = listBoxAddItem.Text;
        contact.EConURL = txtBargain.Text;
        contact.Note = listBoxRemark.Text;
        contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
        contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);

        if (baseBO.Update(contact) != -1)
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

    private void ClearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        txtChargeStart.Text = "";
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        DDownListPenalty.SelectedIndex = 0;
        DDownListTerm.SelectedIndex = 0;
        txtNorentDays.Text = "";
        txtBargain.Text = "";
        listBoxAddItem.Text = "";
        listBoxRemark.Text = "";
    }
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        try
        {
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["SequenceID"]);
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConOverTimeID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = listBoxRemark.Text;
            Resultset rsUnits = new Resultset();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();

            baseBO.WhereClause = "a.ShopTypeID=b.ShopTypeID And ConModID=" + voucherID;
            rs = baseBO.Query(new ConShopMod());

            baseTrans.BeginTrans();

            foreach (ConShopMod conShop in rs)
            {
                baseBO.WhereClause = "ShopModID = " + conShop.ShopId;
                rsUnits = baseBO.Query(new ConShopUnit());

                foreach (ConShopUnitMod unit in rsUnits)
                {
                    if (baseTrans.ExecuteUpdate("Update Unit Set UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + "Where UnitID = " + unit.UnitID) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                        baseTrans.Rollback();
                        return;
                    }
                }

                baseTrans.WhereClause = "ShopModID = " + conShop.ShopId;
                if (baseTrans.Delete(new ConShopUnitMod()) == -1)
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
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            Response.AppendCookie(cookies);
            ClearText();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("租赁合同作废审批信息错误:", ex);
        }
    }
    protected void btnBindDealType_Click(object sender, EventArgs e)
    {
        SelectTradeID();
    }

    private void SelectTradeID()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "TradeID = " + Convert.ToInt32(hidTradeID.Text);
        rs = baseBO.Query(new TradeRelation());
        if (rs.Count == 1)
        {
            TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
            txtTradeID.Text = tradeRelation.TradeName;
        }
    }
}
