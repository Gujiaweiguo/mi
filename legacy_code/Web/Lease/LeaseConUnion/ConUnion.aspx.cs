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
using Lease.Union;
using Lease.SMSPara;
using Lease.Subs;

public partial class Lease_LeaseConUnion_ConUnion : BasePage
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
    #endregion

    #region
    public string baseInfo;  //基本信息
    public string leaseItem;  //相关条款
    public string shopInfo;   //商铺信息
    public string espression;  //结算公式
    #endregion
    public string billOfDocumentDelete;
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //初始化DropDownList
            BindPenalty();
            BindNotice();
            BindSubCompany();
            BindContractType();

            /*获取合同信息
            存入cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);

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
                btnTempSave.Attributes.Add("onclick", "return InputValidator(form1)");
            }


            int contractID = 0;
            if (Request.QueryString["WrkFlwID"] != null || (Request.Cookies["Info"]["wrkFlwID"] != null && Request.Cookies["Info"]["wrkFlwID"] != ""))   //工作流
            {
                if (Request["VoucherID"] != null)
                {
                    contractID = Convert.ToInt32(Request["VoucherID"]);
                    
                    cookies.Values.Add("conID", Request["VoucherID"].ToString());
                    cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                    cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                    cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                    Response.AppendCookie(cookies);
                    btnBalnkOut.Enabled = true;
                    btnPutIn.Enabled = true;
                }
                else if (Request.Cookies["Info"] != null)
                {
                    contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
                    btnBalnkOut.Enabled = true;
                    btnPutIn.Enabled = true;
                }
                btnPutIn.Visible = true;
                btnTempSave.Visible = true;
                btnBalnkOut.Visible = true;
                btnSave.Visible = false;

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
            }
            else
            {
                if (Request.QueryString["VoucherID"] != null)
                {
                    contractID = Convert.ToInt32(Request.QueryString["VoucherID"]);
                    cookies.Values.Add("conID", Request.QueryString["VoucherID"].ToString());
                    cookies.Values.Add("modify", Request.QueryString["modify"].ToString());
                    Response.AppendCookie(cookies);
                }
                else if (Request.Cookies["Info"] != null)
                {
                    contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
                }
                ViewState["myFlag"] = "Updated";
                btnPutIn.Visible = false;
                btnTempSave.Visible = false;
                btnBalnkOut.Visible = false;
                btnSave.Visible = true;
                btnMessage.Visible = false;
            }
            GetContractInfo(contractID);

           
            btnMessage.Attributes.Add("onclick", "ShowMessage()");

            //输入验证与提示信息


            contractCode = (String)GetGlobalResourceObject("BaseInfo", "Prompt_contractCode");
            contractBeginDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractBeginDate");
            contractEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseContractEndDate");
            conLeaseTradeID = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            beginChargeDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_DateTime");
            //txtRefID.Attributes.Add("onkeydown", "textleave()");
            btnPutIn.Attributes.Add("onclick", "return InputValidator(form1)");
            txtTradeID.Attributes.Add("onclick", "ShowTree()");
            btnBalnkOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");

            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasic");
            leaseItem = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblUnionItem");
            shopInfo = (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo");
            espression = (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance");
        }
    }
    #endregion

    #region 提交审批
    protected void btnPutIn_Click(object sender, EventArgs e)
    {

        BaseBO baseBO = new BaseBO();
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
        rs = baseBO.Query(new ConUnion());
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
        /////ADD TJM
        if (rs.Count > 0)
        {
            baseBO.WhereClause = "shopid=(select shopid from conshop where ContractID= '" + ViewState["contractID"] + "')";
            rs = baseBO.Query(new ConShopUnit());
            if (rs.Count < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseShop") + "'", true);
                return;
            }
            if (rs.Count > 0)
            {
                baseBO.WhereClause = "ContractID= " + ViewState["contractID"] + "and a.shoptypeid=b.shoptypeid";
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
        ////////END ADD

        /*判断租金公式信息*/
        baseBO.WhereClause = "ContractID= " + ViewState["contractID"];
        rs = baseBO.Query(new ConFormulaH());
        if (rs.Count < 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_AddLeaseFormulaH") + "'", true);
            return;
        }

        baseTrans.BeginTrans();
        try
        {
            SaveBaseBargain();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int voucherID = Convert.ToInt32(ViewState["contractID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = "";

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

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
            WrkFlwApp.ConfirmVoucher(Convert.ToInt32(wrkFlwID), Convert.ToInt32(nodeID), Convert.ToInt32(sequence), vInfo, baseTrans);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            baseTrans.Rollback();
            return;
        }
        baseTrans.Commit();
        //删除cookies
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddDays(1);
        cookies.Values.Add("conID", "");
        Response.AppendCookie(cookies);
        ClearText();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
    }
    #endregion

    #region 保存草稿
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        SaveBaseBargain();
        btnPutIn.Enabled = true;
    }
    #endregion

    #region 初始化DropDownList

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

            //判断首期费用是否可生成

            if (flag == Contract.CONTRACTSTATUS_TYPE_INGEAR)
            {
                //btnFirstCharge.Enabled = true;
                btnTempSave.Enabled = false;
                btnPutIn.Enabled = false;
            }
            else
            {
                //btnFirstCharge.Enabled = false;
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
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();
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


            hidTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            if (hidTradeID.Text != "")
            {
                SelectTradeID();
            }

            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
            this.ddlContractType.SelectedValue = (contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractTypeID"].ToString());
            this.ddlSubs.SelectedValue = (contractDs.Tables[0].Rows[0]["SubsID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["SubsID"].ToString());

            StringBuilder sb = new StringBuilder();
            if (contractDs.Tables[0].Rows[0]["Note"].ToString() != "")
            {
                sb.Append(contractDs.Tables[0].Rows[0]["Note"].ToString());
            }
            if (Request.QueryString["WrkFlwID"] != null || (Request.Cookies["Info"]["wrkFlwID"] != null && Request.Cookies["Info"]["wrkFlwID"] != ""))   //工作流
            {
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

                WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);
                string ss = objWrkFlwEntity.VoucherMemo;
                if (ss != "")
                {
                    sb.Append("［");
                    sb.Append(ss);
                    sb.Append("］");
                }
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

    #region 基本合同项目保存草稿
    protected void SaveBaseBargain()
    {
        try
        {
            BaseBO baseBO = new BaseBO();
            DataSet dsContract = new DataSet();
            ds.Clear();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(hidTradeID.Text) + "'";
            ds = baseBo.QueryDataSet(new TradeRelation());

            Contract contact = new Contract();
            contact.ContractID = Convert.ToInt32(ViewState["contractID"]);
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
            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_INGEAR);
            }
            contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
            //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
            contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
            contact.AdditionalItem = listBoxAddItem.Text;
            contact.EConURL = txtBargain.Text;
            contact.Note = listBoxRemark.Text;
            contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
            contact.NorentDays = Convert.ToInt32(txtNorentDays.Text);
            contact.ContractTypeID = Int32.Parse(this.ddlContractType.SelectedValue);
            contact.SubsID = Int32.Parse(this.ddlSubs.SelectedValue);

            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractID = " + contact.ContractID;

            if (baseBo.Update(contact) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "    " + (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID") + ":" + contact.ContractCode + "'", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("合同保存草稿错误:", ex);
        }
    }
    #endregion

    private void ClearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        txtTradeID.Text = "";
        hidTradeID.Text = "";
        cmbContractStatus.Text = "";
        txtContractCode.Text = "";
        txtRefID.Text = "";
        txtConStartDate.Text = "";
        txtConEndDate.Text = "";
        DDownListPenalty.SelectedIndex = 0;
        DDownListTerm.SelectedIndex = 0;
        txtChargeStart.Text = "";
        txtNorentDays.Text = "";
        txtBargain.Text = "";
        listBoxAddItem.Text = "";
        listBoxRemark.Text = "";
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
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            Response.AppendCookie(cookies);
            ClearText();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("租赁合同作废审批信息错误:", ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveBaseBargain();
    }
}