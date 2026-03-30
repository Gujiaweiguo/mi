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
public partial class Invoice_InvoiceDetailCancel_InvoiceDetailCancelAu : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
            }
        }
    }
    protected void BindInvoiceDetail(int invID)
    {
        /*绑定结算单明细*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "InvID=" + invID;
        DataTable dt = baseBO.QueryDataSet(new InvoiceDetail()).Tables[0];

        pds.DataSource = dt.DefaultView;

        //GrdVewInvoiceDetail.AllowPaging = false;
        //GrdVewInvoiceDetail.PageSize = 12;
        //pds.PageSize = 12;

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
    protected void butConsent_Click(object sender, EventArgs e)
    {
         decimal invPaidAmt = InvoiceCancelPO.GetInvPaidAmt(Convert.ToInt32(ViewState["InvID"]));
         if (invPaidAmt > 0)
         {
             ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_InvPaidAmtYes") + "'", true);
             return;
         }
         else
         {
             SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
             int deptID = sessionUser.DeptID;
             int userID = sessionUser.UserID;
             int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
             int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
             int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
             int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
             String voucherHints = txtCustName.Text.Trim();
             String voucherMemo = txtContractID.Text.Trim();

             VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
             WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo);

             ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

             TextClear();
         }
    }
    protected void butOverrule_Click(object sender, EventArgs e)
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
            String voucherMemo = txtVoucherMemo.Text;

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);
            TextClear();
        }
        catch
        {
 
        }
    }

    private void TextClear()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtContractID.Text = "";
        txtKeepAccountsMth.Text = "";
        txtAdjReason.Text = "";

        BindInvoiceDetail(0);
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
