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

using Base.DB;
using Base.Biz;
using Invoice.InvoiceH;
using Lease.Customer;
using Base;
using BaseInfo.User;
using WorkFlow;
using WorkFlow.WrkFlw;
using WorkFlow.Uiltil;
using Base.Page;
using Base.Util;

public partial class Invoice_InvoiceDetailCancel_InvoiceDetailCancel : BasePage
{
    public string billOfDocumentDelete;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VoucherID"] != null)
            {
                BaseBO baseBO = new BaseBO();
                Resultset rs = new Resultset();
                InvCancel invCancel = new InvCancel();
                ViewState["InvCelID"] = Request.QueryString["VoucherID"];
                baseBO.WhereClause = "InvCelID=" + Convert.ToInt32(Request.QueryString["VoucherID"]);
                rs = baseBO.Query(invCancel);
                if (rs.Count == 1)
                {
                    invCancel = rs.Dequeue() as InvCancel;

                    txtAdjReason.Text = invCancel.CelReason;
                    txtCustCode.Text = invCancel.CustCode;
                    txtCustName.Text = invCancel.CustName;
                    txtContractID.Text = invCancel.ContractCode;
                    txtKeepAccountsMth.Text = invCancel.InvPeriod.ToShortDateString().ToString();
                    txtNote.Text = invCancel.Note;

                    ViewState["InvID"] = invCancel.InvID;
                    BindInvoiceDetail(invCancel.InvID);
                    btnBlankOut.Enabled = true;
                    btnPutIn.Enabled = true;
                    btnSave.Enabled = true;
                }
                ViewState["ReturnSequence"] = Request.QueryString["Sequence"].ToString();
                ViewState["Flag"] = "Update";
            }
            else
            { 
                BindInvoiceDetail(0);
                btnPutIn.Enabled = false;
                btnSave.Enabled = false;
                ViewState["Flag"] = "Insert";
            }
            btnBlankOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");
        }
    }

    protected void BindInvoiceDetail(int invID)
    {
        /*绑定结算单明细*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "InvID=" + invID + " and RentType != " + Invoice.InvoiceDetail.RENTTYPE_BLANK_RECORD_P;
        DataTable dt = baseBO.QueryDataSet(new InvoiceDetail()).Tables[0];

        pds.DataSource = dt.DefaultView;

        //GrdVewInvoiceDetail.AllowPaging = false;
        //GrdVewInvoiceDetail.PageSize = 9;
        //pds.PageSize = 9;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvoiceDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
        }
        else
        {
            //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = true;
            //}
            this.GrdVewInvoiceDetail.DataSource = pds;
            this.GrdVewInvoiceDetail.DataBind();
            spareRow = GrdVewInvoiceDetail.Rows.Count;
            for (int i = 0; i < GrdVewInvoiceDetail.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
        }
    }

    protected void butAuditing_Click(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        /*提交审批*/
        int voucherID = Convert.ToInt32(ViewState["InvID"]);
        String voucherHints = txtCustName.Text.Trim();
        String voucherMemo = txtCustName.Text.Trim();
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);

        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(ViewState["ReturnSequence"].ToString()), vInfo);

        TextClear();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
    
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        InvoiceHeader invoiceHeader = new InvoiceHeader();
        baseBO.WhereClause = "InvCode='" + txtInvCode.Text.Trim() + "' and InvStatus= " + InvoiceHeader.INVOICEHEADER_AVAILABILITY;
        rs = baseBO.Query(invoiceHeader);
        if (txtInvCode.Text != "")
        {
            if (rs.Count == 1)
            {
                invoiceHeader = rs.Dequeue() as InvoiceHeader;

                txtCustCode.Text = invoiceHeader.CustCode.ToString();
                txtCustName.Text = invoiceHeader.CustName;
                txtContractID.Text = invoiceHeader.ContractCode.ToString();
                txtKeepAccountsMth.Text = invoiceHeader.InvPeriod.ToShortDateString().ToString();

                ViewState["InvID"] = invoiceHeader.InvID;
                BindInvoiceDetail(invoiceHeader.InvID);

                btnSave.Enabled = true;
            }
            else
            {
                TextClear();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "MesPublic_InvCancel") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '结算单号为空。'", true);
        }
    }

    private void TextClear()
    {
        txtInvCode.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtContractID.Text = "";
        txtKeepAccountsMth.Text = "";
        txtAdjReason.Text = "";

        BindInvoiceDetail(0);
    }
    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        try
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
            String voucherHints = txtCustName.Text.Trim();
            String voucherMemo = txtCustName.Text;

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            
            TextClear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
            Logger.Log("租赁合同作废审批信息错误:", ex);
        }
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        InvCancel invCancel = new InvCancel();
        invCancel.InvCelID = Convert.ToInt32(ViewState["InvID"]);
        invCancel.InvID = Convert.ToInt32(ViewState["InvID"]);
        invCancel.CelReason = txtAdjReason.Text.Trim();
        invCancel.InvCelStatus = InvCancel.INVCANCEL_DRAFT;
        invCancel.Note = txtNote.Text;

        decimal invPaidAmt = InvoiceCancelPO.GetInvPaidAmt(Convert.ToInt32(ViewState["InvID"]));
        if (invPaidAmt > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_InvPaidAmtYes") + "'", true);
            return;
        }
        else
        {

            BaseBO baseBO = new BaseBO();
            if (ViewState["Flag"].ToString() == "Insert")
            {
                int result = baseBO.Insert(invCancel);
                if (result == 1)
                {
                    SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                    int deptID = sessionUser.DeptID;
                    int userID = sessionUser.UserID;
                    int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
                    int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
                    int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
                    int voucherID = Convert.ToInt32(ViewState["InvID"]);
                    String voucherHints = txtCustName.Text.Trim();
                    String voucherMemo = txtCustName.Text;

                    VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);

                    WrkFlwApp.CommitVoucherDraft(wrkFlwID, nodeID, vInfo);

                    ViewState["ReturnSequence"] = WrkFlwApp.returnSequence.ToString();

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else if (ViewState["Flag"].ToString() == "Update")
            {
                baseBO.WhereClause = "InvCelID = " + Convert.ToInt32(ViewState["InvID"]);
                int result = baseBO.Update(invCancel);
                if (result == 1)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
        }
        btnPutIn.Enabled = true;
    }
    //protected void btnNextType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindInvoiceDetail(Convert.ToInt32(ViewState["InvID"]));
    //}
    //protected void btnBackType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindInvoiceDetail(Convert.ToInt32(ViewState["InvID"]));
    //}
    protected void gvShopBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        BindInvoiceDetail(Convert.ToInt32(ViewState["InvID"]));
    }

}
