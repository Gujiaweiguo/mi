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

public partial class Lease_ChangeLease_QueryContractChange : BasePage
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
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Response.Buffer = false;
        //Page.Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            if (Request.QueryString["VoucherID"] != null)
            {
                ViewState["ConFormulaModID"] = Convert.ToInt32(Request.QueryString["VoucherID"]);
                SelContractInfo( " ConFormulaModID = " + Convert.ToInt32(ViewState["ConFormulaModID"]));

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("ConFormulaModID", Request.QueryString["VoucherID"].ToString());
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

                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                Response.AppendCookie(cookiesWorkFlow);

                btnQueryContract.Enabled = false;
                btnPutIn.Enabled = true;
                btnBlankOut.Enabled = true;
            }

            else if (Request.QueryString["Type"] == "New")
            {
                /*删除Cookies续约ID*/
                HttpCookie cookiesCustumer = new HttpCookie("Info");

                cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                cookiesCustumer.Values.Add("ConFormulaModID", "");
                Response.AppendCookie(cookiesCustumer);

                /*把驳回状态存入Cookies*/
                HttpCookie cookiesDisprove = new HttpCookie("Disprove");

                cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
                cookiesDisprove.Values.Add("DisproveID", DISPROVE_IN.ToString());
                Response.AppendCookie(cookiesDisprove);

                HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

                cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
                cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
                cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
                Response.AppendCookie(cookiesWorkFlow);
            }
            else if (Request.Cookies["Info"].Values["conID"] != "")
            {
                SelContractInfo("a.ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]));
                btnQueryContract.Enabled = false;
                btnPutIn.Enabled = true;
                btnBlankOut.Enabled = true;
            }

            inserStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            titleName = (String)GetGlobalResourceObject("BaseInfo", "Lease_ExpressionMod");
            changeLease = (String)GetGlobalResourceObject("BaseInfo", "ChangeLease_NotUpdate");
            changeLeaseNow = (String)GetGlobalResourceObject("BaseInfo", "Lease_AddrentalFormulaMod");
            dateError = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            btnBlankOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
            ListBind();
        }
    }

    protected void butAuditing_Click(object sender, EventArgs e)
    {
        //SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //int voucherID = 0;
        //voucherID = Convert.ToInt32(ViewState["ConOverTimeID"]);
        //String voucherHints = ViewState["ContractID"].ToString();
        //String voucherMemo = "";

        //VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        //WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);

        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "Close();", true);

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = 0;
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
                string sqlStr = "Select a.ConFormulaModID,ConStartDate,ConEndDate,FStartDate,FEndDate,ChargeStartDate From " +
                     "ConFormulaMod a left join  Contract b on a.ContractID=b.ContractID left join ConFormulaHMod c on a.ConFormulaModID =c.ConFormulaModID " +
                     "Where a.ContractID= " + ViewState["ContractID"] + " And ChargeTypeID= " + chargeType.ChargeTypeID + " And a.ConFormulaModID=" + Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]);

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



            voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);


            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]), Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]), Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]), Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]), Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]), Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]), vInfo);
            }

            HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

            cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
            cookiesWorkFlow.Values.Add("WorkFlowID", "");
            cookiesWorkFlow.Values.Add("NodeID", "");
            Response.AppendCookie(cookiesWorkFlow);

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

            btnPutIn.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);

            ClearText();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("租金公式变更提交审批错误:", ex);
        }
    }

    protected void btnQueryContract_Click(object sender, EventArgs e)
    {
        GetContractInfo(txtCustCode.Text.Trim());
    }
    #region 获取合同信息
    protected void GetContractInfo(string contractCode)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ContractCode = '" + contractCode + "' And ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_INGEAR;
        DataSet contractDs = baseBO.QueryDataSet(new Contract());
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
            //listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

            SelectTradeID(contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? 0 : Convert.ToInt32(contractDs.Tables[0].Rows[0]["TradeID"]));
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());


            StringBuilder sb = new StringBuilder();

            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", contractDs.Tables[0].Rows[0]["ContractID"].ToString());
            Response.AppendCookie(cookies);

            ViewState["ContractID"] = contractDs.Tables[0].Rows[0]["ContractID"].ToString();

        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "';", true);
        }
    }
    #endregion


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
    private void SelContractInfo(string  whereStr)
    {
        CustContract custContract = new CustContract();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();

        baseBO.WhereClause = whereStr;
        ds = baseBO.QueryDataSet(custContract);
        if(ds.Tables[0].Rows.Count>0)
        {
            txtCustCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            BaseBO baseBO = new BaseBO();
            BaseTrans baseTrans = new BaseTrans();
            ConFormulaMod conFormulaMod = new ConFormulaMod();
            ConFormulaHMod conFormulaHMod = new ConFormulaHMod();
            ConFormulaMMod conFormulaMMod = new ConFormulaMMod();
            ConFormulaPMod conFormulaPMod = new ConFormulaPMod();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int mywrkFlwID = 0;
            int mynodeID = 0;
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";

            if (ViewState["ContractID"].ToString() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ConFormulaMod_NullContractID") + "'", true);
                return;
            }

            /*插入变更单信息*/
            conFormulaMod.ConFormulaModID = BaseApp.GetConFormulaModID();
            conFormulaMod.ContractID = Convert.ToInt32(ViewState["ContractID"]);
            conFormulaMod.IsValid = ConFormulaMod.CONFORMULAMOD_INVLIDATION;
            conFormulaMod.RefID = txtRefID.Text;
            conFormulaMod.ModReason = txtModReason.Text.Trim();

            baseTrans.BeginTrans();
            if (baseTrans.Insert(conFormulaMod) == -1)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                return;
            }

            /*根据合同号查询原租金公式类别中的数据插入到租金公式类别变更表中*/
            DataTable dtHMod = baseBO.QueryDataSet("Select FormulaID,ChargeTypeID,FStartDate,FEndDate,FormulaType,TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt From ConFormulaH Where ContractID=" + Convert.ToInt32(ViewState["ContractID"])).Tables[0];

            if (dtHMod.Rows.Count > 0)
            {
                for (int i = 0; i < dtHMod.Rows.Count; i++)
                {
                    conFormulaHMod.ConFormulaModID = conFormulaMod.ConFormulaModID;
                    conFormulaHMod.FormulaID = BaseApp.GetFormulaIDMod();
                    conFormulaHMod.ChargeTypeID = Convert.ToInt32(dtHMod.Rows[i]["ChargeTypeID"]);
                    conFormulaHMod.FStartDate = Convert.ToDateTime(dtHMod.Rows[i]["FStartDate"]);
                    conFormulaHMod.FEndDate = Convert.ToDateTime(dtHMod.Rows[i]["FEndDate"]);
                    conFormulaHMod.FormulaType = dtHMod.Rows[i]["FormulaType"].ToString();
                    conFormulaHMod.TotalArea = Convert.ToDecimal(dtHMod.Rows[i]["TotalArea"]);
                    conFormulaHMod.UnitPrice = Convert.ToDecimal(dtHMod.Rows[i]["UnitPrice"]);
                    conFormulaHMod.BaseAmt = Convert.ToDecimal(dtHMod.Rows[i]["BaseAmt"]);
                    conFormulaHMod.FixedRental = Convert.ToDecimal(dtHMod.Rows[i]["FixedRental"]);
                    conFormulaHMod.RateType = dtHMod.Rows[i]["RateType"].ToString();
                    conFormulaHMod.PcentOpt = dtHMod.Rows[i]["PcentOpt"].ToString();
                    conFormulaHMod.MinSumOpt = dtHMod.Rows[i]["MinSumOpt"].ToString();

                    if (baseTrans.Insert(conFormulaHMod) == -1)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        return;
                    }
                    if (dtHMod.Rows[i]["FormulaType"].Equals(ConFormulaH.FORMULATYPE_TYPE_TWO))
                    {
                        /*根据合同号查询原结算公式保底中的数据插入到结算公式保底变更表中*/
                        DataTable dtMMod = baseBO.QueryDataSet("Select FormulaID,SalesTo,MinSum From ConFormulaM Where FormulaID =" + Convert.ToInt32(dtHMod.Rows[i]["FormulaID"])).Tables[0];

                        if (dtMMod.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtMMod.Rows.Count; j++)
                            {
                                conFormulaMMod.ConFormulaMModID = BaseApp.GetConFormulaMModID();
                                conFormulaMMod.FormulaID = conFormulaHMod.FormulaID;
                                conFormulaMMod.SalesTo = Convert.ToDecimal(dtMMod.Rows[j]["SalesTo"]);
                                conFormulaMMod.MinSum = Convert.ToDecimal(dtMMod.Rows[j]["MinSum"]);

                                if (baseTrans.Insert(conFormulaMMod) == -1)
                                {
                                    baseTrans.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                                    return;
                                }
                            }
                        }

                        /*根据合同号查询原结算公式抽成中的数据插入到结算公式抽成变更表中*/
                        DataTable dtPMod = baseBO.QueryDataSet("Select FormulaID,SalesTo,Pcent From ConFormulaP Where FormulaID= " + Convert.ToInt32(dtHMod.Rows[i]["FormulaID"])).Tables[0];

                        if (dtPMod.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtPMod.Rows.Count; k++)
                            {
                                conFormulaPMod.ConFormulaPModID = BaseApp.GetConFormulaPModID();
                                conFormulaPMod.FormulaID = conFormulaHMod.FormulaID;
                                conFormulaPMod.SalesTo = Convert.ToDecimal(dtPMod.Rows[k]["SalesTo"]);
                                conFormulaPMod.Pcent = Convert.ToDecimal(dtPMod.Rows[k]["Pcent"]);

                                if (baseTrans.Insert(conFormulaPMod) == -1)
                                {
                                    baseTrans.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                                    return;
                                }
                            }
                        }
                    }


                }
            }

            VoucherInfo vInfo = new VoucherInfo(conFormulaMod.ConFormulaModID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);

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

            baseTrans.Commit();

            /*保存变更单ID(ConFormulaModID)到Cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("ConFormulaModID", conFormulaMod.ConFormulaModID.ToString());
            cookies.Values.Add("conID", ViewState["ContractID"].ToString());
            cookies.Values.Add("CustShortName", txtCustShortName.Text.ToString());
            cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
            Response.AppendCookie(cookies);


            btnQueryContract.Enabled = false;
            btnSave.Enabled = false;
            btnPutIn.Enabled = true;
            btnBlankOut.Enabled = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加租金公式变更错误:", ex);
        }
    }
    private void ClearText()
    {
        txtCustCode.Text = "";
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
    }
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
       try
       {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["ConFormulaModID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);


            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]), Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]), Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]) != 0)
            {
                WrkFlwApp.BlankOutVoucherNode(Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]), Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]), Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }

            HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

            cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
            cookiesWorkFlow.Values.Add("WorkFlowID", "");
            cookiesWorkFlow.Values.Add("NodeID", "");
            Response.AppendCookie(cookiesWorkFlow);

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

            btnPutIn.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
           ClearText();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("租金公式变更提交审批错误:", ex);
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

