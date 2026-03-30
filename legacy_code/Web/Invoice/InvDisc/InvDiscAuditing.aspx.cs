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
using RentableArea;
using Invoice.InvoiceH;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base.Page;
using Base.Util;
public partial class Invoice_InvDisc_InvDiscAuditing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["DiscID"] = Request.QueryString["VoucherID"];
            page("a.DiscID=" + Convert.ToInt32(ViewState["DiscID"]));
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "hidden()", true);
        }
    }
    protected void butConsent_Click(object sender, EventArgs e)
    {
        int row = Convert.ToInt32(ViewState["spareRow"]);
        for (int i = 0; i < row; i++)
        {
            decimal invActPayAmt = Convert.ToDecimal(GrdVewInvAdj.Rows[i].Cells[3].Text);  //实际应结金额
            decimal invPaidAmt = Convert.ToDecimal(GrdVewInvAdj.Rows[i].Cells[4].Text);    //付款金额
            decimal invDscAmt = Convert.ToDecimal(GrdVewInvAdj.Rows[i].Cells[6].Text);    //优惠金额
            decimal discRate = Convert.ToDecimal(GrdVewInvAdj.Rows[i].Cells[5].Text) / 100;    //优惠率
            if ((invActPayAmt - invPaidAmt) * discRate < Math.Abs(invDscAmt))
            {
                clearSelected();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invDiscAmtError") + "'", true);
                return;
            }

        }
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        int deptID = sessionUser.DeptID;
        int userID = sessionUser.UserID;
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
        String voucherHints = DateTime.Now.ToString();
        String voucherMemo = txtContractID.Text.Trim();

        baseTrans.BeginTrans();

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

        baseTrans.Commit();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

        page("a.DiscID= 0");
        TextClear();
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
            String voucherMemo = txtVoucherMemo.Text.Trim();

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);

            TextClear();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回租金公式错误:", ex);
        }
    }
    protected void page(string discID)
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = discID;
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = baseBO.QueryDataSet(new InvDiscAuditing()).Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["DiscRate"] = Convert.ToDecimal(dt.Rows[i]["DiscRate"]) * 100;
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvAdj.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvAdj.DataSource = pds;
            GrdVewInvAdj.DataBind();
        }
        else
        {
            pds.AllowPaging = true;
            pds.PageSize = 10;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;

            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }

            this.GrdVewInvAdj.DataSource = pds;
            this.GrdVewInvAdj.DataBind();
            spareRow = GrdVewInvAdj.Rows.Count;
            ViewState["spareRow"] = spareRow;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvAdj.DataSource = pds;
            GrdVewInvAdj.DataBind();
        }
        clearGridViewRow();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    }

    protected void GrdVewInvAdj_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        InvDiscAuditing invDiscAuditing = new InvDiscAuditing();

        baseBO.WhereClause = "a.DiscDetID=" + Convert.ToInt32(GrdVewInvAdj.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(invDiscAuditing);
        if (rs.Count == 1)
        {
            invDiscAuditing = rs.Dequeue() as InvDiscAuditing;

            txtCustCode.Text = invDiscAuditing.CustCode;
            txtCustName.Text = invDiscAuditing.CustName;
            txtContractID.Text = invDiscAuditing.ContractCode;
            txtShopName.Text = invDiscAuditing.ShopName;
            txtDiscRate.Text = Convert.ToString(invDiscAuditing.DiscRate * 100);
            txtDiscAmt.Text = invDiscAuditing.InvDiscAmt.ToString();
            txtDiscReason.Text = invDiscAuditing.DiscReason;
        }

        foreach (GridViewRow gvr in GrdVewInvAdj.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }

    private void clearSelected()
    {
        foreach (GridViewRow gvr in GrdVewInvAdj.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }

    private void TextClear()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtContractID.Text = "";
        txtShopName.Text = "";
        txtDiscRate.Text = "";
        txtDiscAmt.Text = "";
        txtDiscReason.Text = "";
        txtVoucherMemo.Text = "";
    }

    protected void clearGridViewRow()
    {
        foreach (GridViewRow gvr in GrdVewInvAdj.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }
}

